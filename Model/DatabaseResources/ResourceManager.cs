using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymperia.Model.DatabaseResources
{
  internal class ResourceManager
  {
    protected Dictionary<string, ResourceSet>

    #region Services

    public LocalizationContext Context { get; } = new LocalizationContext();

    #endregion
  }
}
