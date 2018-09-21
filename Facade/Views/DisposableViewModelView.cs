using System;
using System.Windows.Controls;

namespace Hymperia.Facade.Views
{
  public partial class DisposableViewModelView : UserControl
  {
    public DisposableViewModelView()
    {
      Unloaded += (sender, args) => DisposeDataContext();
    }

    protected virtual void DisposeDataContext()
    {
      (DataContext as IDisposable)?.Dispose();
    }
  }
}
