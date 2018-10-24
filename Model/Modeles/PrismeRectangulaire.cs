using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Modeles.JsonObject;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class PrismeRectangulaire : Forme
  {
    #region Properties

    public double Hauteur { get; set; } = 1;
    public double Largeur { get; set; } = 1;
    public double Longueur { get; set; } = 1;

    #endregion

    #region Not Mapped Properties

    /// inheritdoc/>
    [NotMapped]
    public override double Volume => Hauteur * Largeur * Longueur;

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal PrismeRectangulaire() : this(null) { }

    /// inheritdoc/>
    public PrismeRectangulaire([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default)
      : base(materiau, point, quaternion) { }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => ResourcesExtension.PrismToString(Id, Origine, Rotation);

    #endregion
  }
}
