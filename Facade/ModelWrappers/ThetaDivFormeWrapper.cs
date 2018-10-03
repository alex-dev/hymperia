using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;


namespace Hymperia.Facade.ModelWrappers
{
  public class ThetaDivFormeWrapper : FormeWrapper
  {
    
    #region Attributs

    public int ThetaDiv
    {
      get => ((ThetaDivForme)Forme).ThetaDiv;
      set
      {
        ((ThetaDivForme)Forme).ThetaDiv = value;
        OnPropertyChanged();
      }
    }
    #endregion

    public ThetaDivFormeWrapper(Forme forme) : base (forme) {}
  }
}
