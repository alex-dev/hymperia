﻿using System.Windows.Data;
using Hymperia.Facade.DependencyObjects;
using Hymperia.Facade.ViewModels.Editeur.PropertiesEditeur;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditeur
{
  public partial class PositionEditeur : DataUserControl
  {
    public PositionEditeur(PositionEditeurViewModel context)
    {
      DataContext = context;
      InitializeComponent();
      SetBinding(DataProperty, new Binding("Forme") { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }
  }
}
