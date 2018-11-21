/*
 * Auteur : Antoine Mailhot
 * Date de création : 21 novembre 2018
 */

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using B = BCrypt.Net;

namespace Hymperia.Facade.ViewModels.Reglages.Utilisateur
{
  public class ChangementMotDePasseViewModel : ValidatingBase, INotifyDataErrorInfo
  {
    #region Properties

    [Required(
      ErrorMessageResourceName = nameof(Resources.RequiredPassword),
      ErrorMessageResourceType = typeof(Resources))]
    [Compare(nameof(Verification),
      ErrorMessageResourceName = nameof(Resources.DontMatchPassword),
      ErrorMessageResourceType = typeof(Resources))]
    public string Password
    {
      get => password;
      set
      {
        if (SetProperty(ref password, value))
          ValidateProperty<string>(nameof(Verification));
      }
    }

    [Required(
      ErrorMessageResourceName = nameof(Resources.RequiredVerification),
      ErrorMessageResourceType = typeof(Resources))]
    [Compare(nameof(Password),
      ErrorMessageResourceName = nameof(Resources.DontMatchPassword),
      ErrorMessageResourceType = typeof(Resources))]
    public string Verification
    {
      get => verification;
      set
      {
        if (SetProperty(ref verification, value))
          ValidateProperty<string>(nameof(Password));
      }
    }

    public Model.Modeles.Utilisateur Utilisateur
    {
      get => utilisateur;
      set
      {
        SetProperty(ref utilisateur, value);
      }
    }

    public DelegateCommand ChangementMotDePasse { get; }

    #endregion

    #region Constructeur

    public ChangementMotDePasseViewModel(ContextFactory factory)
    {
      ContextFactory = factory;
      ChangementMotDePasse = new DelegateCommand(_ChangementMotDePasse);
    }

    #endregion

    private async void _ChangementMotDePasse()
    {
      if (!await Validate())
        return;

      await ModificationUtilisateur();
    }

    #region Queries
    private async Task ModificationUtilisateur()
    {
      //var utilisateur = new Model.Modeles.Utilisateur(Utilisateur.Nom, B.BCrypt.HashPassword(Password, Model.Modeles.Utilisateur.PasswordWorkFactor, true));

      Utilisateur.MotDePasse = B.BCrypt.HashPassword(Password, Model.Modeles.Utilisateur.PasswordWorkFactor,true);

      using (var context = ContextFactory.GetContext())
      {
        context.Utilisateurs.Update(Utilisateur);
        //await context.SaveChangesAsync();
      }
    }

    #endregion

    #region ValidationBase

    protected override async Task ValidateAsync()
    {
      ValidateProperty<string>(nameof(Password));
      ValidateProperty<string>(nameof(Verification));
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;

    #endregion

    #region Private Fields

    private Model.Modeles.Utilisateur utilisateur;
    private string password;
    private string verification;

    #endregion
  }
}
