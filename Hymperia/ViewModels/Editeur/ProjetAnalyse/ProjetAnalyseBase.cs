﻿using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Loaders;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur.ProjetAnalyse
{
  public abstract class ProjetAnalyseBase : BindableBase
  {
    #region Properties

    #region Bindings

    [CanBeNull]
    [ItemNotNull]
    public Projet Projet
    {
      get => projet;
      private set => SetProperty(ref projet, value, Clear);
    }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader AnalysisLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public ProjetAnalyseBase([NotNull] IEventAggregator aggregator)
    {
      ProjetChanged = aggregator.GetEvent<ProjetChanged>();
      ProjetChanged.Subscribe(OnProjetChanged);
    }

    #endregion

    #region Projet Changed

    protected abstract void Clear();

    #endregion

    #region Aggregated Event Handlers

    protected virtual void OnProjetChanged(Projet projet) => Projet = projet;

    #endregion

    #region Services

    [NotNull]
    private readonly ProjetChanged ProjetChanged;

    #endregion

    #region Private Fields

    private Projet projet;

    #endregion
  }
}
