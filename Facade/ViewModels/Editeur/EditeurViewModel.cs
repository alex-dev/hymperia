﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Hymperia.Model;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private Projet projet;
<<<<<<< HEAD
    private BulkObservableCollection<FormeWrapper<Forme>> formes;
    private BulkObservableCollection<FormeWrapper<Forme>> selected;
=======
    private ICollection<FormeWrapper> changeables;
    private ObservableCollection<MeshElement3D> formes;
    private ObservableCollection<MeshElement3D> selected;
>>>>>>> Avancement T6.3

    #endregion

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
<<<<<<< HEAD
      set => QueryProjet(value, UpdateFormes);
=======
      set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        projet = value;
        Changeables = new Collection<FormeWrapper>();
      }
>>>>>>> Avancement T6.3
    }

    /// <summary>Les formes éditables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
<<<<<<< HEAD
=======
    private ICollection<FormeWrapper> Changeables
    {
      get => changeables;
      set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        changeables = value;
        Formes = new ObservableCollection<MeshElement3D>(CreeFormes(Changeables));
      }
    }

    /// <summary>Les formes affichables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
>>>>>>> Avancement T6.3
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper<Forme>> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value, () => FormesSelectionnees.Clear());
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper<Forme>> FormesSelectionnees
    {
      get => selected;
      private set => SetProperty(ref selected, value);
    }

    #endregion

    #region Commands

    public readonly ICommand AjouterForme;
    public readonly ICommand SupprimerForme;

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
      AjouterForme = new DelegateCommand(_AjouterForme, PeutAjouterForme);
      SupprimerForme = new DelegateCommand(_SupprimerForme, PeutSupprimerForme).ObservesProperty(() => FormesSelectionnees);
      FormesSelectionnees = new BulkObservableCollection<FormeWrapper<Forme>>();
    }

    #region Methods

    #region Command AjouterForme

    private Forme CreerForme()
    {
      throw new NotImplementedException();
    }

    private void _AjouterForme()
    {
      var forme = CreerForme();
      Projet.AjouterForme(forme);
      Formes.Add(ConvertisseurFormes.Convertir(forme));
    }

    // TODO: Add Check
    private bool PeutAjouterForme() => true;

    #endregion

    #region Command SupprimerForme

    private void _SupprimerForme()
    {
      var formes = (from forme in FormesSelectionnees
                    select forme.Forme).ToArray();

      foreach (var forme in formes)
      {
        Projet.SupprimerForme(forme);
      }

      Formes.RemoveRange(FormesSelectionnees);
      FormesSelectionnees.Clear();
    }

    private bool PeutSupprimerForme() => FormesSelectionnees.Count > 0;

    #endregion

    #region Inner Events Handler

<<<<<<< HEAD
    private async Task QueryProjet(Projet _projet, Action onChanged)
=======
    [ItemNotNull]
    private IEnumerable<FormeWrapper> CreeFormes([ItemNotNull] IEnumerable<Forme> formes)
>>>>>>> Avancement T6.3
    {
      // Met le projet à null pour que l'on puisse valider si le projet a été loadé.
      SetProperty(ref projet, null, onChanged, "Projet");

      if (_projet is Projet)
      {
        using (var context = ContextFactory.GetContext())
        {
          context.Attach(_projet);
          await context.Entry(_projet).CollectionFormes().LoadAsync();
        }

        SetProperty(ref projet, _projet, onChanged, "Projet");
      }
    }

<<<<<<< HEAD
    private void UpdateFormes()
=======
    [ItemNotNull]
    private IEnumerable<MeshElement3D> CreeFormes([ItemNotNull] IEnumerable<FormeWrapper> formes)
>>>>>>> Avancement T6.3
    {
      if (Projet is null)
      {
        Formes = null;
      }
      else
      {
        var enumerable = from forme in Projet.Formes
                         select ConvertisseurFormes.Convertir(forme);
        Formes = new BulkObservableCollection<FormeWrapper<Forme>>(enumerable);
      }
    }

    #endregion

    #endregion
  }
}
