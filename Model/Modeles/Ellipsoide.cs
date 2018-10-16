using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Ellipsoide : Forme
  {
    #region Attributes

    public double RayonX { get; set; }
    public double RayonY { get; set; }
    public double RayonZ { get; set; }
    public int PhiDiv { get; set; }
    public int ThetaDiv { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    public override double Volume => (4.0 * Math.PI * RayonX * RayonY * RayonZ) / 3.0;

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Ellipsoide() : this(null) { }

    /// <inheritdoc />
    public Ellipsoide([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default) 
      : base(materiau, point, quaternion)
    {
      RayonX = RayonY = RayonZ = 1;
      PhiDiv = 30;
      ThetaDiv = 60;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Ellipsoïde { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
