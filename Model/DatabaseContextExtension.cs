using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hymperia.Model
{
  public static class DatabaseContextExtension
  {
    #region Find By Id

    /// <summary>Query le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="InvalidOperationException">Aucun <typeparamref name="T"/> n'a été trouvé.</exception>
    [NotNull]
    public static T FindById<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity =>
      data.Where(item => item.Id == id).First();

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="InvalidOperationException">Aucun <typeparamref name="T"/> n'a été trouvé.</exception>
    [NotNull]
    public static Task<T> FindByIdAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity =>
      data.Where(item => item.Id == id).FirstAsync(token);

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> est <see cref="null"/>.</exception>
    [CanBeNull]
    public static T FindByIdOrDefault<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity =>
      data.Where(item => item.Id == id).FirstOrDefault();

    /// <summary>Query asynchronement le <typeparamref name="T"/> avec une clé primaire <paramref name="id"/>.</summary>
    /// <returns>Le <typeparamref name="T"/> trouvé ou <see cref="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> est <see cref="null"/>.</exception>
    [CanBeNull]
    public static Task<T> FindByIdOrDefaultAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity =>
      data.Where(item => item.Id == id).FirstOrDefaultAsync(token);

    #endregion

    #region Includes

    /// <summary>Inclus explicitement les formes et les matériaux dans la query.</summary>
    [NotNull]
    [ItemNotNull]
    public static IQueryable<Projet> IncludeFormes([NotNull][ItemNotNull] this IQueryable<Projet> projets)
    {
      return projets.Include(projet => projet._Formes).ThenInclude(forme => forme.Materiau);
    }

    /// <summary>Inclus explicitement les accès dans la query.</summary>
    [NotNull]
    [ItemNotNull]
    public static IQueryable<Utilisateur> IncludeAcces([NotNull][ItemNotNull] this IQueryable<Utilisateur> projets)
    {
      return projets.Include(utilisateur => utilisateur._Acces).ThenInclude(acces => acces.Projet);
    }

    #endregion

    #region Loading Collections

    #region Projets

    /// <summary>Load les formes du <paramref name="projet"/>.</summary>
    public static void LoadFormes([NotNull] this DatabaseContext context, [NotNull] Projet projet) =>
      context.Entry(projet).Collection(p => p._Formes)
        .Query().Include(forme => forme.Materiau).Load();

    /// <summary>Load asynchronement les formes du <paramref name="projet"/>.</summary>
    public static async Task LoadFormesAsync([NotNull] this DatabaseContext context, [NotNull] Projet projet, [NotNull] CancellationToken token = default) =>
      await context.Entry(projet).Collection(p => p._Formes)
        .Query().Include(forme => forme.Materiau).LoadAsync(token);

    /// <summary>Unload les formes du projets.</summary>
    public static void UnloadFormes([NotNull] this DatabaseContext context, [NotNull] Projet projet)
    {
      var entry = context.Entry(projet);
      var formes = entry.Collection(p => p._Formes);

      foreach (var forme in formes.CurrentValue)
      {
        context.Entry(forme).State = EntityState.Detached;
      }

      formes.CurrentValue = null;
    }

    #endregion

    #endregion
  }
}
