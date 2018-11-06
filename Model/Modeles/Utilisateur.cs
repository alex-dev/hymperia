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
  public class Utilisateur : IIdentity, IEquatable<Utilisateur>
  {
    #region Properties

    /// <summary>La clé primaire de l'utilisateur.</summary>
    public int Id { get; private set; }

    /// <summary>Les accès aux projets de l'utilisateur.</summary>
    /// <remarks>Modifiable, mais privé.</remarks>
    [ItemNotNull]
    internal List<Acces> _Acces { get; private set; } = new List<Acces> { };

    /// <summary>Le nom d'utilisateur.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom d'utilisateur ne peut pas être vide.")]
    public string Nom { get; private set; }

    /// <summary>Le mot de passe encrypté de l'utilisateur.</summary>
    /// <remarks>Probablement hassed avec BCrypt.</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le mot de passe ne peut pas être vide.")]
    public string MotDePasse { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Les accès aux projets de l'utilisateur.</summary>
    /// <remarks>Utilise une <see cref="ReadOnlyCollection{Acces}"/> pour éviter les modifications non contrôlées.</remarks>
    [NotMapped]
    [ItemNotNull]
    public IReadOnlyCollection<Acces> Acces => new ReadOnlyCollection<Acces>(_Acces);

    #endregion

    #region Constructors

    /// <param name="nom">Le nom d'utilisateur.</param>
    /// <param name="motDePasse">Le mot de passe encrypté de l'utilisateur.</param>
    public Utilisateur([NotNull][MinLength(1)] string nom, [NotNull] string motDePasse)
    {
      Nom = nom;
      MotDePasse = motDePasse;
    }

    #endregion

    #region Methods

    /// <summary></summary>
    /// <param name="nom"></param>
    /// <returns></returns>
    [NotNull]
    public Projet CreerProjet([NotNull] string nom)
    {
      var projet = new Projet(nom);
      RecevoirProjet(projet);

      return projet;
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    public void RecevoirProjet([NotNull] Projet projet) =>
      _Acces.Add(new Acces(projet, this, Modeles.Acces.Droit.Possession));

    /// <summary></summary>
    /// <param name="projet"></param>
    /// <remarks>Si l'utilisateur n'est pas track par <see cref="DatabaseContext"/>, il est impossible de supprimer ses acces. De plus, si le projet est possédé par l'utilisateur et qu'une suppression du projet est désirée, il faut
    /// indiqué au <see cref="DatabaseContext"/> de le retirer explicitement. Sinon, l'utilisateur perdra l'accès au
    /// projet, mais les autres utilisateurs en auront toujours une copie.</remarks>
    public void RetirerProjet([NotNull] Projet projet)
    {
      var acces = Acces.SingleOrDefault(a => a.Projet == projet);
      _Acces.Remove(acces);
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    /// <returns></returns>
    [Pure]
    public bool EstPropietaireDe([NotNull] Projet projet)
    {
      var acces = Acces.FirstOrDefault(_acces => _acces.Projet.Id == projet.Id);
      return acces is Acces && acces.EstPropriétaire;
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    /// <returns></returns>
    [Pure]
    public bool PeutModifier([NotNull] Projet projet) => throw new System.NotImplementedException();

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Resources.UtilisateurToString(Id, Nom);

    #endregion

    #region IEquatable<Utilisateur>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Utilisateur);
    [Pure]
    public bool Equals(Utilisateur other) => IdentityEqualityComparer<Utilisateur>.StaticEquals(this, other) && Nom == other.Nom;
    [Pure]
    public override int GetHashCode() => IdentityEqualityComparer<Utilisateur>.StaticGetHashCode(this);

    #endregion
  }
}
