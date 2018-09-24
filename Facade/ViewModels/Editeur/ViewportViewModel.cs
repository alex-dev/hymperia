using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : BindableBase
  {
    #region Attributes

    #region Binding

    public ObservableCollection<ModelVisual3D> Formes => Editeur.Formes;

    #endregion

    #region Commands

    #endregion

    #region Private

    private ProjetEditeurViewModel Editeur { get; set; }

    #endregion

    #endregion

    public ViewportViewModel(ProjetEditeurViewModel editeur)
    {
      Editeur = editeur;
    }
  }
}
