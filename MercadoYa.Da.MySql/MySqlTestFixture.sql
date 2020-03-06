-- Start a transaction.
BEGIN;

-- Plan the tests.
SELECT tap.plan(3);

CALL MercadoYa.TRUNCATEALL();
SELECT tap.ok(SELECT COUNT(*) FROM users = 0, "TruncateAll.");

CALL MercadoYa.CreateRandomEntities(10);
SELECT tap.ok(SELECT COUNT(*) FROM users = 10, "Created 10 random users.");

CALL MercadoYa.InsertFood('Name', 'Description', 'Region', @Id);
SELECT tap.ok(@Id IS NOT NULL, 'InsertFood: OK.');

-- Finish the tests and clean up.
CALL tap.finish();
ROLLBACK;