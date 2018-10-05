using System;
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

    /// <summary>La <see cref="Brush"/> utilisée pour représenter ce <see cref="Materiau"/>.</summary>
    [NotNull]
    [Required]
    protected internal JsonObject<Brush> _Fill { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>La <see cref="Brush"/> utilisée pour représenter ce <see cref="Materiau"/>.</summary>
    [NotMapped]
    public Brush Fill
    {
      get => _Fill.Object;
      private set => _Fill.Object = value;
    }

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    internal Materiau() : this(null, "") { }

    /// <param name="fill">La <see cref="Brush"/> utilisée pour représenter ce <see cref="Materiau"/>.</param>
    /// <param name="nom">Le nom du matériau.</param>
    /// <param name="prix">Le prix du matériaux par volume.</param>
    public Materiau([NotNull] Brush fill, [NotNull] string nom)
    {
      Id = default;
      _Fill = new JsonObject<Brush>(fill);
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
