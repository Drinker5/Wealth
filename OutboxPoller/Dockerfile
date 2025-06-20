FROM golang:1.24 AS build-stage
WORKDIR /app
COPY . .
# Build the Go app
RUN go build -o main .

FROM ubuntu:22.04 AS app
WORKDIR /app
COPY --from=build-stage /app/main .
# Run the executable
ENTRYPOINT ["./main"]