package Pollers

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"time"
)

type UrlPoller struct {
	Url      string
	Method   string
	PeriodMs int
}

func (p *UrlPoller) RunInBackground() {
	go func() {
		log.Println("UrlPoller running in background...")
		log.Printf("Url: %s\n", p.Url)
		log.Printf("Period: %d ms\n", p.PeriodMs)
		for {
			p.Process()
			time.Sleep(time.Duration(p.PeriodMs) * time.Millisecond)
		}
	}()
}

func (p *UrlPoller) Process() {
	req, err := http.NewRequest(p.Method, p.Url, nil)
	if err != nil {
		fmt.Printf("could not create request: %s\n", err)
		os.Exit(1)
	}

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		log.Println(err.Error())
	}

	if (resp.StatusCode != 200) && (resp.StatusCode != 201) {
		log.Println(resp)
	}
}
