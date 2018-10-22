using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ViewModels.Editeur;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class MateriauxSelection : RegionContextAwareUserControl
  {
    public MateriauxSelection() : base()
    {
      InitializeComponent();
      Binding.AddTargetUpdatedHandler(ListBox, ListBoxUpdated);
    }

    private void ListBoxUpdated(object sender, DataTransferEventArgs e)
    {
      if (e.TargetObject == ListBox && e.Property == ItemsControl.ItemsSourceProperty)
      {
        ListBox.SelectedItem = ListBox.ItemsSource?.OfType<Materiau>()
          ?.First(materiau => materiau.Nom == (DataContext as MateriauxSelectionViewModel)?.DefaultName);
      }
    }
  }
}
