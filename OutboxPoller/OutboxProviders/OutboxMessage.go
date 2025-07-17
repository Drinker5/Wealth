package OutboxProviders

import "time"

type OutboxMessage struct {
	Id         string
	Key        string
	Type       string
	Data       []byte
	OccurredOn time.Time
}
