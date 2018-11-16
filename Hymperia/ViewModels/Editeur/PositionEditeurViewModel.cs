using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles.JsonObject;
using Prism.Mvvm;

namespace Hymperia.Facade.Views.Editeur
{
  public class PositionEditeurViewModel : BindableBase
  {
    #region Properties

    public FormeWrapper Forme
    {
      get => forme;
      set => SetProperty(ref forme, value, RaiseAllChanged);
    }

    private Point Position
    {
      get => Forme.Origine;
      set
      {
        if (Forme.Origine != value)
        {
          Forme.Origine = value;
          RaisePositionChanged();
        }
      }
    }

    private Quaternion Rotation
    {
      get => Forme.Rotation;
      set
      {
        if (Forme.Rotation != value)
        {
          Forme.Rotation = value;
          RaiseRotationChanged();
        }
      }
    }

    public double PositionX
    {
      get => Position.X;
      set => Position = new Point(value, Position.Y, Position.Z);
    }

    public double PositionY
    {
      get => Position.Y;
      set => Position = new Point(Position.X, value, Position.Z);
    }

    public double PositionZ
    {
      get => Position.Z;
      set => Position = new Point(Position.X, Position.Y, value);
    }

    public double RotationX
    {
      get => Rotation.X;
      set => Rotation = new Quaternion(value, Rotation.Y, Rotation.Z, Rotation.W);
    }

    public double RotationY
    {
      get => Rotation.Y;
      set => Rotation = new Quaternion(Rotation.X, value, Rotation.Z, Rotation.W);
    }

    public double RotationZ
    {
      get => Rotation.Z;
      set => Rotation = new Quaternion(Rotation.X, Rotation.Y, value, Rotation.W);
    }

    public double RotationW
    {
      get => Rotation.W;
      set => Rotation = new Quaternion(Rotation.X, Rotation.Y, Rotation.Z, value);
    }

    #endregion

    #region Constructors

    public PositionEditeurViewModel()
    {
    }

    #endregion

    #region RaiseChanged

    private void RaiseAllChanged()
    {
      RaisePositionChanged();
      RaiseRotationChanged();
    }

    private void RaisePositionChanged()
    {
      RaisePropertyChanged(nameof(PositionX));
      RaisePropertyChanged(nameof(PositionY));
      RaisePropertyChanged(nameof(PositionZ));
    }

    private void RaiseRotationChanged()
    {
      RaisePropertyChanged(nameof(RotationX));
      RaisePropertyChanged(nameof(RotationY));
      RaisePropertyChanged(nameof(RotationZ));
      RaisePropertyChanged(nameof(RotationW));
    }


    #endregion

    #region Private Fields

    private FormeWrapper forme;

    #endregion
  }
}