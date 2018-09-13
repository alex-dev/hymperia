using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Projet
  {
    #region Properties

    /// <summary>La clé primaire du projet.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom du projet.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du projet ne peut pas être vide.")]
    public string Nom { get; private set; }

    /// <summary>Les formes dans le projet.</summary>
    /// <remarks>Modifiable, mais privé.</remarks>
    [ItemNotNull]
    private IList<Forme> _Formes { get; set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Les formes dans le projet.</summary>
    /// <remarks>Utilise une <see cref="ReadOnlyCollection{Forme}"/> pour éviter les modifications non contrôlées.</remarks>
    [NotMapped]
    [ItemNotNull]
    public IReadOnlyCollection<Forme> Formes
    {
      get => new ReadOnlyCollection<Forme>(_Formes);
    }

    /// <summary>Le prix du plan.</summary>
    [NotMapped]
    public double Prix
    {
      get => throw new System.NotImplementedException();
    }

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
    public void AjouterForme([NotNull] Forme forme)
    {
      throw new System.NotImplementedException();
    }

    /// <summary></summary>
    /// <param name="forme"></param>
    public void SupprimerForme([NotNull] Forme forme)
    {
      throw new System.NotImplementedException();
    }


    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"{ Id } - { Nom }: { Formes.Count } pièces";
    }

    #endregion
  }
}
