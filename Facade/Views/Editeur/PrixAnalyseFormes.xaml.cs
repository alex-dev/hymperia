using System.Collections.Specialized;
using System.ComponentModel;
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
      
      if (!IsBusy() && RegionContext is IProjetViewModel context)
      {
        context.PropertyChanged += ProjetChanged;
      }
    }

    private void ProjetChanged(object sender, PropertyChangedEventArgs args)
    {
      if (sender == RegionContext && sender is IProjetViewModel context)
      {
        switch (args.PropertyName)
        {
          case "Projet":
            Update();
            break;
          case "Formes":
            context.Formes.CollectionChanged += FormesChanged;
            Update();
            break;
        }
      }
    }

    private void FormesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == (RegionContext as IProjetViewModel)?.Formes)
      {
        foreach (FormeWrapper forme in args.NewItems)
        {
          forme.PropertyChanged += FormeChanged;
        }

        Update();
      }
    }

    private void FormeChanged(object sender, PropertyChangedEventArgs args)
    {
      if ((RegionContext as IProjetViewModel)?.Formes?.Contains(sender as FormeWrapper) ?? false)
      {
        Update();
      }
    }

    #endregion
  }
}
