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
using Hymperia.Facade.ModelWrappers;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private int id;
    private Projet projet;
    private ICollection<FormeWrapper<Forme>> changeables;
    private ObservableCollection<MeshElement3D> formes;
    private ObservableCollection<MeshElement3D> selected;

    #endregion

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
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

        projet = value;
        Changeables = new Collection<FormeWrapper<Forme>>();
      }
    }

    /// <summary>Les formes éditables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    /// <remarks>Should never invoke <see cref="PropertyChanged"/> because its changes are propagated to public <see cref="Formes"/>.</remarks>
    private ICollection<FormeWrapper<Forme>> Changeables
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
    private readonly ContextFactory ContextFactory;

    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;

    [NotNull]
    private readonly ConvertisseurFormes ConvertisseurFormes;

    #endregion

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurWrappers wrappers, [NotNull] ConvertisseurFormes formes)
    {
      new System.Windows.Threading.DispatcherTimer(new TimeSpan(5000), System.Windows.Threading.DispatcherPriority.Normal, (sender, args) =>
      {
        if (Projet is null)
          return;

        var r = new Random();
        var x = r.Next(100, 150);

        System.Diagnostics.Debug.Assert(!Projet.Formes.OfType<Ellipsoide>().Any(forme => forme.RayonX == x), "Invalid Test");
        Projet.Formes.OfType<Ellipsoide>().First(forme => forme.RayonX < 100).RayonX = x;
        System.Diagnostics.Debug.Assert(Formes.OfType<EllipsoidVisual3D>().Any(forme => forme.RadiusX == x), "Failed");

        //System.Diagnostics.Debug.Assert(!Projet.Formes.OfType<PrismeRectangulaire>().Any(forme => forme.Hauteur == x), "Invalid Test");
        //Formes.OfType<BoxVisual3D>().First(forme => forme.Height < 100).Height = x;
        //System.Diagnostics.Debug.Assert(Projet.Formes.OfType<PrismeRectangulaire>().Any(forme => forme.Hauteur == x), "Failed");
      }, System.Windows.Threading.Dispatcher.CurrentDispatcher);

      ContextFactory = factory;
      ConvertisseurWrappers = wrappers;
      ConvertisseurFormes = formes;

      ProjetId = 1;
    }

    #region Methods

    #region Query Projet

    private async Task QueryProjet()
    {
      // Met le projet à null pour que l'on puisse valider si le projet a été loadé.
      projet = null;
      formes = null;
      SetProperty(ref formes, null, "Formes");

      using (var context = ContextFactory.GetContext())
      {
        Projet = await context.Projets.IncludeFormes().FindByIdAsync(ProjetId);
      }
    }

    #endregion

    #region Intialize Formes

    [ItemNotNull]
    private IEnumerable<FormeWrapper<Forme>> CreeFormes([ItemNotNull] IEnumerable<Forme> formes)
    {
      return from forme in formes
             select ConvertisseurFormes.Convertir(forme);
    }

    [ItemNotNull]
    private IEnumerable<MeshElement3D> CreeFormes([ItemNotNull] IEnumerable<FormeWrapper<Forme>> formes)
    {
      return from forme in formes
             select ConvertisseurWrappers.Lier(ConvertisseurWrappers.Convertir(forme), forme);
    }

    #endregion

    #endregion
  }
}
