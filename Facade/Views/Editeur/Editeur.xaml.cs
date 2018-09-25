using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf;

namespace Hymperia.Facade.Views.Editeur
{
  /// <summary>
  /// Logique d'interaction pour Viewport.xaml
  /// </summary>
  public partial class Editeur : UserControl
  {
    #region Dependancy Properties

    public static DependencyProperty SourceFormesProperty;
    public static DependencyProperty SelectedItemsProperty;

    #endregion

    #region Properties

    public ObservableCollection<MeshElement3D> SourceFormes
    {
      get => (ObservableCollection<MeshElement3D>)GetValue(SourceFormesProperty);
      set
      {
        SourceFormes.CollectionChanged -= CollectionChanged;
        value.CollectionChanged += CollectionChanged;
        SetValue(SourceFormesProperty, value);
      }
    }

    public ObservableCollection<MeshElement3D> SelectedItems
    {
      get => (ObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set
      {
        SelectedItems.CollectionChanged -= SelectedItemsChanged;
        value.CollectionChanged += SelectedItemsChanged;
        SetValue(SelectedItemsProperty, value);
      }
    }

    #endregion

    public Editeur()
    {
      InitializeComponent();
    }

    #region Methods

    private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      throw new System.NotImplementedException();
    }

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      throw new System.NotImplementedException();
    }

    #endregion
  }
}
