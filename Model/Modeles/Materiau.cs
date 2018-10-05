using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Materiau : IIdentity
  {
    #region Properties

    /// <summary>La clé primaire du matériau.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom du matériau.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    public string Nom { get; private set; }

    /// <summary>Le prix du matériaux par volume.</summary>
    public double Prix { get; private set; }

    /// <summary>Le <see cref="Color.Name"/> utilisée pour représenter cette <see cref="Color"/>.</summary>
    [NotNull]
    [Required]
    [EnumDataType(typeof(KnownColor))]
    protected internal KnownColor Color { get; private set; }

    #endregion

    #region Not Mapped Properties

    [CanBeNull]
    [NotMapped]
    private SolidBrush CachedBrush { get; set; }

    /// <summary>La <see cref="SolidBrush"/> utilisée pour représenter ce <see cref="Materiau"/>.</summary>
    [NotNull]
    [NotMapped]
    public SolidBrush Fill
    {
      get => CachedBrush ?? (CachedBrush = new SolidBrush(System.Drawing.Color.FromKnownColor(Color)));
      private set
      {
        Color = value.Color.ToKnownColor();
        CachedBrush = value;
      }
    }

    #endregion

    #region Constructors

    /// <param name="color">Le nom de la couleur.</param>
    /// <param name="nom">Le nom du matériau.</param>
    public Materiau(KnownColor color, [NotNull] string nom)
    {
      Id = default;
      Color = color;
      Nom = nom;
    }

    /// <param name="fill">La <see cref="SolidBrush"/> utilisée pour représenter ce <see cref="Materiau"/>.</param>
    /// <param name="nom">Le nom du matériau.</param>
    public Materiau([NotNull] SolidBrush fill, [NotNull] string nom)
    {
      Id = default;
      Fill = fill;
      Nom = nom;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"{ Id } - { Nom }: { Prix }";

    #endregion
  }
}
