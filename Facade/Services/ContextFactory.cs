using Hymperia.Model;
using JetBrains.Annotations;

namespace Hymperia.Facade.Services
{
  public class ContextFactory
  {
    [NotNull]
    public DatabaseContext GetContext() => new DatabaseContext();
  }
}
