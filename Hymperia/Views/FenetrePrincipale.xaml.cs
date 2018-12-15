using System.Windows;
using Hymperia.Facade.ViewModels;

namespace Hymperia.Facade.Views
{
  public partial class FenetrePrincipale : Window
  {
    public FenetrePrincipale()
    {
      Loaded += OnLoaded;
      InitializeComponent();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      Loaded -= OnLoaded;
      (DataContext as FenetrePrincipaleViewModel).Load.Execute(null);
    }
  }
}
