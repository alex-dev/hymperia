using System.Resources;

namespace Hymperia.Model.Properties
{
  public static partial class Resources
  {
    private static readonly ResourceManager dbmanager = new DatabaseResourceManager();

    public LocalizationContext LocalizationContext => dbmanager.Context;

  }
}
