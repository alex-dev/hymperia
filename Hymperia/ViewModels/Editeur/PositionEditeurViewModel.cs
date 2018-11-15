using System;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles.JsonObject;
using Prism.Commands;
using Prism.Mvvm;

namespace Hymperia.Facade.Views.Editeur
{
  public class PositionEditeurViewModel : BindableBase
  {
    #region Properties

    public FormeWrapper Forme
    {

    }

    public Point Position
    {
      get => position;
      set
      {
        if (SetProperty(ref position, value))
        {
          RaisePropertyChanged(nameof(PositionX));
          RaisePropertyChanged(nameof(PositionY));
          RaisePropertyChanged(nameof(PositionZ));
        }
      }
    }

    public Quaternion Rotation
    {
      get => rotation;
      set => SetProperty(ref rotation, value);
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
      get => Rotation.X;
      set => Rotation = new Quaternion(Rotation.X, value, Rotation.Z, Rotation.W);
    }

    public double RotationZ
    {
      get => Rotation.X;
      set => Rotation = new Quaternion(Rotation.X, Rotation.Y, value, Rotation.W);
    }

    public double RotationW
    {
      get => Rotation.X;
      set => Rotation = new Quaternion(Rotation.X, Rotation.Y, Rotation.Z, value);
    }

    #endregion

    #region Constructors

    public PositionEditeurViewModel()
    {
    }

    #endregion

    #region Private Fields
    private Point position;
    private Quaternion rotation;
    #endregion
  }
}