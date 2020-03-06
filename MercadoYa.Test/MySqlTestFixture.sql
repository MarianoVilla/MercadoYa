USE MercadoYa;
BEGIN;

CALL tap.no_plan();

-- TruncateAll
CALL TRUNCATEALL();
SELECT tap.ok((SELECT COUNT(*) FROM users) = 0, "No users after TruncateAll.");
SELECT tap.ok((SELECT COUNT(*) FROM tags) = 0, "No tags after TruncateAll.");

-- RandomEntities
CALL CreateRandomEntities(10);
SELECT tap.ok((SELECT COUNT(*) FROM users) = 20, "Should create 20 random users.");
SELECT tap.ok((SELECT COUNT(*) FROM store_users) = 10, "10 of the users should be store users.");
SELECT tap.ok((SELECT COUNT(*) FROM customer_users) = 10, "10 of the users should be customer users.");

-- 
CALL InsertFood('Name', 'Description', 'Region', @Id);
SELECT tap.ok(@Id IS NOT NULL, 'Should instert a food');
SELECT tap.ok((SELECT COUNT(*) FROM tags WHERE id = @Id) = 1, 'The inserted food should be in tags.');

CALL tap.finish();

ROLLBACK;