using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Identity;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoreLinq;

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

    [NotNull]
    [ItemNotNull]
    public static IQueryable<Acces> IncludeUtilisateurs([NotNull][ItemNotNull] this IQueryable<Acces> acces) =>
      acces.Include(_acces => _acces.Utilisateur);

    [NotNull]
    [ItemNotNull]
    public static IQueryable<Acces> IncludeProjets([NotNull][ItemNotNull] this IQueryable<Acces> acces) =>
      acces.Include(_acces => _acces.Projet);

    #endregion
  }
}
