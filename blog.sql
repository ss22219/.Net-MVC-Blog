-- phpMyAdmin SQL Dump
-- version 3.5.2
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 08 月 18 日 12:42
-- 服务器版本: 5.5.25a
-- PHP 版本: 5.4.4

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `blog`
--

-- --------------------------------------------------------

--
-- 表的结构 `article`
--

CREATE TABLE IF NOT EXISTS `article` (
  `ArticleId` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) DEFAULT NULL,
  `Content` varchar(255) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `ModifyDate` datetime DEFAULT NULL,
  `CommentCount` int(11) DEFAULT NULL,
  `Browse` int(11) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ArticleId`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB  DEFAULT CHARSET=UTF8 AUTO_INCREMENT=3 ;

--
-- 转存表中的数据 `article`
--

INSERT INTO `article` (`ArticleId`, `Title`, `Content`, `CreateDate`, `ModifyDate`, `CommentCount`, `Browse`, `Type`, `Status`, `UserId`) VALUES
(2, '???', '???', '2013-08-18 00:00:00', '2013-08-18 00:00:00', 1, 1, 0, 1, NULL);

-- --------------------------------------------------------

--
-- 表的结构 `articleextend`
--

CREATE TABLE IF NOT EXISTS `articleextend` (
  `ArticleExtendId` int(11) NOT NULL AUTO_INCREMENT,
  `ExtendKey` varchar(255) DEFAULT NULL,
  `ExtendValue` varchar(255) DEFAULT NULL,
  `ArticleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ArticleExtendId`),
  KEY `ArticleId` (`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `attach`
--

CREATE TABLE IF NOT EXISTS `attach` (
  `AttachId` int(11) NOT NULL AUTO_INCREMENT,
  `ArticleId` int(11) DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Path` varchar(255) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  PRIMARY KEY (`AttachId`),
  KEY `ArticleId` (`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `category`
--

CREATE TABLE IF NOT EXISTS `category` (
  `CategoryId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `Count` int(11) DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  PRIMARY KEY (`CategoryId`),
  KEY `ParentId` (`ParentId`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `categoryrelationship`
--

CREATE TABLE IF NOT EXISTS `categoryrelationship` (
  `RelationShipId` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryId` int(11) DEFAULT NULL,
  `ArticleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`RelationShipId`),
  KEY `CategoryId` (`CategoryId`),
  KEY `ArticleId` (`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `comment`
--

CREATE TABLE IF NOT EXISTS `comment` (
  `CommentId` int(11) NOT NULL AUTO_INCREMENT,
  `Content` varchar(255) DEFAULT NULL,
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
  KEY `ParentId` (`ParentId`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `setting`
--

CREATE TABLE IF NOT EXISTS `setting` (
  `SettingId` int(11) NOT NULL AUTO_INCREMENT,
  `SettingKey` varchar(255) DEFAULT NULL,
  `SettingValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`SettingId`)
) ENGINE=InnoDB  DEFAULT CHARSET=UTF8 AUTO_INCREMENT=35 ;

--
-- 转存表中的数据 `setting`
--

INSERT INTO `setting` (`SettingId`, `SettingKey`, `SettingValue`) VALUES
(18, 'BlogTitle', '????'),
(19, 'AllRegister', '1'),
(20, 'DefaultRole', '1'),
(21, 'WeekStart', '1'),
(22, 'BlogDescription', '??????????????????'),
(23, 'WebSite', 'GOOL.COM.CN'),
(24, 'ArticlePageSize', '10'),
(25, 'LastArticleCount', '10'),
(26, 'LastCommentCount', '5'),
(27, 'CloseComment', '0'),
(28, 'CommentSatus', '1'),
(29, 'NoLoginComment', '1'),
(30, 'MaxReComment', '5'),
(31, 'CommentPageSize', '5'),
(32, 'AdminArticlePageSize', '10'),
(33, 'AdminCommentPageSize', '10'),
(34, 'AdminUserPageSize', '10');

-- --------------------------------------------------------

--
-- 表的结构 `user`
--

CREATE TABLE IF NOT EXISTS `user` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `NiceName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `RegisterDate` datetime DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `Role` int(11) DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB  DEFAULT CHARSET=UTF8 AUTO_INCREMENT=2 ;

--
-- 转存表中的数据 `user`
--

INSERT INTO `user` (`UserId`, `UserName`, `Password`, `NiceName`, `Email`, `RegisterDate`, `Status`, `Role`) VALUES
(1, 'ss22219', '303384755', '??', 'ss22219@qq.com', '2013-08-18 00:00:00', 1, 2);

-- --------------------------------------------------------

--
-- 表的结构 `userextend`
--

CREATE TABLE IF NOT EXISTS `userextend` (
  `UserExtendId` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) DEFAULT NULL,
  `ExtendKey` varchar(255) DEFAULT NULL,
  `ExtendValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`UserExtendId`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB  DEFAULT CHARSET=UTF8 AUTO_INCREMENT=2 ;

--
-- 转存表中的数据 `userextend`
--

INSERT INTO `userextend` (`UserExtendId`, `UserId`, `ExtendKey`, `ExtendValue`) VALUES
(1, 1, 'Cover', 'http://img.baidu.com/img/iknow/avarta/66/r6s1g1.gif');

--
-- 限制导出的表
--

--
-- 限制表 `article`
--
ALTER TABLE `article`
  ADD CONSTRAINT `FK873F04FBE7840C6E` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`);

--
-- 限制表 `articleextend`
--
ALTER TABLE `articleextend`
  ADD CONSTRAINT `FK705E406EB4128634` FOREIGN KEY (`ArticleId`) REFERENCES `article` (`ArticleId`);

--
-- 限制表 `attach`
--
ALTER TABLE `attach`
  ADD CONSTRAINT `FK33C9C03EB4128634` FOREIGN KEY (`ArticleId`) REFERENCES `article` (`ArticleId`);

--
-- 限制表 `category`
--
ALTER TABLE `category`
  ADD CONSTRAINT `FK6482F241E5F0AE1` FOREIGN KEY (`ParentId`) REFERENCES `category` (`CategoryId`);

--
-- 限制表 `categoryrelationship`
--
ALTER TABLE `categoryrelationship`
  ADD CONSTRAINT `FK24112662B4128634` FOREIGN KEY (`ArticleId`) REFERENCES `article` (`ArticleId`),
  ADD CONSTRAINT `FK24112662A6C14A26` FOREIGN KEY (`CategoryId`) REFERENCES `category` (`CategoryId`);

--
-- 限制表 `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `FKE2466703F002AFC9` FOREIGN KEY (`ParentId`) REFERENCES `comment` (`CommentId`),
  ADD CONSTRAINT `FKE2466703B4128634` FOREIGN KEY (`ArticleId`) REFERENCES `article` (`ArticleId`),
  ADD CONSTRAINT `FKE2466703E7840C6E` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`);

--
-- 限制表 `userextend`
--
ALTER TABLE `userextend`
  ADD CONSTRAINT `FK41993EB172A82051` FOREIGN KEY (`UserExtendId`) REFERENCES `user` (`UserId`),
  ADD CONSTRAINT `FK41993EB1E7840C6E` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
