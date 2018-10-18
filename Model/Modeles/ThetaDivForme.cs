using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public abstract class ThetaDivForme : Forme
  {
    #region Attribute

    public int ThetaDiv { get; set; } = 60;

    #endregion

    #region Not Mapped Properties

    [NotMapped]
    protected abstract double Aire { get; }

    [NotMapped]
    private double Tan180N => Math.Tan(Math.PI / Cotes);

    [NotMapped]
    private double AngleBaseInterne => ((double)(2 - Cotes) / (2 * Cotes)).ConvertToRadian();

    [NotMapped]
    private int Cotes => ThetaDiv - 1;

    #endregion

    #region Constructors

    /// inheritdoc/>
    public ThetaDivForme([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default) 
      : base(materiau, point, quaternion) { }

    #endregion

    #region Methods

    [Pure]
    protected double CalculeAire(double rayon) => ThetaDiv * CalculeDemiDimensionCote(rayon) / Tan180N;

    [Pure]
    private double CalculeDemiDimensionCote(double rayon) => rayon * Math.Cos(AngleBaseInterne);

    #endregion
  }
}
