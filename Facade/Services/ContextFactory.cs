using JetBrains.Annotations;
using Hymperia.Model;

namespace Hymperia.Facade.Services
{
  public class ContextFactory
  {
    [NotNull]
    public DatabaseContext GetContext() => new DatabaseContext();
  }
}
