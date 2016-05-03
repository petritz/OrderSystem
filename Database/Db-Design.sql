-- --------------------------------------------------------
-- Host:                         localhost
-- Server Version:               10.1.13-MariaDB - mariadb.org binary distribution
-- Server Betriebssystem:        Win32
-- HeidiSQL Version:             9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Exportiere Struktur von Tabelle pd_order.credit
DROP TABLE IF EXISTS `credit`;
CREATE TABLE IF NOT EXISTS `credit` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user` int(10) unsigned NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `user` (`user`),
  CONSTRAINT `credit_ibfk_1` FOREIGN KEY (`user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Exportiere Daten aus Tabelle pd_order.credit: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `credit` DISABLE KEYS */;
/*!40000 ALTER TABLE `credit` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle pd_order.food_order
DROP TABLE IF EXISTS `food_order`;
CREATE TABLE IF NOT EXISTS `food_order` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `time` datetime NOT NULL,
  `created` datetime NOT NULL,
  `admin` int(10) unsigned NOT NULL,
  `closed` tinyint(4) NOT NULL,
  `closed_time` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `admin` (`admin`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Exportiere Daten aus Tabelle pd_order.food_order: ~3 rows (ungefähr)
/*!40000 ALTER TABLE `food_order` DISABLE KEYS */;
INSERT INTO `food_order` (`id`, `time`, `created`, `admin`, `closed`, `closed_time`) VALUES
	(1, '2016-05-03 12:25:00', '2016-05-01 20:11:23', 1, 0, '0000-00-00 00:00:00'),
	(2, '2016-05-04 11:35:00', '2016-05-02 20:22:59', 1, 0, '0000-00-00 00:00:00'),
	(3, '2016-05-05 13:15:00', '2016-05-02 20:33:41', 1, 0, '0000-00-00 00:00:00');
/*!40000 ALTER TABLE `food_order` ENABLE KEYS */;


-- Exportiere Struktur von View pd_order.food_orders
DROP VIEW IF EXISTS `food_orders`;
-- Erstelle temporäre Tabelle um View Abhängigkeiten zuvorzukommen
CREATE TABLE `food_orders` (
	`user` INT(11) UNSIGNED NOT NULL,
	`time` DATETIME NOT NULL,
	`amount` DECIMAL(30,0) NULL,
	`sum` DECIMAL(36,2) NULL
) ENGINE=MyISAM;


-- Exportiere Struktur von Tabelle pd_order.product
DROP TABLE IF EXISTS `product`;
CREATE TABLE IF NOT EXISTS `product` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `price_buy` decimal(6,2) NOT NULL,
  `price_sell` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `product_name_unique` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Exportiere Daten aus Tabelle pd_order.product: ~3 rows (ungefähr)
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` (`id`, `name`, `price_buy`, `price_sell`, `created`, `modified`) VALUES
	(2, 'Döner mild', 3.40, 3.50, '2016-05-01 19:19:49', '2016-05-01 20:32:41'),
	(3, 'Döner scharf', 3.40, 3.50, '2016-05-02 20:32:58', '2016-05-02 20:32:58'),
	(4, 'Pizza Salami', 8.40, 8.60, '2016-05-02 20:33:09', '2016-05-02 20:33:09');
/*!40000 ALTER TABLE `product` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle pd_order.product_line
DROP TABLE IF EXISTS `product_line`;
CREATE TABLE IF NOT EXISTS `product_line` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user` int(11) unsigned NOT NULL,
  `food_order` int(11) unsigned NOT NULL,
  `product` int(11) unsigned NOT NULL,
  `quantity` mediumint(8) unsigned NOT NULL,
  `added` datetime NOT NULL,
  `paid` tinyint(4) NOT NULL,
  `pay_type` enum('credit','admin') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'admin',
  PRIMARY KEY (`id`),
  KEY `user` (`user`),
  KEY `food_order` (`food_order`),
  KEY `product` (`product`),
  CONSTRAINT `product_line_ibfk_1` FOREIGN KEY (`user`) REFERENCES `user` (`id`),
  CONSTRAINT `product_line_ibfk_2` FOREIGN KEY (`food_order`) REFERENCES `food_order` (`id`),
  CONSTRAINT `product_line_ibfk_3` FOREIGN KEY (`product`) REFERENCES `product` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Exportiere Daten aus Tabelle pd_order.product_line: ~5 rows (ungefähr)
/*!40000 ALTER TABLE `product_line` DISABLE KEYS */;
INSERT INTO `product_line` (`id`, `user`, `food_order`, `product`, `quantity`, `added`, `paid`, `pay_type`) VALUES
	(4, 1, 1, 2, 1, '2016-05-02 19:06:35', 0, 'admin'),
	(5, 1, 2, 2, 1, '2016-05-02 20:27:14', 0, 'admin'),
	(6, 1, 2, 2, 3, '2016-05-02 20:27:15', 0, 'admin'),
	(7, 1, 3, 4, 1, '2016-05-02 20:34:05', 0, 'admin'),
	(8, 1, 3, 3, 3, '2016-05-02 20:34:05', 0, 'admin');
/*!40000 ALTER TABLE `product_line` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle pd_order.user
DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `email` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `firstname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `lastname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(60) COLLATE utf8_unicode_ci NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL,
  `ip` varchar(15) COLLATE utf8_unicode_ci NOT NULL,
  `last_login` datetime NOT NULL,
  `admin` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `user_email_unique` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Exportiere Daten aus Tabelle pd_order.user: ~0 rows (ungefähr)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`id`, `email`, `firstname`, `lastname`, `password`, `created`, `modified`, `ip`, `last_login`, `admin`) VALUES
	(1, 'petritzdesigns@gmail.com', 'Markus', 'Petritz', 'A35150EF15CB9E333F47DB398F398E63', '2016-03-03 09:00:23', '2016-05-01 20:35:11', '80.120.208.218', '2016-04-26 13:20:31', 1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


-- Exportiere Struktur von Trigger pd_order.credit_created
DROP TRIGGER IF EXISTS `credit_created`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `credit_created` BEFORE INSERT ON `credit` FOR EACH ROW BEGIN
SET new.created = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.food_order_created
DROP TRIGGER IF EXISTS `food_order_created`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `food_order_created` BEFORE INSERT ON `food_order` FOR EACH ROW BEGIN
SET new.created = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.product_created
DROP TRIGGER IF EXISTS `product_created`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `product_created` BEFORE INSERT ON `product` FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.product_line_added
DROP TRIGGER IF EXISTS `product_line_added`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `product_line_added` BEFORE INSERT ON `product_line` FOR EACH ROW BEGIN
SET new.added = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.product_modified
DROP TRIGGER IF EXISTS `product_modified`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `product_modified` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
SET new.modified = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.user_created
DROP TRIGGER IF EXISTS `user_created`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `user_created` BEFORE INSERT ON `user` FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von Trigger pd_order.user_modified
DROP TRIGGER IF EXISTS `user_modified`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `user_modified` BEFORE UPDATE ON `user` FOR EACH ROW BEGIN
SET new.modified = NOW();
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;


-- Exportiere Struktur von View pd_order.food_orders
DROP VIEW IF EXISTS `food_orders`;
-- Entferne temporäre Tabelle und erstelle die eigentliche View
DROP TABLE IF EXISTS `food_orders`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `food_orders` AS SELECT l.user, f.time, SUM(l.quantity) "amount", SUM(l.quantity * p.price_sell) "sum"
FROM product_line l
	INNER JOIN product p ON l.product = p.id
	INNER JOIN food_order f ON l.food_order = f.id
GROUP BY l.food_order ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
