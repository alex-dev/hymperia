using Hymperia.Model;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  /// <summary>Gère la création des <see cref="DatabaseContext"/>.</summary>
  public class ContextFactory
  {
    /// <summary>Wrap <see cref="DatabaseContext()"/>.</summary>
    [NotNull]
    public DatabaseContext GetContext() => new DatabaseContext();

    /// <summary>Wrap <see cref="DatabaseContext(string)"/>.</summary>
    /// <param name="connection">Connection string.</param>
    [NotNull]
    public DatabaseContext GetContext([NotNull] string connection) => new DatabaseContext(connection);
  }
}
