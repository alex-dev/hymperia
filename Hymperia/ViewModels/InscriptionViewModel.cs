/**
* Auteur : Antoine Mailhot
* Date de création : 9 novembre 2018
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Hymperia.Facade.ViewModels
{
  public class InscriptionViewModel : ValidatingBase, INotifyDataErrorInfo
  {
    #region Properties

    [StringLength(500000, MinimumLength = 3, ErrorMessage = "adfiadusgyfsdyuthgudfgb")]
    public string Username {
      get => username;
      set
      {
        if (ValidateProperty(value))
          SetProperty(ref username, value);
      }
    }

    [StringLength(500000, MinimumLength = 3, ErrorMessage = "adfiadusgyfsdyuthgudfgb")]
    public string Password
    {
      get => password;
      set
      {
        if (ValidateProperty(value))
          SetProperty(ref password, value);
      }
    }

    public DelegateCommand<PasswordBox> Inscription { get; }

    #endregion

    #region Constructeur

    public InscriptionViewModel(ContextFactory factory, IRegionManager manager)
    {
      Factory = factory;
      Manager = manager;
      Inscription = new DelegateCommand<PasswordBox>(_Inscription);
    }

    #endregion

    private void _Inscription(PasswordBox password)
    {

    }

    #region Navigation

    private void Navigate(Utilisateur utilisateur) =>
      Manager.RequestNavigate("ContentRegion", NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, utilisateur }
      });

    #endregion

    #region INotifyDataErrorInfo

    protected override void RaiseAllErrorsChanged()
    {
      RaiseErrorsChanged(nameof(Username));
      RaiseErrorsChanged(nameof(Password));
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private string username;
    private string password;

    #endregion

  }
}
