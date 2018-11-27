using System.ComponentModel;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur.PropertiesEditeur
{
  public class PositionEditeurViewModel : BindableBase
  {
    #region Properties

    public bool IsReadOnly
    {
      get => isreadonly;
      set => SetProperty(ref isreadonly, value);
    }

    [CanBeNull]
    public FormeWrapper Forme
    {
      get => forme;
      set
      {
        var old = forme;

        if (SetProperty(ref forme, value, RaiseAllChanged))
        {
          old?.Remove(OnPropertyChanged);
          forme?.Add(OnPropertyChanged);
        }
      }
    }

    private Point Position
    {
      get => Forme?.Origine;
      set
      {
        if (Forme is null)
          return;

        if (Forme.Origine != value)
        {
          Forme.Origine = value;
          RaisePositionChanged();
        }
      }
    }

    private Quaternion Rotation
    {
      get => Forme?.Rotation;
      set
      {
        if (Forme is null)
          return;

        if (Forme.Rotation != value)
        {
          Forme.Rotation = value;
          RaiseRotationChanged();
        }
      }
    }

    public double PositionX
    {
      get => Position?.X ?? 0;
      set => Position = new Point(value, Position.Y, Position.Z);
    }

    public double PositionY
    {
      get => Position?.Y ?? 0;
      set => Position = new Point(Position.X, value, Position.Z);
    }

    public double PositionZ
    {
      get => Position?.Z ?? 0;
      set => Position = new Point(Position.X, Position.Y, value);
    }

    public double RotationX
    {
      get => Rotation?.X ?? 0;
      set => Rotation = GeometryExtension.CreateNormalized(value, Rotation.Y, Rotation.Z, Rotation.W);
    }

    public double RotationY
    {
      get => Rotation?.Y ?? 0;
      set => Rotation = GeometryExtension.CreateNormalized(Rotation.X, value, Rotation.Z, Rotation.W);
    }

    public double RotationZ
    {
      get => Rotation?.Z ?? 0;
      set => Rotation = GeometryExtension.CreateNormalized(Rotation.X, Rotation.Y, value, Rotation.W);
    }

    public double RotationW
    {
      get => Rotation?.W ?? 0;
      set => Rotation = GeometryExtension.CreateNormalized(Rotation.X, Rotation.Y, Rotation.Z, value);
    }

    #endregion

    #region Constructors

    public PositionEditeurViewModel(IEventAggregator events)
    {
      events.GetEvent<AccesChanged>().Subscribe(OnAccesChanged);
    }

    #endregion

    #region FormeChanged

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case nameof(FormeWrapper.Origine):
          RaisePositionChanged(); break;
        case nameof(FormeWrapper.Rotation):
          RaiseRotationChanged(); break;
      }
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

    #region Aggregated Events Handlers

    private void OnAccesChanged(Acces.Droit droit) => IsReadOnly = (droit < Acces.Droit.LectureEcriture);

    #endregion

    #region Private Fields

    private bool isreadonly = true;
    private FormeWrapper forme;

    #endregion
  }
}