package EventBuses

import (
	"OutboxPoller/OutboxProviders"
	"log"
)

type LoggingEventBus struct {
	logger *log.Logger
}

func (b *LoggingEventBus) Publish(message OutboxProviders.OutboxMessage) error {
	b.logger.Printf(
		"Publishing event: ID=%s, Type=%s, Data=%s, OccurredOn=%v\n",
		message.Id,
		message.Type,
		message.Data,
		message.OccurredOn,
	)
	return nil
}

func NewLoggingEventBus(logger *log.Logger) *LoggingEventBus {
	if logger == nil {
		logger = log.Default()
	}
	return &LoggingEventBus{logger: logger}
}
