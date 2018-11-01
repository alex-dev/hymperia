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
    ///   et
    /// </summary>
    public static string And => GetString("And");

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
    ///   Impossible d'inverser un processus détérioratif.
    /// </summary>
    public static string DestructiveConversion => GetString("DestructiveConversion");

    /// <summary>
    ///   , 
    /// </summary>
    public static string EnumerationSeparator => GetString("EnumerationSeparator");

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
    ///   Prix
    /// </summary>
    public static string Price => GetString("Price");

    /// <summary>
    ///   Sauvegarder
    /// </summary>
    public static string Save => GetString("Save");

    /// <summary>
    ///   Redimensionnement
    /// </summary>
    public static string Scaling => GetString("Scaling");

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

    private static string GetString(string name, params string[] names) => Regex.Replace(
      manager.GetString(name),
      @"\{(\w+)(.*?)\}",
      match => $"{{{ Array.IndexOf(names, match.Groups[1].Value) }{ match.Groups[2].Value }}}");
  }
}
