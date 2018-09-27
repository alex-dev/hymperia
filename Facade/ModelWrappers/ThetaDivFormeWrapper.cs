using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;


namespace Hymperia.Facade.ModelWrappers
{
  public class ThetaDivFormeWrapper<T> : FormeWrapper<T> where T : ThetaDivForme
  {
    #region Attributs

    public int ThetaDiv
    {
      get => Forme.ThetaDiv;
      set
      {
        Forme.ThetaDiv = value;
        OnPropertyChanged();
      }
    }
    #endregion

    public ThetaDivFormeWrapper(T forme) : base(forme) { }
  }
}
