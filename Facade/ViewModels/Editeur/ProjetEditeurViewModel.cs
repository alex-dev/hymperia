using System.Collections.ObjectModel;
using System.Windows.Threading;
using HelixToolkit.Wpf;
using Prism.Mvvm;
using Hymperia.Model;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ProjetEditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<MeshElement3D> formes;

    private DatabaseContext Context { get; set; }

    #endregion

    #region Binding

    public ObservableCollection<MeshElement3D> Formes
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

    public ProjetEditeurViewModel(DatabaseContext context)
    {
      Context = context;
    }
  }
}
