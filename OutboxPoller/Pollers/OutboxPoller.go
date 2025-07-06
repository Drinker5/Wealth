package Pollers

import (
	"OutboxPoller/EventBuses"
	"OutboxPoller/OutboxProviders"
)

type OutboxPoller struct {
	provider OutboxProviders.IOutboxProvider
	bus      EventBuses.IEventBus
}

func (p *OutboxPoller) Process() {
	messages := p.provider.PullMessages(1)
	for _, message := range messages {
		err := p.bus.Publish(message)
		if err != nil {
			continue
		}
		p.provider.MarkPublished(message.Id)
	}
}
