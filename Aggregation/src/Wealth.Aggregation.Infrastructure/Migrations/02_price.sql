-- +goose up
CREATE TABLE IF NOT EXISTS instrument_price
(
    instrument_id   Int32,
    instrument_type UInt8,
    price           Decimal(18, 4)
) ENGINE = ReplacingMergeTree
      ORDER BY (instrument_id, instrument_type);

CREATE VIEW IF NOT EXISTS instrument_price_view AS
SELECT *
FROM default.instrument_price FINAL;

CREATE DICTIONARY IF NOT EXISTS instrument_price_dictionary
(
    instrument_id   Int32,
    instrument_type UInt8,
    price           Decimal(18, 4)
) PRIMARY KEY instrument_id, instrument_type
    SOURCE (CLICKHOUSE(TABLE 'instrument_price_view' USER 'default' PASSWORD 'default'))
    LIFETIME (MIN 600 MAX 900)
    LAYOUT (HASHED());

-- +goose down
DROP DICTIONARY IF EXISTS instrument_price_dictionary;
DROP VIEW IF EXISTS instrument_price_view;
DROP TABLE IF EXISTS instrument_price;
