using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class MateriauxSelectionViewModel : BindableBase
  {
    #region Attributes

    [CanBeNull]
    [ItemNotNull]
    public ICollection<Materiau> Materiaux
    {
      get => materiaux;
      private set => SetProperty(ref materiaux, value);
    }

    [CanBeNull]
    public Task<Materiau[]> Loading
    {
      get => loading;
      set => SetProperty(ref loading, value, () =>
      {
        IsLoading = true;
        value.ContinueWith(result => IsLoading = false);
      });
    }

    [NotNull]
    public string DefaultName => "Bois";

    public ICommand RefreshItems { get; private set; }

    private bool IsLoading
    {
      get => isLoading;
      set => SetProperty(ref isLoading, value);
    }

    #endregion

    #region Constructors

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory)
    {
      Factory = factory;
      RefreshItems = new DelegateCommand(RefreshMateriaux).ObservesCanExecute(() => IsLoading);
      RefreshItems.Execute(null);
    }

    #endregion

    #region Queries


    private async void RefreshMateriaux()
    {
      if (IsLoading)
      {
        using (var context = Factory.GetContext())
        {
          Loading = context.Materiaux.ToArrayAsync();
          Materiaux = await Loading;
        }
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private Task<Materiau[]> loading;
    private bool isLoading;
    private ICollection<Materiau> materiaux;

    #endregion
  }
}
