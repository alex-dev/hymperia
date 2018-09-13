using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [MinLength(1, ErrorMessage = "Le nom d'utilisateur ne peut pas être vide.")]
    [Required]
    public string Nom { get; private set; }

    [NotNull]
    [MinLength(1, ErrorMessage = "Le mot de passe ne peut pas être vide.")]
    [Required]
    public string MotDePasse { get; set; }


    public IEnumerable<Acces> Acces { get; private set; }

    #endregion

    #region Constructors

    /// <param name="nom">Le nom d'utilisateur.</param>
    /// <param name="motDePasse">Le mot de passe de l'utilisateur. { <code>!string.IsNullOrWhitespace()</code> }</param>
    public Utilisateur([NotNull][MinLength(1)] string nom, [NotNull] string motDePasse)
    {
      Nom = nom;
      MotDePasse = motDePasse;
    }

    #endregion

    #region Methods

    public Acces CreerProjet([NotNull] string nom)
    {
      throw new System.NotImplementedException();
    }

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
