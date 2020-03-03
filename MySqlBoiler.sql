CALL InsertStoreUser('','','','','','','','',-87.6770458,41.9631174,'',@Uid);
CALL InsertCustomerUser('','','','','','','','',1,1,'',@Uid);
CALL CreateRandomEntities(100);
CALL TRUNCATEALL;

CALL GetNearbyStores(-87.6770450, 41.9631174, 500, NULL);

SELECT @Uid;



SELECT * FROM store_users;
SELECT * FROM customer_users;
SELECT * FROM users;
SELECT * FROM user_credentials;
SELECT * FROM users_foods;


SELECT * FROM store_users s INNER JOIN users u ON s.Uid = u.Uid WHERE (select ST_Distance_Sphere(point(-87.6770450, 41.9631174), u.Location) <= 0.00000001);


