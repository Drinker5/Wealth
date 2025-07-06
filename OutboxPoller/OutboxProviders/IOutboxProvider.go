package OutboxProviders

type IOutboxProvider interface {
	PullMessages(batchSize int) []OutboxMessage
	MarkPublished(id string)
}
