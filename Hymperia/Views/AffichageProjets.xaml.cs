using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Utilisateur] is Utilisateur;
    public void OnNavigatedTo(NavigationContext context) => Utilisateur = (Utilisateur)context.Parameters[NavigationParameterKeys.Utilisateur];
    public void OnNavigatedFrom(NavigationContext context) => Utilisateur = null;

    #endregion

<<<<<<< HEAD
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
=======
    private void RaiseSelectionChanged(object sender, SelectionChangedEventArgs e) =>
>>>>>>> 519ce70cd4a21715476741342b0cf1d0fd752e37
      (btnSuppression.Command as DelegateCommand<IList>)?.RaiseCanExecuteChanged();
  }
}
