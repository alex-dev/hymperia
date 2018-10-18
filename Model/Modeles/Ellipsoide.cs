using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Ellipsoide : Forme
  {
    #region Properties

    public double RayonX { get; set; } = 1;
    public double RayonY { get; set; } = 1;
    public double RayonZ { get; set; } = 1;
    public int PhiDiv { get; set; } = 30;
    public int ThetaDiv { get; set; } = 60;

    #endregion

    #region Not Mapped Properties

    /// inheritdoc/>
    [NotMapped]
    public override double Volume => (4.0 * Math.PI * RayonX * RayonY * RayonZ) / 3.0;

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Ellipsoide() : this(null) { }

    /// inheritdoc/>
    public Ellipsoide([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default) 
      : base(materiau, point, quaternion) { }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Ellipsoïde { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
