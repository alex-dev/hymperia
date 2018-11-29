/*
 * Auteur : Antoine Mailhot
 * Date de création : 29 novembre 2018
*/
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Reglages.Editeur
{
  class AccesProjetViewModel : BindableBase
  {
    #region Propriete

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value);
    }

    DelegateCommand<string> AjouterAcces;
    DelegateCommand ModifierAcces;
    DelegateCommand SupprimerAcces;

    #endregion

    #region Constructeur

    public AccesProjetViewModel(ContextFactory factory, ICommandAggregator commands, IEventAggregator events)
    {
      Factory = factory;

      AjouterAcces = new DelegateCommand<string>(_AjouterAcces);
      commands.GetCommand<PreSauvegarderReglageEditeur>().RegisterCommand(new DelegateCommand(PreSauvegarder));
      events.GetEvent<ReglageProjetChanged>().Subscribe(OnProjetChanged);
    }

    #endregion

    #region CommandAcces

    #region AjouterAcces

    private void _AjouterAcces(string utilisateur)
    {
      //if()
      
    }

    #endregion

    #region ModifierAcces

    #endregion

    #region SupprimerAcces

    #endregion

    #endregion

    #region Sauvegarder

    private void PreSauvegarder()
    {

    }

    #endregion

    #region Query

    //private Utilisateur EstUtilisateurExistant()
    //{
    //  Utilisateur utilisateur;
    //  return utilisateur;
    //}

    private bool EstPossedeurProjet(string utilisateur)
    {
      Acces acces;

      using (var context = Factory.GetReglageEditeurContext())
        //acces = context.Context.Acces.FindAsync(a => a.Projet.Id == Projet.Id && a.DroitDAcces == Acces.Droit.Possession);

      return false;
    }

    #endregion

    private void OnProjetChanged(Projet projet) => Projet = projet;

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private Projet projet;

    #endregion

  }
}
