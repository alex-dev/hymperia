using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
{
  /// <summary>
  /// Logique d'interaction pour SelectionExploration.xaml
  /// </summary>
  public partial class SelectionExploration : UserControl
  {
    public SelectionExploration()
    {
      InitializeComponent();

      var vm = new MainWindowViewModel(viewport.Viewport);
      DataContext = vm;
      viewport.InputBindings.Add(new MouseBinding(vm.RectangleSelectionCommand, new MouseGesture(MouseAction.LeftClick, ModifierKeys.Shift)));
      viewport.InputBindings.Add(new MouseBinding(vm.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick)));
      viewport.InputBindings.Add(new MouseBinding(vm.PointSelectionCommand2, new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));

    }
  }

  public class MainWindowViewModel : INotifyPropertyChanged
  {
    private IList<Model3D> selectedModels;
    private IList<Visual3D> selectedVisuals;

    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public MainWindowViewModel(Viewport3D viewport)
    {
      RectangleSelectionCommand = new RectangleSelectionCommand(viewport, (sender, args) => { selectedVisuals.Clear(); HandleSelectionVisualsEvent(sender, args); });
      PointSelectionCommand = new PointSelectionCommand(viewport, (sender, args) => { selectedVisuals.Clear(); HandleSelectionVisualsEvent(sender, args); });
      PointSelectionCommand2 = new PointSelectionCommand(viewport, (sender, args) => { HandleSelectionVisualsEvent(sender, args); });
    }

    public RectangleSelectionCommand RectangleSelectionCommand { get; private set; }
    public PointSelectionCommand PointSelectionCommand { get; private set; }
    public PointSelectionCommand PointSelectionCommand2 { get; private set; }

    public SelectionHitMode SelectionMode
    {
      get
      {
        return RectangleSelectionCommand.SelectionHitMode;
      }

      set
      {
        RectangleSelectionCommand.SelectionHitMode = value;
      }
    }

    public IEnumerable<SelectionHitMode> SelectionModes
    {
      get
      {
        return Enum.GetValues(typeof(SelectionHitMode)).Cast<SelectionHitMode>();
      }
    }

    public string SelectedVisuals
    {
      get
      {
        return selectedVisuals == null ? "" : string.Join("; ", selectedVisuals.Select(x => x.GetType().Name));
      }
    }

    private void HandleSelectionVisualsEvent(object sender, VisualsSelectedEventArgs args)
    {
      ChangeMaterial(args.SelectedVisuals, Materials.Blue);

      if (args is VisualsSelectedByRectangleEventArgs rectangleSelectionArgs)
      {
        ChangeMaterial(
            args.SelectedVisuals,
            rectangleSelectionArgs.Rectangle.Size != default ? Materials.Red : Materials.Green);
      }
      else
      {
        ChangeMaterial(args.SelectedVisuals, Materials.Orange);
      }
      RaisePropertyChanged(nameof(SelectedVisuals));
    }

    private void HandleSelectionModelsEvent(object sender, ModelsSelectedEventArgs args)
    {

    }

    private void ChangeMaterial(IEnumerable<Visual3D> models, Material material)
    {
      if (models == null)
      {
        return;
      }

      foreach (MeshElement3D model in models)
      {
        model.Material = model.BackMaterial = material;
      }
    }
  }
}
