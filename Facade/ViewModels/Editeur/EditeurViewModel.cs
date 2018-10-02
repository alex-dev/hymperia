using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;
using Prism.Mvvm;
using Hymperia.Model;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Hymperia.Facade.ModelWrappers;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private Projet projet;
    private ObservableCollection<FormeWrapper<Forme>> formes;
    private ObservableCollection<FormeWrapper<Forme>> selected;

    #endregion

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
      set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        QueryProjet(value, UpdateFormes);
      }
    }

    /// <summary>Les formes éditables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public ObservableCollection<FormeWrapper<Forme>> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public ObservableCollection<FormeWrapper<Forme>> FormesSelectionnees
    {
      get => selected;
      private set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        SetProperty(ref selected, value, () => FormesSelectionnees.Clear());
      }
    }

    #endregion

    #region Commands

    #endregion

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;

    [NotNull]
    private readonly ConvertisseurFormes ConvertisseurFormes;

    #endregion

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes formes)
    {
      ContextFactory = factory;
      ConvertisseurFormes = formes;
      FormesSelectionnees = new ObservableCollection<FormeWrapper<Forme>>();
    }

    #region Methods

    private async Task QueryProjet(Projet _projet, Action onChanged)
    {
      // Met le projet à null pour que l'on puisse valider si le projet a été loadé.
      SetProperty(ref projet, null, onChanged, "Projet");

      using (var context = ContextFactory.GetContext())
      {
        context.Attach(_projet);
        await context.Entry(_projet).CollectionFormes().LoadAsync();
      }

      SetProperty(ref projet, _projet, onChanged, "Projet");
    }

    private void UpdateFormes()
    {
      Formes = Projet is null
        ? null
        : new ObservableCollection<FormeWrapper<Forme>>(from forme in Projet.Formes
                                                        select ConvertisseurFormes.Convertir(forme));
    }

    #endregion
  }
}
