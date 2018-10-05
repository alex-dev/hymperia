using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class MateriauxSelectionViewModel : BindableBase
  {
    #region Attributes

    private static readonly TaskStatus[] TaskStatusToAccept;

    private Task Loading;

    private ICollection<Materiau> materiaux;

    [CanBeNull]
    public ICollection<Materiau> Materiaux
    {
      get => materiaux;
      private set => SetProperty(ref materiaux, value);
    }

    public ICommand RefreshItems { get; private set; }

    #endregion

    #region Services

    private readonly ContextFactory Factory;

    #endregion

    static MateriauxSelectionViewModel()
    {
      TaskStatusToAccept = new TaskStatus[]
      {
        TaskStatus.Canceled,
        TaskStatus.Faulted,
      };
    }

    public MateriauxSelectionViewModel([NotNull] ContextFactory factory)
    {
      Factory = factory;
      RefreshItems = new DelegateCommand(() => Loading = RefreshMateriaux());
      Loading = RefreshMateriaux();
    }

    private async Task RefreshMateriaux()
    {
      if (!TaskStatusToAccept.Contains(Loading?.Status ?? TaskStatus.RanToCompletion))
      {
        using (var context = Factory.GetContext())
        {
          Materiaux = await context.Materiaux.ToArrayAsync();
        }
      }
    }
  }
}
