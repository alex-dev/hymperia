using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.ModelWrappers;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class MateriauxSelection : UserControl
  {
    public MateriauxSelection() : base()
    {
      InitializeComponent();
      Binding.AddTargetUpdatedHandler(ListBox, ListBoxUpdated);
    }

    private void ListBoxUpdated(object sender, DataTransferEventArgs e)
    {
      if (e.Property == ItemsControl.ItemsSourceProperty)
        ((ListBox)sender).SelectedItem = ((ListBox)sender).ItemsSource?.OfType<MateriauWrapper>()
          ?.SingleOrFirst(materiau => materiau.Materiau.Nom == "Bois");
    }
  }
}
