-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Client :  127.0.0.1
-- Généré le :  Mar 25 Septembre 2018 à 17:41
-- Version du serveur :  5.6.17
-- Version de PHP :  5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données :  `hymperia_test_deploy`
--
CREATE DATABASE IF NOT EXISTS `hymperia_test_deploy` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `hymperia_test_deploy`;

-- --------------------------------------------------------

--
-- Structure de la table `acces`
--

DROP TABLE IF EXISTS `acces`;
CREATE TABLE IF NOT EXISTS `acces` (
  `idProjet` int(11) NOT NULL,
  `idUtilisateur` int(11) NOT NULL,
  `DroitDAcces` longtext NOT NULL,
  PRIMARY KEY (`idProjet`,`idUtilisateur`),
  KEY `IX_Acces_idUtilisateur` (`idUtilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Contenu de la table `acces`
--

INSERT INTO `acces` VALUES
(1, 1, 'Possession'),
(1, 3, 'Lecture'),
(2, 2, 'Possession'),
(2, 3, 'LectureEcriture'),
(3, 1, 'LectureEcriture'),
(3, 2, 'Lecture'),
(3, 3, 'Possession');

-- --------------------------------------------------------

--
-- Structure de la table `formes`
--

DROP TABLE IF EXISTS `formes`;
CREATE TABLE IF NOT EXISTS `formes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MateriauId` int(11) NOT NULL,
  `Discriminator` longtext NOT NULL,
  `ProjetId` int(11) DEFAULT NULL,
  `Point1` longtext,
  `Point2` longtext,
  `Diametre` double DEFAULT NULL,
  `InnerDiametre` double DEFAULT NULL,
  `ThetaDiv` int(11) DEFAULT NULL,
  `Centre` longtext,
  `RayonX` double DEFAULT NULL,
  `RayonY` double DEFAULT NULL,
  `RayonZ` double DEFAULT NULL,
  `PhiDiv` int(11) DEFAULT NULL,
  `Ellipsoide_ThetaDiv` int(11) DEFAULT NULL,
  `PrismeRectangulaire_Centre` longtext,
  `Hauteur` double DEFAULT NULL,
  `Largeur` double DEFAULT NULL,
  `Longueur` double DEFAULT NULL,
  `Origine` longtext,
  `RayonBase` double DEFAULT NULL,
  `RayonTop` double DEFAULT NULL,
  `Cylindre_ThetaDiv` int(11) DEFAULT NULL,
  `PrismeRectangulaire_Hauteur` double DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Formes_MateriauId` (`MateriauId`),
  KEY `IX_Formes_ProjetId` (`ProjetId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=94 ;

--
-- Contenu de la table `formes`
--

INSERT INTO `formes` VALUES
(1, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":50.0,"Y":27.0,"Z":25.0}', 3, 9, 12, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":40.0,"Y":52.0,"Z":22.0}', NULL, 2, 1, NULL, NULL, NULL, NULL, 14),
(3, 1, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":50.0,"Y":0.0,"Z":24.0}', 8, 1, 5, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(4, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":92.0,"Y":0.0,"Z":42.0}', NULL, 13, 14, NULL, NULL, NULL, NULL, 2),
(5, 1, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":99.0,"Y":78.0,"Z":42.0}', 14, 11, 7, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(6, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":40.0,"Y":90.0,"Z":93.0}', NULL, 10, 10, NULL, NULL, NULL, NULL, 1),
(7, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":89.0,"Y":11.0,"Z":47.0}', NULL, 11, 3, NULL, NULL, NULL, NULL, 4),
(8, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":31.0,"Y":20.0,"Z":39.0}', NULL, 10, 4, NULL, NULL, NULL, NULL, 13),
(9, 2, 'Cylindre', 3, '{"X":77.0,"Y":25.0,"Z":47.0}', '{"X":2.0,"Y":62.0,"Z":88.0}', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(10, 2, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL, '{"X":59.0,"Y":73.0,"Z":48.0}', 10, 0, NULL, NULL),
(11, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":70.0,"Y":95.0,"Z":38.0}', NULL, 5, 5, NULL, NULL, NULL, NULL, 11),
(12, 2, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, NULL, NULL, '{"X":56.0,"Y":55.0,"Z":67.0}', 12, 0, NULL, NULL),
(13, 1, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL, '{"X":83.0,"Y":53.0,"Z":73.0}', 12, 0, NULL, NULL),
(14, 1, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 14, NULL, NULL, '{"X":18.0,"Y":20.0,"Z":10.0}', 6, 0, NULL, NULL),
(15, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":7.0,"Y":44.0,"Z":21.0}', NULL, 9, 4, NULL, NULL, NULL, NULL, 14),
(16, 2, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 11, NULL, NULL, '{"X":88.0,"Y":71.0,"Z":60.0}', 8, 0, NULL, NULL),
(17, 1, 'Cylindre', 3, '{"X":75.0,"Y":12.0,"Z":92.0}', '{"X":71.0,"Y":77.0,"Z":88.0}', 3, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(18, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":89.0,"Y":86.0,"Z":46.0}', 6, 5, 2, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(19, 1, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":44.0,"Y":87.0,"Z":27.0}', 9, 14, 12, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(20, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":97.0,"Y":8.0,"Z":11.0}', NULL, 14, 13, NULL, NULL, NULL, NULL, 13),
(21, 2, 'Cylindre', 3, '{"X":94.0,"Y":26.0,"Z":88.0}', '{"X":86.0,"Y":85.0,"Z":28.0}', 10, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(22, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":84.0,"Y":32.0,"Z":71.0}', 12, 14, 9, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(23, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":62.0,"Y":49.0,"Z":63.0}', 7, 10, 2, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(24, 2, 'Cylindre', 3, '{"X":38.0,"Y":54.0,"Z":24.0}', '{"X":23.0,"Y":25.0,"Z":51.0}', 13, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(25, 1, 'Cylindre', 3, '{"X":54.0,"Y":85.0,"Z":14.0}', '{"X":65.0,"Y":66.0,"Z":95.0}', 8, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(26, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":9.0,"Y":36.0,"Z":53.0}', NULL, 12, 2, NULL, NULL, NULL, NULL, 14),
(27, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":78.0,"Y":99.0,"Z":66.0}', NULL, 12, 8, NULL, NULL, NULL, NULL, 13),
(28, 1, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":59.0,"Y":30.0,"Z":36.0}', 7, 4, 6, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(29, 2, 'Cylindre', 3, '{"X":64.0,"Y":76.0,"Z":77.0}', '{"X":59.0,"Y":1.0,"Z":23.0}', 3, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(30, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":55.0,"Y":64.0,"Z":84.0}', NULL, 13, 5, NULL, NULL, NULL, NULL, 10),
(31, 2, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 14, NULL, NULL, '{"X":65.0,"Y":12.0,"Z":41.0}', 10, 0, NULL, NULL),
(32, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":96.0,"Y":9.0,"Z":50.0}', NULL, 7, 4, NULL, NULL, NULL, NULL, 13),
(33, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":43.0,"Y":44.0,"Z":91.0}', 3, 11, 7, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(34, 1, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 12, NULL, NULL, '{"X":48.0,"Y":92.0,"Z":0.0}', 12, 0, NULL, NULL),
(35, 1, 'Cylindre', 3, '{"X":64.0,"Y":15.0,"Z":75.0}', '{"X":73.0,"Y":32.0,"Z":2.0}', 4, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(36, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":57.0,"Y":89.0,"Z":22.0}', NULL, 1, 7, NULL, NULL, NULL, NULL, 10),
(37, 2, 'Cylindre', 3, '{"X":47.0,"Y":88.0,"Z":28.0}', '{"X":98.0,"Y":48.0,"Z":84.0}', 9, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(38, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":29.0,"Y":43.0,"Z":89.0}', NULL, 6, 3, NULL, NULL, NULL, NULL, 1),
(39, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":33.0,"Y":61.0,"Z":0.0}', NULL, 10, 6, NULL, NULL, NULL, NULL, 12),
(40, 1, 'Cylindre', 3, '{"X":41.0,"Y":43.0,"Z":1.0}', '{"X":79.0,"Y":91.0,"Z":33.0}', 3, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(41, 1, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":20.0,"Y":12.0,"Z":47.0}', 11, 7, 3, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(42, 2, 'Cone', 3, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL, '{"X":35.0,"Y":50.0,"Z":52.0}', 4, 0, NULL, NULL),
(43, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":68.0,"Y":22.0,"Z":99.0}', 5, 4, 7, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(44, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":16.0,"Y":66.0,"Z":9.0}', 6, 7, 4, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(45, 1, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":25.0,"Y":76.0,"Z":59.0}', NULL, 5, 8, NULL, NULL, NULL, NULL, 4),
(46, 2, 'Ellipsoide', 3, NULL, NULL, NULL, NULL, NULL, '{"X":12.0,"Y":35.0,"Z":34.0}', 11, 13, 5, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(47, 1, 'Ellipsoide', 2, NULL, NULL, NULL, NULL, NULL, '{"X":2.0,"Y":46.0,"Z":78.0}', 10, 12, 13, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(48, 2, 'PrismeRectangulaire', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":37.0,"Y":29.0,"Z":67.0}', NULL, 14, 3, NULL, NULL, NULL, NULL, 2),
(49, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":59.0,"Y":20.0,"Z":46.0}', 12, 9, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(50, 1, 'Cylindre', 1, '{"X":84.0,"Y":64.0,"Z":57.0}', '{"X":83.0,"Y":5.0,"Z":79.0}', 2, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(51, 2, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":60.0,"Y":15.0,"Z":22.0}', 12, 4, 3, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(52, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":12.0,"Y":83.0,"Z":16.0}', 8, 8, 3, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(53, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 12, NULL, NULL, '{"X":65.0,"Y":52.0,"Z":12.0}', 6, 0, NULL, NULL),
(54, 2, 'PrismeRectangulaire', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":57.0,"Y":12.0,"Z":10.0}', NULL, 10, 4, NULL, NULL, NULL, NULL, 14),
(55, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, NULL, NULL, '{"X":81.0,"Y":2.0,"Z":84.0}', 14, 0, NULL, NULL),
(56, 2, 'PrismeRectangulaire', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":44.0,"Y":88.0,"Z":11.0}', NULL, 11, 12, NULL, NULL, NULL, NULL, 5),
(57, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 13, NULL, NULL, '{"X":49.0,"Y":41.0,"Z":18.0}', 2, 0, NULL, NULL),
(58, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":5.0,"Y":35.0,"Z":2.0}', 3, 9, 12, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(59, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9, NULL, NULL, '{"X":5.0,"Y":1.0,"Z":4.0}', 14, 0, NULL, NULL),
(60, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":96.0,"Y":53.0,"Z":90.0}', 12, 8, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(61, 1, 'PrismeRectangulaire', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":43.0,"Y":1.0,"Z":71.0}', NULL, 13, 13, NULL, NULL, NULL, NULL, 9),
(62, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":5.0,"Y":76.0,"Z":63.0}', 1, 2, 14, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(63, 1, 'PrismeRectangulaire', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":77.0,"Y":36.0,"Z":65.0}', NULL, 6, 11, NULL, NULL, NULL, NULL, 10),
(64, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL, '{"X":60.0,"Y":24.0,"Z":33.0}', 12, 0, NULL, NULL),
(65, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7, NULL, NULL, '{"X":38.0,"Y":25.0,"Z":35.0}', 13, 0, NULL, NULL),
(66, 1, 'PrismeRectangulaire', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":33.0,"Y":82.0,"Z":98.0}', NULL, 1, 6, NULL, NULL, NULL, NULL, 9),
(67, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, NULL, NULL, '{"X":36.0,"Y":45.0,"Z":78.0}', 1, 0, NULL, NULL),
(68, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":45.0,"Y":89.0,"Z":0.0}', 3, 7, 8, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(69, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7, NULL, NULL, '{"X":50.0,"Y":50.0,"Z":83.0}', 6, 0, NULL, NULL),
(70, 2, 'Cylindre', 1, '{"X":90.0,"Y":6.0,"Z":28.0}', '{"X":17.0,"Y":82.0,"Z":59.0}', 5, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(71, 2, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":73.0,"Y":21.0,"Z":96.0}', 14, 1, 12, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(72, 2, 'Ellipsoide', 2, NULL, NULL, NULL, NULL, NULL, '{"X":99.0,"Y":59.0,"Z":28.0}', 13, 7, 13, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(73, 2, 'Cone', 2, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 14, NULL, NULL, '{"X":4.0,"Y":32.0,"Z":89.0}', 10, 0, NULL, NULL),
(74, 1, 'Ellipsoide', 2, NULL, NULL, NULL, NULL, NULL, '{"X":62.0,"Y":77.0,"Z":45.0}', 6, 8, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(75, 2, 'Cylindre', 2, '{"X":59.0,"Y":36.0,"Z":60.0}', '{"X":47.0,"Y":51.0,"Z":29.0}', 11, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(76, 1, 'PrismeRectangulaire', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":95.0,"Y":6.0,"Z":89.0}', NULL, 12, 14, NULL, NULL, NULL, NULL, 1),
(77, 2, 'Cylindre', 2, '{"X":21.0,"Y":17.0,"Z":76.0}', '{"X":5.0,"Y":11.0,"Z":5.0}', 7, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(78, 2, 'Ellipsoide', 2, NULL, NULL, NULL, NULL, NULL, '{"X":91.0,"Y":77.0,"Z":9.0}', 3, 4, 7, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(79, 1, 'Cone', 2, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, '{"X":99.0,"Y":14.0,"Z":88.0}', 14, 0, NULL, NULL),
(80, 2, 'Cylindre', 1, '{"X":83.0,"Y":45.0,"Z":45.0}', '{"X":28.0,"Y":26.0,"Z":50.0}', 10, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(81, 2, 'Cylindre', 2, '{"X":2.0,"Y":70.0,"Z":5.0}', '{"X":42.0,"Y":69.0,"Z":68.0}', 13, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(82, 2, 'Cylindre', 1, '{"X":39.0,"Y":53.0,"Z":73.0}', '{"X":73.0,"Y":98.0,"Z":53.0}', 8, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(83, 2, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":10.0,"Y":59.0,"Z":1.0}', 10, 13, 12, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(84, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":67.0,"Y":26.0,"Z":18.0}', 5, 9, 10, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(85, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, '{"X":51.0,"Y":73.0,"Z":27.0}', 4, 0, NULL, NULL),
(86, 2, 'Cylindre', 1, '{"X":21.0,"Y":16.0,"Z":81.0}', '{"X":91.0,"Y":4.0,"Z":0.0}', 12, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(87, 2, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":80.0,"Y":4.0,"Z":67.0}', 8, 11, 10, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(88, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, '{"X":47.0,"Y":14.0,"Z":57.0}', 7, 0, NULL, NULL),
(89, 2, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 11, NULL, NULL, '{"X":96.0,"Y":69.0,"Z":55.0}', 5, 0, NULL, NULL),
(90, 1, 'Ellipsoide', 1, NULL, NULL, NULL, NULL, NULL, '{"X":13.0,"Y":81.0,"Z":75.0}', 8, 5, 4, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(91, 1, 'Cone', 1, NULL, NULL, NULL, NULL, 60, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, '{"X":79.0,"Y":77.0,"Z":76.0}', 11, 0, NULL, NULL),
(92, 1, 'Cylindre', 1, '{"X":34.0,"Y":9.0,"Z":48.0}', '{"X":38.0,"Y":91.0,"Z":40.0}', 12, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 60, NULL),
(93, 2, 'PrismeRectangulaire', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '{"X":79.0,"Y":7.0,"Z":32.0}', NULL, 10, 12, NULL, NULL, NULL, NULL, 13);

-- --------------------------------------------------------

--
-- Structure de la table `materiaux`
--

DROP TABLE IF EXISTS `materiaux`;
CREATE TABLE IF NOT EXISTS `materiaux` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nom` varchar(255) NOT NULL,
  `Prix` double NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AK_Materiaux_Nom` (`Nom`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Contenu de la table `materiaux`
--

INSERT INTO `materiaux` VALUES
(1, 'Bois', 1.55),
(2, 'Acier', 2.55);

-- --------------------------------------------------------

--
-- Structure de la table `projets`
--

DROP TABLE IF EXISTS `projets`;
CREATE TABLE IF NOT EXISTS `projets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nom` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AK_Projets_Nom` (`Nom`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Contenu de la table `projets`
--

INSERT INTO `projets` VALUES
(1, 'Projet 1'),
(2, 'Projet 2'),
(3, 'Projet 3');

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurs`
--

DROP TABLE IF EXISTS `utilisateurs`;
CREATE TABLE IF NOT EXISTS `utilisateurs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nom` varchar(255) NOT NULL,
  `MotDePasse` longtext NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AK_Utilisateurs_Nom` (`Nom`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Contenu de la table `utilisateurs`
--

INSERT INTO `utilisateurs` VALUES
(1, 'Alexandre', '$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q'),
(2, 'Guillaume', '$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q'),
(3, 'Antoine', '$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q');

-- --------------------------------------------------------

--
-- Structure de la table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Contenu de la table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` VALUES
('20180920181807_InitialDatabase', '2.2.0-preview1-35029'),
('20180920221024_ConesMigration', '2.2.0-preview1-35029');

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `acces`
--
ALTER TABLE `acces`
  ADD CONSTRAINT `FK_Acces_Projets_idProjet` FOREIGN KEY (`idProjet`) REFERENCES `projets` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Acces_Utilisateurs_idUtilisateur` FOREIGN KEY (`idUtilisateur`) REFERENCES `utilisateurs` (`Id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `formes`
--
ALTER TABLE `formes`
  ADD CONSTRAINT `FK_Formes_Materiaux_MateriauId` FOREIGN KEY (`MateriauId`) REFERENCES `materiaux` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Formes_Projets_ProjetId` FOREIGN KEY (`ProjetId`) REFERENCES `projets` (`Id`) ON DELETE NO ACTION;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
