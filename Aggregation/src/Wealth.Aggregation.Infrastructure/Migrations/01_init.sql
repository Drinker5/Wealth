-- +goose up
CREATE TABLE IF NOT EXISTS stock_trade (
    op_id String,
    date DateTime,
    portfolio_id Int32,
    stock_id Int32,
    quantity Int64,
    amount Decimal(18, 2),
    currency UInt8,
    type UInt8
) ENGINE = MergeTree(op_id)
ORDER BY (portfolio_id, stock_id);


-- +goose down
DROP TABLE IF EXISTS stock_trade;