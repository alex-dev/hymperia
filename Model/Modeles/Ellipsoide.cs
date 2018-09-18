using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public class Ellipsoide : Forme
  {
    #region Attribute

    public JsonObject<Point> Centre { get; set; }
    public double RayonX { get; set; }
    public double RayonY { get; set; }
    public double RayonZ { get; set; }
    public int PhiDiv { get; set; }
    public int ThetaDiv { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    protected override double Volume
    {
      get => (4.0 * Math.PI * RayonX * RayonY * RayonZ) / 3.0;
    }

    #endregion

    #region Constructors

    public Ellipsoide([NotNull] Materiau materiau): base(materiau) { }

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
