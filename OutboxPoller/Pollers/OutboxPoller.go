package Pollers

import (
	"OutboxPoller/EventBuses"
	"OutboxPoller/OutboxProviders"
	"log"
	"time"
)

type OutboxPoller struct {
	Provider OutboxProviders.IOutboxProvider
	Bus      EventBuses.IEventBus
	PeriodMs int
}

func (p *OutboxPoller) RunInBackground() {
	go func() {
		log.Println("OutboxPoller running in background...")
		log.Printf("Provider: %T\n", p.Provider)
		log.Printf("Bus: %T\n", p.Bus)
		log.Printf("Period: %d ms\n", p.PeriodMs)
		for {
			p.Process()
			time.Sleep(time.Duration(p.PeriodMs) * time.Millisecond)
		}
	}()
}

func (p *OutboxPoller) Process() {
	messages := p.Provider.PullMessages(1)
	for _, message := range messages {
		err := p.Bus.Publish(message)
		if err != nil {
			continue
		}
		err = p.Provider.MarkPublished(message.Id)
		if err != nil {
			log.Printf("Error while marking message as published: %s", err)
			continue
		}
	}
}
