package EventBuses

import "OutboxPoller/OutboxProviders"

type IEventBus interface {
	Publish(message OutboxProviders.OutboxMessage) error
}
