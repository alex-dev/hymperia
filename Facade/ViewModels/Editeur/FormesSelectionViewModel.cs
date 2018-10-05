using System;
using System.Collections.Generic;
using Hymperia.Model.Modeles;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class FormesSelectionViewModel : BindableBase
  {
    #region Attributes

    private IDictionary<Type, string> formes;

    public IDictionary<Type, string> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    #endregion

    public FormesSelectionViewModel()
    {
      Formes = new Dictionary<Type, string>
      {
        { typeof(PrismeRectangulaire), "pack://application:,,,/Images/PrismeRectangulaire.jpg" },
        { typeof(Cone), "pack://application:,,,/Images/PrismeRectangulaire.jpg" },
        { typeof(Cylindre), "pack://application:,,,/Images/PrismeRectangulaire.jpg" },
        { typeof(Ellipsoide), "pack://application:,,,/Images/PrismeRectangulaire.jpg" }
      };
    }

  }
}