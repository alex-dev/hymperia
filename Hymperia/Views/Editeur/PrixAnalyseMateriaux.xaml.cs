using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.ViewModels.Editeur;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class PrixAnalyseMateriaux : RegionContextAwareUserControl
  {
    public Projet Projet => (RegionContext as IProjetViewModel)?.Projet;

    public PrixAnalyseMateriaux() : base()
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

      if (!IsBusy() && RegionContext is IProjetViewModel context)
      {
        context.PropertyChanged += ProjetChanged;
      }
    }

    private void ProjetChanged(object sender, PropertyChangedEventArgs e)
    {
      if (sender == RegionContext && sender is IProjetViewModel context)
      {
        switch (e.PropertyName)
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

    private void FormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (sender == (RegionContext as IProjetViewModel)?.Formes)
      {
        foreach (FormeWrapper forme in e.NewItems)
        {
          forme.PropertyChanged += FormeChanged;
        }

        Update();
      }
    }

    private void FormeChanged(object sender, PropertyChangedEventArgs e)
    {
      if ((RegionContext as IProjetViewModel)?.Formes?.Contains(sender as FormeWrapper) ?? false)
      {
        Update();
      }
    }

    #endregion
  }
}
