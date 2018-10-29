using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class MateriauxSelectionViewModel : BindableBase, IActiveAware
  {
    #region Properties

    #region Bindings

    [CanBeNull]
    [ItemNotNull]
    public ICollection<MateriauWrapper> Materiaux
    {
      get => materiaux;
      private set => SetProperty(ref materiaux, value);
    }

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
    }

    #endregion

    #region Queries

    private async Task QueryMateriaux()
    {
      if (!IsLoading)
      {
        using (var context = Factory.GetContext())
        {
          Materiaux = await (from materiau in context.Materiaux.AsNoTracking()
                             join localized in await Resources.LoadMateriaux()
                               on materiau.Nom equals localized.Key
                             select new MateriauWrapper(materiau, localized.Value)).ToArrayAsync();
        }
      }
    }

    #endregion

    #region IActiveAware

    public event EventHandler IsActiveChanged;

    public bool IsActive
    {
      get => isActive;
      set
      {
        isActive = value;

        if (isActive)
        {
          OnActivation();
        }

        IsActiveChanged?.Invoke(this, null);
      }
    }

    private void OnActivation() => Loading = QueryMateriaux();

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private bool isActive;
    private Task loading;
    private bool isLoading;
    private ICollection<MateriauWrapper> materiaux;

    #endregion
  }
}
