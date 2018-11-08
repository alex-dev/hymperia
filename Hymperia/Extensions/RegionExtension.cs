using JetBrains.Annotations;
using Prism.Regions;

namespace Hymperia.Facade.Extensions
{
  public static class RegionExtension
  {
    public static void Deactivate([NotNull] this IRegion region)
    {
      foreach (var view in region.ActiveViews)
        region.Deactivate(view);
    }
  }
}
