using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Acces
  {
    public enum Droit { Lecture, LectureEcriture, Possession }

    #region Attributes

    /// <summary>L'utilisateur pouvant accéder au projet.</summary>
    [NotNull]
    [Required]
    public Projet Projet { get; private set; }

    [NotNull]
    [Required]
    public Utilisateur Utilisateur { get; private set; }

    public Droit DroitDAcces { get; set; }

    [NotMapped]
    public bool EstPropriétaire
    {
      get => DroitDAcces >= Droit.Possession;
    }

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
