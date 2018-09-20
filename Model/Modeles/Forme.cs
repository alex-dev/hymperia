using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public abstract class Forme
  {
    #region Attribute

    /// <summary>La clé primaire de la forme.</summary>
    public int Id { get; private set; }

    /// <summary>Le matériau de la forme.</summary>
    [NotNull]
    [Required]
    public Materiau Materiau { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Le prix de la forme selon son <see cref="Materiau"/>.</summary>
    [NotMapped]
    public double Prix
    {
      get => Materiau.Prix * Volume;
    }

    /// <summary>Le volume de la forme.</summary>
    [NotMapped]
    public abstract double Volume { get; }

    #endregion

    #region Constructors

    /// <param name="materiau">Le matériau composant la forme.</param>
    public Forme([NotNull] Materiau materiau)
    {
      Id = default;
      Materiau = materiau;
    }

    #endregion

    #region Methods
    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public abstract override string ToString();

    #endregion
  }
}
