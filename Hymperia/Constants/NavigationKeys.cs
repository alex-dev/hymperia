namespace Hymperia.Facade.Constants
{
  public static class NavigationKeys
  {
    public const string Connexion = nameof(Views.Connexion);
    public const string AffichageProjets = nameof(Views.AffichageProjets);
    public const string Editeur = nameof(Views.Editeur.Editeur);
    public const string Inscription = nameof(Views.Inscription);
    public const string ReglageUtilisateur = nameof(Views.Reglages.Application) + nameof(Views.Reglages.Application.Reglage);
    public const string ReglageEditeur = nameof(Views.Reglages.Editeur) + nameof(Views.Reglages.Editeur.Reglage);
    public const string ReglageBD = nameof(Views.Reglages.BD) + nameof(Views.Reglages.BD.Reglage);
  }
}
