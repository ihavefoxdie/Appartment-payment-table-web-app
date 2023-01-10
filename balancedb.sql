CREATE DATABASE  IF NOT EXISTS `balancedb` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `balancedb`;
-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: balancedb
-- ------------------------------------------------------
-- Server version	8.0.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `apartvalues`
--

DROP TABLE IF EXISTS `apartvalues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `apartvalues` (
  `fk_id_month` int DEFAULT NULL,
  `additional` decimal(10,0) DEFAULT NULL,
  `paid` decimal(10,0) DEFAULT NULL,
  `remaining` decimal(10,0) DEFAULT NULL,
  `fk_id_apartment` int unsigned NOT NULL,
  `year` int unsigned NOT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `fk_id_apartment_idx` (`fk_id_apartment`),
  KEY `fk_id_month_idx` (`fk_id_month`),
  CONSTRAINT `fk_id_apartment` FOREIGN KEY (`fk_id_apartment`) REFERENCES `turnoversheet` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `apartvalues`
--

LOCK TABLES `apartvalues` WRITE;
/*!40000 ALTER TABLE `apartvalues` DISABLE KEYS */;
INSERT INTO `apartvalues` VALUES (1,4000,2000,6500,2,2020,2),(1,4000,0,5000,12,2022,7),(2,4000,2000,8500,2,2020,25),(5,5000,10000,-2000,30,2023,26),(6,4000,100,1900,30,2023,27),(7,3000,2500,1500,28,2020,28),(8,1000,790,1710,28,2020,29),(3,4000,771,11729,2,2020,30),(4,4000,2999,12730,2,2020,31),(5,3000,8999,6731,2,2020,32),(6,3000,8999,732,2,2020,33),(7,3000,8999,-5267,2,2020,34),(8,3000,8999,-11266,2,2020,35),(9,4000,3000,-10266,2,2020,36),(10,1000,5000,-14266,2,2020,37),(11,3256,1705,-12715,2,2020,38),(12,10000,1000,-3715,2,2020,39),(1,10000,5000,1285,2,2021,40),(2,566,123,1728,2,2021,41);
/*!40000 ALTER TABLE `apartvalues` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `months`
--

DROP TABLE IF EXISTS `months`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `months` (
  `id_month` int unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id_month`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `months`
--

LOCK TABLES `months` WRITE;
/*!40000 ALTER TABLE `months` DISABLE KEYS */;
INSERT INTO `months` VALUES (4,'Apr'),(8,'Aug'),(12,'Dec'),(2,'Feb'),(1,'Jan'),(7,'July'),(6,'June'),(3,'Mar'),(5,'May'),(11,'Nov'),(10,'Oct'),(9,'Sept');
/*!40000 ALTER TABLE `months` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `turnoversheet`
--

DROP TABLE IF EXISTS `turnoversheet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `turnoversheet` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `inSaldo` decimal(10,0) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=214 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `turnoversheet`
--

LOCK TABLES `turnoversheet` WRITE;
/*!40000 ALTER TABLE `turnoversheet` DISABLE KEYS */;
INSERT INTO `turnoversheet` VALUES (2,4500),(12,1000),(28,1000),(30,3000);
/*!40000 ALTER TABLE `turnoversheet` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-01-10 15:48:59
