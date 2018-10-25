using System;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Properties;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="Forme"/> en <see cref="FormeWrapper"/>.</summary>
  public class ConvertisseurFormes
  {
    /// <summary>Permet de convertir une Forme en FormeWrapper.</summary>
    /// <param name="forme">Une forme.</param>
    /// <returns>La forme en FormeWrapper</returns>
    [NotNull]
    public FormeWrapper Convertir([NotNull] Forme forme)
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
          throw new ArgumentException(Resources.UnknownChild(nameof(Forme)), nameof(forme));
      }
    }
  }
}
