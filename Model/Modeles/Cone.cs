using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public class Cone : ThetaDivForme
  {
    #region Attributes

    public JsonObject<Point> Origine { get; set; }
    public double Hauteur { get; set; }
    public double RayonBase { get; set; }
    public double RayonTop { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    public override double Volume
    {
      get => RayonTop == 0 ? FullVolume : PartialVolume;
    }

    [NotMapped]
    private double FullVolume
    {
      get => Aire * Hauteur / 3;
    }

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
    private double FullHauteur
    {
      get => Hauteur / (RayonBase - RayonTop) * RayonBase;
    }

    [NotMapped]
    private double AngleBase
    {
      get => Math.Atan(Hauteur / (RayonBase - RayonTop));
    }

    [NotMapped]
    protected override double Aire
    {
      get => CalculeAire(RayonBase / 2);
    }

    [NotMapped]
    protected double AireTop
    {
      get => CalculeAire(RayonTop / 2);
    }

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Cone() : this(null) { }

    /// <inheritdoc />
    public Cone([NotNull] Materiau materiau) : base(materiau)
    {
      RayonTop = 0;
      RayonBase = 1;
      Hauteur = 1;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Cylindre { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
