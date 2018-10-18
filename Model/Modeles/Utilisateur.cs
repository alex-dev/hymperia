using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Utilisateur : IIdentity
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
      throw new System.NotImplementedException();
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    public void RecevoirProjet([NotNull] Projet projet)
    {
      throw new System.NotImplementedException();
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    public void RetirerProjet([NotNull] Projet projet)
    {
      throw new System.NotImplementedException();
    }

    /// <summary></summary>
    /// <param name="projet"></param>
    /// <returns></returns>
    [Pure]
    public bool EstPropietaireDe([NotNull] Projet projet) => throw new System.NotImplementedException();

    /// <summary></summary>
    /// <param name="projet"></param>
    /// <returns></returns>
    [Pure]
    public bool PeutModifier([NotNull] Projet projet) => throw new System.NotImplementedException();

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => $"{ Id } - { Nom }";

    #endregion
  }
}
