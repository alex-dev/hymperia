using System;
using System.Collections.Generic;
using Hymperia.Model;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  /// <summary>Gère la création des <see cref="DatabaseContext"/>.</summary>
  public class ContextFactory : IDisposable
  {
    /// <summary>Wrap <see cref="DatabaseContext()"/>.</summary>
    [NotNull]
    public DatabaseContext GetContext() => new DatabaseContext();

    [NotNull]
    public DatabaseContext GetEditorContext()
    {
      if (EditorContext is null)
        EditorContext = new Tracker();

      ++EditorContext.Count;
      return EditorContext.Context;
    }
    public void ReleaseEditorContext()
    {
      if (--EditorContext.Count == 0)
      {
        EditorContext.Dispose();
        EditorContext = null;
      }
    }

    public void Dispose() => EditorContext.Dispose();

    private sealed class Tracker : IDisposable
    {
      public int Count { get; set; }
      public DatabaseContext Context { get; } = new DatabaseContext();

      public void Dispose() => Context.Dispose();
    }

    private Tracker EditorContext;
  }
}
