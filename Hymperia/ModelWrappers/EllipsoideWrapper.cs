using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ModelWrappers
{
  public class EllipsoideWrapper : FormeWrapper
  {
    #region Attributs

    public double RayonX
    {
      get => ((Ellipsoide)Forme).RayonX;
      set
      {
        ((Ellipsoide)Forme).RayonX = value;
        OnPropertyChanged();
      }
    }

    public double RayonY
    {
      get => ((Ellipsoide)Forme).RayonY;
      set
      {
        ((Ellipsoide)Forme).RayonY = value;
        OnPropertyChanged();
      }
    }

    public double RayonZ
    {
      get => ((Ellipsoide)Forme).RayonZ;
      set
      {
        ((Ellipsoide)Forme).RayonZ = value;
        OnPropertyChanged();
      }
    }

    public int PhiDiv
    {
      get => ((Ellipsoide)Forme).PhiDiv;
      set
      {
        ((Ellipsoide)Forme).PhiDiv = value;
        OnPropertyChanged();
      }
    }

    public int ThetaDiv
    {
      get => ((Ellipsoide)Forme).ThetaDiv;
      set
      {
        ((Ellipsoide)Forme).ThetaDiv = value;
        OnPropertyChanged();
      }
    }

    #endregion

    public EllipsoideWrapper(Ellipsoide forme) : base(forme) { }
  }
}
