using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Events;

namespace Hymperia.Facade.ViewModels.Editeur.ProjetAnalyse
{
  public class MateriauxAnalyseViewModel : ProjetAnalyseBase
  {
    #region Properties

    #region Bindings

    public double Prix
    {
      get => prix;
      private set => SetProperty(ref prix, value);
    }

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

    protected async Task MakeAnalyse()
    {
      Analyse = await ConvertisseurMateriaux.Convertir(Projet.PrixMateriaux);
      // Rather than forcing a full enumeration of the dataset, better optimize a bit and do the sum right here.
      // Anyway, we gotta affect something to Prix so bindings can update.
      Prix = Analyse.Sum(pair => pair.Value.Item2);
    }

    #endregion

    #region Aggregated Event Handlers

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

    private double prix;
    private IDictionary<MateriauWrapper, Tuple<double, double>> analyse;

    #endregion
  }
}
