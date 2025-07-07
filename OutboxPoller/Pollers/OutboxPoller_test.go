package Pollers

import (
	"OutboxPoller/OutboxProviders"
	"github.com/stretchr/testify/mock"
	"testing"
	"time"
)

type MockProvider struct {
	mock.Mock
}

func (m *MockProvider) PullMessages(batchSize int) []OutboxProviders.OutboxMessage {
	args := m.Called(batchSize)
	return args.Get(0).([]OutboxProviders.OutboxMessage)
}

func (m *MockProvider) MarkPublished(id string) error {
	args := m.Called(id)
	return args.Error(0)
}

type MockBus struct {
	mock.Mock
}

func (m *MockBus) Publish(message OutboxProviders.OutboxMessage) error {
	args := m.Called(message)
	return args.Error(0)
}

func TestProcessWithoutMessages(t *testing.T) {
	mockProvider := new(MockProvider)
	mockBus := new(MockBus)
	mockProvider.On("PullMessages", mock.Anything).Return([]OutboxProviders.OutboxMessage{})
	mockProvider.On("MarkPublished", mock.Anything).Return(nil)
	mockBus.On("Publish", mock.Anything).Return(nil)
	poller := OutboxPoller{
		Provider: mockProvider,
		Bus:      mockBus,
	}

	poller.Process()

	mockProvider.AssertCalled(t, "PullMessages", mock.Anything)
	mockProvider.AssertNotCalled(t, "MarkPublished", mock.Anything)
	mockBus.AssertNotCalled(t, "Publish", mock.Anything)
}

func TestProcess(t *testing.T) {
	message := OutboxProviders.OutboxMessage{
		Id:         "foo",
		Type:       "bar",
		Data:       "baz",
		OccurredOn: time.Now(),
	}
	mockProvider := new(MockProvider)
	mockBus := new(MockBus)
	mockProvider.On("PullMessages", mock.Anything).Return([]OutboxProviders.OutboxMessage{
		message,
	})
	mockProvider.On("MarkPublished", mock.Anything).Return(nil)
	mockBus.On("Publish", mock.Anything).Return(nil)
	poller := OutboxPoller{
		Provider: mockProvider,
		Bus:      mockBus,
	}

	poller.Process()

	mockProvider.AssertCalled(t, "PullMessages", mock.Anything)
	mockProvider.AssertCalled(t, "MarkPublished", message.Id)
	mockBus.AssertCalled(t, "Publish", message)
}
