-- phpMyAdmin SQL Dump
-- version 4.4.10
-- http://www.phpmyadmin.net
--
-- Host: localhost:8889
-- Erstellungszeit: 05. Mai 2016 um 22:12
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `food_order`
--

INSERT INTO `food_order` (`id`, `time`, `created`, `admin`, `closed`, `closed_time`) VALUES
(4, '2016-05-05 13:15:00', '2016-05-04 08:21:48', 1, 1, '2016-05-05 20:46:16'),
(5, '2016-05-06 12:25:00', '2016-05-04 08:38:50', 1, 0, '0000-00-00 00:00:00'),
(6, '2016-05-07 11:35:00', '2016-05-04 08:39:25', 1, 0, '0000-00-00 00:00:00'),
(7, '2016-05-08 12:25:00', '2016-05-04 08:39:43', 1, 0, '0000-00-00 00:00:00'),
(8, '2016-05-09 10:00:00', '2016-05-04 13:25:29', 1, 0, '0000-00-00 00:00:00');

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
,`time` datetime
,`amount` decimal(30,0)
,`sum` decimal(36,2)
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `product`
--

INSERT INTO `product` (`id`, `name`, `price_buy`, `price_sell`, `created`, `modified`) VALUES
(5, 'Döner mild', '3.40', '3.50', '2016-05-04 08:22:49', '2016-05-04 08:22:49'),
(6, 'Döner scharf', '3.40', '3.50', '2016-05-04 08:23:00', '2016-05-04 08:23:00'),
(7, 'Pizza Salami', '8.40', '8.60', '2016-05-04 08:23:14', '2016-05-04 08:23:14');

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
  `quantity` mediumint(8) unsigned NOT NULL,
  `added` datetime NOT NULL,
  `paid` tinyint(4) NOT NULL,
  `pay_type` enum('credit','admin') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'admin'
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `product_line`
--

INSERT INTO `product_line` (`id`, `user`, `food_order`, `product`, `quantity`, `added`, `paid`, `pay_type`) VALUES
(9, 1, 4, 7, 1, '2016-05-04 08:30:05', 0, 'admin'),
(10, 1, 4, 5, 4, '2016-05-04 08:30:05', 0, 'admin'),
(11, 1, 5, 7, 2, '2016-05-04 08:40:54', 0, 'admin'),
(12, 1, 5, 6, 5, '2016-05-04 08:40:54', 0, 'admin'),
(13, 1, 5, 5, 2, '2016-05-04 08:40:54', 0, 'admin'),
(14, 1, 6, 5, 3, '2016-05-04 08:42:04', 0, 'admin'),
(15, 1, 7, 5, 1, '2016-05-04 08:42:13', 0, 'admin'),
(16, 1, 7, 6, 1, '2016-05-04 08:42:13', 0, 'admin'),
(17, 1, 8, 7, 6, '2016-05-04 13:25:49', 0, 'admin'),
(18, 1, 8, 5, 1, '2016-05-04 13:25:49', 0, 'admin');

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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `product_price_log`
--

INSERT INTO `product_price_log` (`id`, `product_id`, `field`, `price`, `valid_from`, `valid_to`) VALUES
(13, 5, 'price_sell', '3.50', '2016-05-04 08:22:49', NULL),
(14, 5, 'price_buy', '3.40', '2016-05-04 08:22:49', NULL),
(15, 6, 'price_sell', '3.50', '2016-05-04 08:23:00', NULL),
(16, 6, 'price_buy', '3.40', '2016-05-04 08:23:00', NULL),
(17, 7, 'price_sell', '8.60', '2016-05-04 08:23:14', NULL),
(18, 7, 'price_buy', '8.40', '2016-05-04 08:23:14', NULL);

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `user`
--

INSERT INTO `user` (`id`, `email`, `firstname`, `lastname`, `password`, `created`, `modified`, `ip`, `last_login`, `admin`) VALUES
(1, 'petritzdesigns@gmail.com', 'Markus', 'Petritz', 'A35150EF15CB9E333F47DB398F398E63', '2016-03-03 09:00:23', '2016-05-01 20:35:11', '80.120.208.218', '2016-04-26 13:20:31', 1);

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

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `food_orders` AS select `l`.`user` AS `user`,`f`.`time` AS `time`,sum(`l`.`quantity`) AS `amount`,sum((`l`.`quantity` * (select `lg`.`price` from `product_price_log` `lg` where ((`lg`.`field` = 'price_sell') and (`lg`.`product_id` = `p`.`id`) and (`l`.`added` >= `lg`.`valid_from`) and (`l`.`added` <= coalesce(`lg`.`valid_to`,now())))))) AS `sum` from ((`product_line` `l` join `product` `p` on((`l`.`product` = `p`.`id`))) join `food_order` `f` on((`l`.`food_order` = `f`.`id`))) group by `l`.`food_order`;

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
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT für Tabelle `product`
--
ALTER TABLE `product`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT für Tabelle `product_line`
--
ALTER TABLE `product_line`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=19;
--
-- AUTO_INCREMENT für Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=19;
--
-- AUTO_INCREMENT für Tabelle `user`
--
ALTER TABLE `user`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=2;
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
