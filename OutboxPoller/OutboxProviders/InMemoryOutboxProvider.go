package OutboxProviders

import "slices"

type InMemoryOutboxProvider struct {
	messages []OutboxMessage
}

func (p *InMemoryOutboxProvider) PullMessages(batchSize int) []OutboxMessage {
	return p.messages[:batchSize]
}

func (p *InMemoryOutboxProvider) MarkPublished(id string) {
	predicate := func(item OutboxMessage) bool {
		return item.Id == id
	}

	p.messages = slices.DeleteFunc(p.messages, predicate)
}

func NewInMemoryOutboxProvider() *InMemoryOutboxProvider {
	return &InMemoryOutboxProvider{
		messages: make([]OutboxMessage, 0),
	}
}

func (p *InMemoryOutboxProvider) AddMessage(message OutboxMessage) {
	p.messages = append(p.messages, message)
}
