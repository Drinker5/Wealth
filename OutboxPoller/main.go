package main

import (
	"flag"
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"
)

func main() {
	url := flag.String("url", "http://localhost:5280/api/OutboxTrigger/Next", "trigger url")
	period := flag.Int64("period", 1000, "period in millisecond")
	method := flag.String("method", "POST", "GET | POST")
	flag.Parse()

	go func() {
		log.Println("Polling running in background...")
		log.Printf("Url: %s\n", *url)
		log.Printf("Period: %d ms\n", *period)
		for {
			trigger(*url, *method)
			time.Sleep(time.Duration(*period) * time.Millisecond)
		}
	}()

	done := make(chan os.Signal, 1)
	signal.Notify(done, syscall.SIGINT, syscall.SIGTERM)
	log.Println("Press ctrl+c to exit...")
	<-done
}

func trigger(url string, method string) {
	req, err := http.NewRequest(method, url, nil)
	if err != nil {
		fmt.Printf("could not create request: %s\n", err)
		os.Exit(1)
	}

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		log.Println(err.Error())
	}

	if (resp.StatusCode != 200) && (resp.StatusCode != 201) {
		log.Println(resp)
	}
}
