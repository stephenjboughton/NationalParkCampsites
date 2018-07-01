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
INSERT INTO park (park_id, name, location, establish_date, area,visitors, description ) VALUES (1,'Sample Park', 'Nowhere', '1900/01/01', 5000, 10000, 'A Park.');
SET IDENTITY_INSERT park OFF;

-- Insert NEW CAMPGROUND
SET IDENTITY_INSERT campground ON;
INSERT INTO campground (campground_id, park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 1, 'Test Campground', 1, 12, 35.00);
SET IDENTITY_INSERT campground OFF;


-- Insert SITE
SET IDENTITY_INSERT site ON;
INSERT INTO site (site_id, campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUES (1, 1, 1, 6, 1, 20, 1);
INSERT INTO site (site_id, campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUES (2, 1, 2, 6, 1, 0, 0);
INSERT INTO site (site_id, campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUES (3, 1, 3, 4, 1, 20, 1);
SET IDENTITY_INSERT site OFF;

---- Insert RESERVATIONS

SET IDENTITY_INSERT reservation ON;
INSERT INTO reservation (reservation_id, site_id, name, from_date, to_date, create_date) VALUES (1, 1, 'Test Reservation', '2018/06/01', '2018/06/07', '2018/03/15');
INSERT INTO reservation (reservation_id, site_id, name, from_date, to_date, create_date) VALUES (2, 1, 'Test Reservation Two', '2018/06/15', '2018/06/21', '2018/04/12');
INSERT INTO reservation (reservation_id, site_id, name, from_date, to_date, create_date) VALUES (3, 2, 'Test Reservation Three', '2018/06/01', '2018/06/05', '2018/03/15');
INSERT INTO reservation (reservation_id, site_id, name, from_date, to_date, create_date) VALUES (4, 2, 'Test Reservation Four', '2018/06/15', '2018/06/21', '2018/04/12');
INSERT INTO reservation (reservation_id, site_id, name, from_date, to_date, create_date) VALUES (5, 3, 'Test Reservation Four', '2018/05/01', '2018/09/01', '2018/04/12');
SET IDENTITY_INSERT reservation OFF;