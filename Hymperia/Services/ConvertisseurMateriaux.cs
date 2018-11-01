using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

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
      new MateriauWrapper(materiau, await Resources.GetMateriau(materiau.Nom).ConfigureAwait(false));

    [NotNull]
    [ItemNotNull]
    public async Task<MateriauWrapper[]> Convertir([NotNull][ItemNotNull] IQueryable<Materiau> materiaux) =>
      await (from materiau in materiaux
             join localized in await Resources.LoadMateriaux().ConfigureAwait(false)
               on materiau.Nom equals localized.Key
             select new MateriauWrapper(materiau, localized.Value)).ToArrayAsync().ConfigureAwait(false);

    public async Task<IDictionary<MateriauWrapper, Tuple<double, double>>> Convertir([NotNull][ItemNotNull] IEnumerable<KeyValuePair<Materiau, double>> materiaux) =>
      // C# doesn't support deconstruction in from/let clause... Yet!
      // (from (materiau, prix) in Projet.PrixMateriaux
      (from pair in materiaux
       let materiau = pair.Key
       let prix = pair.Value
       join localized in await Resources.LoadMateriaux().ConfigureAwait(false)
         on materiau.Nom equals localized.Key
       select new KeyValuePair<MateriauWrapper, Tuple<double, double>>(
         new MateriauWrapper(materiau, localized.Value),
         Tuple.Create(prix / materiau.Prix, prix))).ToDictionary();
  }
}
