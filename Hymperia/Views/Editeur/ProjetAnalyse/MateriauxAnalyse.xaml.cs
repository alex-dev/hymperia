using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.ViewModels.Editeur.ProjetAnalyse;
using Prism.Mvvm;

namespace Hymperia.Facade.Views.Editeur.ProjetAnalyse
{
  public partial class MateriauxAnalyse : UserControl
  {
    public MateriauxAnalyse()
    {
      InitializeComponent();
      TotalExpr = Total.GetBindingExpression(TextBlock.TextProperty);
      MateriauxExpr = Materiaux.GetBindingExpression(ItemsControl.ItemsSourceProperty);

      //DataContextChanged += OnDataContextChanged;
      //(DataContext as BindableBase)?.Add(OnDataContextPropertyChanged);
    }

    //#region DataContext Handling

    //private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    //{
    //  (e.OldValue as BindableBase)?.Remove(OnDataContextPropertyChanged);
    //  (e.NewValue as BindableBase)?.Add(OnDataContextPropertyChanged);

    //  TotalExpr.UpdateTarget();
    //  MateriauxExpr.UpdateTarget();
    //}

    //private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //  switch (e.PropertyName)
    //  {
    //    case nameof(MateriauxAnalyseViewModel.Projet):
    //      TotalExpr.UpdateTarget(); break;
    //    case nameof(MateriauxAnalyseViewModel.Analyse):
    //      MateriauxExpr.UpdateTarget(); break;
    //  }
    //}

    //#endregion

    #region Private Fields

    private readonly BindingExpression TotalExpr;
    private readonly BindingExpression MateriauxExpr;

    #endregion
  }
}
