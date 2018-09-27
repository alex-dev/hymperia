using System;
using Hymperia.Model;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Moq
{
  public class MockedContext : DatabaseContext
  {
    public string DatabaseString => Connection;

    public MockedContext() : base() { }

    public MockedContext(string database) : base(database) { }
  }
}
