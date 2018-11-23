using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Constants;
using Hymperia.Model.Modeles;
using Prism.Commands;
using Prism.Regions;

namespace Hymperia.Facade.Views
{
  public partial class AffichageProjets : UserControl, INavigationAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty UtilisateurProperty =
      DependencyProperty.Register("Utilisateur", typeof(Utilisateur), typeof(AffichageProjets));

    #endregion

    public Utilisateur Utilisateur
    {
      get => (Utilisateur)GetValue(UtilisateurProperty);
      set => SetValue(UtilisateurProperty, value);
    }

    #region Constructors

    public AffichageProjets()
    {
      InitializeComponent();
      BindingOperations.SetBinding(this, UtilisateurProperty, new Binding("Utilisateur") { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
      (btnSuppression.Command as DelegateCommand<IList>)?.RaiseCanExecuteChanged();


    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Utilisateur] is Utilisateur;
    public void OnNavigatedTo(NavigationContext context)
    {
      void Set() =>
        Utilisateur = (Utilisateur)context.Parameters[NavigationParameterKeys.Utilisateur];

      void Load(object sender, RoutedEventArgs e)
      {
        Set();
        Loaded -= Load;
      }

      if (IsLoaded)
        Set();
      else
        Loaded += Load;
    }
    public void OnNavigatedFrom(NavigationContext context) => Utilisateur = null;

    #endregion
  }
}
