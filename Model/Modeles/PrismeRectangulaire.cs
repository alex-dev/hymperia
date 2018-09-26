using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class PrismeRectangulaire : Forme
  {
    #region Attributes

    public double Hauteur { get; set; }
    public double Largeur { get; set; }
    public double Longueur { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    public override double Volume
    {
      get => Hauteur * Largeur * Longueur;
    }

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal PrismeRectangulaire() : this(null) { }

    /// <inheritdoc />
    public PrismeRectangulaire([NotNull] Materiau materiau) : base(materiau)
    {
      Hauteur = Largeur = Longueur = 1;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Prisme rectangulaire { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
