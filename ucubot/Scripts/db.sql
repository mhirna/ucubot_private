CREATE DATABASE ucubot;
CREATE USER 'mary'@'%' IDENTIFIED BY 'pass';
GRANT ALL PRIVILEGES ON ucubot.* To 'mary'@'%';
FLUSH PRIVILEGES;
