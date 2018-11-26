using System.Windows.Controls;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.Views.Reglages.Application
{
  public partial class ConnexionAutomatique : UserControl
  {
    public ConnexionAutomatique()
    {
      InitializeComponent();
      if (!S.Default.ConnexionAutomatique) { ConnexionAutomatiqueComboBox.SelectedIndex = 0; }
    }
  }
}
