using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
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
      if (MateriauxLoader.IsLoading)
        return await MateriauxLoader.Loading;

      using (var context = Factory.GetContext())
      {
        return Materiaux =
          await (await ConvertisseurMateriaux.Convertir(context.Materiaux.AsNoTracking())).ToArrayAsync();
      }
    }

    #endregion

    #region SelectedChanged Handling

    protected virtual void OnSelectedChanged(int key) =>
      SelectedMateriau = Materiaux.Single(materiau => materiau.Materiau.Id == key).Materiau;

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

    private void OnActivation() => MateriauxLoader.Loading = QueryMateriaux();

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
