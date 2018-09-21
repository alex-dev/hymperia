using System.Collections.ObjectModel;
using System.Windows.Threading;
using HelixToolkit.Wpf;
using Hymperia.Model;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ProjetEditeurViewModel : DatabaseContextAwareViewModel
  {
    #region Attributes

    #region Fields

    private ObservableCollection<MeshElement3D> formes;

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

    public ProjetEditeurViewModel(DatabaseContext context) : base(context)
    {
      Dispatcher.CurrentDispatcher.ShutdownStarted += (sender, args) => Dispose();
    }
  }
}
