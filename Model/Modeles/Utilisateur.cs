﻿using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Utilisateur
  {
    #region Attributes
    
    [Key]
    [NotNull]
    public int Id { get; private set; }

    [NotNull]
    [MaxLength(25)]
    [Required]
    public string Nom { get; private set; }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"{ Id } - { Nom }";
    }

    #endregion
  }
}
