package EventBuses

import (
	"OutboxPoller/OutboxProviders"
	"log"
)

type LoggingEventBus struct {
}

func (b *LoggingEventBus) Publish(message OutboxProviders.OutboxMessage) error {
	log.Printf(
		"Publishing event: ID=%s, Type=%s, Data=%s, OccurredOn=%v\n",
		message.Id,
		message.Type,
		message.Data,
		message.OccurredOn,
	)
	return nil
}

func NewLoggingEventBus() *LoggingEventBus {
	return &LoggingEventBus{}
}
