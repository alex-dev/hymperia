using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Cylindre : ThetaDivForme
  {
    #region Attributes

    public double Hauteur { get; set; }
    public double Diametre { get; set; }
    public double InnerDiametre { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <inheritdoc />
    [NotMapped]
    public override double Volume => Aire * Hauteur;

    [NotMapped]
    protected override double Aire => CalculeAire(Diametre / 2)
        - (InnerDiametre == 0 ? 0 : CalculeAire(InnerDiametre / 2));

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Cylindre() : this(null) { }

    /// <inheritdoc />
    public Cylindre([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default)
      : base(materiau, point, quaternion)
    {
      Diametre = Hauteur = 1;
      InnerDiametre = 0;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"Cylindre { Id }: { Origine } - { Materiau }";

    #endregion
  }
}
