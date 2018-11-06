using System;
using System.Diagnostics.CodeAnalysis;
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

    [NotNull]
    public IContextWrapper<DatabaseContext> GetEditorContext()
    {
      if (EditorContext is null)
        EditorContext = new Tracker<DatabaseContext>();

      ++EditorContext.Count;
      return new ContextWrapper<DatabaseContext>(EditorContext.Context, () => Release(ref EditorContext));
    }

    #region IDisposable

    public void Dispose() => EditorContext.Dispose();

    #endregion

    #region Wrapper

    public interface IContextWrapper<T> : IDisposable where T : DbContext
    {
      T Context { get; }
    }

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

    #endregion

    #region Trackers

    private void Release<T>(ref Tracker<T> tracker) where T : DbContext, new()
    {
      if (--tracker.Count == 0)
      {
        tracker.Dispose();
        tracker = null;
      }
    }

    private Tracker<DatabaseContext> EditorContext;

    private sealed class Tracker<T> : IDisposable where T : DbContext, new()
    {
      public int Count { get; set; }
      public T Context { get; } = new T();

      [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
        Justification = @"Done through property.")]
      public void Dispose() => Context.Dispose();
    }

    #endregion
  }
}
