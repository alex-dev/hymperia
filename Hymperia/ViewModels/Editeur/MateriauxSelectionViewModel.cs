using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
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
      set => SetProperty(ref selected, value, RaiseSelectedChanged);
    }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<ICollection<MateriauWrapper>> MateriauxLoader { get; } = new AsyncLoader<ICollection<MateriauWrapper>>();

    #endregion

    #endregion

    #region Constructors

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurMateriaux convertisseur, [NotNull] IEventAggregator aggregator)
    {
      Factory = factory;
      ConvertisseurMateriaux = convertisseur;
      SelectedChanged = aggregator.GetEvent<SelectedMateriauChanged>();
      SelectedChanged.Subscribe(OnSelectedChanged, FilterSelectedChanged);
    }

    #endregion

    #region Queries

    private async Task<ICollection<MateriauWrapper>> QueryMateriaux()
    {
      using (await AsyncLock.Lock(MateriauxLoader))
        using (var wrapper = Factory.GetEditorContext())
          using (await AsyncLock.Lock(wrapper.Context))
            return Materiaux = await ConvertisseurMateriaux.Convertir(wrapper.Context.Materiaux);
    }

    #endregion

    #region Aggregated Event Handlers

    protected virtual void OnSelectedChanged(Materiau materiau) =>
      SelectedMateriau = Materiaux.Single(_materiau => _materiau.Materiau == materiau).Materiau;

    protected virtual bool FilterSelectedChanged(Materiau materiau) =>  materiau is Materiau && SelectedMateriau != materiau;

    protected virtual void RaiseSelectedChanged() => SelectedChanged.Publish(SelectedMateriau);

    #endregion

    #region IActiveAware

    public event EventHandler IsActiveChanged;

    public bool IsActive
    {
      get => isActive;
      set
      {
        if (isActive == value)
          return;

        isActive = value;

        if (value)
          OnActivation();
        else
          OnDeactivation();

        IsActiveChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    protected virtual void OnActivation() => MateriauxLoader.Loading = QueryMateriaux();
    protected virtual void OnDeactivation() { }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly SelectedMateriauChanged SelectedChanged;
    [NotNull]
    private readonly ConvertisseurMateriaux ConvertisseurMateriaux;

    #endregion

    #region Private Fields

    private bool isActive;
    private ICollection<MateriauWrapper> materiaux;
    private Materiau selected;

    #endregion
  }
}
