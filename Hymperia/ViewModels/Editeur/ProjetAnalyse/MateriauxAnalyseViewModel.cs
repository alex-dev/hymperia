using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using MoreLinq;
using Prism.Events;

namespace Hymperia.Facade.ViewModels.Editeur.ProjetAnalyse
{
  public class MateriauxAnalyseViewModel : ProjetAnalyseBase
  {
    #region Properties

    #region Bindings

    /// <remarks>Cannot use magic tuples here because I need bindable values.</remarks>
    public IDictionary<MateriauWrapper, Tuple<double, double>> Analyse
    {
      get => analyse;
      private set => SetProperty(ref analyse, value);
    }

    #endregion

    #endregion

    #region Constructors

    public MateriauxAnalyseViewModel([NotNull] ConvertisseurMateriaux convertisseur, [NotNull] IEventAggregator aggregator)
      : base(aggregator)
    {
      ConvertisseurMateriaux = convertisseur;
    }

    #endregion

    #region Analyse

    protected async Task MakeAnalyse() =>
      // C# doesn't support deconstruction in from/let clause... Yet!
      // Analyse = (from (materiau, prix) in Projet.PrixMateriaux
      Analyse = (from pair in Projet.PrixMateriaux
                   let materiau = pair.Key
                   let prix = pair.Value
                 join localized in await Resources.LoadMateriaux()
                   on materiau.Nom equals localized.Key
                 select new KeyValuePair<MateriauWrapper, Tuple<double, double>>(
                   new MateriauWrapper(materiau, localized.Value),
                   Tuple.Create(prix / materiau.Prix, prix))).ToDictionary();

    #endregion

    #region ProjetChanged Handling

    protected override void OnProjetChanged(Projet projet)
    {
      base.OnProjetChanged(projet);
      AnalysisLoader.Loading = MakeAnalyse();
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ConvertisseurMateriaux ConvertisseurMateriaux;

    #endregion

    #region Private Fields

    private IDictionary<MateriauWrapper, Tuple<double, double>> analyse;

    #endregion
  }
}
