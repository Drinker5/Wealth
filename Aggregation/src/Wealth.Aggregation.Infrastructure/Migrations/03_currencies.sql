-- +goose up
CREATE TABLE IF NOT EXISTS currency_trade
(
    op_id        String,
    date         DateTime,
    portfolio_id Int32,
    currency_id  Int32,
    quantity     Int64,
    amount       Decimal(18, 2),
    currency     UInt8,
    type         UInt8
) ENGINE = ReplacingMergeTree
      ORDER BY (op_id);

CREATE TABLE IF NOT EXISTS currency_money_operation
(
    op_id        String,
    date         DateTime,
    portfolio_id Int32,
    currency_id  Int32,
    amount       Decimal(18, 2),
    currency     UInt8,
    type         UInt8
) ENGINE = ReplacingMergeTree
      ORDER BY (op_id);

-- +goose down
DROP TABLE IF EXISTS currency_trade;
DROP TABLE IF EXISTS currency_money_operation;
