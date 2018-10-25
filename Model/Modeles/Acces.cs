using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Acces : IEquatable<Acces>
  {
    public enum Droit { Lecture, LectureEcriture, Possession }

    #region Properties

    /// <summary>La clé primaire de l'accès.</summary>
    public int Id { get; private set; }

    /// <summary>Le projet protégé par ce droit d'accès.</summary>
    [NotNull]
    [Required]
    public Projet Projet { get; private set; }

    /// <summary>L'utilisateur pouvant accéder au <see cref="Projet"/>.</summary>
    [NotNull]
    [Required]
    public Utilisateur Utilisateur { get; private set; }

    /// <summary>Le droit d'accès de l'<see cref="Utilisateur"/> sur le <see cref="Projet"/>.</summary>
    [EnumDataType(typeof(Droit))]
    public Droit DroitDAcces { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Indique si l'<see cref="Utilisateur"/> est le propriétaire du <see cref="Projet"/>.</summary>
    [NotMapped]
    public bool EstPropriétaire => DroitDAcces >= Droit.Possession;

    /// <summary>Indique si l'<see cref="Utilisateur"/> peut modifier le <see cref="Projet"/>.</summary>
    [NotMapped]
    public bool PeutModifier => DroitDAcces >= Droit.LectureEcriture;

    #endregion

    #region Constructors

    /// <summary>Constructeur pour EFCore. Ne pas utiliser directement.</summary>
    /// <param name="droitDAcces"></param>
    internal Acces(Droit droitDAcces) : this(null, null, droitDAcces) { }

    /// <param name="utilisateur">L'utilisateur pouvant accéder au projet.</param>
    /// <param name="projet">Le projet auquel l'utilisateur peut accéder.</param>
    /// <param name="droit">Le droit d'accès.</param>
    public Acces([NotNull] Projet projet, [NotNull] Utilisateur utilisateur, Droit droit)
    {
      Projet = projet;
      Utilisateur = utilisateur;
      DroitDAcces = droit;
    }

    public Acces(Projet projet, Utilisateur utilisateur)
    {
      Projet = projet;
      Utilisateur = utilisateur;
    }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Resources.AccesToString(Utilisateur.Nom, Projet.Nom, DroitDAcces);

    #endregion

    #region IEquatable<Acces>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Acces);
    [Pure]
    public bool Equals(Acces other) => Projet.Equals(other?.Projet) && Utilisateur.Equals(other?.Utilisateur);
    [Pure]
    public override int GetHashCode()
    {
      var hashCode = -155109889;
      hashCode = hashCode * -1521134295 + Projet.GetHashCode();
      hashCode = hashCode * -1521134295 + Utilisateur.GetHashCode();
      return hashCode;
    }

    #endregion
  }
}
