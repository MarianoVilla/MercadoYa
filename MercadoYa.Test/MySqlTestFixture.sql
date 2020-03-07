USE MercadoYa;
BEGIN;

CALL tap.no_plan();

-- TruncateAll
CALL TRUNCATEALL();
SELECT tap.eq((SELECT COUNT(*) FROM users), 0, "No users after TruncateAll.");
SELECT tap.eq((SELECT COUNT(*) FROM tags), 0, "No tags after TruncateAll.");
SELECT tap.eq((SELECT COUNT(*) FROM tag_foods), 0, "No foods after TruncateAll.");
SELECT tap.eq((SELECT COUNT(*) FROM user_credentials), 0, "No user_credentials after TruncateAll.");
SELECT tap.eq((SELECT COUNT(*) FROM users_tags), 0, "No users_tags after TruncateAll.");


-- RandomEntities
CALL CreateRandomEntities(10);
SELECT tap.eq((SELECT COUNT(*) FROM users), 20, "Should create 20 random users.");
SELECT tap.eq((SELECT COUNT(*) FROM store_users), 10, "10 of the users should be store users.");
SELECT tap.eq((SELECT COUNT(*) FROM customer_users), 10, "10 of the users should be customer users.");
SELECT tap.eq((SELECT COUNT(*) FROM users_tags), 20, "Should create 1 user/tag relation for each user");
SELECT tap.eq((SELECT COUNT(*) FROM tags), 10, "Sould create 10 tags.");
SELECT tap.eq((SELECT COUNT(*) FROM tag_foods), 10, "All 10 tags should be foods.");


CALL tap.finish();

ROLLBACK;