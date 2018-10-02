using System;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Services
{
  public class ConvertisseurFormes
  {
    /// <summary>
    /// Permet de convertir une Forme en FormeWrapper.
    /// </summary>
    /// <param name="forme">Une forme.</param>
    /// <returns>La forme en FormeWrapper</returns>
    public FormeWrapper Convertir(Forme forme)
    {
      switch (forme)
      {
        case Cone cone:
          return new ConeWrapper(cone);
        case Cylindre cylindre:
          return new CylindreWrapper(cylindre);
        case Ellipsoide ellipsoide:
          return new EllipsoideWrapper(ellipsoide);
        case PrismeRectangulaire prisme:
          return new PrismeRectangulaireWrapper(prisme);
        default:
          throw new ArgumentException("Unknown child of Form.", nameof(forme));
      }
    }
  }
}
