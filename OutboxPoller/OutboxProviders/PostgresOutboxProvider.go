package OutboxProviders

import (
	"database/sql"
	"errors"
	"log"
	"time"
)

type PostgresOutboxProvider struct {
	db *sql.DB
}

func NewPostgresOutboxProvider(db *sql.DB) *PostgresOutboxProvider {
	log.Printf("Creating postgres outbox provider\n")
	return &PostgresOutboxProvider{
		db: db,
	}
}

func (p *PostgresOutboxProvider) PullMessages(batchSize int) []OutboxMessage {
	query := `
		SELECT "Id", "Type", "Data", "OccurredOn" 
		FROM "OutboxMessages" 
		WHERE "ProcessedOn" IS null
		ORDER BY "OccurredOn" 
		LIMIT $1
	`

	rows, err := p.db.Query(query, batchSize)
	if err != nil {
		return nil
	}

	var messages []OutboxMessage
	for rows.Next() {
		var msg OutboxMessage
		err := rows.Scan(&msg.Id, &msg.Type, &msg.Data, &msg.OccurredOn)
		if err != nil {
			continue
		}
		messages = append(messages, msg)
	}

	_ = rows.Close()
	return messages
}

func (p *PostgresOutboxProvider) MarkPublished(id string) error {
	if id == "" {
		return errors.New("empty id")
	}

	query := `
		UPDATE "OutboxMessages"
		SET "ProcessedOn" = $1
		WHERE "Id" = $2
	`

	_, err := p.db.Exec(query, time.Now(), id)
	return err
}
