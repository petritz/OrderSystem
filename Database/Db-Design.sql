-- phpMyAdmin SQL Dump
-- version 4.4.10
-- http://www.phpmyadmin.net
--
-- Host: localhost:8889
-- Erstellungszeit: 14. Mai 2016 um 21:24
-- Server-Version: 5.5.42
-- PHP-Version: 5.6.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `pd_order`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `credit`
--

DROP TABLE IF EXISTS `credit`;
CREATE TABLE `credit` (
  `id` int(10) unsigned NOT NULL,
  `user` int(10) unsigned NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Trigger `credit`
--
DROP TRIGGER IF EXISTS `credit_created`;
DELIMITER $$
CREATE TRIGGER `credit_created` BEFORE INSERT ON `credit`
 FOR EACH ROW BEGIN
SET new.created = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `food_order`
--

DROP TABLE IF EXISTS `food_order`;
CREATE TABLE `food_order` (
  `id` int(10) unsigned NOT NULL,
  `time` datetime NOT NULL,
  `created` datetime NOT NULL,
  `admin` int(10) unsigned NOT NULL,
  `closed` tinyint(4) NOT NULL,
  `closed_time` datetime NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `food_order`
--

INSERT INTO `food_order` (`id`, `time`, `created`, `admin`, `closed`, `closed_time`) VALUES
(4, '2016-05-05 13:15:00', '2016-05-04 08:21:48', 1, 1, '2016-05-05 20:46:16'),
(5, '2016-05-06 12:25:00', '2016-05-04 08:38:50', 1, 1, '2016-05-14 16:27:00'),
(6, '2016-05-07 12:35:00', '2016-05-04 08:39:25', 1, 1, '2016-05-14 16:27:00'),
(7, '2016-05-08 12:25:00', '2016-05-04 08:39:43', 1, 1, '2016-05-14 16:27:01'),
(8, '2016-05-09 10:00:00', '2016-05-04 13:25:29', 1, 1, '2016-05-14 16:27:02'),
(9, '2016-05-18 14:15:00', '2016-05-07 11:15:24', 1, 0, '0000-00-00 00:00:00'),
(10, '2016-05-20 16:10:00', '2016-05-14 09:13:46', 1, 0, '0000-00-00 00:00:00'),
(11, '2016-05-18 10:10:00', '2016-05-14 13:52:11', 1, 0, '0000-00-00 00:00:00'),
(12, '2016-05-24 11:30:18', '2016-05-14 13:53:20', 1, 0, '0000-00-00 00:00:00'),
(13, '2016-05-25 11:30:00', '2016-05-14 13:53:52', 1, 0, '0000-00-00 00:00:00'),
(14, '2016-05-26 11:30:00', '2016-05-14 13:53:52', 1, 0, '0000-00-00 00:00:00'),
(15, '2016-05-27 11:30:00', '2016-05-14 13:53:52', 1, 0, '0000-00-00 00:00:00'),
(16, '2016-05-28 11:30:00', '2016-05-14 13:53:52', 1, 0, '0000-00-00 00:00:00'),
(17, '2016-05-29 11:30:00', '2016-05-14 13:53:52', 1, 0, '0000-00-00 00:00:00'),
(18, '2016-05-14 12:00:00', '2016-05-14 16:25:58', 1, 1, '2016-05-14 16:27:05'),
(19, '2016-05-30 12:00:00', '2016-05-14 16:26:54', 1, 0, '0000-00-00 00:00:00'),
(20, '2016-05-31 14:00:00', '2016-05-14 16:27:32', 1, 0, '0000-00-00 00:00:00');

--
-- Trigger `food_order`
--
DROP TRIGGER IF EXISTS `food_order_created`;
DELIMITER $$
CREATE TRIGGER `food_order_created` BEFORE INSERT ON `food_order`
 FOR EACH ROW BEGIN
SET new.created = NOW();
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `food_order_modified`;
DELIMITER $$
CREATE TRIGGER `food_order_modified` BEFORE UPDATE ON `food_order`
 FOR EACH ROW BEGIN
	IF NEW.closed = '1' THEN
    	SET NEW.closed_time = NOW();
    END IF;
    IF NEW.closed = '0' THEN
    	SET NEW.closed_time = '0000-00-00 00:00:00';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `food_orders`
--
DROP VIEW IF EXISTS `food_orders`;
CREATE TABLE `food_orders` (
`user` int(11) unsigned
,`order` int(11) unsigned
,`time` datetime
,`amount` decimal(32,0)
,`sum` decimal(38,2)
);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `product`
--

DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `id` int(10) unsigned NOT NULL,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `price_buy` decimal(6,2) NOT NULL,
  `price_sell` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `product`
--

INSERT INTO `product` (`id`, `name`, `price_buy`, `price_sell`, `created`, `modified`) VALUES
(5, 'Döner mild', '3.30', '3.40', '2016-05-04 08:22:49', '2016-05-14 16:36:32'),
(6, 'Döner scharf', '3.30', '3.40', '2016-05-04 08:23:00', '2016-05-14 16:36:35'),
(7, 'Pizza Salami', '7.50', '7.60', '2016-05-04 08:23:14', '2016-05-14 16:36:22'),
(8, 'Schinken-Stangerl', '6.30', '6.40', '2016-05-14 16:36:55', '2016-05-14 16:36:55'),
(9, 'Bauernpizza', '7.10', '7.20', '2016-05-14 16:37:12', '2016-05-14 16:37:12'),
(10, 'Grüner Salat', '3.20', '3.30', '2016-05-14 16:37:47', '2016-05-14 16:37:47'),
(11, 'Gemischter Salat', '4.20', '4.30', '2016-05-14 16:38:00', '2016-05-14 16:38:00'),
(12, 'Big Kebap', '3.80', '3.90', '2016-05-14 16:38:55', '2016-05-14 16:38:55'),
(13, 'Schnitzelsemmel', '3.40', '3.50', '2016-05-14 16:39:11', '2016-05-14 16:39:11'),
(14, 'Wienerschnitzel', '7.50', '7.60', '2016-05-14 16:39:21', '2016-05-14 16:39:21'),
(15, 'Portion Pommes frites (groß)', '2.70', '2.80', '2016-05-14 16:39:36', '2016-05-14 16:39:36'),
(16, 'Topfenstrudel', '2.90', '3.00', '2016-05-14 16:39:48', '2016-05-14 16:39:48'),
(17, 'Coca Cola 0,33l', '1.90', '2.00', '2016-05-14 16:39:59', '2016-05-14 16:39:59'),
(18, 'Puntigamer Bier 0,5l', '2.50', '2.70', '2016-05-14 16:40:11', '2016-05-14 16:40:11');

--
-- Trigger `product`
--
DROP TRIGGER IF EXISTS `after_insert_product`;
DELIMITER $$
CREATE TRIGGER `after_insert_product` AFTER INSERT ON `product`
 FOR EACH ROW BEGIN
	INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) 
		VALUES (NULL, NEW.id, 'price_sell', NEW.price_sell, NOW(), NULL);
	INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) 
		VALUES (NULL, NEW.id, 'price_buy', NEW.price_buy, NOW(), NULL); 
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `after_update_products`;
DELIMITER $$
CREATE TRIGGER `after_update_products` AFTER UPDATE ON `product`
 FOR EACH ROW BEGIN
        IF NEW.price_sell <> OLD.price_sell THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_sell' AND valid_to IS NULL;             INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_sell', NEW.price_sell, NOW(), NULL);         END IF;
        
        IF NEW.price_buy <> OLD.price_buy THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_buy' AND valid_to IS NULL;             INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_buy', NEW.price_buy, NOW(), NULL);         END IF;
    END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `product_created`;
DELIMITER $$
CREATE TRIGGER `product_created` BEFORE INSERT ON `product`
 FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `product_modified`;
DELIMITER $$
CREATE TRIGGER `product_modified` BEFORE UPDATE ON `product`
 FOR EACH ROW BEGIN

SET new.modified = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `product_line`
--

DROP TABLE IF EXISTS `product_line`;
CREATE TABLE `product_line` (
  `id` int(10) unsigned NOT NULL,
  `user` int(11) unsigned NOT NULL,
  `food_order` int(11) unsigned NOT NULL,
  `product` int(11) unsigned NOT NULL,
  `quantity` int(10) unsigned NOT NULL,
  `added` datetime NOT NULL,
  `paid` tinyint(4) NOT NULL,
  `pay_type` enum('credit','admin') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'admin',
  `status` enum('ok','cancelled','deleted') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'ok'
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `product_line`
--

INSERT INTO `product_line` (`id`, `user`, `food_order`, `product`, `quantity`, `added`, `paid`, `pay_type`, `status`) VALUES
(9, 1, 4, 7, 1, '2016-05-04 08:30:05', 0, 'admin', 'ok'),
(10, 1, 4, 5, 4, '2016-05-04 08:30:05', 0, 'admin', 'ok'),
(11, 1, 5, 7, 2, '2016-05-04 08:40:54', 0, 'admin', 'ok'),
(12, 1, 5, 6, 5, '2016-05-04 08:40:54', 0, 'admin', 'ok'),
(13, 1, 5, 5, 2, '2016-05-04 08:40:54', 0, 'admin', 'ok'),
(14, 1, 6, 5, 3, '2016-05-04 08:42:04', 0, 'admin', 'ok'),
(15, 1, 7, 5, 1, '2016-05-04 08:42:13', 0, 'admin', 'cancelled'),
(16, 1, 7, 6, 1, '2016-05-04 08:42:13', 0, 'admin', 'cancelled'),
(17, 1, 8, 7, 6, '2016-05-04 13:25:49', 0, 'admin', 'ok'),
(18, 1, 8, 5, 1, '2016-05-04 13:25:49', 0, 'admin', 'ok'),
(19, 1, 9, 5, 5, '2016-05-07 11:15:51', 0, 'admin', 'cancelled'),
(20, 1, 9, 6, 1, '2016-05-14 09:04:22', 0, 'admin', 'cancelled'),
(21, 1, 9, 6, 10, '2016-05-14 09:05:57', 0, 'admin', 'cancelled'),
(22, 1, 10, 6, 2, '2016-05-14 09:14:07', 0, 'admin', 'cancelled'),
(23, 1, 10, 7, 1, '2016-05-14 09:14:07', 0, 'admin', 'cancelled'),
(24, 1, 9, 7, 20, '2016-05-14 09:14:28', 0, 'admin', 'cancelled'),
(25, 2, 9, 6, 1, '2016-05-14 09:19:50', 0, 'admin', 'cancelled'),
(26, 2, 9, 7, 0, '2016-05-14 09:23:25', 0, 'admin', 'cancelled'),
(27, 2, 9, 5, 16777215, '2016-05-14 09:31:02', 0, 'admin', 'cancelled'),
(28, 2, 9, 6, 16777215, '2016-05-14 09:31:02', 0, 'admin', 'cancelled'),
(29, 2, 9, 7, 16777215, '2016-05-14 09:31:02', 0, 'admin', 'cancelled'),
(30, 2, 9, 5, 10, '2016-05-14 09:33:22', 0, 'admin', 'cancelled'),
(31, 2, 9, 6, 10, '2016-05-14 09:33:22', 0, 'admin', 'cancelled'),
(32, 2, 9, 7, 90, '2016-05-14 09:33:22', 0, 'admin', 'cancelled'),
(33, 2, 9, 5, 4294966528, '2016-05-14 09:44:45', 0, 'admin', 'cancelled'),
(34, 2, 9, 6, 2147483504, '2016-05-14 09:44:45', 0, 'admin', 'cancelled'),
(35, 2, 9, 7, 1717986496, '2016-05-14 09:44:45', 0, 'admin', 'cancelled'),
(36, 2, 10, 5, 10, '2016-05-14 09:50:49', 0, 'admin', 'cancelled'),
(37, 2, 10, 6, 3, '2016-05-14 09:50:49', 0, 'admin', 'cancelled'),
(38, 2, 10, 7, 1, '2016-05-14 09:50:49', 0, 'admin', 'cancelled'),
(39, 2, 9, 5, 10, '2016-05-14 09:54:57', 0, 'admin', 'ok'),
(40, 2, 10, 6, 9, '2016-05-14 10:02:33', 0, 'admin', 'cancelled'),
(41, 1, 10, 5, 3, '2016-05-14 13:50:12', 0, 'admin', 'cancelled'),
(42, 1, 10, 7, 3, '2016-05-14 13:50:41', 0, 'admin', 'cancelled'),
(43, 1, 11, 6, 3, '2016-05-14 13:52:25', 0, 'admin', 'cancelled'),
(44, 1, 9, 7, 6, '2016-05-14 13:52:35', 0, 'admin', 'ok'),
(45, 1, 9, 5, 3, '2016-05-14 13:52:35', 0, 'admin', 'ok'),
(46, 1, 12, 6, 3, '2016-05-14 13:54:38', 0, 'admin', 'ok'),
(47, 1, 13, 7, 1, '2016-05-14 13:54:42', 0, 'admin', 'ok'),
(48, 1, 14, 6, 1, '2016-05-14 13:54:48', 0, 'admin', 'ok'),
(49, 1, 15, 5, 1, '2016-05-14 13:54:54', 0, 'admin', 'cancelled'),
(50, 1, 16, 7, 1, '2016-05-14 13:55:00', 0, 'admin', 'cancelled'),
(51, 1, 16, 5, 1, '2016-05-14 13:55:01', 0, 'admin', 'cancelled'),
(52, 1, 17, 6, 2, '2016-05-14 13:55:06', 0, 'admin', 'cancelled'),
(53, 1, 19, 6, 1, '2016-05-14 16:33:14', 0, 'admin', 'cancelled'),
(54, 1, 20, 7, 1, '2016-05-14 16:33:23', 0, 'admin', 'ok'),
(55, 1, 10, 17, 1, '2016-05-14 16:40:33', 0, 'admin', 'ok'),
(56, 1, 10, 15, 1, '2016-05-14 16:40:33', 0, 'admin', 'ok'),
(57, 1, 10, 7, 1, '2016-05-14 16:40:33', 0, 'admin', 'ok'),
(58, 1, 11, 9, 1, '2016-05-14 16:40:44', 0, 'admin', 'cancelled'),
(59, 1, 11, 18, 1, '2016-05-14 16:40:44', 0, 'admin', 'cancelled'),
(60, 1, 11, 9, 1, '2016-05-14 21:18:27', 0, 'admin', 'ok'),
(61, 1, 11, 8, 1, '2016-05-14 21:18:27', 0, 'admin', 'ok'),
(62, 1, 11, 16, 1, '2016-05-14 21:18:27', 0, 'admin', 'ok'),
(63, 1, 11, 17, 2, '2016-05-14 21:18:27', 0, 'admin', 'ok');

--
-- Trigger `product_line`
--
DROP TRIGGER IF EXISTS `product_line_added`;
DELIMITER $$
CREATE TRIGGER `product_line_added` BEFORE INSERT ON `product_line`
 FOR EACH ROW BEGIN
SET new.added = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `product_price_log`
--

DROP TABLE IF EXISTS `product_price_log`;
CREATE TABLE `product_price_log` (
  `id` int(10) unsigned NOT NULL,
  `product_id` int(10) unsigned NOT NULL,
  `field` enum('price_sell','price_buy') NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `valid_from` datetime NOT NULL,
  `valid_to` datetime DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `product_price_log`
--

INSERT INTO `product_price_log` (`id`, `product_id`, `field`, `price`, `valid_from`, `valid_to`) VALUES
(13, 5, 'price_sell', '3.50', '2016-05-04 08:22:49', '2016-05-14 16:35:58'),
(14, 5, 'price_buy', '3.40', '2016-05-04 08:22:49', '2016-05-14 16:35:54'),
(15, 6, 'price_sell', '3.50', '2016-05-04 08:23:00', '2016-05-14 16:35:58'),
(16, 6, 'price_buy', '3.40', '2016-05-04 08:23:00', '2016-05-14 16:35:54'),
(17, 7, 'price_sell', '8.60', '2016-05-04 08:23:14', '2016-05-14 16:35:58'),
(18, 7, 'price_buy', '8.40', '2016-05-04 08:23:14', '2016-05-14 16:35:54'),
(19, 7, 'price_buy', '7.60', '2016-05-14 16:35:54', '2016-05-14 16:36:18'),
(20, 7, 'price_sell', '7.50', '2016-05-14 16:35:58', '2016-05-14 16:36:22'),
(21, 7, 'price_buy', '7.50', '2016-05-14 16:36:18', '2016-05-14 16:36:26'),
(22, 7, 'price_sell', '7.60', '2016-05-14 16:36:22', '2016-05-14 16:36:32'),
(23, 6, 'price_buy', '3.30', '2016-05-14 16:36:26', '2016-05-14 16:36:29'),
(24, 5, 'price_buy', '3.30', '2016-05-14 16:36:29', NULL),
(25, 5, 'price_sell', '3.40', '2016-05-14 16:36:32', '2016-05-14 16:36:35'),
(26, 6, 'price_sell', '3.40', '2016-05-14 16:36:35', NULL),
(27, 8, 'price_sell', '6.40', '2016-05-14 16:36:55', NULL),
(28, 8, 'price_buy', '6.30', '2016-05-14 16:36:55', NULL),
(29, 9, 'price_sell', '7.20', '2016-05-14 16:37:12', NULL),
(30, 9, 'price_buy', '7.10', '2016-05-14 16:37:12', NULL),
(31, 10, 'price_sell', '3.30', '2016-05-14 16:37:47', NULL),
(32, 10, 'price_buy', '3.20', '2016-05-14 16:37:47', NULL),
(33, 11, 'price_sell', '4.30', '2016-05-14 16:38:00', NULL),
(34, 11, 'price_buy', '4.20', '2016-05-14 16:38:00', NULL),
(35, 12, 'price_sell', '3.90', '2016-05-14 16:38:55', NULL),
(36, 12, 'price_buy', '3.80', '2016-05-14 16:38:55', NULL),
(37, 13, 'price_sell', '3.50', '2016-05-14 16:39:11', NULL),
(38, 13, 'price_buy', '3.40', '2016-05-14 16:39:11', NULL),
(39, 14, 'price_sell', '7.60', '2016-05-14 16:39:21', NULL),
(40, 14, 'price_buy', '7.50', '2016-05-14 16:39:21', NULL),
(41, 15, 'price_sell', '2.80', '2016-05-14 16:39:36', NULL),
(42, 15, 'price_buy', '2.70', '2016-05-14 16:39:36', NULL),
(43, 16, 'price_sell', '3.00', '2016-05-14 16:39:48', NULL),
(44, 16, 'price_buy', '2.90', '2016-05-14 16:39:48', NULL),
(45, 17, 'price_sell', '2.00', '2016-05-14 16:39:59', NULL),
(46, 17, 'price_buy', '1.90', '2016-05-14 16:39:59', NULL),
(47, 18, 'price_sell', '2.70', '2016-05-14 16:40:11', NULL),
(48, 18, 'price_buy', '2.50', '2016-05-14 16:40:11', NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int(10) unsigned NOT NULL,
  `email` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `firstname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `lastname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(60) COLLATE utf8_unicode_ci NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL,
  `ip` varchar(15) COLLATE utf8_unicode_ci NOT NULL,
  `last_login` datetime NOT NULL,
  `admin` tinyint(3) unsigned NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `user`
--

INSERT INTO `user` (`id`, `email`, `firstname`, `lastname`, `password`, `created`, `modified`, `ip`, `last_login`, `admin`) VALUES
(1, 'petritzdesigns@gmail.com', 'Markus', 'Petritz', '1B0064CB1369C44E4C9810FD1590F88C', '2016-03-03 09:00:23', '2016-05-14 21:17:48', '10.0.1.22:64423', '2016-05-14 21:17:48', 1),
(2, 'scomputerlp@gmail.com', 'Julian', 'Maierl', 'EBAD97202F75B446D0662F81959CEA85', '2016-05-14 09:19:23', '2016-05-14 10:01:45', 'localhost:29580', '2016-05-14 10:01:45', 0);

--
-- Trigger `user`
--
DROP TRIGGER IF EXISTS `user_created`;
DELIMITER $$
CREATE TRIGGER `user_created` BEFORE INSERT ON `user`
 FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `user_modified`;
DELIMITER $$
CREATE TRIGGER `user_modified` BEFORE UPDATE ON `user`
 FOR EACH ROW BEGIN
SET new.modified = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Struktur des Views `food_orders`
--
DROP TABLE IF EXISTS `food_orders`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `food_orders` AS select `l`.`user` AS `user`,`l`.`food_order` AS `order`,`f`.`time` AS `time`,sum(`l`.`quantity`) AS `amount`,sum((`l`.`quantity` * (select `lg`.`price` from `product_price_log` `lg` where ((`lg`.`field` = 'price_sell') and (`lg`.`product_id` = `p`.`id`) and (`l`.`added` >= `lg`.`valid_from`) and (`l`.`added` <= coalesce(`lg`.`valid_to`,now())))))) AS `sum` from ((`product_line` `l` join `product` `p` on((`l`.`product` = `p`.`id`))) join `food_order` `f` on((`l`.`food_order` = `f`.`id`))) where (`l`.`status` = 'ok') group by `l`.`food_order`;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `credit`
--
ALTER TABLE `credit`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user` (`user`);

--
-- Indizes für die Tabelle `food_order`
--
ALTER TABLE `food_order`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `time` (`time`),
  ADD KEY `admin` (`admin`);

--
-- Indizes für die Tabelle `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `product_name_unique` (`name`);

--
-- Indizes für die Tabelle `product_line`
--
ALTER TABLE `product_line`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user` (`user`),
  ADD KEY `food_order` (`food_order`),
  ADD KEY `product` (`product`);

--
-- Indizes für die Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  ADD PRIMARY KEY (`id`),
  ADD KEY `product_id` (`product_id`);

--
-- Indizes für die Tabelle `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_email_unique` (`email`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `credit`
--
ALTER TABLE `credit`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `food_order`
--
ALTER TABLE `food_order`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=21;
--
-- AUTO_INCREMENT für Tabelle `product`
--
ALTER TABLE `product`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=19;
--
-- AUTO_INCREMENT für Tabelle `product_line`
--
ALTER TABLE `product_line`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=64;
--
-- AUTO_INCREMENT für Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=49;
--
-- AUTO_INCREMENT für Tabelle `user`
--
ALTER TABLE `user`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=3;
--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `credit`
--
ALTER TABLE `credit`
  ADD CONSTRAINT `credit_ibfk_1` FOREIGN KEY (`user`) REFERENCES `user` (`id`);

--
-- Constraints der Tabelle `food_order`
--
ALTER TABLE `food_order`
  ADD CONSTRAINT `food_order_ibfk_1` FOREIGN KEY (`admin`) REFERENCES `user` (`id`);

--
-- Constraints der Tabelle `product_line`
--
ALTER TABLE `product_line`
  ADD CONSTRAINT `product_line_ibfk_1` FOREIGN KEY (`user`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `product_line_ibfk_2` FOREIGN KEY (`food_order`) REFERENCES `food_order` (`id`),
  ADD CONSTRAINT `product_line_ibfk_3` FOREIGN KEY (`product`) REFERENCES `product` (`id`);

--
-- Constraints der Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  ADD CONSTRAINT `product_price_log_ibfk_1` FOREIGN KEY (`product_id`) REFERENCES `product` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
