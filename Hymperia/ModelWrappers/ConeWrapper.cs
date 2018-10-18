using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// inheritdoc/>
  public class ConeWrapper : ThetaDivFormeWrapper
  {
    #region Attributs

    public double Hauteur
    {
      get => ((Cone)Forme).Hauteur;
      set
      {
        ((Cone)Forme).Hauteur = value;
        OnPropertyChanged();
      }
    }

    public double RayonBase
    {
      get => ((Cone)Forme).RayonBase;
      set
      {
        ((Cone)Forme).Hauteur = value;
        OnPropertyChanged();
      }
    }

    public double RayonTop
    {
      get => ((Cone)Forme).RayonTop;
      set
      {
        ((Cone)Forme).Hauteur = value;
        OnPropertyChanged();
      }
    }

    #endregion

    public ConeWrapper([NotNull] Cone forme) : base(forme) { }
  }
}
