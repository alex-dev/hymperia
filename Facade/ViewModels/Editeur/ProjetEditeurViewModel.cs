using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using Prism.Mvvm;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ProjetEditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<ModelVisual3D> formes;

    private IDictionary<Forme, ModelVisual3D> MappingFormes { get; set; }

    #endregion

    #region Binding

    public ObservableCollection<ModelVisual3D> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    #endregion

    #region Commands

    #endregion

    #region Private

    private ContextFactory ContextFactory { get; set; }

    #endregion

    #endregion

    public ProjetEditeurViewModel(ContextFactory factory)
    {
      ContextFactory = factory;

      using (var context = ContextFactory.GetContext())
      {
      }

      Formes = new ObservableCollection<ModelVisual3D>(MappingFormes.Values);
    }
  }
}
