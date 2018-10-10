using System;
using System.Collections.Generic;
using Hymperia.Model.Modeles;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class PrixAnalyseViewModel : BindableBase
  {
    #region Attributs
    private double PrixTotal = 0;
    private IDictionary<Type, double> formesPrix;

    public IDictionary<Type, double> FormesPrix
    {
      get => formesPrix;
      private set => SetProperty(ref formesPrix, value);
    }
    #endregion

    public PrixAnalyseViewModel()
    {
      /*foreach (Forme forme in COLLECTION_FORME_DANS_VIEWPORT3D)
      {
        formesPrix.Add(typeof(forme), forme.Prix);
        PrixTotal += forme.Prix;
      } */
    }
  }
}
