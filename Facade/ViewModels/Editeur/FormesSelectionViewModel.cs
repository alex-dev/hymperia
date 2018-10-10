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

    public Type DefaultForme => typeof(PrismeRectangulaire);

    #endregion

    public FormesSelectionViewModel()
    {
      Formes = new Dictionary<Type, string>
      {
        { typeof(PrismeRectangulaire), "pack://application:,,,/Images/PrismeRectangulaire.png" },
        { typeof(Cone), "pack://application:,,,/Images/Cone.png" },
        { typeof(Cylindre), "pack://application:,,,/Images/Cylindre.png" },
        { typeof(Ellipsoide), "pack://application:,,,/Images/Ellipsoide.png" }
      };
    }
  }
}