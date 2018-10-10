using System.Windows.Data;
using Hymperia.Facade.BaseClasses;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class FormesProperties : RegionContextAwareUserControl
  {
    public FormesProperties() : base(BindingMode.OneWayToSource)
    {
      InitializeComponent();
    }
  }
}
