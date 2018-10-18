using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Modeles
{
  public abstract class Forme : IIdentity
  {
    #region Properties

    /// <summary>La clé primaire de la forme.</summary>
    public int Id { get; private set; }

    /// <summary>Le matériau de la forme.</summary>
    [NotNull]
    [Required]
    public Materiau Materiau { get; set; }

    [NotNull]
    [Required]
    internal JsonObject<Point> _Origine { get; set; }

    [NotNull]
    [Required]
    internal JsonObject<Quaternion> _Rotation { get; set; }

    #endregion

    #region Not Mapped Properties

    [NotNull]
    [NotMapped]
    public Point Origine
    {
      get => _Origine.Object;
      set => _Origine.Object = value;
    }

    [NotNull]
    [NotMapped]
    public Quaternion Rotation
    {
      get => _Rotation.Object;
      set => _Rotation.Object = value;
    }

    /// <summary>Le prix de la forme selon son <see cref="Materiau"/>.</summary>
    [NotMapped]
    public double Prix => Materiau.Prix * Volume;

    /// <summary>Le volume de la forme.</summary>
    [NotMapped]
    public abstract double Volume { get; }

    #endregion

    #region Constructors

    /// <param name="materiau">Le matériau composant la forme.</param>
    public Forme([NotNull] Materiau materiau, [NotNull] Point point = default, [NotNull] Quaternion quaternion = default)
    {
      Id = default;
      Materiau = materiau;
      _Origine = new JsonObject<Point>(point ?? Point.Center);
      _Rotation = new JsonObject<Quaternion>(quaternion ?? Quaternion.Identity);
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public abstract override string ToString();

    #endregion
  }
}
