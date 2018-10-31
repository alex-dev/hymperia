﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    #region Properties

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value);
    }

    public ReadOnlyObservableCollection<Projet> Projets { get; private set; }

    public ICommand Navigate { get; private set; }

    #endregion

    public FenetrePrincipaleViewModel(ContextFactory factory, IRegionManager manager)
    {
      Manager = manager;
      Navigate = new DelegateCommand(NavigateToViewport, () => Projet is Projet).ObservesProperty(() => Projet);

      using (var context = factory.GetContext())
      {
        Projets = new ReadOnlyObservableCollection<Projet>(
          new ObservableCollection<Projet>(context.Projets.ToList()));
      }
    }

    private void NavigateToViewport()
    {
      foreach (var view in Manager.Regions["ContentRegion"].ActiveViews)
      {
        Manager.Regions["ContentRegion"].Deactivate(view);
      }

      Manager.RequestNavigate("ContentRegion", "Editeur", new NavigationParameters
      {
        { "Projet", Projet }
      });
    }

    #region Private Fields

    private readonly IRegionManager Manager;
    private Projet projet;

    #endregion
  }
}
