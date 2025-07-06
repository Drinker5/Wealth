package OutboxProviders

import "time"

type OutboxMessage struct {
	Id         string
	Type       string
	Data       string
	OccurredOn time.Time
}
