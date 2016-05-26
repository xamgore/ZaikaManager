CREATE OR REPLACE FUNCTION update_last_operations()
    RETURNS void AS $$
BEGIN
    REFRESH MATERIALIZED VIEW "LastOperations";
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION allow_edit_the_last_row()
    RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'UPDATE' THEN
        RAISE EXCEPTION 'Update operations are deprecated';
    END IF;

    IF EXISTS (SELECT "Id" FROM "Operations" WHERE "Id" > OLD."Id") THEN
        RAISE EXCEPTION 'Cant % operation, which is not the last in the table', lower(TG_OP);
    END IF;

    IF TG_OP = 'DELETE' THEN RETURN old; END IF;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION check_has_enough_stuff()
    RETURNS TRIGGER AS $$
DECLARE last_date      DATE;
        product_amount INT;
        warehouse_amnt INT;
        stuff_id       INT;
        last_op_id     INT;
BEGIN
    SELECT "Date" FROM "Operations" ORDER BY "Id" DESC LIMIT 1 INTO last_date;

    IF new."Date" < last_date THEN
        RAISE EXCEPTION 'Outdated operations (before %) are deprecated', last_date;
        RAISE NOTICE 'ooooooooooooooooooooooooooooooooooooooooooooo';
    END IF;

    -- that is ok to take new products
    IF (new."Augment" >= 0) THEN RETURN new; END IF;

    -- otherwise we have to recount available amount
    SELECT "Id", "Amount", "LastUpdate"
      INTO stuff_id, warehouse_amnt, last_op_id
      FROM "Stuffs"
     WHERE "WarehouseId" = new."WarehouseId"
       AND "ProductId"   = new."ProductId" LIMIT 1;

    warehouse_amnt := COALESCE(warehouse_amnt, 0);
    last_op_id     := COALESCE(last_op_id, 0);

    -- augment is always > 0
    SELECT SUM("Augment") INTO product_amount FROM "Operations"
     WHERE "Id" > COALESCE(last_op_id, 0) AND
           "ProductId" = new."ProductId"  AND
           "WarehouseId" = new."WarehouseId";
           
    product_amount := warehouse_amnt + COALESCE(product_amount, 0);

    IF product_amount < abs(new."Augment") THEN
        RAISE EXCEPTION 'Not enough amount of product on the warehouse to proceed operation';
    END IF;

    -- compute new amount
    IF stuff_id IS NULL THEN
        INSERT INTO "Stuffs" ("ProductId", "WarehouseId", "Amount", "LastUpdate")
            VALUES (new."ProductId", new."WarehouseId", product_amount + new."Augment", new."Id");
    ELSE
        UPDATE "Stuffs"
           SET "Amount" = product_amount + new."Augment", "LastUpdate" = new."Id"
         WHERE "Id" = stuff_id;
    END IF;

    RETURN new;
END;
$$ LANGUAGE plpgsql;



DROP TRIGGER IF EXISTS check_has_enough_stuff ON "Operations";
DROP TRIGGER IF EXISTS allow_edit_the_last_row ON "Operations";


CREATE TRIGGER check_has_enough_stuff BEFORE INSERT ON "Operations"
    FOR EACH ROW EXECUTE PROCEDURE check_has_enough_stuff();

CREATE TRIGGER allow_edit_the_last_row BEFORE UPDATE OR DELETE ON "Operations"
    FOR EACH ROW EXECUTE PROCEDURE allow_edit_the_last_row();
