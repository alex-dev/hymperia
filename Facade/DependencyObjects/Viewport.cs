using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;

namespace Hymperia.Facade.DependencyObjects
{
  public class Viewport : HelixViewport3D
  {
    #region Dependancy Properties

    public static readonly DependencyProperty SourceFormesProperty;
    public static readonly DependencyProperty SelectedItemsProperty;

    #endregion

    #region Properties

    public ObservableCollection<MeshElement3D> SourceFormes
    {
      get => (ObservableCollection<MeshElement3D>)GetValue(SourceFormesProperty);
      set
      {
        value.CollectionChanged += SourceFormesChanged;
        SetValue(SourceFormesProperty, value);
      }
    }

    public ObservableCollection<MeshElement3D> SelectedItems
    {
      get => (ObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set
      {
        value.CollectionChanged += SelectedItemsChanged;
        SetValue(SelectedItemsProperty, value);
      }
    }

    #endregion

    static Viewport()
    {
      SourceFormesProperty = DependencyProperty.Register("SourceFormes", typeof(BulkObservableCollection<MeshElement3D>), typeof(Viewport));
      SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(BulkObservableCollection<MeshElement3D>), typeof(Viewport));
    }

    public Viewport() : base() { }

    #region Methods

    private void SourceFormesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == SourceFormes)
      {
        throw new System.NotImplementedException();
      }
    }

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == SelectedItems)
      {
        throw new System.NotImplementedException();
      }
    }

    #endregion
  }
}
