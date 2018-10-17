using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// <inheritdoc />
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

    public ThetaDivFormeWrapper([NotNull] Forme forme) : base (forme) {}
  }
}
