using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Hymperia.Facade.DependencyObjects
{
  public class ResizeManipulator : ModelVisual3D
  {
    #region Depedancy Properties

    public static readonly DependencyProperty WidthValueProperty;
    public static readonly DependencyProperty HeightValueProperty;
    public static readonly DependencyProperty LengthValueProperty;

    #endregion

    #region Properties

    public double Width
    {
      get => (double)GetValue(WidthValueProperty);

      set => SetValue(WidthValueProperty, value);
    }

    public double Height
    {
      get => (double)GetValue(HeightValueProperty);

      set => SetValue(HeightValueProperty, value);
    }

    public double Length
    {
      get => (double)GetValue(LengthValueProperty);

      set => SetValue(LengthValueProperty, value);
    }

    public Vector3D Offset
    {
      get => Children.OfType<Manipulator>().First().Offset;
      set
      {
        foreach (var manipulator in Children.OfType<Manipulator>())
        {
          manipulator.Offset = value;
        }
      }
    }

    public Point3D Position
    {
      get => Children.OfType<Manipulator>().First().Position;
      set
      {
        foreach (var manipulator in Children.OfType<Manipulator>())
        {
          manipulator.Position = value;
        }
      }
    }

    #endregion

    #region Constructors

    public ResizeManipulator()
    {
      var binding = new Binding("TargetTransform") { Source = this };

      Children.Add(new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(-1, 0, 0), Color = Colors.Red });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, -1, 0), Color = Colors.Green });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, 0, -1), Color = Colors.Blue });

      foreach (var manipulator in Children.OfType<Manipulator>())
      {
        BindingOperations.SetBinding(manipulator, Manipulator.ValueProperty, binding);
      }
    }

    static ResizeManipulator()
    {
      {
        var metadata = new UIPropertyMetadata(2.0, (sender, args) => ((ResizeManipulator)sender).OnDiameterChanged());
        WidthValueProperty = DependencyProperty.Register("Width", typeof(double), typeof(ResizeManipulator), metadata);
      }
      {
        var metadata = new UIPropertyMetadata(2.0, (sender, args) => ((ResizeManipulator)sender).OnDiameterChanged());
        HeightValueProperty = DependencyProperty.Register("Height", typeof(double), typeof(ResizeManipulator), metadata);
      }
      {
        var metadata = new UIPropertyMetadata(2.0, (sender, args) => ((ResizeManipulator)sender).OnDiameterChanged());
        LengthValueProperty = DependencyProperty.Register("Length", typeof(double), typeof(ResizeManipulator), metadata);
      }

    }

    #endregion

    #region Binding Methods

    public virtual void Bind(BoxVisual3D source)
    {
      var converter = new LinearConverter { M = 0.5 };

      var bindingWidth = new Binding("Value") { Source = source.Width, Converter = converter };
      var bindingHeight = new Binding("Value") { Source = source.Height, Converter = converter };
      var bindingLength = new Binding("Value") { Source = source.Length, Converter = converter };

      BindingOperations.SetBinding(this, WidthValueProperty, bindingWidth);
      BindingOperations.SetBinding(this, HeightValueProperty, bindingHeight);
      BindingOperations.SetBinding(this, LengthValueProperty, bindingLength);
      //BindingOperations.SetBinding(this, TransformProperty, binding);
    }

    public virtual void Unbind()
    {
      BindingOperations.ClearBinding(this, WidthValueProperty);
      BindingOperations.ClearBinding(this, HeightValueProperty);
      BindingOperations.ClearBinding(this, LengthValueProperty);
      //BindingOperations.ClearBinding(this, TransformProperty);
    }

    #endregion

    #region Event Handlers

    protected virtual void OnDiameterChanged()
    {
      var length = Height * 1.25;
      var width = Height * 0.1;

      foreach (var translateManipulator in Children.OfType<TranslateManipulator>())
      {
        translateManipulator.Length = length;
        translateManipulator.Diameter = width;
      }

    }

    #endregion

  }
}
