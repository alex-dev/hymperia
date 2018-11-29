/**
* Auteur : Antoine Mailhot
* Date de création : 8 novembre 2018
*/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Hymperia.Facade.Views
{
  public partial class Connexion : UserControl
  {
    #region Dependency Properties

    public static readonly DependencyProperty ConnectionProperty =
      DependencyProperty.Register(nameof(Connection), typeof(ICommand), typeof(Connexion));

    #endregion

    public ICommand Connection
    {
      get => (ICommand)GetValue(ConnectionProperty);
      set => SetValue(ConnectionProperty, value);
    }

    public Connexion()
    {
      Loaded += TryConnect;
      InitializeComponent();
      SetBinding(ConnectionProperty, new Binding("ConnexionAutomatique") { Source = DataContext, Mode = BindingMode.OneWay });
    }

    private void TryConnect(object sender, RoutedEventArgs e)
    {
      Loaded -= TryConnect;

      if (Connection.CanExecute(null))
        Connection.Execute(null);
    }
  }
}
