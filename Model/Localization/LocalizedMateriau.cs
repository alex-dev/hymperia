using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Hymperia.Model.Localization
{
  public class LocalizedMateriau
  {
    public int Id { get; private set; }

    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string StringKey { get; private set; }

    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string CultureKey { get; private set; }

    /// <summary>Le nom du matériau.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string Nom { get; set; }

    public LocalizedMateriau([NotNull] string stringKey, [NotNull] string cultureKey, [NotNull] string nom)
    {
      StringKey = stringKey;
      CultureKey = cultureKey;
      Nom = nom;
    }
  }
}
