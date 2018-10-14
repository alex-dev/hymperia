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

    [NotNull]
    public string DefaultName => "Bois";

    public ICommand RefreshItems { get; private set; }

    #endregion

    #region Constructors

    static MateriauxSelectionViewModel()
    {
      TaskStatusToAccept = new TaskStatus[]
      {
        TaskStatus.Canceled,
        TaskStatus.Faulted,
        TaskStatus.RanToCompletion
      };
    }

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory)
    {
      Factory = factory;
      RefreshItems = new DelegateCommand(() => Loading = RefreshMateriaux());
      Loading = RefreshMateriaux();
    }

    #endregion

    #region Queries

    private static readonly TaskStatus[] TaskStatusToAccept;
    private Task Loading;

    private async Task RefreshMateriaux()
    {
      if (TaskStatusToAccept.Contains(Loading?.Status ?? TaskStatus.RanToCompletion))
      {
        using (var context = Factory.GetContext())
        {
          Materiaux = await context.Materiaux.ToArrayAsync();
        }
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private ICollection<Materiau> materiaux;

    #endregion
  }
}
