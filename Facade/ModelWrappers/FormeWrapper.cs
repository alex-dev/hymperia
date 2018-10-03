using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ModelWrappers
{
  public abstract class FormeWrapper<T> : INotifyPropertyChanged where T : Forme
  {
    #region Attribute

    public readonly T Forme;

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

    #endregion

    public FormeWrapper(T forme)
    {
      Forme = forme;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion
  }
}
