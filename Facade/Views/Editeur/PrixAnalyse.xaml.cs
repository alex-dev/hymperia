using System.Windows.Data;
using Hymperia.Facade.BaseClasses;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class PrixAnalyse : RegionContextAwareUserControl
  {
    public PrixAnalyse() : base(BindingMode.OneWayToSource)
    {
      InitializeComponent();
    }
  }
}
