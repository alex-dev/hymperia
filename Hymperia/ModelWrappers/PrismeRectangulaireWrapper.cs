using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// inheritdoc/>
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

    public PrismeRectangulaireWrapper([NotNull] PrismeRectangulaire forme) : base(forme) { }
  }
}
