using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.ViewModels.Editeur;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class PrixAnalyseFormes : RegionContextAwareUserControl
  {
    public Projet Projet => (RegionContext as IProjetViewModel)?.Projet;

    public PrixAnalyseFormes() : base()
    {
      InitializeComponent();
    }

    #region Region Context Changed Handlers

    private void Update()
    {
      MateriauxPrix.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
      PrixTotal.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
    }

    protected override void RegionContextChanged(object sender, PropertyChangedEventArgs e)
    {
      base.RegionContextChanged(sender, e);

      if (RegionContext is IProjetViewModel context)
      {
        context.PropertyChanged += ProjetChanged;
      }
    }

    private void ProjetChanged(object sender, PropertyChangedEventArgs e)
    {
      if (sender is IProjetViewModel context)
      {
        switch (e.PropertyName)
        {
          case "Formes":
            if (!(context.Formes is null))
            {
              context.Formes.CollectionChanged += FormesChanged;
              foreach (FormeWrapper forme in context.Formes)
              {
                forme.PropertyChanged += FormeChanged;
              }

              Update();
            }
            break;
        }
      }
    }

    private void FormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      foreach (FormeWrapper forme in (IEnumerable)e.NewItems ?? Enumerable.Empty<FormeWrapper>())
      {
        forme.PropertyChanged += FormeChanged;
      }

      Update();
    }

    private void FormeChanged(object sender, PropertyChangedEventArgs e) => Update();

    #endregion
  }
}
