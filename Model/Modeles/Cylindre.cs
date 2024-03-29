﻿using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Cylindre : ThetaDivForme
  {
    #region Properties

    public double Hauteur { get; set; } = 1;
    public double Diametre { get; set; } = 1;
    public double InnerDiametre { get; set; }

    #endregion

    #region Not Mapped Properties

    /// inheritdoc/>
    [NotMapped]
    public override double Volume => Aire * Hauteur;

    [NotMapped]
    protected override double Aire => CalculeAire(Diametre / 2)
        - (InnerDiametre == 0 ? 0 : CalculeAire(InnerDiametre / 2));

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Cylindre() : this(null) { }

    /// inheritdoc/>
    public Cylindre([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default)
      : base(materiau, point, quaternion) { }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Resources.CylinderToString(Id, Origine, Rotation);

    #endregion
  }
}
