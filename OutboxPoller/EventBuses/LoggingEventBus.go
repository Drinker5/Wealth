package EventBuses

import (
	"OutboxPoller/OutboxProviders"
	"log"
)

type LoggingEventBus struct {
}

func (b *LoggingEventBus) Publish(message OutboxProviders.OutboxMessage) error {
	log.Printf(
		"Publishing event: ID=%s, Key=%s, Type=%s, Data=%s, OccurredOn=%v\n",
		message.Id,
		message.Key,
		message.Type,
		message.Data,
		message.OccurredOn,
	)
	return nil
}

func NewLoggingEventBus() *LoggingEventBus {
	return &LoggingEventBus{}
}
