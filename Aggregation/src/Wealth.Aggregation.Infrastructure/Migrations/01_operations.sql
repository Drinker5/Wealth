-- +goose up
CREATE TABLE IF NOT EXISTS operations
(
    op_id           String,
    date            DateTime,
    portfolio_id    Int32,
    instrument_id   Int32,
    instrument_type UInt8,
    quantity        Int64,
    amount          Decimal(18, 4),
    currency        UInt8,
    operation_type  UInt8
) ENGINE = ReplacingMergeTree
      ORDER BY (op_id)
      PARTITION BY toStartOfYear(date);

-- +goose down
DROP TABLE IF EXISTS operations;
