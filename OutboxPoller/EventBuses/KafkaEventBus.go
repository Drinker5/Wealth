package EventBuses

import (
	"OutboxPoller/OutboxProviders"
	"context"
	"github.com/segmentio/kafka-go"
	"log"
	"time"
)

type KafkaEventBus struct {
	Writer *kafka.Writer
}

func (b *KafkaEventBus) Publish(message OutboxProviders.OutboxMessage) error {
	err := b.Writer.WriteMessages(
		context.Background(),
		kafka.Message{
			Key:   []byte(message.Key),
			Value: []byte(message.Data),
			Headers: []kafka.Header{
				{Key: "EventType", Value: []byte(message.Type)},
				{Key: "OccurredOn", Value: []byte(message.OccurredOn.Format(time.RFC3339))},
			},
		},
	)

	if err != nil {
		log.Printf("failed to publish message '%s' to Kafka: %s", message.Type, err)
		return err
	}
	return err
}

func NewKafkaEventBus(brokers []string, topic string) *KafkaEventBus {
	log.Printf("Creating kafka event bus for: %v, topic: %s\n", brokers, topic)
	return &KafkaEventBus{
		Writer: &kafka.Writer{
			Addr:     kafka.TCP(brokers...),
			Topic:    topic,
			Balancer: &kafka.RoundRobin{},
			Logger:   kafka.LoggerFunc(log.Printf),
		},
	}
}

func (b *KafkaEventBus) Close() error {
	return b.Writer.Close()
}
