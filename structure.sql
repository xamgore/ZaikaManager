-- What can be sold.
DROP TABLE IF EXISTS "Products" CASCADE;
CREATE TABLE "Products" (
    "Id" 		serial	PRIMARY KEY,
    "Name"		text	NOT NULL,
    "Measure"	text	NOT NULL
);


-- Who makes the products.
DROP TABLE IF EXISTS "Producers" CASCADE;
CREATE TABLE "Producers" (
    "Id"	serial	PRIMARY KEY,
    "Name"	text	NOT NULL,
    "City"	text	NOT NULL,
    "Phone"	text
);


-- Where products are stored.
DROP TABLE IF EXISTS "Warehouses" CASCADE;
CREATE TABLE "Warehouses" (
	"Id"   serial	PRIMARY KEY,
	"Name" text		NOT NULL,
	"City" text		NOT NULL
);


-- Available amount of products for now.
-- Table is generated from Operations table.
DROP TABLE IF EXISTS "Stuffs" CASCADE;
CREATE TABLE "Stuffs" (
	"Id"          serial	PRIMARY KEY,
	"ProductId"   integer	NOT NULL  REFERENCES "Products"  ("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	"WarehouseId" integer	NOT NULL  REFERENCES "Warehouses"("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	"Amount"      integer
);


-- Log of all operations.
DROP TABLE IF EXISTS "Operations" CASCADE;
CREATE TABLE "Operations" (
	"Id"			serial	PRIMARY KEY,
	"ProductId"		integer	NOT NULL  REFERENCES "Products"  ("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	"ProducerId"	integer NOT NULL  REFERENCES "Producers" ("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	"WarehouseId"	integer	NOT NULL  REFERENCES "Warehouses"("Id")  ON DELETE CASCADE ON UPDATE CASCADE,
	"Price"			decimal CHECK ("Price" > 0),
	"Augment"       integer,
	"Date"			date
);


-- Producers last operations
DROP MATERIALIZED VIEW IF EXISTS "LastOperations" CASCADE;
CREATE MATERIALIZED VIEW "LastOperations" AS
    SELECT P."Id",
       IP."Name" AS "IncomeProduct",
       IO."Price" AS "IncomePrice",
       IO."Date" AS "IncomeDate",
       OP."Name" AS "OutcomeProduct",
       OO."Price" AS "OutcomePrice",
       OO."Date" AS "OutcomeDate"
    FROM "Producers" P
        LEFT JOIN "Operations" IO ON IO."Id" = (
            SELECT O."Id" FROM "Operations" O
            WHERE "Augment" > 0 AND "ProducerId" = P."Id"
            ORDER BY O."Id" LIMIT 1)
        LEFT JOIN "Operations" OO ON OO."Id" = (
            SELECT O."Id" FROM "Operations" O
            WHERE "Augment" < 0 AND "ProducerId" = P."Id"
            ORDER BY O."Id" LIMIT 1)
        LEFT JOIN "Products" IP ON IO."ProductId" = IP."Id"
        LEFT JOIN "Products" OP ON OO."ProductId" = OP."Id";
