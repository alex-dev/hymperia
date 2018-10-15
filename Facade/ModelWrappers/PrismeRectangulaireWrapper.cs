using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ModelWrappers
{
  public class PrismeRectangulaireWrapper : FormeWrapper
  {
    #region Attributs

    public double Hauteur
    {
      get => ((PrismeRectangulaire)Forme).Hauteur;
      set
      {
        ((PrismeRectangulaire)Forme).Hauteur = value;
        OnPropertyChanged();
      }
    }

    public double Largeur
    {
      get => ((PrismeRectangulaire)Forme).Largeur;
      set
      {
        ((PrismeRectangulaire)Forme).Largeur = value;
        OnPropertyChanged();
      }
    }

    public double Longueur
    {
      get => ((PrismeRectangulaire)Forme).Longueur;
      set
      {
        ((PrismeRectangulaire)Forme).Longueur = value;
        OnPropertyChanged();
      }
    }

    #endregion

    public PrismeRectangulaireWrapper(PrismeRectangulaire forme) : base(forme) { }
  }
}
