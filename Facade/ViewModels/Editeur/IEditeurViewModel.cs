using System;
using System.ComponentModel;
using System.Windows.Input;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public interface IEditeurViewModel
  {
    event PropertyChangedEventHandler PropertyChanged;

    BulkObservableCollection<FormeWrapper> Formes { get; }
    BulkObservableCollection<FormeWrapper> FormesSelectionnees { get; }
    SelectionMode SelectedSelectionMode { get; }
    Type SelectedForme { get; }
    Materiau SelectedMateriau { get; }

    ICommand AjouterForme { get; }
    ICommand SupprimerForme { get; }
  }
}
