using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class FormesSelectionViewModel : BindableBase
  {
    #region Properties

    [CanBeNull]
    [ItemNotNull]
    public IDictionary<Type, string> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    [CanBeNull]
    public Type SelectedForme
    {
      get => selected;
      set => SetProperty(ref selected, value, () => RaiseSelectedChanged(value));
    }

    #endregion

    #region Constructors

#pragma warning disable 4014 // Justification: The async call is meant to run after everything is bound. It should not need to be awaited and it cannot fail.

    public FormesSelectionViewModel([NotNull] IEventAggregator aggregator)
    {
      SelectedChanged = aggregator.GetEvent<SelectedFormeChanged>();
      SelectedChanged.Subscribe(OnSelectedChanged, FilterSelectedChanged);
      QueryFormes();
    }

#pragma warning restore 4014

    #endregion

    #region Queries

    private async Task QueryFormes() => Formes = await Task.Run(() => new Dictionary<Type, string>
    {
      { typeof(PrismeRectangulaire), "PrismeRectangulaire" },
      { typeof(Cone), "Cone" },
      { typeof(Cylindre), "Cylindre" },
      { typeof(Ellipsoide), "Ellipsoide" }
    });

    #endregion

    #region Aggregated Event Handlers

    protected virtual void OnSelectedChanged(Type type) => SelectedForme = type;

    protected virtual bool FilterSelectedChanged(Type type) => type != SelectedForme;

    protected virtual void RaiseSelectedChanged(Type type) => SelectedChanged.Publish(type);

    #endregion

    #region Services

    [NotNull]
    private readonly SelectedFormeChanged SelectedChanged;

    #endregion

    #region Private Fields

    private IDictionary<Type, string> formes;
    private Type selected;

    #endregion
  }
}