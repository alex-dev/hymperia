using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Identity;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model
{
  public static class DatabaseContextExtension
  {
    /// <summary>Convert a <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> into a <see cref="IDictionary{TKey, TValue}"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="query"/> is <see cref="null"/>.</exception>
    public static async Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(this IQueryable<KeyValuePair<TKey, TValue>> query, CancellationToken token = default)
    {
      if (query is null)
        throw new ArgumentNullException(nameof(query));

      return await query.ToDictionaryAsync(pair => pair.Key, pair => pair.Value, token).ConfigureAwait(false);
    }

    #region Find By Id

    /// <summary>Query le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="InvalidOperationException">Aucun <typeparamref name="T"/> n'a été trouvé.</exception>
    [NotNull]
    public static T FindById<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity =>
      data.Where(item => item.Id == id).First();

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="InvalidOperationException">Aucun <typeparamref name="T"/> n'a été trouvé.</exception>
    [NotNull]
    public static async Task<T> FindByIdAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity =>
      await data.Where(item => item.Id == id).FirstAsync(token).ConfigureAwait(false);

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> est <see cref="null"/>.</exception>
    [CanBeNull]
    public static T FindByIdOrDefault<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity =>
      data.Where(item => item.Id == id).FirstOrDefault();

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <returns>Le <typeparamref name="T"/> trouvé ou <see cref="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> est <see cref="null"/>.</exception>
    [CanBeNull]
    public static async Task<T> FindByIdOrDefaultAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity =>
      await data.Where(item => item.Id == id).FirstOrDefaultAsync(token).ConfigureAwait(false);

    #endregion

    #region Includes

    /// <summary>Inclus explicitement les formes et les matériaux dans la query.</summary>
    [NotNull]
    [ItemNotNull]
    public static IQueryable<Projet> IncludeFormes([NotNull][ItemNotNull] this IQueryable<Projet> projets) =>
      projets.Include(projet => projet._Formes).ThenInclude(forme => forme.Materiau);

    /// <summary>Inclus explicitement les accès dans la query.</summary>
    [NotNull]
    [ItemNotNull]
    public static IQueryable<Utilisateur> IncludeAcces([NotNull][ItemNotNull] this IQueryable<Utilisateur> projets) =>
      projets.Include(utilisateur => utilisateur._Acces).ThenInclude(acces => acces.Projet);

    #endregion

    #region Loading Collections

    #region Projets

    /// <summary>Load les formes du <paramref name="projet"/>.</summary>
    [Obsolete("Interface replaced by more long lived context and requery inside each context.")]
    public static void LoadFormes([NotNull] this DatabaseContext context, [NotNull] Projet projet) =>
      context.Entry(projet).Collection(p => p._Formes)
        .Query().Include(forme => forme.Materiau).Load();

    /// <summary>Load asynchronement les formes du <paramref name="projet"/>.</summary>
    [Obsolete("Interface replaced by more long lived context and requery inside each context.")]
    public static async Task LoadFormesAsync([NotNull] this DatabaseContext context, [NotNull] Projet projet, [NotNull] CancellationToken token = default) =>
      await context.Entry(projet).Collection(p => p._Formes)
        .Query().Include(forme => forme.Materiau)
        .LoadAsync(token).ConfigureAwait(false);

    /// <summary>Unload les formes du <paramref name="projet"/>.</summary>
    [Obsolete("Interface replaced by more long lived context and requery inside each context.")]
    public static void UnloadFormes([NotNull] this DatabaseContext context, [NotNull] Projet projet)
    {
      var entry = context.Entry(projet);
      var formes = entry.Collection(p => p._Formes);

      foreach (var forme in formes.CurrentValue)
        context.Entry(forme).State = EntityState.Detached;

      formes.CurrentValue = null;
    }

    #endregion

    #region Utilisateurs

    /// <summary>Load les formes du <paramref name="utilisateur"/>.</summary>
    public static void LoadProjets([NotNull] this DatabaseContext context, [NotNull] Utilisateur utilisateur) =>
      context.Entry(utilisateur).Collection(u => u._Acces)
        .Query().Include(acces => acces.Projet).Load();

    /// <summary>Load asynchronement les projets de l'<paramref name="utilisateur"/>.</summary>
    public static async Task LoadProjetsAsync([NotNull] this DatabaseContext context, [NotNull] Utilisateur utilisateur, [NotNull] CancellationToken token = default) =>
      await context.Entry(utilisateur).Collection(u => u._Acces)
        .Query().Include(acces => acces.Projet).LoadAsync();

    /// <summary>Unload les projets de l'<paramref name="utilisateur"/>.</summary>
    public static void UnloadProjets([NotNull] this DatabaseContext context, [NotNull] Utilisateur utilisateur)
    {
      var entry = context.Entry(utilisateur);
      var acces = entry.Collection(u => u._Acces);

      foreach (var _acces in acces.CurrentValue)
      {
        context.Entry(_acces.Projet).State = EntityState.Detached;
        context.Entry(_acces).State = EntityState.Detached;
      }

      acces.CurrentValue = null;
    }

    #endregion

    #endregion
  }
}
