-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.21 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for demo
CREATE DATABASE IF NOT EXISTS 'demo' /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE 'demo';

-- Dumping structure for table demo.log
CREATE TABLE IF NOT EXISTS 'log' (
  'ID' int NOT NULL AUTO_INCREMENT,
  'msg' varchar(200) DEFAULT NULL,
  'Date' varchar(50) DEFAULT NULL,
  PRIMARY KEY ('ID')
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.product
CREATE TABLE IF NOT EXISTS 'product' (
  'PLU' varchar(15) NOT NULL,
  'Desc' varchar(50) DEFAULT NULL,
  'Price' float DEFAULT NULL,
  'ALLFRA' bit(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Test table used for an idea';

-- Data exporting was unselected.

-- Dumping structure for table demo.salelines
CREATE TABLE IF NOT EXISTS 'salelines' (
  'PLU' varchar(50) NOT NULL,
  'Value' double DEFAULT NULL,
  'CartID' bigint DEFAULT NULL,
  'Date' date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.sales
CREATE TABLE IF NOT EXISTS 'sales' (
  'CartID' bigint NOT NULL,
  'value' double NOT NULL,
  'Date' date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

-- Dumping structure for table demo.tender
CREATE TABLE IF NOT EXISTS 'tender' (
  'Type' varchar(50) DEFAULT NULL,
  'Value' double DEFAULT NULL,
  'CartId' bigint DEFAULT NULL,
    'Date' date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
