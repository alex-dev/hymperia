using System;
using System.Diagnostics.CodeAnalysis;
using Hymperia.Facade.Constants;
using Hymperia.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Facade.Services
{
  /// <summary>Gère la création des <see cref="DatabaseContext"/>.</summary>
  public sealed class ContextFactory : IDisposable
  {
    /// <summary>Wrap <see cref="DatabaseContext()"/>.</summary>
    [NotNull]
    public DatabaseContext GetContext() => new DatabaseContext();

    #region EditeurContext

    [NotNull]
    public IContextWrapper<DatabaseContext> GetEditeurContext()
    {
      if (EditeurContext is null)
        EditeurContext = new Tracker<DatabaseContext>();

      ++EditeurContext.Count;
      return new ContextWrapper<DatabaseContext>(EditeurContext.Context, () => Release(ref EditeurContext));
    }

    private Tracker<DatabaseContext> EditeurContext;

    #endregion

    #region ReglageEditeurContext

    [NotNull]
    public IContextWrapper<DatabaseContext> GetReglageEditeurContext()
    {
      if (ReglageEditeurContext is null)
        ReglageEditeurContext = new Tracker<DatabaseContext>();

      ++ReglageEditeurContext.Count;
      return new ContextWrapper<DatabaseContext>(ReglageEditeurContext.Context, () => Release(ref ReglageEditeurContext));
    }

    private Tracker<DatabaseContext> ReglageEditeurContext;

    #endregion

    #region ReglageUtilisateurContext

    [NotNull]
    public IContextWrapper<DatabaseContext> GetReglageUtilisateurContext()
    {
      if (ReglageUtilisateurContext is null)
        ReglageUtilisateurContext = new Tracker<DatabaseContext>();

      ++ReglageUtilisateurContext.Count;
      return new ContextWrapper<DatabaseContext>(ReglageUtilisateurContext.Context, () => Release(ref ReglageUtilisateurContext));
    }

    private Tracker<DatabaseContext> ReglageUtilisateurContext;

    #endregion

    public interface IContextWrapper<T> : IDisposable where T : DbContext
    {
      T Context { get; }
    }

    #region Dispose

    public void Dispose()
    {
      ReglageUtilisateurContext.Dispose();
      EditeurContext.Dispose();
      ReglageEditeurContext.Dispose();
    }

    #region Dispose Wrapper

    private sealed class ContextWrapper<T> : IContextWrapper<T> where T : DbContext
    {
      private Action DisposeAction { get; }
      public T Context { get; }

      public ContextWrapper(T context, Action dispose)
      {
        DisposeAction = dispose;
        Context = context;
      }

      public void Dispose() => DisposeAction();
    }

    private void Release<T>(ref Tracker<T> tracker) where T : DbContext, new()
    {
      if (--tracker.Count == 0)
      {
        tracker.Dispose();
        tracker = null;
      }
    }

    #endregion

    #region Tracker

    private sealed class Tracker<T> : IDisposable where T : DbContext, new()
    {
      public int Count { get; set; }
      public T Context { get; } = new T();

      [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
        Justification = @"Done through property.")]
      public void Dispose() => Context.Dispose();
    }

    #endregion

    #endregion

  }
}