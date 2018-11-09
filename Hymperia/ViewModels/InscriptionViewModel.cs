/**
* Auteur : Antoine Mailhot
* Date de création : 9 novembre 2018
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Facade.ViewModelInterface;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Hymperia.Facade.ViewModels
{
  public class InscriptionViewModel : BindableBase, INotifyDataErrorInfo, IPasswordHolder
  {
    #region Properties

    public string Username { get; set; }

    public bool PasswordIsValid
    {
      get => password;
      set => SetProperty(ref password, value, () => RaiseErrorsChanged(nameof(PasswordIsValid)));
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

    private void  _Inscription(PasswordBox password)
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

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    private bool DuplicateUsername;

    public bool HasErrors => DuplicateUsername && !false;

    public IEnumerable GetErrors(string property)
    {
      if (DuplicateUsername && property == nameof(Username))
        yield return Resources.InvalidCredential;
      if (!PasswordIsValid && property == nameof(PasswordIsValid))
        yield return Resources.InvalidCredential;
    }

    protected virtual void RaiseErrorsChanged([CallerMemberName] string property = null) =>
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private bool duplicateusername;
    private bool password = true;

    #endregion

  }
}
