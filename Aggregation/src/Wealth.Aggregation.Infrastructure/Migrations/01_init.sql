CREATE TABLE IF NOT EXISTS stock_aggregation (
    id Int32,
    name String,
    stock_price Decimal(18, 2),
    dividend_per_year Decimal(18, 2),
    lot_size Int32,
    quantity Int32,
    current_value Decimal(18, 2) DEFAULT stock_price * quantity,
    current_dividend_value Decimal(18, 2) DEFAULT dividend_per_year * quantity,
    total_investments Decimal(18, 2),
    total_dividends Decimal(18, 2)
) ENGINE = MergeTree()
ORDER BY (id);