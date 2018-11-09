using Hymperia.Facade.ModelWrappers;
using Prism.Commands;
using Prism.Mvvm;

namespace Hymperia.Facade.Views.Editeur
{
  public class PositionEditeurViewModel : BindableBase
  {
    #region Properties

    public DelegateCommand<FormeWrapper> Translate { get; }
    public DelegateCommand<FormeWrapper> Rotate { get; }

    #endregion

    #region Constructors

    public PositionEditeurViewModel()
    {
    }

    #endregion
  }
}