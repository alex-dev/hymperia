using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Materiau
  {
    #region Properties

    /// <summary>La clé primaire du matériau.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom du matériau.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string Nom { get; private set; }

    /// <summary>Le prix du matériaux par volume.</summary>
    public double Prix { get; private set; }

    #endregion

    #region Constructors

    /// <param name="nom">Le nom du matériau.</param>
    /// <param name="prix">Le prix du matériaux par volume.</param>
    public Materiau([NotNull] string nom, double prix)
    {
      Nom = nom;
      Prix = prix;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"{ Id } - { Nom }: { Prix }";

    #endregion
  }
}
