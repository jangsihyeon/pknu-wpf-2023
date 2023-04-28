SELECT Id,
    Dev_id,
    Name,
    Loc,
    Coordx,
    Coordy,
    Ison,
    Pm10_after,
    Pm25_after,
    State,
    Timestamp,
    Company_id,
    Company_name
FROM dustsensor
WHERE DATE_FORMAT (Timestamp, '%Y-%m-%d') = '2023-04-26';

SELECT date_format(Timestamp, '%Y-%m-%d') AS Save_date
From dustsensor
                                              group by 1
                                              order by 1;

CREATE TABLE `dustsensor` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dev_id` varchar(20) DEFAULT NULL,
  `Name` varchar(20) CHARACTER SET DEFAULT NULL,
  `Loc` varchar(100) CHARACTER SETDEFAULT NULL,
  `Coordx` double DEFAULT NULL,
  `Coordy` double DEFAULT NULL,
  `Ison` bit(1) DEFAULT NULL,
  `Pm10_after` int DEFAULT NULL,
  `Pm25_after` int DEFAULT NULL,
  `State` int DEFAULT NULL,
  `Timestamp` datetime DEFAULT NULL,
  `Company_id` varchar(100) DEFAULT NULL,
  `Company_name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`);


