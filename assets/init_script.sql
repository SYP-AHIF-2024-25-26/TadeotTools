CREATE DATABASE IF NOT EXISTS tadeot;

USE tadeot;

CREATE TABLE IF NOT EXISTS StopGroups (
    stopGroupID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NOT NULL,
    color VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Stops (
    stopID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(50) NOT NULL,
    roomNr VARCHAR(5),
    stopGroupID INT,
    FOREIGN KEY (stopGroupID) REFERENCES StopGroups(stopGroupID)
);

CREATE TABLE IF NOT EXISTS StopStatistics (
    stopStatisticID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    time DATETIME NOT NULL,
    isDone TINYINT NOT NULL,
    stopID INT NOT NULL,
    FOREIGN KEY (stopID) REFERENCES Stops(stopID)
);

CREATE TABLE IF NOT EXISTS APIKeys(
    apiKeyValue VARCHAR(255) NOT NULL PRIMARY KEY
);

INSERT INTO StopGroups (name, description, color) VALUES
('Informatik', 'Informatik beste Abteilung', '#0059a7'),
('Medientechnik', 'Medientechnik super Beschreibung', '#70b4d9'),
('Elektronik', 'Super Beschreibung Elektronik', '#ce1223'),
('Medizintechnik', 'Super Beschreibung Medizintechnik', '#f1a102'),
('Neutral', 'Neutral wie die Schweiz', '#80c076');

INSERT INTO Stops (name, description, roomNr, stopGroupID) VALUES
('Roboterführerschein', 'Robos Besuch abstatten', '109', 1),
('Web Garden', 'Webanwendungen / Webprojekte', 'E72', 2),
('Hardware- und Software-Entwicklung', 'super beschreibung oder so', 'U10', 3),
('Check your health!', 'lest sich des je wer durch?', 'U04', 4),
('Buffet', 'Hoffentlich keine mica preise', 'U', 5);

INSERT INTO StopStatistics (time, isDone, stopID) VALUES
('2024-10-01 10:00:00', 1, 1),
('2024-10-01 11:00:00', 0, 2),
('2024-10-01 12:00:00', 1, 3),
('2024-10-02 09:30:00', 1, 4),
('2024-10-02 10:15:00', 0, 5);
