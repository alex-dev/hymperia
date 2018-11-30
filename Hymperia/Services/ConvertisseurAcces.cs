using System.Collections.Generic;
using System.Linq;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="Acces"/> en <see cref="AccesWrapper"/>.</summary>
  public class ConvertisseurAcces
  {
    [NotNull]
    public AccesWrapper Convertir([NotNull] Acces acces) => new AccesWrapper(acces);
  }
}
