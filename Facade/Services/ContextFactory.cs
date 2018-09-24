using Hymperia.Model;

namespace Hymperia.Facade.Services
{
  public class ContextFactory
  {
    public DatabaseContext GetContext() => new DatabaseContext();
  }
}
