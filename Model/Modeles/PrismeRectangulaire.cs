using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public class PrismeRectangulaire : Forme
  {
    #region Attribute

    public JsonObject<Point> Centre { get; set; }
    public double Hauteur { get; set; }
    public double Largeur { get; set; }
    public double Longueur { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    protected override double Volume
    {
      get => Hauteur * Largeur * Longueur;
    }

    #endregion

    #region Constructors

    public PrismeRectangulaire([NotNull] Materiau materiau) : base(materiau) { }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"Prisme rectangulaire { Id }: { Centre } - { Materiau }";
    }

    #endregion
  }
}
