USE MERCADOYA;
CALL InsertStoreUser('','','','','','','','',-87.6770458,41.9631174,'',@Uid);
CALL InsertCustomerUser('','','','','','','','',1,1,'',@Uid);
CALL CreateRandomEntities(1000);
CALL TRUNCATEALL;

CALL GetNearbyStores(-87.6770450, 41.9631174, 500, NULL);

SELECT @Uid;

DROP FUNCTION IF EXISTS DayFromString;
CREATE FUNCTION DayFromInt(s INT) RETURNS VARCHAR(10)
RETURN elt(s+1, 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');

SELECT DayFromInt(1);

CREATE FUNCTION DayFromString(s VARCHAR(10)) RETURNS INT
RETURN CASE 
WHEN s = 'Sunday' THEN 0
WHEN s = 'Monday' THEN 1
WHEN s = 'Tuesday' THEN 2
WHEN s = 'Wednesday' THEN 3
WHEN s = 'Thursday' THEN 4
WHEN s = 'Friday' THEN 5
WHEN s = 'Saturday' THEN 6
END;

CREATE FUNCTION RandomDayName() RETURNS VARCHAR(10)
RETURN DayFromInt(RandomNumber(0,7));




SELECT * FROM store_users;
SELECT * FROM customer_users;
SELECT * FROM users LIMIT 0, 10000;
SELECT * FROM user_credentials  LIMIT 0, 10000;
SELECT * FROM users_foods;
SELECT * FROM users_tags;

SELECT COUNT(*) FROM users;


SELECT * FROM store_users s INNER JOIN users u ON s.Uid = u.Uid WHERE (select ST_Distance_Sphere(point(-87.6770450, 41.9631174), u.Location) <= 0.00000001);



SELECT RandomNumber(0,100000000);

CALL TRUNCATEALL;

CALL InsertFood('Name', 'Description', 'Region', @Id);

SELECT * FROM tags;
SELECT * FROM tag_foods;
SELECT * FROM users_tags;
SELECT * FROM user_credentials;

CALL AddUserFoodRelation(1, '50579a10-5f49-11ea-a535-d050997c9b7d');





CALL GetUserFoods('439a24db-5f4b-11ea-a535-d050997c9b7d');


-- -87.6770450, 41.9631174, 500
DECLARE StartingPoint POINT;
SET StartingPoint = POINT(-87.6770450,41.9631174);
SELECT u.Uid, u.DisplayableName, Direction, City, ProfilePic, CreatedAt, Phone, ST_X(u.Location) AS Longitude, ST_Y(u.Location) as Latitude,
(SELECT OpensAt, ClosesAt FROM store_regular_schedule WHERE StoreUid = u.Uid AND DAYNAME(NOW()) = srs.DayOfWeek)
FROM store_users s INNER JOIN users u ON s.Uid = u.Uid
WHERE (select ST_Distance_Sphere(StartingPoint, u.Location) <= 500) 







