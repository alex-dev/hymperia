using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Facade.Collections;
using Hymperia.Facade.Constants;
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

namespace Hymperia.Facade.ViewModels.Editeur.PropertiesEditor
{
  public class EditorViewModel : BindableBase, IActiveAware
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
        return SelectedFormes.Skip(1).All(forme => forme.Materiau.Equals(materiau))
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

    public EditorViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurMateriaux convertisseur, [NotNull] IRegionManager manager, [NotNull] IEventAggregator events)
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
            return Materiaux = await ConvertisseurMateriaux.Convertir(wrapper.Context.Materiaux.AsQueryable());
    }

    #endregion

    #region SelectedFormes Changes Handlers

    private void OnSelectedFormeChanged()
    {
      var nav = new NavigationParameters
      {
        { NavigationParameterKeys.Forme, SelectedForme }
      };

      SelectedSingleFormeChanged.Publish(SelectedForme);

      switch (SelectedForme)
      {
        case EllipsoideWrapper ellipsoide:
          Manager.RequestNavigate("FormeProperties", NavigationKeys.EllipsoideEditor, nav); break;
        case PrismeRectangulaireWrapper prisme:
          Manager.RequestNavigate("FormeProperties", NavigationKeys.PrismeEditor, nav); break;
        case CylindreWrapper cylindre:
          Manager.RequestNavigate("FormeProperties", NavigationKeys.CylindreEditor, nav); break;
        case ConeWrapper cone:
          Manager.RequestNavigate("FormeProperties", NavigationKeys.ConeEditor, nav); break;
        default:
          Deactivate(); break;
      }
    }

    private void Deactivate()
    {
      var region = Manager.Regions["FormeProperties"];

      foreach (var view in region.ActiveViews)
        region.Remove(view);
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
