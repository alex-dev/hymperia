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

    protected override void RegionContextChanged(object sender, PropertyChangedEventArgs args)
    {
      base.RegionContextChanged(sender, args);
      
      if (RegionContext is IProjetViewModel context)
      {
        context.PropertyChanged += ProjetChanged;
      }
    }

    private void ProjetChanged(object sender, PropertyChangedEventArgs args)
    {
      if (sender is IProjetViewModel context)
      {
        switch (args.PropertyName)
        {
          case "Formes":
            if (!(context.Formes is null))
            {
              context.Formes.CollectionChanged += FormesChanged;
              Update();
            }
            break;
        }
      }
    }

    private void FormesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == (RegionContext as IProjetViewModel)?.Formes)
      {
        foreach (FormeWrapper forme in (IEnumerable)args.NewItems ?? Enumerable.Empty<FormeWrapper>())
        {
          forme.PropertyChanged += FormeChanged;
        }

        Update();
      }
    }

    private void FormeChanged(object sender, PropertyChangedEventArgs args) => Update();

    #endregion
  }
}
