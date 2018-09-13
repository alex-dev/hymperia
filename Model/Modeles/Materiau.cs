using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Materiau
  {
    #region Attributes

    /// <summary>La clé primaire du matériau.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom d'utilisateur.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string Nom { get; private set; }

    public double Prix { get; private set; }

    #endregion

    #region Constructors
    #endregion

    #region Methods
    #endregion

    #region ToString
    #endregion
  }
}
