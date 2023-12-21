CREATE TABLE auction (
    id TEXT NOT NULL PRIMARY KEY,
    created TIMESTAMP NOT NULL DEFAULT now()
);

CREATE TABLE item_data (
    id INTEGER NOT NULL PRIMARY KEY,
    name TEXT,
    quality TEXT,
    level INTEGER,
    required_level INTEGER,
    item_class TEXT,
    item_subclass TEXT,
    inventory_type TEXT,
    purchase_price INTEGER,
    sell_price INTEGER,
    max_count INTEGER,
    is_equippable BOOLEAN,
    is_stackable BOOLEAN
);

CREATE TABLE auction_entry (
    id INTEGER NOT NULL PRIMARY KEY,
    auction_id TEXT REFERENCES auction(id) ON DELETE CASCADE,
    item_id INTEGER REFERENCES item_data(id),
    bid INTEGER,
    buyout INTEGER,
    quantity INTEGER,
    time_left TEXT
);
