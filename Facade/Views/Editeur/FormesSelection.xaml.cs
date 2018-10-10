using System.Windows.Data;
using Hymperia.Facade.BaseClasses;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class FormesSelection : RegionContextAwareUserControl
  {
    public FormesSelection() : base(BindingMode.OneWay)
    {
      InitializeComponent();
    }
  }
}
