using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Hymperia.Model.Identity;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Projet : IIdentity, IEquatable<Projet>
  {
    #region Properties

    /// <summary>La clé primaire du projet.</summary>
    public int Id { get; private set; }

    /// <summary>Les formes dans le projet.</summary>
    /// <remarks>Modifiable, mais privé.</remarks>
    [ItemNotNull]
    internal List<Forme> _Formes { get; } = new List<Forme> { };

    /// <summary>Le nom du projet.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du projet ne peut pas être vide.")]
    [MaxLength(250)]
    public string Nom { get; private set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Les formes dans le projet.</summary>
    /// <remarks>Utilise une <see cref="ReadOnlyCollection{Forme}"/> pour éviter les modifications non contrôlées.</remarks>
    [NotMapped]
    [ItemNotNull]
    public IReadOnlyCollection<Forme> Formes => new ReadOnlyCollection<Forme>(_Formes);

    /// <summary>Le prix du plan.</summary>
    [NotMapped]
    public double Prix => Formes.Sum(forme => forme.Prix);

    /// <summary>Le prix du plan séparé par matériaux.</summary>
    [NotMapped]
    public IEnumerable<KeyValuePair<Materiau, double>> PrixMateriaux =>
     from forme in Formes
     group forme.Prix by forme.Materiau into pair
     select new KeyValuePair<Materiau, double>(pair.Key, pair.Sum());

    /// <summary>Le prix du plan séparé par formes.</summary>
    [NotMapped]
    public IEnumerable<KeyValuePair<int, double>> PrixFormes =>
     from forme in Formes
     select new KeyValuePair<int, double>(forme.Id, forme.Prix);

    #endregion

    #region Constructors

    /// <param name="name">Le nom du projet.</param>
    public Projet([NotNull] string nom)
    {
      Nom = nom;
    }

    #endregion

    #region Methods

    /// <summary></summary>
    /// <param name="forme"></param>
    public void AjouterForme([NotNull] Forme forme) => _Formes.Add(forme);

    /// <summary></summary>
    /// <param name="forme"></param>
    public void SupprimerForme([NotNull] Forme forme) => _Formes.Remove(forme);

    /// <summary>
    /// Renomme un projet.
    /// </summary>
    /// <param name="nom">Le nouveau nom pour le projet.</param>
    public void RenommerProjet([NotNull] string nom) => Nom = nom;

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Resources.ProjetToString(Id, Nom);

    #endregion

    #region IEquatable<Projet>

    [Pure]
    public static bool operator ==(Projet left, Projet right) => left is Projet && right is Projet && left.Equals(right);
    [Pure]
    public static bool operator !=(Projet left, Projet right) => !(left == right);

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Projet);
    [Pure]
    public bool Equals(Projet other) => IdentityEqualityComparer<Projet>.StaticEquals(this, other) && Nom == other.Nom;
    [Pure]
    public override int GetHashCode() => IdentityEqualityComparer<Projet>.StaticGetHashCode(this);

    #endregion

  }
}
