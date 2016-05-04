-- phpMyAdmin SQL Dump
-- version 4.6.0
-- http://www.phpmyadmin.net
--
-- Host: localhost:3306
-- Erstellungszeit: 04. Mai 2016 um 08:25
-- Server-Version: 5.5.49-0+deb8u1
-- PHP-Version: 5.6.14

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

CREATE TABLE `credit` (
  `id` int(10) UNSIGNED NOT NULL,
  `user` int(10) UNSIGNED NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Trigger `credit`
--
DELIMITER $$
CREATE TRIGGER `credit_created` BEFORE INSERT ON `credit` FOR EACH ROW BEGIN
SET new.created = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `food_order`
--

CREATE TABLE `food_order` (
  `id` int(10) UNSIGNED NOT NULL,
  `time` datetime NOT NULL,
  `created` datetime NOT NULL,
  `admin` int(10) UNSIGNED NOT NULL,
  `closed` tinyint(4) NOT NULL,
  `closed_time` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `food_order`
--

INSERT INTO `food_order` (`id`, `time`, `created`, `admin`, `closed`, `closed_time`) VALUES
(4, '2016-05-05 13:15:00', '2016-05-04 08:21:48', 1, 0, '0000-00-00 00:00:00');

--
-- Trigger `food_order`
--
DELIMITER $$
CREATE TRIGGER `food_order_created` BEFORE INSERT ON `food_order` FOR EACH ROW BEGIN
SET new.created = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `food_orders`
-- (Siehe unten für die tatsächliche Ansicht)
--
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

CREATE TABLE `product` (
  `id` int(10) UNSIGNED NOT NULL,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `price_buy` decimal(6,2) NOT NULL,
  `price_sell` decimal(6,2) NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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
DELIMITER $$
CREATE TRIGGER `after_insert_product` AFTER INSERT ON `product` FOR EACH ROW BEGIN
	INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) 
		VALUES (NULL, NEW.id, 'price_sell', NEW.price_sell, NOW(), NULL);
	INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) 
		VALUES (NULL, NEW.id, 'price_buy', NEW.price_buy, NOW(), NULL); 
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_update_products` AFTER UPDATE ON `product` FOR EACH ROW BEGIN
        IF NEW.price_sell <> OLD.price_sell THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_sell' AND valid_to IS NULL;             INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_sell', NEW.price_sell, NOW(), NULL);         END IF;
        
        IF NEW.price_buy <> OLD.price_buy THEN         	UPDATE product_price_log SET valid_to = NOW() WHERE field = 'price_buy' AND valid_to IS NULL;             INSERT INTO product_price_log (id, product_id, field, price, valid_from, valid_to) VALUES (NULL, NEW.id, 'price_buy', NEW.price_buy, NOW(), NULL);         END IF;
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `product_created` BEFORE INSERT ON `product` FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `product_modified` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
SET new.modified = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `product_line`
--

CREATE TABLE `product_line` (
  `id` int(10) UNSIGNED NOT NULL,
  `user` int(11) UNSIGNED NOT NULL,
  `food_order` int(11) UNSIGNED NOT NULL,
  `product` int(11) UNSIGNED NOT NULL,
  `quantity` mediumint(8) UNSIGNED NOT NULL,
  `added` datetime NOT NULL,
  `paid` tinyint(4) NOT NULL,
  `pay_type` enum('credit','admin') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'admin'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Trigger `product_line`
--
DELIMITER $$
CREATE TRIGGER `product_line_added` BEFORE INSERT ON `product_line` FOR EACH ROW BEGIN
SET new.added = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `product_price_log`
--

CREATE TABLE `product_price_log` (
  `id` int(10) UNSIGNED NOT NULL,
  `product_id` int(10) UNSIGNED NOT NULL,
  `field` enum('price_sell','price_buy') NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `valid_from` datetime NOT NULL,
  `valid_to` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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

CREATE TABLE `user` (
  `id` int(10) UNSIGNED NOT NULL,
  `email` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `firstname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `lastname` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(60) COLLATE utf8_unicode_ci NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL,
  `ip` varchar(15) COLLATE utf8_unicode_ci NOT NULL,
  `last_login` datetime NOT NULL,
  `admin` tinyint(3) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Daten für Tabelle `user`
--

INSERT INTO `user` (`id`, `email`, `firstname`, `lastname`, `password`, `created`, `modified`, `ip`, `last_login`, `admin`) VALUES
(1, 'petritzdesigns@gmail.com', 'Markus', 'Petritz', 'A35150EF15CB9E333F47DB398F398E63', '2016-03-03 09:00:23', '2016-05-01 20:35:11', '80.120.208.218', '2016-04-26 13:20:31', 1);

--
-- Trigger `user`
--
DELIMITER $$
CREATE TRIGGER `user_created` BEFORE INSERT ON `user` FOR EACH ROW BEGIN
SET new.created = NOW();
SET new.modified = NOW();
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `user_modified` BEFORE UPDATE ON `user` FOR EACH ROW BEGIN
SET new.modified = NOW();
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Struktur des Views `food_orders`
--
DROP TABLE IF EXISTS `food_orders`;

CREATE ALGORITHM=UNDEFINED DEFINER=`order`@`%` SQL SECURITY DEFINER VIEW `food_orders`  AS  select `l`.`user` AS `user`,`f`.`time` AS `time`,sum(`l`.`quantity`) AS `amount`,sum((`l`.`quantity` * `p`.`price_sell`)) AS `sum` from ((`product_line` `l` join `product` `p` on((`l`.`product` = `p`.`id`))) join `food_order` `f` on((`l`.`food_order` = `f`.`id`))) group by `l`.`food_order` ;

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
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT für Tabelle `food_order`
--
ALTER TABLE `food_order`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT für Tabelle `product`
--
ALTER TABLE `product`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT für Tabelle `product_line`
--
ALTER TABLE `product_line`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT für Tabelle `product_price_log`
--
ALTER TABLE `product_price_log`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;
--
-- AUTO_INCREMENT für Tabelle `user`
--
ALTER TABLE `user`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
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
