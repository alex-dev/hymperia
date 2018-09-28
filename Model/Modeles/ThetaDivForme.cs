using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public abstract class ThetaDivForme : Forme
  {
    #region Attribute

    public int ThetaDiv { get; set; }

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

    /// <inheritdoc />
    public ThetaDivForme([NotNull] Materiau materiau) : base(materiau)
    {
      ThetaDiv = 60;
    }

    #endregion

    #region Methods

    protected double CalculeAire(double rayon) => ThetaDiv * CalculeDemiDimensionCote(rayon) / Tan180N;

    private double CalculeDemiDimensionCote(double rayon) => rayon * Math.Cos(AngleBaseInterne);

    #endregion
  }
}
