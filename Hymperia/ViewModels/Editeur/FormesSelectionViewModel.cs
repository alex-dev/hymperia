using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
#pragma warning disable 4014 
  public class FormesSelectionViewModel : BindableBase
  {
    #region Properties

    [CanBeNull]
    [ItemNotNull]
    public IDictionary<Type, string> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    [NotNull]
    public Type DefaultForme => typeof(PrismeRectangulaire);

    #endregion

    #region Constructors

    public FormesSelectionViewModel()
    {
      QueryFormes();
    }

    #endregion

    #region Queries

    private async Task QueryFormes()
    {
      await Task.Run(() => Formes = new Dictionary<Type, string>
      {
        { typeof(PrismeRectangulaire), @"pack://application:,,,/Images/PrismeRectangulaire.png" },
        { typeof(Cone), @"pack://application:,,,/Images/Cone.png" },
        { typeof(Cylindre), @"pack://application:,,,/Images/Cylindre.png" },
        { typeof(Ellipsoide), @"pack://application:,,,/Images/Ellipsoide.png" }
      });
    }

    #endregion

    #region Private Fields

    private IDictionary<Type, string> formes;

    #endregion
  }
}