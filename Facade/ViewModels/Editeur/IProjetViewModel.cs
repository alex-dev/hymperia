using System.ComponentModel;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public interface IProjetViewModel
  {
    event PropertyChangedEventHandler PropertyChanged;

    Projet Projet { get; }
    BulkObservableCollection<FormeWrapper> Formes { get;  }
  }
}
