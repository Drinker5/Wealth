package OutboxProviders

import (
	"log"
	"slices"
)

type InMemoryOutboxProvider struct {
	Messages []OutboxMessage
}

func (p *InMemoryOutboxProvider) PullMessages(batchSize int) []OutboxMessage {
	if batchSize > len(p.Messages) {
		batchSize = len(p.Messages)
	}

	return p.Messages[:batchSize]
}

func (p *InMemoryOutboxProvider) MarkPublished(id string) error {
	log.Printf("Mark Published %s", id)
	predicate := func(item OutboxMessage) bool {
		return item.Id == id
	}
	p.Messages = slices.DeleteFunc(p.Messages, predicate)
	return nil
}

func NewInMemoryOutboxProvider() *InMemoryOutboxProvider {
	return &InMemoryOutboxProvider{
		Messages: make([]OutboxMessage, 0),
	}
}

func (p *InMemoryOutboxProvider) AddMessage(message OutboxMessage) {
	p.Messages = append(p.Messages, message)
}
