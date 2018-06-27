-- Clean the Database
--UPDATE employee SET department_id = NULL;
--UPDATE project_employee SET project_id = NULL;

BEGIN TRANSACTION;

DELETE FROM reservation;
DELETE FROM site;
DELETE FROM campground;
DELETE FROM park;


COMMIT TRANSACTION;

-- Insert NEW Park
SET IDENTITY_INSERT park ON;
INSERT INTO park (park_id, name, location, establish_date, area, visitors, description) VALUES (1,'Sample Park', 'Anywhere', '1900/01/01', 5000, 10000, 'A Park.');
SET IDENTITY_INSERT park OFF;

---- Insert NEW CAMPGROUND
--SET IDENTITY_INSERT campground ON;
--INSERT INTO campground (campground_id, name) VALUES (1,'Test Campground');
--SET IDENTITY_INSERT campground OFF;


---- Insert SITE
--SET IDENTITY_INSERT site ON;
--INSERT INTO site (site_id, site_number) VALUES (1,2);
--INSERT INTO site (site_id, site_number) VALUES (2,2);
--SET IDENTITY_INSERT site OFF;

---- Insert RESERVATIONS

--SET IDENTITY_INSERT site ON;
--INSERT INTO reservation (reservation_id, site_id) VALUES (1,1);
--INSERT INTO reservation (reservation_id, site_id) VALUES (2,1);
--SET IDENTITY_INSERT site OFF;