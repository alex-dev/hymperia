using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ModelWrappers
{
  public class CylindreWrapper : ThetaDivFormeWrapper
  {
    #region Attributs

    public double Diametre
    {
      get => ((Cylindre)Forme).Diametre;
      set
      {
        ((Cylindre)Forme).Diametre = value;
        OnPropertyChanged();
      }
    }

    public double InnerDiametre
    {
      get => ((Cylindre)Forme).InnerDiametre;
      set
      {
        ((Cylindre)Forme).InnerDiametre = value;
        OnPropertyChanged();
      }
    }

    public double Hauteur
    {
      get => ((Cylindre)Forme).Hauteur;
      set
      {
        ((Cylindre)Forme).Hauteur = value;
        OnPropertyChanged();
      }
    }

    #endregion

    public CylindreWrapper(Cylindre forme) : base(forme) { }
  }
}
