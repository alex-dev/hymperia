﻿// <auto-generated />

using System;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Hymperia.Model.Properties
{
  /// <summary>This class wraps <see cref="ResourceManager" /> to ease usage.</summary>
  public static partial class Resources
  {
    private static void RaiseStaticPropertiesChanged()
    {
      RaiseStaticPropertyChanged(nameof(Cone));
      RaiseStaticPropertyChanged(nameof(Cylinder));
      RaiseStaticPropertyChanged(nameof(Ellipsoid));
      RaiseStaticPropertyChanged(nameof(Prism));
    }

    private static readonly ResourceManager manager =
      new ResourceManager("Hymperia.Model.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);

    /// <summary>
    ///   {username} - {project}: {acces}
    /// </summary>
    public static string AccesToString([CanBeNull] object username, [CanBeNull] object project, [CanBeNull] object acces) =>
      string.Format(
        GetString("AccesToString", nameof(username), nameof(project), nameof(acces)),
        username, project, acces);

    /// <summary>
    ///   Cone
    /// </summary>
    public static string Cone => GetString("Cone");

    /// <summary>
    ///   Cylindre
    /// </summary>
    public static string Cylinder => GetString("Cylinder");

    /// <summary>
    ///   Ellipsoïde
    /// </summary>
    public static string Ellipsoid => GetString("Ellipsoid");

    /// <summary>
    ///   {type} #{id}: {origin} - {rotation}
    /// </summary>
    public static string FormeToString([CanBeNull] object type, [CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      string.Format(
        GetString("FormeToString", nameof(type), nameof(id), nameof(origin), nameof(rotation)),
        type, id, origin, rotation);

    /// <summary>
    ///   {id} - {nom}: {prix:C}
    /// </summary>
    public static string MateriauToString([CanBeNull] object id, [CanBeNull] object nom, [CanBeNull] object prix) =>
      string.Format(
        GetString("MateriauToString", nameof(id), nameof(nom), nameof(prix)),
        id, nom, prix);

    /// <summary>
    ///   Prisme rectangulaire
    /// </summary>
    public static string Prism => GetString("Prism");

    /// <summary>
    ///   {id} - {nom}
    /// </summary>
    public static string ProjetToString([CanBeNull] object id, [CanBeNull] object nom) =>
      string.Format(
        GetString("ProjetToString", nameof(id), nameof(nom)),
        id, nom);

    /// <summary>
    ///   {id} - {nom}
    /// </summary>
    public static string UtilisateurToString([CanBeNull] object id, [CanBeNull] object nom) =>
      string.Format(
        GetString("UtilisateurToString", nameof(id), nameof(nom)),
        id, nom);

    private static string GetString(string name, params string[] names) => Regex.Replace(
      manager.GetString(name),
      @"\{(\w+)(.*?)\}",
      match => $"{{{ Array.IndexOf(names, match.Groups[1].Value) }{ match.Groups[2].Value }}}");
  }
}
