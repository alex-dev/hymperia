﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// <summary>Wrap les formes du modèle avec un <see cref="INotifyPropertyChanged"/> wrapper.</summary>
  public abstract class FormeWrapper : INotifyPropertyChanged
  {
    #region Attributs

    [NotNull]
    public readonly Forme Forme;

    public int Id => Forme.Id;

    public Materiau Materiau
    {
      get => Forme.Materiau;
      set
      {
        Forme.Materiau = value;
        OnPropertyChanged();
      }
    }

    [NotNull]
    public Point Origine
    {
      get => Forme.Origine;
      set
      {
        Forme.Origine = value;
        OnPropertyChanged();
      }
    }

    [NotNull]
    public Quaternion Rotation
    {
      get => Forme.Rotation;
      set
      {
        Forme.Rotation = value;
        OnPropertyChanged();
      }
    }

    public double Prix => Forme.Prix;

    public double Volume => Forme.Volume;

    #endregion

    protected FormeWrapper([NotNull] Forme forme)
    {
      Forme = forme;
    }

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Forme.ToString();

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion
  }
}
