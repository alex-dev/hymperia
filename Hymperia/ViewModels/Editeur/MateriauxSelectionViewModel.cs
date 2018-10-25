using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class MateriauxSelectionViewModel : BindableBase
  {
    #region Properties

    [NotNull]
    public string DefaultName => "Bois";

    #region Bindings

    [CanBeNull]
    [ItemNotNull]
    public ICollection<Materiau> Materiaux
    {
      get => materiaux;
      private set => SetProperty(ref materiaux, value);
    }

    #endregion

    #region Commands

    public ICommand RefreshItems { get; }

    #endregion

    #region Asynchronous Loading

    [CanBeNull]
    private Task Loading
    {
      get => loading;
      set => SetProperty(ref loading, value, () =>
      {
        IsLoading = true;
        value
          .ContinueWith(
            result => throw result.Exception.Flatten(),
            default,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext())
          .ContinueWith(result => IsLoading = false, TaskScheduler.FromCurrentSynchronizationContext());
      });
    }

    public bool IsLoading
    {
      get => isLoading;
      private set => SetProperty(ref isLoading, value);
    }

    #endregion

    #endregion

    #region Constructors

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory)
    {
      Factory = factory;
      RefreshItems = new DelegateCommand(() => Loading = RefreshMateriaux())
        .ObservesCanExecute(() => IsLoading);

      RefreshItems.Execute(null);
    }

    #endregion

    #region Queries

    private async Task RefreshMateriaux()
    {
      if (!IsLoading)
      {
        using (var context = Factory.GetContext())
        {
          Materiaux = await context.Materiaux.ToArrayAsync();
        }
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private Task loading;
    private bool isLoading;
    private ICollection<Materiau> materiaux;

    #endregion
  }
}
