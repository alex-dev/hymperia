using System;
using System.ComponentModel.DataAnnotations;
using Hymperia.Model.Identity;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Model.Localization
{
  public class LocalizedMateriau : ILocalizedIdentity<Materiau>, IEquatable<LocalizedMateriau>
  {
    /// <remarks>Primary Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    [MaxLength(100)]
    public string StringKey { get; private set; }

    /// <remarks>Primary Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    [MaxLength(5)]
    public string CultureKey { get; private set; }

    /// <summary>Le nom du matériau.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    [MaxLength(100)]
    public string Nom { get; set; }

    public LocalizedMateriau([NotNull] string stringKey, [NotNull] string cultureKey, [NotNull] string nom)
    {
      StringKey = stringKey;
      CultureKey = cultureKey;
      Nom = nom;
    }

    #region IEquatable<LocalizedMateriau>

    public override bool Equals(object obj) => Equals(obj as LocalizedMateriau);
    public bool Equals(LocalizedMateriau other) => LocalizedIdentityEqualityComparer<Materiau>.StaticEquals(this, other);
    public override int GetHashCode() => LocalizedIdentityEqualityComparer<Materiau>.StaticGetHashCode(this);

    #endregion
  }
}
