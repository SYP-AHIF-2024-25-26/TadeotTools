CREATE DATABASE IF NOT EXISTS tadeot;

USE tadeot;

DROP TABLE IF EXISTS StopStatistics;
DROP TABLE IF EXISTS Stops;
DROP TABLE IF EXISTS Divisions;
DROP TABLE IF EXISTS StopGroups;
DROP TABLE IF EXISTS APIKeys;

CREATE TABLE IF NOT EXISTS StopGroups (
    stopGroupID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NOT NULL,
    isPublic BOOLEAN NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Divisions (
    divisionID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    color VARCHAR(7) NOT NULL
);

CREATE TABLE IF NOT EXISTS Stops (
    stopID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NOT NULL,
    roomNr VARCHAR(5),
    stopGroupID INT,
    divisionID INT NOT NULL,
    FOREIGN KEY (stopGroupID) REFERENCES StopGroups(stopGroupID) ON DELETE CASCADE,
    FOREIGN KEY (divisionID) REFERENCES Divisions(divisionID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS StopStatistics (
    stopStatisticID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    time DATETIME NOT NULL,
    isDone TINYINT NOT NULL,
    stopID INT NOT NULL,
    FOREIGN KEY (stopID) REFERENCES Stops(stopID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS APIKeys(
    apiKeyValue VARCHAR(255) NOT NULL PRIMARY KEY
);

INSERT INTO StopGroups (name, description) VALUES
    ('Kurzpräsentationen', 'Kurzer Überblick zur Schule'),
    ('Informatik Medientechnik Tour', 'Tour zu den Informatik und Medientechnik Abteilungen'),
    ('Elektronik Medizintechnik Tour', 'Tour zu den Elektronik und Medizintechnik Abteilungen'),
    ('Aula Highlights', 'Highlights in der Aula'),
    ('Beratungszentrum', 'Zu Ihrer persönlichen Beratung'),
    ('Voranmeldung', 'Voranmeldung für die Schule');

INSERT INTO Divisions (name, color) VALUES
    ('Informatik', '#0059a7'),
    ('Medientechnik', '#70b4d9'),
    ('Elektronik', '#ce1223'),
    ('Medizintechnik', '#f1a102'),
    ('Neutral', '#80c076');


INSERT INTO Stops (name, description, roomNr, stopGroupID, divisionID) VALUES
    ('Roboterführerschein', 'Robos Besuch abstatten', '109', 2, 1),
    ('Web Garden', 'Webanwendungen / Webprojekte', 'E72', 2, 2),
    ('Hardware- und Software-Entwicklung', 'In der Hard- und Software-Entwicklung lernt man über verschiedene Anwendungsfälle dieser', 'U10', 3, 3),
    ('Check your health!', 'Check your personal health', 'U04', 3, 4),
    ('Buffet', 'Hoffentlich keine mica preise', 'U', 4, 5);

INSERT INTO StopStatistics (time, isDone, stopID) VALUES
    ('2024-10-01 10:00:00', 1, 1),
    ('2024-10-01 11:00:00', 0, 2),
    ('2024-10-01 12:00:00', 1, 3),
    ('2024-10-02 09:30:00', 1, 4),
    ('2024-10-02 10:15:00', 0, 5);
