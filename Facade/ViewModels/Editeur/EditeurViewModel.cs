using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HelixToolkit.Wpf;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using Hymperia.Model;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private int id;
    private Projet projet;
    private ObservableCollection<MeshElement3D> formes;
    private ObservableCollection<MeshElement3D> selected;

    #endregion

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    private Projet Projet
    {
      get => projet;
      set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        SetProperty(ref projet, value);
        Formes = new ObservableCollection<MeshElement3D>(CreeFormes(Projet.Formes));
      }
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public ObservableCollection<MeshElement3D> Formes
    {
      get => formes;
      private set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        SetProperty(ref formes, value);
      }
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public ObservableCollection<MeshElement3D> Selected
    {
      get => selected;
      private set
      {
        if (value is null)
        {
          throw new ArgumentNullException();
        }

        SetProperty(ref selected, value);
      }
    }

    /// <summary>La clé unique du projet à éditer.</summary>
    public int ProjetId
    {
      get => id;
      set => SetProperty(ref id, value, () => QueryProjet()); // Laisse la query se poursuivre async.
    }

    #endregion

    #region Commands

    #endregion

    #endregion

    #region Services

    [NotNull]
    private ContextFactory ContextFactory { get; set; }

    [NotNull]
    private ConvertisseurFormes Convertisseur { get; set; }

    #endregion

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes convertisseur)
    {
      ContextFactory = factory;
      Convertisseur = convertisseur;

      ProjetId = 1;
    }

    #region Methods

    #region Query Projet

    private async Task QueryProjet()
    {
      // Met le projet à null pour que l'on puisse valider si le projet a été loadé.
      SetProperty(ref projet, null, "Projet");
      SetProperty(ref formes, null, "Formes");

      using (var context = ContextFactory.GetContext())
      {
        Projet = await context.Projets.IncludeFormes().FindByIdAsync(ProjetId);
      }
    }

    #endregion

    #region Intialize Formes

    [ItemNotNull]
    private IEnumerable<MeshElement3D> CreeFormes([ItemNotNull] IEnumerable<Forme> formes)
    {
      int index = -1;

      return from forme in formes
             select CreeForme(forme, ref index);
    }

    [NotNull]
    private MeshElement3D CreeForme([NotNull] Forme forme, ref int index)
    {
      return Convertisseur.Lier(Convertisseur.Convertir(forme), $"Formes[{ ++index }]");
    }

    #endregion

    #endregion
  }
}
