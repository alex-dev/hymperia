using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Prism
{
  public class AsyncLoader<T> : INotifyPropertyChanged
  {
    [CanBeNull]
    public Task<T> Loading
    {
      get => loading;
      set
      {
        loading = value;

        value.ContinueWith(
          result => throw result.Exception.Flatten(),
          default,
          TaskContinuationOptions.OnlyOnFaulted,
          TaskScheduler.FromCurrentSynchronizationContext());

        if (!value.IsCompleted && !value.IsFaulted && !value.IsFaulted)
        {
          IsLoading = true;
          value.ContinueWith(result => IsLoading = false, TaskScheduler.FromCurrentSynchronizationContext());
        }

        RaisePropertyChanged();
      }
    }

    public bool IsLoading
    {
      get => isLoading;
      private set
      {
        isLoading = value;
        RaisePropertyChanged();
      }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion

    #region Private Fields

    private Task<T> loading;
    private bool isLoading;

    #endregion
  }
}
