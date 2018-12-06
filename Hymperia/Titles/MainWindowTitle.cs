using Hymperia.Facade.Properties;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Titles;

namespace Hymperia.Facade.Titles
{
  public class MainWindowTitle : ITitle
  {
    [Pure]
    [NotNull]
    public sealed override string ToString() => Title;

    public event TitleChangedEventHandler TitleChanged;

    [NotNull]
    public string Title => InnerTitle?.ToString() ?? Resources.Hymperia;

    public void SetTitle([NotNull] string subtitle) => InnerTitle = new SubtitledTitle(subtitle);
    public void SetTitle([NotNull] Utilisateur utilisateur) => InnerTitle = new UtilisateurTitle(utilisateur);
    public void SetTitle([NotNull] Projet projet) => InnerTitle = new ProjetTitle((InnerTitle as UtilisateurTitle)?.Utilisateur, projet);

    protected void RaiseTitleChanged() => TitleChanged?.Invoke(this, new TitleChangedEventArgs(Title));

    [CanBeNull]
    private WindowTitle InnerTitle
    {
      get => title;
      set
      {
        title = value;
        RaiseTitleChanged();
      }
    }

    private WindowTitle title;

    #region Inner Titles

    private abstract class WindowTitle
    {
      [Pure]
      [NotNull]
      public abstract override string ToString();
    }

    private class SubtitledTitle : WindowTitle
    {
      [NotNull]
      public string Subtitle { get; }

      public SubtitledTitle([NotNull] string subtitle)
      {
        Subtitle = subtitle;
      }

      [Pure]
      [NotNull]
      public override string ToString() => $"{ Resources.Hymperia } - { Subtitle }";
    }

    private class UtilisateurTitle : WindowTitle
    {
      [CanBeNull]
      public Utilisateur Utilisateur { get; }

      public UtilisateurTitle([CanBeNull] Utilisateur user)
      {
        Utilisateur = user;
      }

      [Pure]
      [NotNull]
      public override string ToString() => $"{ Resources.Hymperia } - { Utilisateur?.Nom }";
    }

    private class ProjetTitle : UtilisateurTitle
    {
      [CanBeNull]
      public Projet Projet { get; }

      public ProjetTitle([CanBeNull] Utilisateur user, [CanBeNull] Projet projet) : base(user)
      {
        Projet = projet;
      }

      [Pure]
      [NotNull]
      public override string ToString() => $"{ Resources.Hymperia } - { Utilisateur?.Nom } - { Projet?.Nom }";
    }

    #endregion
  }
}
