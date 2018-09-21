using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ProjetEditeurViewModel : BindableBase
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

    #endregion

    #endregion

    public ProjetEditeurViewModel()
    {
    }
  }
}
