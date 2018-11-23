/*
 * Auteur : Antoine Mailhot
 * Date de création : 23 novembre 2018 
*/

using System;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Prism.Mvvm;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public class ConnexionAutomatiqueViewModel : BindableBase
  {


    #region Propriete

    public int Selectionne
    {
      get => selectionne;
      set => SetProperty(ref selectionne, value);
    }

    #endregion

    #region Constructeur

    public ConnexionAutomatiqueViewModel(ContextFactory context)
    {
      ContextFactory = context;
    }

    #endregion

    #region Sauvegarder

    private void Sauvegarder()
    {
      S.Default.ConnexionAutomatique = Selectionne;
    }

    #endregion

    #region Services

    ContextFactory ContextFactory;

    #endregion

    #region Private Fields

    private int selectionne = S.Default.ConnexionAutomatique;
    
    #endregion
  }
}
