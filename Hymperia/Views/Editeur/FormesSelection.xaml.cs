using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class FormesSelection : UserControl
  {
    public FormesSelection()
    {
      InitializeComponent();
      Binding.AddTargetUpdatedHandler(ListBox, ListBoxUpdated);
    }

    private void ListBoxUpdated(object sender, DataTransferEventArgs e)
    {
      if (e.Property == ItemsControl.ItemsSourceProperty)
      {
        ((ListBox)sender).SelectedItem = ((ListBox)sender).ItemsSource?.OfType<KeyValuePair<Type, string>>()
          ?.SingleOrFirst(forme => forme.Key == typeof(PrismeRectangulaire));
      }
    }
  }
}
