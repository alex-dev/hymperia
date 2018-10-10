using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ViewModels.Editeur;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class FormesSelection : RegionContextAwareUserControl
  {
    public FormesSelection() : base()
    {
      InitializeComponent();
      Binding.AddTargetUpdatedHandler(ListBox, ListBoxUpdated);
    }

    private void ListBoxUpdated(object sender, DataTransferEventArgs args)
    {
      if (args.TargetObject == ListBox && args.Property == ItemsControl.ItemsSourceProperty)
      {
        ListBox.SelectedItem = ListBox.ItemsSource?.OfType<KeyValuePair<Type, string>>()
          ?.First(forme => forme.Key == (DataContext as FormesSelectionViewModel)?.DefaultForme);
      }
    }
  }
}
