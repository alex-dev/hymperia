using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Hymperia.Facade.Loaders
{
  public class AsyncLoader : INotifyPropertyChanged
  {
    [CanBeNull]
    public Task Loading
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
          value.ContinueWith(result => RaisePropertyChanged(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        RaisePropertyChanged();
      }
    }

    public bool IsCompleted => Loading is Task && Loading.IsCompleted;
    public bool IsFaulted => Loading is Task && (Loading.IsCanceled || Loading.IsFaulted);
    public bool IsReallyLoading => !IsCompleted && !IsFaulted;
    public bool IsLoading => Loading is null || IsReallyLoading;

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged()
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loading)));
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
    }

    #endregion

    #region Private Fields

    private Task loading;

    #endregion
  }
}
