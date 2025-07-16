package main

import (
	"OutboxPoller/EventBuses"
	"OutboxPoller/OutboxProviders"
	"OutboxPoller/Pollers"
	"database/sql"
	"flag"
	"fmt"
	_ "github.com/lib/pq"
	"log"
	"os"
	"os/signal"
	"syscall"
)

func main() {
	cmd := os.Args[1]
	switch cmd {
	case "outbox":
		fs := flag.NewFlagSet("outbox", flag.ExitOnError)
		if OutboxCommand(fs) != nil {
			os.Exit(1)
		}
	case "url":
		fs := flag.NewFlagSet("url", flag.ExitOnError)
		if UrlCommand(fs) != nil {
			os.Exit(1)
		}
	default:
		fmt.Println("Expected a subcommand: outbox|url")
		os.Exit(1)
	}

	done := make(chan os.Signal, 1)
	signal.Notify(done, syscall.SIGINT, syscall.SIGTERM)
	log.Println("Press ctrl+c to exit...")
	<-done
}
func OutboxCommand(cmd *flag.FlagSet) error {
	connectionString := cmd.String("connectionString", "postgres://postgres:postgres@localhost/InstrumentManagement?sslmode=disable", "PostgreSQL connection string")
	kafkaBrokers := cmd.String("kafka-brokers", "localhost:9092", "Kafka brokers addresses")
	kafkaTopic := cmd.String("kafka-topic", "wealth", "Kafka topic name")
	period := cmd.Int("period", 1000, "period in millisecond")
	if err := cmd.Parse(os.Args[2:]); err != nil {
		fmt.Printf("error: %s", err)
		return err
	}

	provider := initPostgresProvider(*connectionString)
	bus := EventBuses.NewKafkaEventBus([]string{*kafkaBrokers}, *kafkaTopic)
	poller := Pollers.OutboxPoller{
		Provider: provider,
		Bus:      bus,
		PeriodMs: *period,
	}
	poller.RunInBackground()
	return nil
}

func initPostgresProvider(connectionString string) OutboxProviders.IOutboxProvider {
	db, err := sql.Open("postgres", connectionString)
	if err != nil {
		log.Fatalf("Error connecting to postgres: %s", err)
	}

	if err := db.Ping(); err != nil {
		log.Fatalf("Error pinging postgres: %s", err)
	}

	return OutboxProviders.NewPostgresOutboxProvider(db)
}

func UrlCommand(cmd *flag.FlagSet) error {
	url := cmd.String("url", "http://localhost:5280/api/OutboxTrigger/Next", "trigger url")
	period := cmd.Int("period", 1000, "period in millisecond")
	method := cmd.String("method", "POST", "GET | POST")
	if err := cmd.Parse(os.Args[2:]); err != nil {
		fmt.Printf("error: %s", err)
		return err
	}
	var poller = Pollers.UrlPoller{
		Url:      *url,
		Method:   *method,
		PeriodMs: *period,
	}
	poller.RunInBackground()
	return nil
}
