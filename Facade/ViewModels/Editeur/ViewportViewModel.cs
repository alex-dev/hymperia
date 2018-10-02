using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HelixToolkit.Wpf;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private ObservableCollection<MeshElement3D> formes;
    private ObservableCollection<MeshElement3D> selected;

    #endregion

    #region Binding

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
    private readonly ConvertisseurWrappers ConvertisseurWrappers;

    #endregion

    public ViewportViewModel([NotNull] ConvertisseurWrappers wrappers)
    {
      ConvertisseurWrappers = wrappers;

      ProjetId = 1;
    }

    #region Methods

    [ItemNotNull]
    private IEnumerable<MeshElement3D> CreeFormes([ItemNotNull] IEnumerable<FormeWrapper<Forme>> formes)
    {
      return from forme in formes
             select ConvertisseurWrappers.Lier(ConvertisseurWrappers.Convertir(forme), forme);
    }

    #endregion
  }
}
