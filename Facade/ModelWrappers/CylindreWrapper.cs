using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  public class CylindreWrapper : FormeWrapper
  {
    #region Attributs
    public double Diametre
    {
      get => ((Cylindre)((Cylindre)Forme)).Diametre;
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
    #endregion

    #region Proprietees
    [NotNull]
    public Point Point
    {
      get => ((Cylindre)Forme).Point;
      set
      {
        ((Cylindre)Forme).Point = value;
        OnPropertyChanged();
      }
    }

    public Point Centre { get => ((Cylindre)Forme).Centre; }

    public double Hauteur => ((Cylindre)Forme).Hauteur;
    #endregion

    public CylindreWrapper(Cylindre forme) : base(forme) { }
  }
}
