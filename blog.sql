-- MySQL dump 10.13  Distrib 5.1.69, for redhat-linux-gnu (x86_64)
--
-- Host: localhost    Database: blog
-- ------------------------------------------------------
-- Server version	5.5.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Article`
--

DROP TABLE IF EXISTS `Article`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Article` (
  `ArticleId` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) DEFAULT NULL,
  `Content` text DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `ModifyDate` datetime DEFAULT NULL,
  `CommentCount` int(11) DEFAULT NULL,
  `Browse` int(11) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ArticleId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `FK379164D66426CB5F` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Article`
--

LOCK TABLES `Article` WRITE;
/*!40000 ALTER TABLE `Article` DISABLE KEYS */;
INSERT INTO `Article` VALUES (5,'Hello World!','<h2><strong>This is default article,you can delte it.</strong></h2>\n<h2><strong>have a good time!<img src=\"/Scripts/tinymce/plugins/emoticons/img/smiley-laughing.gif\" alt=\"\" /></strong></h2>','2013-10-15 17:47:45','2013-10-15 17:47:45',1,5,0,1,2);
/*!40000 ALTER TABLE `Article` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ArticleExtend`
--

DROP TABLE IF EXISTS `ArticleExtend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ArticleExtend` (
  `ArticleExtendId` int(11) NOT NULL AUTO_INCREMENT,
  `ExtendKey` varchar(255) DEFAULT NULL,
  `ExtendValue` varchar(255) DEFAULT NULL,
  `ArticleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ArticleExtendId`),
  KEY `ArticleId` (`ArticleId`),
  KEY `ArticleId_2` (`ArticleId`),
  CONSTRAINT `FK2AFB97906C15FD71` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ArticleExtend`
--

LOCK TABLES `ArticleExtend` WRITE;
/*!40000 ALTER TABLE `ArticleExtend` DISABLE KEYS */;
/*!40000 ALTER TABLE `ArticleExtend` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Attach`
--

DROP TABLE IF EXISTS `Attach`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Attach` (
  `AttachId` int(11) NOT NULL AUTO_INCREMENT,
  `ArticleId` int(11) DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Path` varchar(255) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  PRIMARY KEY (`AttachId`),
  KEY `ArticleId` (`ArticleId`),
  KEY `ArticleId_2` (`ArticleId`),
  CONSTRAINT `FK7583DA656C15FD71` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Attach`
--

LOCK TABLES `Attach` WRITE;
/*!40000 ALTER TABLE `Attach` DISABLE KEYS */;
/*!40000 ALTER TABLE `Attach` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Category`
--

DROP TABLE IF EXISTS `Category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Category` (
  `CategoryId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `Count` int(11) DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  PRIMARY KEY (`CategoryId`),
  KEY `ParentId` (`ParentId`),
  KEY `ParentId_2` (`ParentId`),
  CONSTRAINT `FK6DD211EEE8D096F` FOREIGN KEY (`ParentId`) REFERENCES `Category` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Category`
--

LOCK TABLES `Category` WRITE;
/*!40000 ALTER TABLE `Category` DISABLE KEYS */;
INSERT INTO `Category` VALUES (1,'default',0,1,NULL);
/*!40000 ALTER TABLE `Category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CategoryRelationShip`
--

DROP TABLE IF EXISTS `CategoryRelationShip`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CategoryRelationShip` (
  `RelationshipId` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryId` int(11) DEFAULT NULL,
  `ArticleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`RelationshipId`),
  KEY `CategoryId` (`CategoryId`),
  KEY `ArticleId` (`ArticleId`),
  KEY `CategoryId_2` (`CategoryId`),
  KEY `ArticleId_2` (`ArticleId`),
  CONSTRAINT `FK74AFA2966C15FD71` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`ArticleId`),
  CONSTRAINT `FK74AFA296E5C118B3` FOREIGN KEY (`CategoryId`) REFERENCES `Category` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CategoryRelationShip`
--

LOCK TABLES `CategoryRelationShip` WRITE;
/*!40000 ALTER TABLE `CategoryRelationShip` DISABLE KEYS */;
INSERT INTO `CategoryRelationShip` VALUES (3,1,5);
/*!40000 ALTER TABLE `CategoryRelationShip` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Comment`
--

DROP TABLE IF EXISTS `Comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Comment` (
  `CommentId` int(11) NOT NULL AUTO_INCREMENT,
  `Content` text DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Author` varchar(255) DEFAULT NULL,
  `AuthorMail` varchar(255) DEFAULT NULL,
  `AuthorIP` varchar(255) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `ArticleId` int(11) DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  PRIMARY KEY (`CommentId`),
  KEY `UserId` (`UserId`),
  KEY `ArticleId` (`ArticleId`),
  KEY `ParentId` (`ParentId`),
  KEY `UserId_2` (`UserId`),
  KEY `ArticleId_2` (`ArticleId`),
  KEY `ParentId_2` (`ParentId`),
  CONSTRAINT `FK9BDE863FF792384E` FOREIGN KEY (`ParentId`) REFERENCES `Comment` (`CommentId`),
  CONSTRAINT `FK9BDE863F6426CB5F` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`),
  CONSTRAINT `FK9BDE863F6C15FD71` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`ArticleId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Comment`
--

LOCK TABLES `Comment` WRITE;
/*!40000 ALTER TABLE `Comment` DISABLE KEYS */;
INSERT INTO `Comment` VALUES (1,'Hi,you a using NetBlog','2013-10-15 17:52:46','admin','admin@admin.com','127.0.0.1',1,2,5,NULL);
/*!40000 ALTER TABLE `Comment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Setting`
--

DROP TABLE IF EXISTS `Setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Setting` (
  `SettingId` int(11) NOT NULL AUTO_INCREMENT,
  `SettingKey` varchar(255) DEFAULT NULL,
  `SettingValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`SettingId`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Setting`
--

LOCK TABLES `Setting` WRITE;
/*!40000 ALTER TABLE `Setting` DISABLE KEYS */;
INSERT INTO `Setting` VALUES (18,'BlogTitle','Blue container'),(19,'AllRegister','1'),(20,'DefaultRole','1'),(21,'WeekStart','1'),(22,'BlogDescription','My time is losting...'),(23,'WebSite','GOOL.COM.CN'),(24,'ArticlePageSize','10'),(25,'LastArticleCount','10'),(26,'LastCommentCount','5'),(27,'CloseComment','0'),(28,'CommentSatus','1'),(29,'NoLoginComment','1'),(30,'MaxReComment','5'),(31,'CommentPageSize','5'),(32,'AdminArticlePageSize','10'),(33,'AdminCommentPageSize','10'),(34,'AdminUserPageSize','10');
/*!40000 ALTER TABLE `Setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `User` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `NiceName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `RegisterDate` datetime DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `Role` int(11) DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `User`
--

LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
INSERT INTO `User` VALUES (2,'admin','123456','admin','admin@admin.com','2013-10-15 17:43:03',0,2);
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserExtend`
--

DROP TABLE IF EXISTS `UserExtend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UserExtend` (
  `UserExtendId` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) DEFAULT NULL,
  `ExtendKey` varchar(255) DEFAULT NULL,
  `ExtendValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`UserExtendId`),
  KEY `UserId` (`UserId`),
  KEY `UserId_2` (`UserId`),
  CONSTRAINT `FKBE0D2EE56426CB5F` FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserExtend`
--

LOCK TABLES `UserExtend` WRITE;
/*!40000 ALTER TABLE `UserExtend` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserExtend` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-10-15 17:55:57
