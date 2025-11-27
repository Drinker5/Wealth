-- +goose up
CREATE TABLE IF NOT EXISTS bond_money_operation
(
    op_id        String,
    date         DateTime,
    portfolio_id Int32,
    bond_id      Int32,
    amount       Decimal(18, 2),
    currency     UInt8,
    type         UInt8
) ENGINE = ReplacingMergeTree
      ORDER BY (op_id);

-- +goose down
DROP TABLE IF EXISTS bond_money_operation;
