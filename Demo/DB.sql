-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.21 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             12.0.0.6468
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for demo
CREATE DATABASE IF NOT EXISTS `demo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `demo`;

-- Dumping structure for table demo.currency
CREATE TABLE IF NOT EXISTS `currency` (
  `Currency` varchar(5) NOT NULL,
  `Rate` float NOT NULL DEFAULT '0',
  `UpdatedTime` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Currency`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.log
CREATE TABLE IF NOT EXISTS `log` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `msg` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Date` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3341 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.product
CREATE TABLE IF NOT EXISTS `product` (
  `PLU` varchar(15) NOT NULL,
  `Desc` varchar(50) DEFAULT NULL,
  `Price` float DEFAULT NULL,
  `ALLFRA` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Test table used for an idea';

-- Data exporting was unselected.

-- Dumping structure for table demo.salelines
CREATE TABLE IF NOT EXISTS `salelines` (
  `PLU` varchar(50) NOT NULL,
  `Value` double DEFAULT NULL,
  `CartID` bigint DEFAULT NULL,
  `Sent` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.sales
CREATE TABLE IF NOT EXISTS `sales` (
  `CartID` bigint NOT NULL,
  `value` double NOT NULL,
  `Date` date NOT NULL,
  `Sent` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `setting` varchar(50) DEFAULT NULL,
  `data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Type` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.stocklvl
CREATE TABLE IF NOT EXISTS `stocklvl` (
  `PLU` varchar(50) DEFAULT NULL,
  `QTY` smallint DEFAULT '0',
  `lastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.tender
CREATE TABLE IF NOT EXISTS `tender` (
  `Type` varchar(50) DEFAULT NULL,
  `Value` double DEFAULT NULL,
  `CartId` bigint DEFAULT NULL,
  `Date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.voucher
CREATE TABLE IF NOT EXISTS `voucher` (
  `Number` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Balance` double DEFAULT NULL,
  PRIMARY KEY (`Number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.


-- Dumping database structure for masterdb
CREATE DATABASE IF NOT EXISTS `masterdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `masterdb`;

-- Dumping structure for table masterdb.product
CREATE TABLE IF NOT EXISTS `product` (
  `PLU` varchar(15) NOT NULL,
  `Desc` varchar(50) DEFAULT NULL,
  `Price` float DEFAULT NULL,
  `ALLFRA` bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Test table used for an idea';

-- Data exporting was unselected.

-- Dumping structure for table masterdb.salelines
CREATE TABLE IF NOT EXISTS `salelines` (
  `PLU` varchar(50) NOT NULL,
  `Value` double DEFAULT NULL,
  `CartID` bigint DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table masterdb.sales
CREATE TABLE IF NOT EXISTS `sales` (
  `CartID` bigint NOT NULL,
  `value` double NOT NULL,
  `Date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table masterdb.stockbck
CREATE TABLE IF NOT EXISTS `stockbck` (
  `PLU` varchar(50) DEFAULT NULL,
  `QTY` smallint DEFAULT '0',
  `lastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table masterdb.stocklvl
CREATE TABLE IF NOT EXISTS `stocklvl` (
  `PLU` varchar(50) DEFAULT NULL,
  `QTY` smallint DEFAULT '0',
  `lastChanged` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table masterdb.tender
CREATE TABLE IF NOT EXISTS `tender` (
  `Type` varchar(50) DEFAULT NULL,
  `Value` double DEFAULT NULL,
  `CartId` bigint DEFAULT NULL,
  `Date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
