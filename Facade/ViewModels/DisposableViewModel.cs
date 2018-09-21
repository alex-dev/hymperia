using System;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels
{
  public abstract class DisposableViewModel : BindableBase, IDisposable
  {
    private bool disposed;

    public DisposableViewModel()
    {
      disposed = false;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected abstract void DisposeResource();

    private void Dispose(bool disposing)
    {
      if (disposed)
        return;

      if (disposing)
      {
        DisposeResource();
      }

      disposed = true;
    }
  }
}
