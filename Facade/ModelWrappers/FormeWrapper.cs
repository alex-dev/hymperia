using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.ModelWrappers
{
  public abstract class FormeWrapper
  {
    #region Attributs

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

    [NotNull]
    public double Prix => Forme.Prix;

    public double Volume => Forme.Volume;

    #endregion

    protected FormeWrapper(Forme forme)
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
