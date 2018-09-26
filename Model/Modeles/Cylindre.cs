using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public class Cylindre : ThetaDivForme
  {
    #region Attributes

    internal JsonObject<Point> _Point { get; set; }
    public double Diametre { get; set; }
    public double InnerDiametre { get; set; }

    #endregion

    #region Not Mapped Properties

    [NotNull]
    [NotMapped]
    public Point Point
    {
      get => _Point.Object;
      set => _Point.Object = value;
    }

    [NotNull]
    [NotMapped]
    public Point Centre
    {
      get => new Point
      {
        X = (Origine.X + Point.X) / 2,
        Y = (Origine.Y + Point.Y) / 2,
        Z = (Origine.Z + Point.Z) / 2,
      };
    }

    [NotMapped]
    public double Hauteur
    {
      get => Math.Sqrt(
        Math.Pow(Origine.X - Point.X, 2)
        + Math.Pow(Origine.Y - Point.Y, 2)
        + Math.Pow(Origine.Z - Point.Z, 2));
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
      _Point = new JsonObject<Point>();
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
