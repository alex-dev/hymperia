using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model.Modeles;

namespace Hymperia.Model
{
  public static class DatabaseContextExtension
  {
    #region FindById

    public static T FindById<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity
    {
      return data.Where(item => item.Id == id).First();
    }

    public static T FindById<T>([NotNull]this IQueryable<T> data, int id, [NotNull] Expression<Func<T, bool>> predicate) where T : IIdentity
    {
      return data.Where(item => item.Id == id).First(predicate);
    }

    public static Task<T> FindByIdAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstAsync(token);
    }

    public static Task<T> FindByIdAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] Expression<Func<T, bool>> predicate, [NotNull] CancellationToken token = default) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstAsync(predicate, token);
    }

    public static T FindByIdOrDefault<T>([NotNull] this IQueryable<T> data, int id) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstOrDefault();
    }

    public static T FindByIdOrDefault<T>([NotNull] this IQueryable<T> data, int id, [NotNull] Expression<Func<T, bool>> predicate) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstOrDefault(predicate);
    }

    public static Task<T> FindByIdOrDefaultAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] CancellationToken token = default) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstAsync(token);
    }

    public static Task<T> FindByIdOrDefaultAsync<T>([NotNull] this IQueryable<T> data, int id, [NotNull] Expression<Func<T, bool>> predicate, [NotNull] CancellationToken token = default) where T : IIdentity
    {
      return data.Where(item => item.Id == id).FirstAsync(predicate, token);
    }

    #endregion

    public static IQueryable<Projet> IncludeFormes(this IQueryable<Projet> projets)
    {
      return projets.Include(projet => projet._Formes).ThenInclude(forme => forme.Materiau);
    }

    public static IQueryable<Utilisateur> IncludeAcces(this IQueryable<Utilisateur> projets)
    {
      return projets.Include(utilisateur => utilisateur._Acces).ThenInclude(acces => acces.Projet);
    }
  }
}
