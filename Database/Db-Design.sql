-- phpMyAdmin SQL Dump
-- version 4.4.10
-- http://www.phpmyadmin.net
--
-- Host: localhost:8889
-- Erstellungszeit: 17. Mai 2016 um 12:17
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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
,`paid` decimal(7,4)
,`pay_type` enum('credit','admin')
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
  `status` enum('ok','deleted','unavailable') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'ok',
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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
        IF NEW.price_sell <> OLD.price_sell THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_sell' AND valid_to IS NULL AND product_id = NEW.id;                INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_sell', NEW.price_sell, NOW(), NULL);         END IF;
        
        IF NEW.price_buy <> OLD.price_buy THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_buy' AND valid_to IS NULL AND product_id = NEW.id;                INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_buy', NEW.price_buy, NOW(), NULL);         END IF;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `food_orders` AS select `l`.`user` AS `user`,`l`.`food_order` AS `order`,`f`.`time` AS `time`,sum(`l`.`quantity`) AS `amount`,coalesce(sum((`l`.`quantity` * (select `lg`.`price` from `product_price_log` `lg` where ((`lg`.`field` = 'price_sell') and (`lg`.`product_id` = `p`.`id`) and (`l`.`added` >= `lg`.`valid_from`) and (`l`.`added` <= coalesce(`lg`.`valid_to`,now())))))),0) AS `sum`,avg(`l`.`paid`) AS `paid`,`l`.`pay_type` AS `pay_type` from ((`product_line` `l` join `product` `p` on((`l`.`product` = `p`.`id`))) join `food_order` `f` on((`l`.`food_order` = `f`.`id`))) where (`l`.`status` = 'ok') group by `l`.`food_order`,`l`.`user`;

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
  ADD PRIMARY KEY (`id`);

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
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `product`
--
ALTER TABLE `product`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `product_line`
--
ALTER TABLE `product_line`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `user`
--
ALTER TABLE `user`
  MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT;
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
