using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels
{
  public class ViewportViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<ModelVisual3D> formes;

    #endregion

    public ObservableCollection<ModelVisual3D> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    private IEnumerable<MeshElement3D> ElementFormes
    {
      get
      {
        yield return new SphereVisual3D { Radius = 5 };
      }
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

    public ViewportViewModel()
    {
      Formes = new ObservableCollection<ModelVisual3D>(Enumerable.Union(BaseFormes, ElementFormes));
    }
  }
}
