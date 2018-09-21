using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<ModelVisual3D> formes;

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

    private ProjetEditeurViewModel Editeur { get; set; }

    private IEnumerable<MeshElement3D> ElementFormes
    {
      get;
    }

    private static IEnumerable<ModelVisual3D> BaseFormes
    {
      get
      {
        yield return new SunLight { };
        yield return new GridLinesVisual3D { };
      }
    }

    #endregion

    #endregion

    public ViewportViewModel(ProjetEditeurViewModel editeur)
    {
      Editeur = editeur;
      Formes = new ObservableCollection<ModelVisual3D>(Enumerable.Union(BaseFormes, ElementFormes));
    }
  }
}
