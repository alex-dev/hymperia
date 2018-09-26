DROP TABLE IF EXISTS Acces;
DROP TABLE IF EXISTS Formes;
DROP TABLE IF EXISTS Projets;
DROP TABLE IF EXISTS Materiaux;
DROP TABLE IF EXISTS Utilisateurs;

CREATE TABLE IF NOT EXISTS Utilisateurs (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  Nom VARCHAR(255) NOT NULL,
  MotDePasse LONGTEXT NOT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY AK_Utilisateurs_Nom (Nom)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

CREATE TABLE IF NOT EXISTS Materiaux (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  Nom VARCHAR(255) NOT NULL,
  Prix DOUBLE NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AK_Materiaux_Nom` (`Nom`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

CREATE TABLE IF NOT EXISTS Projets (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  Nom VARCHAR(255) NOT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY AK_Projets_Nom (Nom)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

CREATE TABLE IF NOT EXISTS Formes (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  IdMateriau INT(11) NOT NULL,
  Origine LONGTEXT NOT NULL,
  Discriminator LONGTEXT NOT NULL,
  IdProjet INT(11) DEFAULT NULL,
  RayonX DOUBLE DEFAULT NULL,
  RayonY DOUBLE DEFAULT NULL,
  RayonZ DOUBLE DEFAULT NULL,
  PhiDiv INT(11) DEFAULT NULL,
  ThetaDiv INT(11) DEFAULT NULL,
  PrismeRectangulaire_Hauteur DOUBLE DEFAULT NULL,
  Largeur DOUBLE DEFAULT NULL,
  Longueur DOUBLE DEFAULT NULL,
  ThetaDivForme_ThetaDiv INT(11) DEFAULT NULL,
  Hauteur DOUBLE DEFAULT NULL,
  RayonBase DOUBLE DEFAULT NULL,
  RayonTop DOUBLE DEFAULT NULL,
  Point LONGTEXT,
  Diametre DOUBLE DEFAULT NULL,
  InnerDiametre DOUBLE DEFAULT NULL,
  PRIMARY KEY (Id),
  KEY IX_Formes_IdMateriau (IdMateriau),
  KEY IX_Formes_IdProjet (IdProjet)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=94 ;

ALTER TABLE Formes
  ADD CONSTRAINT FK_Formes_Materiaux_MateriauId FOREIGN KEY (MateriauId) REFERENCES Materiaux (Id) ON DELETE CASCADE,
  ADD CONSTRAINT FK_Formes_Projets_ProjetId FOREIGN KEY (ProjetId) REFERENCES Projets (Id) ON DELETE NO ACTION;

CREATE TABLE IF NOT EXISTS Acces(
  IdProjet INT(11) NOT NULL,
  IdUtilisateur INT(11) NOT NULL,
  DroitDAcces LONGTEXT NOT NULL,
  PRIMARY KEY (IdProjet, idUtilisateur),
  KEY IX_Acces_IdUtilisateur (IdUtilisateur)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE Acces
  ADD CONSTRAINT FK_Acces_Projets_idProjet FOREIGN KEY (IdProjet) REFERENCES Projets (Id) ON DELETE CASCADE,
  ADD CONSTRAINT FK_Acces_Utilisateurs_idUtilisateur FOREIGN KEY (IdUtilisateur) REFERENCES Utilisateurs (Id) ON DELETE CASCADE;
