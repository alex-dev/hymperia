using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur.PropertiesEditeur
{
  public class FormePropertiesViewModel : BindableBase
  {
    #region Properties

    public bool IsReadOnly
    {
      get => isreadonly;
      set => SetProperty(ref isreadonly, value);
    }

    [CanBeNull]
    public FormeWrapper Forme
    {
      get => forme;
      set => SetProperty(ref forme, value);
    }

    #endregion

    #region Constructors

    public FormePropertiesViewModel(IEventAggregator events)
    {
      events.GetEvent<AccesChanged>().Subscribe(OnAccesChanged);
    }

    #endregion

    #region Aggregated Events Handlers

    private void OnAccesChanged(Acces.Droit droit) => IsReadOnly = (droit < Acces.Droit.LectureEcriture);

    #endregion

    #region

    private bool isreadonly = true;
    private FormeWrapper forme;

    #endregion
  }
}
