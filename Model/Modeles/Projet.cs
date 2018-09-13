using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Projet
  {
    #region Attributes

    /// <summary>La clé primaire du projet.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom du projet.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du projet ne peut pas être vide.")]
    public string Nom { get; private set; }



    /// <summary>Le prix du plan.</summary>
    [NotMapped]
    public double Prix
    {
      get => throw new System.NotImplementedException();
    }

    #endregion

    #region Constructors
    #endregion

    #region Methods
    #endregion

    #region ToString
    #endregion
  }
}
