using Hymperia.Model;

namespace ModelIntegrationTests
{
  public class MockedContext : DatabaseContext
  {
    public string DatabaseString => Connection;

    public MockedContext() : base() { }

    public MockedContext(string database) : base(database) { }
  }
}
