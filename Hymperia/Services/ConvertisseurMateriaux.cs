using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="Materiau"/> en <see cref="MateriauWrapper"/>.</summary>
  public class ConvertisseurMateriaux
  {
    /// <summary>Permet de convertir une Forme en FormeWrapper.</summary>
    /// <param name="forme">Une forme.</param>
    /// <returns>La forme en FormeWrapper</returns>
    [NotNull]
    public async Task<MateriauWrapper> Convertir([NotNull] Materiau materiau) =>
      new MateriauWrapper(materiau, await Resources.GetMateriau(materiau.Nom));

    [NotNull]
    [ItemNotNull]
    public async Task<IQueryable<MateriauWrapper>> Convertir([NotNull][ItemNotNull] IQueryable<Materiau> materiaux) =>
      from materiau in materiaux
      join localized in await Resources.LoadMateriaux()
        on materiau.Nom equals localized.Key
      select new MateriauWrapper(materiau, localized.Value);
  }
}
