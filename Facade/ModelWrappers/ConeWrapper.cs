using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ModelWrappers
{
  public class ConeWrapper : ThetaDivFormeWrapper
  {
    #region Attributs
    public double Hauteur
    {
      get => ((Cone)((Cone)Forme)).Hauteur;
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

    public ConeWrapper(Cone forme) : base(forme) { }
  }
}
