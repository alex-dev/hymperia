using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.ModelWrappers;

namespace Hymperia.Facade.Views.Reglages.Editeur
{
  public partial class AccesProjet : UserControl
  {
    public AccesProjet()
    {
      InitializeComponent();
    }

    private void OnFiltering(object sender, FilterEventArgs e) =>
      e.Accepted = !((e.Item as AccesWrapper)?.EstPropriétaire ?? true);
  }
}
