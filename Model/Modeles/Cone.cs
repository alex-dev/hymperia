using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Cone : ThetaDivForme
  {
    #region Properties

    public double Hauteur { get; set; } = 1;
    public double RayonBase { get; set; } = 1;
    public double RayonTop { get; set; }

    #endregion

    #region Not Mapped Properties

    /// inheritdoc/>
    [NotMapped]
    public override double Volume => RayonTop == 0 ? FullVolume : PartialVolume;

    [NotMapped]
    private double FullVolume => Aire * Hauteur / 3;

    [NotMapped]
    private double PartialVolume
    {
      get
      {
        double hauteur = FullHauteur;
        return (Aire * hauteur - AireTop * (hauteur - Hauteur)) / 3;
      }
    }

    [NotMapped]
    private double FullHauteur => Hauteur / (RayonBase - RayonTop) * RayonBase;

    [NotMapped]
    private double AngleBase => Math.Atan(Hauteur / (RayonBase - RayonTop));

    [NotMapped]
    protected override double Aire => CalculeAire(RayonBase / 2);

    [NotMapped]
    protected double AireTop => CalculeAire(RayonTop / 2);

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Cone() : this(null) { }

    /// inheritdoc/>
    public Cone([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default) 
      : base(materiau, point, quaternion) { }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => ResourcesExtension.ConeToString(Id, Origine, Rotation);

    #endregion
  }
}
