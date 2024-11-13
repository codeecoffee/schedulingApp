-- Log into MySQL as root first, then run these commands:

-- Create the user
CREATE USER 'sqlUser'@'localhost' IDENTIFIED BY 'Passw0rd!';

-- Grant all privileges on the database
GRANT ALL PRIVILEGES ON client_schedule.* TO 'sqlUser'@'localhost';

-- Grant create database permission (needed for initial setup)
GRANT CREATE ON *.* TO 'sqlUser'@'localhost';

-- Apply the privileges
FLUSH PRIVILEGES;

-- Verify the user was created
SELECT User, Host FROM mysql.user WHERE User = 'sqlUser';

-- Verify the privileges
SHOW GRANTS FOR 'sqlUser'@'localhost';
