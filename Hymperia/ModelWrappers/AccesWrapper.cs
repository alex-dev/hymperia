using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// <summary>Wrap les accès du modèle avec un <see cref="INotifyPropertyChanged"/> wrapper.</summary>
  public class AccesWrapper : INotifyPropertyChanged
  {
    #region Attributs

    [NotNull]
    public readonly Acces Acces;

    public Projet Projet => Acces.Projet;

    public Utilisateur Utilisateur => Acces.Utilisateur;

    public Acces.Droit DroitDAcces
    {
      get => Acces.DroitDAcces;
      set
      {
        Acces.DroitDAcces = value;
        OnPropertyChanged();
      }
    }

    public bool EstPropriétaire => Acces.EstPropriétaire;

    public bool PeutModifier => Acces.PeutModifier;

    #endregion

    public AccesWrapper([NotNull] Acces acces)
    {
      Acces = acces;
    }

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Acces.ToString();

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion
  }
}
