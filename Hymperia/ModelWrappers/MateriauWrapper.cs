using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Hymperia.Model.Localization;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Facade.ModelWrappers
{
  /// <summary>Wrap les matériaux du modèle avec un <see cref="INotifyPropertyChanged"/> wrapper.</summary>
  public class MateriauWrapper : INotifyPropertyChanged
  {
    #region Attributs

    [NotNull]
    public Materiau Materiau { get; }

    [CanBeNull]
    public LocalizedMateriau Localized
    {
      get => localized;
      set
      {
        localized = value;
        OnPropertyChanged(nameof(Nom));
      }
    }

    public int Id => Materiau.Id;

    [NotNull]
    public string Nom => Localized?.Nom ?? Materiau.Nom;

    public double Prix => Materiau.Prix;

    public SolidColorBrush Fill => Materiau.Fill;

    #endregion

    #region Asynchronous Loading

    [CanBeNull]
    private Task Loading
    {
      get => loading;
      set
      {
        (loading = value)
          .ContinueWith(
            result => throw result.Exception.Flatten(),
            default,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext())
          .ContinueWith(result => IsLoading = false, TaskScheduler.FromCurrentSynchronizationContext());
        IsLoading = true;
        OnPropertyChanged();
      }
    }

    public bool IsLoading
    {
      get => isLoading;
      private set
      {
        isLoading = value;
        OnPropertyChanged();
      }
    }

    #endregion

    public MateriauWrapper([NotNull] Materiau materiau)
    {
      Materiau = materiau;
      Loading = QueryLocalized();
    }

    public MateriauWrapper([NotNull] Materiau materiau, [NotNull] LocalizedMateriau localized)
    {
      if (localized.StringKey != materiau.Nom)
        throw new ArgumentException(Properties.Resources.InvalidLocalizationKey(localized.StringKey, materiau));

      Materiau = materiau;
      Localized = localized;
    }

    #region Query Localized

    private async Task QueryLocalized() => Localized = await Resources.GetMateriau(Materiau.Nom);

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Materiau.ToString();

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion

    #region Fields

    private LocalizedMateriau localized;
    private Task loading;
    private bool isLoading;

    #endregion
  }
}
