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

    /// <summary></summary>
    [NotNull]
    [Required]
    public Materiau Materiau { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Le prix de la forme selon son <see cref="Materiau"/>.</summary>
    [NotMapped]
    public double Prix
    {
      get => throw new System.NotImplementedException();
    }

    /// <summary>Le volume de la forme.</summary>
    [NotMapped]
    protected double Volume
    {
      get
      {
        throw new System.NotImplementedException();
      }
    }

    #endregion

    #region Constructors

    public Forme([NotNull] Materiau materiau)
    {
      Materiau = materiau;
    }

    #endregion

    #region Methods
    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"{ Id }: { Materiau.Nom }";
    }

    #endregion
  }
}
