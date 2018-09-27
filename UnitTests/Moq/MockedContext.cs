using System;
using Hymperia.Model;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Moq
{
  public class MockedContext : DatabaseContext
  {
    public readonly string DatabaseString;

    public MockedContext()
    {
      DatabaseString = new Guid().ToString();
    }

    public MockedContext(string database)
    {
      DatabaseString = database;
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseInMemoryDatabase(DatabaseString);
      builder.EnableRichDataErrorHandling();
      builder.EnableSensitiveDataLogging();
      BaseOnConfiguring(builder);
    }
  }
}
