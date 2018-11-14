/**
* Auteur : Antoine Mailhot
* Date de création : 9 novembre 2018
*/
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
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

    [Required(ErrorMessage = "R1")]
    [StringLength(1000000, MinimumLength = 3, ErrorMessage = "adfiadusgyfsdyuthgudfgb")]
    public string Username {
      get => username;
      set
      {
        if (ValidateProperty(value))
          SetProperty(ref username, value);
      }
    }

    [Compare(nameof(Verification), ErrorMessage = "dont match")]
    [Required(ErrorMessage = "R2")]
    [StringLength(1000000, MinimumLength = 3, ErrorMessage = "adfiadusgyfsdyuthgudfgb")]
    public string Password
    {
      get => password;
      set
      {
        if (ValidateProperty(value))
          SetProperty(ref password, value);
      }
    }
    
    
    [Compare(nameof(Password), ErrorMessage = "dont match2")]
    [Required(ErrorMessage = "R3")]
    [StringLength(1000000, MinimumLength = 3, ErrorMessage = "adfiadusgyfsdyuthgudfgb")]
    public string Verification
    {
      get => verification;
      set
      {
        if (ValidateProperty(value))
          SetProperty(ref verification, value);
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
      RaiseErrorsChanged(nameof(Verification));
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
    private string verification;
    #endregion

  }
}
