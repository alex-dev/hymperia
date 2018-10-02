using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using HelixToolkit.Wpf;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : RegionContextBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<MeshElement3D> formes;
    private ObservableCollection<MeshElement3D> selected;

    #endregion

    #region Binding

    /// <summary>Les formes affichables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public ObservableCollection<MeshElement3D> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public ObservableCollection<MeshElement3D> FormesSelectionnees
    {
      get => selected;
      private set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        SetProperty(ref selected, value);
      }
    }

    #endregion

    #region Commands

    #endregion

    #endregion

    #region Services

    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;

    #endregion

    public ViewportViewModel([NotNull] ConvertisseurWrappers wrappers)
    {
      ConvertisseurWrappers = wrappers;
    }

    #region Methods

    protected override void OnRegionContextChanged()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      context.PropertyChanged += (sender, args) =>
      {
        switch (args.PropertyName)
        {
          case nameof(context.Formes):
            UpdateFormes();
            break;
          case nameof(context.FormesSelectionnees):
            UpdateFormesSelectionnees();
            break;
        }
      };

      UpdateFormes();
      UpdateFormesSelectionnees();

      base.OnRegionContextChanged();
    }

    private void OnFormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      throw new NotImplementedException();
    }

    private void OnFormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      throw new NotImplementedException();
    }

    private void UpdateFormes()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      context.Formes.CollectionChanged += OnFormesChanged;
      Formes = new ObservableCollection<MeshElement3D>(from forme in context.Formes
                                                       select ConvertisseurWrappers.Lier(ConvertisseurWrappers.Convertir(forme), forme));
    }

    private void UpdateFormesSelectionnees()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      context.FormesSelectionnees.CollectionChanged += OnFormesSelectionneesChanged;
      FormesSelectionnees = new ObservableCollection<MeshElement3D>(context.FormesSelectionnees.Join(
        Formes,
        wrapper => wrapper,
        mesh => BindingOperations.GetBinding(mesh, MeshElement3D.TransformProperty).Source,
        (wrapper, mesh) => mesh));
    }

    #endregion
  }
}
