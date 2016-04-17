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
        stuff_id       INT;
BEGIN
    SELECT "Date" FROM "Operations" ORDER BY "Id" DESC LIMIT 1 INTO last_date;

    IF new."Date" < last_date THEN
        RAISE EXCEPTION 'Outdated operations (before %) are deprecated', last_date;
    END IF;


    SELECT "Id", "Amount"
      INTO stuff_id, product_amount
      FROM "Stuffs"
     WHERE "WarehouseId" = new."WarehouseId"
       AND "ProductId"   = new."ProductId" LIMIT 1;

    product_amount := COALESCE(product_amount, 0);
    -- RAISE NOTICE 'Amount: %', product_amount;

    IF (new."Augment" < 0) AND product_amount < abs(new."Augment") THEN
        RAISE EXCEPTION 'Not enough amount of product on the warehouse to proceed operation';
    END IF;

    -- compute new amount
    IF stuff_id IS NULL THEN
        INSERT INTO "Stuffs" ("ProductId", "WarehouseId", "Amount")
            VALUES (new."ProductId", new."WarehouseId", product_amount + new."Augment");
    ELSE
        UPDATE "Stuffs" SET "Amount" = product_amount + new."Augment" WHERE "Id" = stuff_id;
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
