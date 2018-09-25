using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public class Cylindre : ThetaDivForme
  {
    #region Attributes

    public JsonObject<Point> Point1 { get; set; }
    public JsonObject<Point> Point2 { get; set; }
    public double Diametre { get; set; }
    public double InnerDiametre { get; set; }

    #endregion

    #region Not Mapped Properties

    [NotMapped]
    public JsonObject<Point> Centre
    {
      get => new Point(
        (Point1.Object.X + Point2.Object.X) / 2,
        (Point1.Object.Y + Point2.Object.Y) / 2,
        (Point1.Object.Z + Point2.Object.Z) / 2);
    }

    [NotMapped]
    public double Hauteur
    {
      get => Math.Sqrt(
        Math.Pow(Point1.Object.X - Point2.Object.X, 2)
        + Math.Pow(Point1.Object.Y - Point2.Object.Y, 2)
        + Math.Pow(Point1.Object.Z - Point2.Object.Z, 2));
    }

    /// <inheritdoc />
    [NotMapped]
    public override double Volume
    {
      get => Aire * Hauteur;
    }

    [NotMapped]
    protected override double Aire
    {
      get => CalculeAire(Diametre / 2)
        - (InnerDiametre == 0 ? 0 : CalculeAire(InnerDiametre / 2));
    }

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Cylindre() : this(null) { }

    /// <inheritdoc />
    public Cylindre([NotNull] Materiau materiau) : base(materiau)
    {
      Diametre = 1;
      InnerDiametre = 0;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Cylindre { Id }: { Centre } - { Materiau }";

    #endregion
  }
}
