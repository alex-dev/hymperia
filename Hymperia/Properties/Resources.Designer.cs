﻿// <auto-generated />

using System;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Hymperia.Facade.Properties
{
  /// <summary>This class wraps <see cref="ResourceManager" /> to ease usage.</summary>
  internal static partial class Resources
  {
    private static readonly ResourceManager manager =
      new ResourceManager("Hymperia.Facade.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);

    /// <summary>
    ///   Ajouter
    /// </summary>
    public static string Ajouter => GetString("Ajouter");

    /// <summary>
    ///   Ajouter un projet
    /// </summary>
    public static string AjouterProjet => GetString("AjouterProjet");

    /// <summary>
    ///   Analyse Matériaux
    /// </summary>
    public static string AnalyseMateriau => GetString("AnalyseMateriau");

    /// <summary>
    ///   et
    /// </summary>
    public static string And => GetString("And");

    /// <summary>
    ///   Annuler
    /// </summary>
    public static string Annuler => GetString("Annuler");

    /// <summary>
    ///   Peut seulement convertir depuis {origins}.
    /// </summary>
    public static string CanOnlyConvertFrom([CanBeNull] object origins)
      => string.Format(
        GetString("CanOnlyConvertFrom", nameof(origins)),
        origins);

    /// <summary>
    ///   Peut seulement convertir vers {targets}.
    /// </summary>
    public static string CanOnlyConvertTo([CanBeNull] object targets)
      => string.Format(
        GetString("CanOnlyConvertTo", nameof(targets)),
        targets);

    /// <summary>
    ///   Changement du mot de passe
    /// </summary>
    public static string ChangementDuMotDePasse => GetString("ChangementDuMotDePasse");

    /// <summary>
    ///   Confirmer
    /// </summary>
    public static string Confirmer => GetString("Confirmer");

    /// <summary>
    ///   Connexion
    /// </summary>
    public static string Connexion => GetString("Connexion");

    /// <summary>
    ///   Connexion automatique
    /// </summary>
    public static string ConnexionAutomatique => GetString("ConnexionAutomatique");

    /// <summary>
    ///   Impossible d'inverser un processus détérioratif.
    /// </summary>
    public static string DestructiveConversion => GetString("DestructiveConversion");

    /// <summary>
    ///   Les mots de passes doivent être identique.
    /// </summary>
    public static string DontMatchPassword => GetString("DontMatchPassword");

    /// <summary>
    ///   , 
    /// </summary>
    public static string EnumerationSeparator => GetString("EnumerationSeparator");

    /// <summary>
    ///   Fermer
    /// </summary>
    public static string Fermer => GetString("Fermer");

    /// <summary>
    ///   Le champ ne peut pas être vide.
    /// </summary>
    public static string FieldCannotBeEmpty => GetString("FieldCannotBeEmpty");

    /// <summary>
    ///   N'as pas pu convertir {value} en {targets}.
    /// </summary>
    public static string ImpossibleCast([CanBeNull] object value, [CanBeNull] object targets)
      => string.Format(
        GetString("ImpossibleCast", nameof(value), nameof(targets)),
        value, targets);

    /// <summary>
    ///   N'a pas pu instantier un {type}.
    /// </summary>
    public static string ImpossibleInstantiation([CanBeNull] object type)
      => string.Format(
        GetString("ImpossibleInstantiation", nameof(type)),
        type);

    /// <summary>
    ///   Inscription
    /// </summary>
    public static string Inscription => GetString("Inscription");

    /// <summary>
    ///   Vos informations sont invalides.
    /// </summary>
    public static string InvalidCredential => GetString("InvalidCredential");

    /// <summary>
    ///   Clé de localisation {key} est invalide pour l'entité {entity}.
    /// </summary>
    public static string InvalidLocalizationKey([CanBeNull] object key, [CanBeNull] object entity)
      => string.Format(
        GetString("InvalidLocalizationKey", nameof(key), nameof(entity)),
        key, entity);

    /// <summary>
    ///   Ce manipulateur ne supporte que les {visuals}.
    /// </summary>
    public static string ManipulatorSupport([CanBeNull] object visuals)
      => string.Format(
        GetString("ManipulatorSupport", nameof(visuals)),
        visuals);

    /// <summary>
    ///   Matériau
    /// </summary>
    public static string Material => GetString("Material");

    /// <summary>
    ///   Prix du matériau
    /// </summary>
    public static string MaterialPrice => GetString("MaterialPrice");

    /// <summary>
    ///   Déplacement
    /// </summary>
    public static string Movement => GetString("Movement");

    /// <summary>
    ///   Non
    /// </summary>
    public static string Non => GetString("Non");

    /// <summary>
    ///   Merci de saisir le nom du nouveau projet
    /// </summary>
    public static string NouveauNomProjet => GetString("NouveauNomProjet");

    /// <summary>
    ///   Oui
    /// </summary>
    public static string Oui => GetString("Oui");

    /// <summary>
    ///   Ouvrir
    /// </summary>
    public static string Ouvrir => GetString("Ouvrir");

    /// <summary>
    ///   Ouvrir dans une fenêtre
    /// </summary>
    public static string OuvrirFenetre => GetString("OuvrirFenetre");

    /// <summary>
    ///   Position
    /// </summary>
    public static string Position => GetString("Position");

    /// <summary>
    ///   Position X
    /// </summary>
    public static string PositionX => GetString("PositionX");

    /// <summary>
    ///   Position Y
    /// </summary>
    public static string PositionY => GetString("PositionY");

    /// <summary>
    ///   Position Z
    /// </summary>
    public static string PositionZ => GetString("PositionZ");

    /// <summary>
    ///   Prix
    /// </summary>
    public static string Price => GetString("Price");

    /// <summary>
    ///   Propriétés
    /// </summary>
    public static string ProprieteForme => GetString("ProprieteForme");

    /// <summary>
    ///   Rayon _Base
    /// </summary>
    public static string Radius_Base => GetString("Radius_Base");

    /// <summary>
    ///   Rayon _Top
    /// </summary>
    public static string Radius_Top => GetString("Radius_Top");

    /// <summary>
    ///   Rayon _X
    /// </summary>
    public static string Radius_X => GetString("Radius_X");

    /// <summary>
    ///   Rayon _Y
    /// </summary>
    public static string Radius_Y => GetString("Radius_Y");

    /// <summary>
    ///   Rayon _Z
    /// </summary>
    public static string Radius_Z => GetString("Radius_Z");

    /// <summary>
    ///   Réglage
    /// </summary>
    public static string Reglage => GetString("Reglage");

    /// <summary>
    ///   Rotation
    /// </summary>
    public static string Rotation => GetString("Rotation");

    /// <summary>
    ///   Rotation W
    /// </summary>
    public static string RotationW => GetString("RotationW");

    /// <summary>
    ///   Rotation X
    /// </summary>
    public static string RotationX => GetString("RotationX");

    /// <summary>
    ///   Rotation Y
    /// </summary>
    public static string RotationY => GetString("RotationY");

    /// <summary>
    ///   Rotation Z
    /// </summary>
    public static string RotationZ => GetString("RotationZ");

    /// <summary>
    ///   Un mot de passe est requis.
    /// </summary>
    public static string RequiredPassword => GetString("RequiredPassword");

    /// <summary>
    ///   Un nom d'utilisateur est requis.
    /// </summary>
    public static string RequiredUsername => GetString("RequiredUsername");

    /// <summary>
    ///   Une confirmation du mot de passe est requis.
    /// </summary>
    public static string RequiredVerification => GetString("RequiredVerification");

    /// <summary>
    ///   Sauvegarder
    /// </summary>
    public static string Save => GetString("Save");

    /// <summary>
    ///   Redimensionnement
    /// </summary>
    public static string Scaling => GetString("Scaling");

    /// <summary>
    ///   Sélection Formes
    /// </summary>
    public static string SelectionForme => GetString("SelectionForme");

    /// <summary>
    ///   Sélection Matériaux
    /// </summary>
    public static string SelectionMateriau => GetString("SelectionMateriau");

    /// <summary>
    ///   Supprimer
    /// </summary>
    public static string Supprimer => GetString("Supprimer");

    /// <summary>
    ///   Supprimer les projets
    /// </summary>
    public static string SupprimerProjets => GetString("SupprimerProjets");

    /// <summary>
    ///   Prix Total
    /// </summary>
    public static string TotalPrice => GetString("TotalPrice");

    /// <summary>
    ///   Enfant inconnu de {parent}.
    /// </summary>
    public static string UnknownChild([CanBeNull] object parent)
      => string.Format(
        GetString("UnknownChild", nameof(parent)),
        parent);

    /// <summary>
    ///   Le nom d'utilisateur est déjà utilisé.
    /// </summary>
    public static string UsedUsername => GetString("UsedUsername");

    /// <summary>
    ///   Ce viewport ne support que les manipulateurs de {types}.
    /// </summary>
    public static string ViewportManipulatorSupport([CanBeNull] object types)
      => string.Format(
        GetString("ViewportManipulatorSupport", nameof(types)),
        types);

    /// <summary>
    ///   Volume
    /// </summary>
    public static string Volume => GetString("Volume");

    /// <summary>
    ///   _Diametre
    /// </summary>
    public static string _Diametre => GetString("_Diametre");

    /// <summary>
    ///   _Hauteur
    /// </summary>
    public static string _Height => GetString("_Height");

    /// <summary>
    ///   _Diametre Interne
    /// </summary>
    public static string _InnerDiametre => GetString("_InnerDiametre");

    /// <summary>
    ///   _Longueur
    /// </summary>
    public static string _Length => GetString("_Length");

    /// <summary>
    ///   _Matériau
    /// </summary>
    public static string _Material => GetString("_Material");

    /// <summary>
    ///   _Mot de passe :
    /// </summary>
    public static string _MotDePasse => GetString("_MotDePasse");

    /// <summary>
    ///   _Nom d'utilisateur :
    /// </summary>
    public static string _Utilisateur => GetString("_Utilisateur");

    /// <summary>
    ///   _Vérification du mot de passe
    /// </summary>
    public static string _VerificationMotDePasse => GetString("_VerificationMotDePasse");

    /// <summary>
    ///   L_argeur
    /// </summary>
    public static string _Width => GetString("_Width");

    /// <summary>
    ///   _Ancient mot de passe
    /// </summary>
    public static string _AncientMotDePasse => GetString("_AncientMotDePasse");

    /// <summary>
    ///   Ok
    /// </summary>
    public static string Ok => GetString("Ok");

    /// <summary>
    ///   Échec
    /// </summary>
    public static string Echec => GetString("Echec");

    /// <summary>
    ///   Réussite
    /// </summary>
    public static string Reussite => GetString("Reussite");

    /// <summary>
    ///   Voulez vous sauvegardées les données.
    /// </summary>
    public static string SaveDataInfo => GetString("SaveDataInfo");

    /// <summary>
    ///   Renommer un projet
    /// </summary>
    public static string TitleRenameProject => GetString("TitleRenameProject");

    /// <summary>
    ///   Droit édition
    /// </summary>
    public static string DroitEdition => GetString("DroitEdition");

    /// <summary>
    ///   Retour
    /// </summary>
    public static string Retour => GetString("Retour");

    /// <summary>
    ///   Supprimer le projet.
    /// </summary>
    public static string SupprimerProjet => GetString("SupprimerProjet");

    /// <summary>
    ///   Partage des projets
    /// </summary>
    public static string TitleAccesProjet => GetString("TitleAccesProjet");

    /// <summary>
    ///   L'utilisateur possède déjà ce projet.
    /// </summary>
    public static string UserExistProjet => GetString("UserExistProjet");

    /// <summary>
    ///   L'utilisateur n'existe pas.
    /// </summary>
    public static string UserNotExist => GetString("UserNotExist");

    private static string GetString(string name, params string[] names) => Regex.Replace(
      manager.GetString(name),
      @"\{(\w+)(.*?)\}",
      match => $"{{{ Array.IndexOf(names, match.Groups[1].Value) }{ match.Groups[2].Value }}}");
  }
}
