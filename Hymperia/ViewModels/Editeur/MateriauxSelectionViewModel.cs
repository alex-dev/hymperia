using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism;
using Prism.Events;
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

    [CanBeNull]
    public Materiau SelectedMateriau
    {
      get => selected;
      set => SetProperty(ref selected, value, () => RaiseSelectedChanged(value.Id));
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

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory, [NotNull] IEventAggregator aggregator)
    {
      Factory = factory;
      SelectedChanged = aggregator.GetEvent<SelectedMateriauChanged>();
      SelectedChanged.Subscribe(OnSelectedChanged, FilterSelectedChanged);
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

    #region SelectedChanged Handling

    protected virtual void OnSelectedChanged(int key)
    {
      if (key != SelectedMateriau.Id)
      {
        SelectedMateriau = Materiaux.Single(materiau => materiau.Materiau.Id == key).Materiau;
      }
    }

    protected virtual bool FilterSelectedChanged(int key) => key != SelectedMateriau.Id;

    protected virtual void RaiseSelectedChanged(int key) => SelectedChanged.Publish(key);

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
    [NotNull]
    private readonly SelectedMateriauChanged SelectedChanged;

    #endregion

    #region Private Fields

    private bool isActive;
    private Task loading;
    private bool isLoading;
    private ICollection<MateriauWrapper> materiaux;
    private Materiau selected;

    #endregion
  }
}
