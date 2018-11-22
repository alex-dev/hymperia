using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Hymperia.Facade.Collections;
using Hymperia.Facade.Constants;
using Hymperia.Facade.DependencyObjects;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using MoreLinq;
using Prism;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class PropertiesEditeurViewModel : BindableBase, IActiveAware
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

    [NotNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper> SelectedFormes { get; } = new BulkObservableCollection<FormeWrapper>();

    [NotNull]
    public Materiau Materiau
    {
      get
      {
        var materiau = SelectedFormes.FirstOrDefault()?.Materiau;
        return SelectedFormes.Skip(1).All(forme => forme.Materiau == materiau)
          ? materiau
          : null;
      }
      set
      {
        SelectedFormes.ForEach(forme => forme.Materiau = value);
      }
    }

    public double Volume => SelectedFormes.Sum(forme => forme.Volume);
    public double Prix => SelectedFormes.Sum(forme => forme.Prix);

    [CanBeNull]
    public FormeWrapper SelectedForme
    {
      get => selected;
      set => SetProperty(ref selected, value, OnSelectedFormeChanged);
    }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<ICollection<MateriauWrapper>> MateriauxLoader { get; } = new AsyncLoader<ICollection<MateriauWrapper>>();

    #endregion

    #endregion

    #region Constructors

    public PropertiesEditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurMateriaux convertisseur, [NotNull] IRegionManager manager, [NotNull] IEventAggregator events)
    {
      SelectedFormes.CollectionChanged += OnSelectedFormesChanged;

      Factory = factory;
      ConvertisseurMateriaux = convertisseur;
      Manager = manager;
      SelectedSingleFormeChanged = events.GetEvent<SelectedSingleFormeChanged>();
      events.GetEvent<SelectedFormesChanged>().Subscribe(OnSelectedFormesChanged);
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

    #region Region Activation

    private void Activate(string name, FormeWrapper selected)
    {
      void Execute(string viewname, IRegion region)
      {
        var view = region.GetView(viewname) as DataUserControl;

        region.Activate(view);

        if (view is DataUserControl)
          view.Data = selected;
      }

      Execute(ViewKeys.PositionEditeur, Manager.Regions[RegionKeys.PositionPropertiesRegion]);
      Execute(name, Manager.Regions[RegionKeys.SpecificPropertiesRegion]);
    }

    private void Deactivate()
    {
      void Execute(IRegion region)
      {
        foreach (var view in region.ActiveViews)
          region.Deactivate(view);
      }

      Execute(Manager.Regions[RegionKeys.PositionPropertiesRegion]);
      Execute(Manager.Regions[RegionKeys.SpecificPropertiesRegion]);
    }

    #endregion

    #region SelectedFormes Changes Handlers

    private void OnSelectedFormeChanged()
    {
      switch (SelectedForme)
      {
        case EllipsoideWrapper ellipsoide:
          Activate(ViewKeys.EllipsoideEditor, SelectedForme); break;
        case PrismeRectangulaireWrapper prisme:
          Activate(ViewKeys.PrismeRectangulaireEditor, SelectedForme); break;
        case CylindreWrapper cylindre:
          Activate(ViewKeys.CylindreEditor, SelectedForme); break;
        case ConeWrapper cone:
          Activate(ViewKeys.ConeEditor, SelectedForme); break;
        default:
          Deactivate(); break;
      }

      SelectedSingleFormeChanged.Publish(SelectedForme);
    }

    private void OnSelectedFormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      e.OldItems?.Cast<FormeWrapper>()?.ForEach(wrapper => wrapper.PropertyChanged -= OnFormeChanged);
      e.NewItems?.Cast<FormeWrapper>()?.ForEach(wrapper => wrapper.PropertyChanged += OnFormeChanged);
      Raises();

      SelectedForme = ((ICollection<FormeWrapper>)sender).Count <= 1 
        ? ((ICollection<FormeWrapper>)sender).SingleOrDefault()
        : null;
    }

    private void OnFormeChanged(object sender, PropertyChangedEventArgs e) => Raises();

    private void Raises()
    {
      RaisePropertyChanged(nameof(Materiau));
      RaisePropertyChanged(nameof(Volume));
      RaisePropertyChanged(nameof(Prix));
    }

    #endregion

    #region Aggregated Events Handlers

    private void OnSelectedFormesChanged(NotifyCollectionChangedEventArgs e)
    {
      var newitems = e.NewItems?.Cast<FormeWrapper>() ?? Enumerable.Empty<FormeWrapper>();
      var olditems = e.OldItems?.Cast<FormeWrapper>() ?? Enumerable.Empty<FormeWrapper>();

      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          SelectedFormes.AddRange(newitems); break;
        case NotifyCollectionChangedAction.Remove:
          SelectedFormes.RemoveRange(olditems); break;
        case NotifyCollectionChangedAction.Replace:
          SelectedFormes.ReplaceRange(olditems, newitems); break;
        case NotifyCollectionChangedAction.Reset:
          SelectedFormes.Clear(); break;
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
    private readonly ConvertisseurMateriaux ConvertisseurMateriaux;
    [NotNull]
    private readonly IRegionManager Manager;
    [NotNull]
    private readonly SelectedSingleFormeChanged SelectedSingleFormeChanged;

    #endregion

    #region Private Fields

    private FormeWrapper selected;
    private bool isActive;
    private ICollection<MateriauWrapper> materiaux;

    #endregion
  }
}
