using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Acces
  {
    public enum Droit { Lecture, LectureEcriture, Possession }

    #region Properties

    /// <summary>Le projet protégé par ce droit d'accès.</summary>
    [NotNull]
    [Required]
    public Projet Projet { get; private set; }

    /// <summary>L'utilisateur pouvant accéder au <see cref="Projet"/>.</summary>
    [NotNull]
    [Required]
    public Utilisateur Utilisateur { get; private set; }

    /// <summary>Le droit d'accès de l'<see cref="Utilisateur"/> sur le <see cref="Projet"/>.</summary>
    public Droit DroitDAcces { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Indique si l'<see cref="Utilisateur"/> est le propriétaire du <see cref="Projet"/>.</summary>
    [NotMapped]
    public bool EstPropriétaire
    {
      get => DroitDAcces >= Droit.Possession;
    }

    /// <summary>Indique si l'<see cref="Utilisateur"/> peut modifier le <see cref="Projet"/>.</summary>
    [NotMapped]
    public bool PeutModifier
    {
      get => DroitDAcces >= Droit.LectureEcriture;
    }

    #endregion

    #region Constructors

    /// <param name="utilisateur">L'utilisateur pouvant accéder au projet.</param>
    /// <param name="projet">Le projet auquel l'utilisateur peut accéder.</param>
    /// <param name="droit">Le droit d'accès.</param>
    public Acces(Projet projet, Utilisateur utilisateur, Droit droit)
    {
      Projet = projet;
      Utilisateur = utilisateur;
      DroitDAcces = droit;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"{ Utilisateur.Nom } - { Projet.Nom }: { DroitDAcces }";
    }

    #endregion
  }
}
