using System.Windows.Controls;
using Hymperia.Facade.BaseClasses;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class MateriauxSelection : RegionContextAwareUserControl
  {
    public string FormeChoisie { get; set; }

    public MateriauxSelection()
    {
      InitializeComponent();
      MateriauxSelectionListeBox.SelectionChanged += MateriauxSelectionListeBox_SelectionChanged;
    }

    private void MateriauxSelectionListeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Materiau materiau = ((ListBox)e.Source).SelectedValue ;
      //TEXTBOXMATERIAUCHOISI.Text = materiau.Nom;
    }
  }
}
