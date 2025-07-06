package EventBuses

import (
	"OutboxPoller/OutboxProviders"
	"context"
	"fmt"
	"github.com/segmentio/kafka-go"
	"time"
)

type KafkaEventBus struct {
	writer *kafka.Writer
}

func (b *KafkaEventBus) Publish(message OutboxProviders.OutboxMessage) error {
	err := b.writer.WriteMessages(
		context.Background(),
		kafka.Message{
			Key:   []byte(message.Id),
			Value: []byte(message.Data),
			Headers: []kafka.Header{
				{Key: "EventType", Value: []byte(message.Type)},
				{Key: "OccurredOn", Value: []byte(message.OccurredOn.Format(time.RFC3339))},
			},
		},
	)

	if err != nil {
		return fmt.Errorf("failed to publish message to Kafka: %w", err)
	}
	return err
}

func NewKafkaEventBus(brokers []string, topic string) *KafkaEventBus {
	return &KafkaEventBus{
		writer: &kafka.Writer{
			Addr:     kafka.TCP(brokers...),
			Topic:    topic,
			Balancer: &kafka.RoundRobin{},
		},
	}
}

func (b *KafkaEventBus) Close() error {
	return b.writer.Close()
}
