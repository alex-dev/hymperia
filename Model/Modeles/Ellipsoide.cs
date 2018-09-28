using System;
using System.ComponentModel.DataAnnotations.Schema;
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
    public Ellipsoide([NotNull] Materiau materiau): base(materiau)
    {
      RayonX = RayonY = RayonZ = 1;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Ellipsoïde { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
