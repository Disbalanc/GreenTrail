USE [spirkin]
GO

---- Создание таблицы 'pollution'
--CREATE TABLE Pollution (
--  id_pollution BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_contemplation BIGINT,
--  levels FLOAT,
--  id_region BIGINT,
--  data_time DATETIME
--);

---- Создание таблицы 'USERS'
--CREATE TABLE Users (
--  id_user BIGINT PRIMARY KEY IDENTITY(1,1),
--  full_name VARCHAR(50),
--  login VARCHAR(50),
--  password VARCHAR(MAX),
--  id_roles BIGINT,
--  dateOfBirth DATE,
--  phoneNumber VARCHAR(11),
--  email VARCHAR(50),
--  address VARCHAR(50)
--);

---- Создание таблицы 'event'
--CREATE TABLE Event (
--  id_event BIGINT PRIMARY KEY IDENTITY(1,1),
--  name VARCHAR(50),
--  data_time DATETIME,
--  id_region BIGINT
--);

---- Создание таблицы 'contemplation'
--CREATE TABLE Contemplation (
--  id_contemplation BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_sample BIGINT,
--  id_user BIGINT,
--  result VARCHAR(50),
--  id_norm BIGINT,
--  date_contemplation DATETIME
--);

---- Создание таблицы 'norm'
--CREATE TABLE Norm (
--  id_norm BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_type BIGINT,
--  name VARCHAR(50),
--  norma VARCHAR(50)
--);

---- Создание таблицы 'type'
--CREATE TABLE Type (
--  id_type BIGINT PRIMARY KEY IDENTITY(1,1),
--  name VARCHAR(50)
--);

---- Создание таблицы 'news'
--CREATE TABLE News (
--  id_news BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_user BIGINT,
--  heading VARCHAR(50),
--  text TEXT,
--  data_time DATETIME
--);

---- Создание таблицы 'sample'
--CREATE TABLE Sample (
--  id_sample BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_region BIGINT,
--  id_user BIGINT,
--  articul VARCHAR(50),
--  id_type BIGINT,
--  date_sample DATETIME
--);

---- Создание таблицы 'region'
--CREATE TABLE Region (
--  id_region BIGINT PRIMARY KEY IDENTITY(1,1),
--  name VARCHAR(50),
--  geographical_coordinates VARCHAR(100),
--  population INT
--);

---- Создание таблицы 'roles'
--CREATE TABLE Roles (
--  id_roles BIGINT PRIMARY KEY IDENTITY(1,1),
--  name VARCHAR(50)
--);

---- Создание таблицы 'ecological_recommendations'
--CREATE TABLE EcologicalRecommendations (
--  id_recommendations BIGINT PRIMARY KEY IDENTITY(1,1),
--  id_user BIGINT,
--  heading VARCHAR(50),
--  text TEXT
--);

---- Создание связи между таблицами 'pollution' и 'contemplation' 
--ALTER TABLE Pollution ADD FOREIGN KEY (id_contemplation) REFERENCES Contemplation(id_contemplation);

---- Создание связи между таблицами 'contemplation' и 'USERS' 
--ALTER TABLE Contemplation ADD FOREIGN KEY (id_user) REFERENCES Users(id_user);

---- Создание связи между таблицами 'norm' и 'type' 
--ALTER TABLE Norm ADD FOREIGN KEY (id_type) REFERENCES Type(id_type);

---- Создание связи между таблицами 'news' и 'USERS' 
--ALTER TABLE News ADD FOREIGN KEY (id_user) REFERENCES Users(id_user);

---- Создание связи между таблицами 'sample' и 'region' 
--ALTER TABLE Sample ADD FOREIGN KEY (id_region) REFERENCES Region(id_region);

---- Создание связи между таблицами 'sample' и 'type' 
--ALTER TABLE Sample ADD FOREIGN KEY (id_type) REFERENCES Type(id_type);

---- Создание связи между таблицами 'ecological_recommendations' и 'USERS' 
--ALTER TABLE EcologicalRecommendations ADD FOREIGN KEY (id_user) REFERENCES Users(id_user);

---- Создание связи между таблицами 'USERS' и 'roles' 
--ALTER TABLE Users ADD FOREIGN KEY (id_roles) REFERENCES Roles(id_roles);



---- Вставка тестовых данных в таблицу 'norm'
--INSERT INTO Norm (id_type, name, norma) VALUES
--  (1, 'Sulfur', '0,5'),
--  (2, 'hydrogen sulfide', '0,003');

---- Вставка тестовых данных в таблицу 'type'
--INSERT INTO Type (name) VALUES
--  ('Air'),
--  ('Water');

---- Вставка тестовых данных в таблицу 'region'
--INSERT INTO Region (name, geographical_coordinates, population) VALUES
--  ('California', '36.1162;119.6816', 39200000),
--  ('Texas', '31.0545;97.5635', 29100000);

---- Вставка тестовых данных в таблицу 'roles'
--INSERT INTO Roles (name) VALUES
--  ('Администратор'),
--  ('Эколог'),
--  ('Лаборант');

  -- Вставка тестовых данных в таблицу 'USERS'
INSERT INTO Users (full_name, login, password, id_roles, dateOfBirth, phoneNumber, email, address) VALUES
  ('Иван Иванов', 'ivan', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 1, '1990-01-01', '79999999999', 'ivan@example.com', 'Москва'),
  ('Петр Петров', 'petr', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 2, '1995-05-05', '79998888888', 'petr@example.com', 'Санкт-Петербург'),
  ('Сергей Сергеев', 'ergey', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 3, '1980-10-10', '79997777777', 'ergey@example.com', 'Москва');
-- Пароль: password

-- Вставка тестовых данных в таблицу 'ecological_recommendations'
INSERT INTO EcologicalRecommendations (id_user, heading, text) VALUES
  (1, 'Reduce air pollution', 'Use public transportation or walk/bike instead of driving...'),
  (2, 'Conserve water', 'Take shorter showers, fix leaks, water your lawn efficiently...');

--  -- Вставка тестовых данных в таблицу 'sample'
--INSERT INTO Sample (id_region, id_user, articul, id_type, date_sample) VALUES
--  (1, 1, 'Air filter', 1, '2023-03-08 10:00:00'),
--  (2, 2, 'Water purifier', 2, '2023-03-09 12:00:00'),
--  (3, 1, 'Air filter', 1, '2023-03-08 10:00:00');

-- Вставка тестовых данных в таблицу 'contemplation'
INSERT INTO Contemplation (id_sample, id_user, result, id_norm, date_contemplation) VALUES
  (1, 1, '0,7', 1, '2023-03-08 10:00:00'),
  (4, 2, '0,002', 2, '2023-03-09 12:00:00');

-- Вставка тестовых данных в таблицу 'pollution'
INSERT INTO Pollution (id_contemplation, levels, id_region, data_time) VALUES
  (7, 0.2, 1, '2023-03-08 10:00:00');

-- Вставка тестовых данных в таблицу 'event'
INSERT INTO Event (name, data_time, id_region) VALUES
  ('Earthquake', '2023-03-08 11:00:00', 1),
  ('Flood', '2023-03-09 13:00:00', 2);

  -- Вставка тестовых данных в таблицу 'news'
INSERT INTO News (id_user, heading, text, data_time) VALUES
  (1, 'Earthquake in California', 'A strong earthquake hit California...', '2023-03-08 11:00:00'),
  (2, 'Flooding in Texas', 'Heavy rains caused widespread flooding in Texas...', '2023-03-09 13:00:00');