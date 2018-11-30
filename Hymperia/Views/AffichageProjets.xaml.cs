using System.Collections;
using System.Windows.Controls;
using Prism.Commands;

namespace Hymperia.Facade.Views
{
  public partial class AffichageProjets : UserControl
  {
    public AffichageProjets()
    {
      InitializeComponent();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
      (btnSuppression.Command as DelegateCommand<IList>)?.RaiseCanExecuteChanged();
  }
}
