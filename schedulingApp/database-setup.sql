-- Create database
CREATE DATABASE IF NOT EXISTS schedulingdb;
USE schedulingdb;

-- Create country table
CREATE TABLE IF NOT EXISTS country (
    countryId INT(10) PRIMARY KEY AUTO_INCREMENT,
    country VARCHAR(50) NOT NULL,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL
);

-- Create city table
CREATE TABLE IF NOT EXISTS city (
    cityId INT(10) PRIMARY KEY AUTO_INCREMENT,
    city VARCHAR(50) NOT NULL,
    countryId INT(10) NOT NULL,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL,
    FOREIGN KEY (countryId) REFERENCES country(countryId)
);

-- Create address table
CREATE TABLE IF NOT EXISTS address (
    addressId INT(10) PRIMARY KEY AUTO_INCREMENT,
    address VARCHAR(50) NOT NULL,
    address2 VARCHAR(50),
    cityId INT(10) NOT NULL,
    postalCode VARCHAR(10) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL,
    FOREIGN KEY (cityId) REFERENCES city(cityId)
);

-- Create user table
CREATE TABLE IF NOT EXISTS user (
    userId INT PRIMARY KEY AUTO_INCREMENT,
    userName VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL,
    active TINYINT NOT NULL DEFAULT 1,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL
);

-- Create customer table
CREATE TABLE IF NOT EXISTS customer (
    customerId INT(10) PRIMARY KEY AUTO_INCREMENT,
    customerName VARCHAR(45) NOT NULL,
    addressId INT(10) NOT NULL,
    active TINYINT(1) NOT NULL DEFAULT 1,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL,
    FOREIGN KEY (addressId) REFERENCES address(addressId)
);

-- Create appointment table
CREATE TABLE IF NOT EXISTS appointment (
    appointmentId INT(10) PRIMARY KEY AUTO_INCREMENT,
    customerId INT(10) NOT NULL,
    userId INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    location TEXT,
    contact TEXT,
    type TEXT,
    url VARCHAR(255),
    start DATETIME NOT NULL,
    end DATETIME NOT NULL,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(40) NOT NULL,
    lastUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    lastUpdateBy VARCHAR(40) NOT NULL,
    FOREIGN KEY (customerId) REFERENCES customer(customerId),
    FOREIGN KEY (userId) REFERENCES user(userId)
);

-- Create login_logs table for tracking login attempts
CREATE TABLE IF NOT EXISTS login_logs (
    logId INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) NOT NULL,
    login_timestamp DATETIME NOT NULL,
    success BOOLEAN NOT NULL
);

-- Insert initial admin user
INSERT INTO user (userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy)
VALUES ('admin', 'admin', 1, NOW(), 'SYSTEM', NOW(), 'SYSTEM');
