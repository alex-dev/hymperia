using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Hymperia.Facade.DependencyObjects
{
  public class MovementManipulator : ModelVisual3D
  {
    #region Dependancy Properties

    public static readonly DependencyProperty DiameterProperty;
    public static readonly DependencyProperty TargetTransformProperty;

    #endregion

    #region Properties

    public double Diameter
    {
      get => (double)GetValue(DiameterProperty);

      set => SetValue(DiameterProperty, value);

    }

    public Transform3D TargetTransform
    {
      get => (Transform3D)GetValue(TargetTransformProperty);
      set => SetValue(TargetTransformProperty, value);
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

    public Point3D Pivot
    {
      get => Children.OfType<RotateManipulator>().First().Pivot;
      set
      {
        foreach (var manipulator in Children.OfType<TranslateManipulator>())
        {
          manipulator.Position = value;
        }

        foreach (var manipulator in Children.OfType<RotateManipulator>())
        {
          manipulator.Pivot = value;
        }
      }
    }

    #endregion

    #region Constructors

    static MovementManipulator()
    {
      {
        var metadata = new UIPropertyMetadata(2.0, (sender, args) => ((MovementManipulator)sender).OnDiameterChanged());
        DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(MovementManipulator), metadata);
      }
      {
        var metadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
        TargetTransformProperty = DependencyProperty.Register("TargetTransform", typeof(Transform3D), typeof(MovementManipulator), metadata);
      }
    }

    public MovementManipulator(MeshElement3D source)
    {
      var binding = new Binding("TargetTransform") { Source = this };

      Children.Add(new RotateManipulator { Axis = new Vector3D(1, 0, 0), Color = Colors.Red });
      Children.Add(new RotateManipulator { Axis = new Vector3D(0, 1, 0), Color = Colors.Green });
      Children.Add(new RotateManipulator { Axis = new Vector3D(0, 0, 1), Color = Colors.Blue });

      Children.Add(new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green });
      Children.Add(new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue });

      foreach (var manipulator in Children.OfType<Manipulator>())
      {
        BindingOperations.SetBinding(manipulator, Manipulator.TargetTransformProperty, binding);
      }

      if (source is BoxVisual3D)
      {
        double[] sides = { (source as BoxVisual3D).Length, (source as BoxVisual3D).Height, (source as BoxVisual3D).Width };
        var largest = GetLargestValue(sides);
        Diameter = largest * 1.1;
      }
      if (source is EllipsoidVisual3D)
      {
        double[] sides = { (source as EllipsoidVisual3D).RadiusX, (source as EllipsoidVisual3D).RadiusY, (source as EllipsoidVisual3D).RadiusZ };
        var largest = GetLargestValue(sides);
        Diameter = largest * 1.5;
      }
      if (source is TruncatedConeVisual3D)
      {
        double[] sides = { (source as TruncatedConeVisual3D).Height, (source as TruncatedConeVisual3D).BaseRadius };
        var largest = GetLargestValue(sides);
        Diameter = largest * 1.5;
      }
      if (source is PipeVisual3D)
      {
        double[] sides = { (source as PipeVisual3D).Diameter };
        var largest = GetLargestValue(sides);
        Diameter = largest * 1.1;
      }
    }

    #endregion

    #region Binding Methods

    public virtual void Bind(ModelVisual3D source)
    {
      var binding = new Binding("Transform") { Source = source };
      BindingOperations.SetBinding(this, TargetTransformProperty, binding);
      BindingOperations.SetBinding(this, TransformProperty, binding);
    }

    public virtual void Unbind()
    {
      BindingOperations.ClearBinding(this, TargetTransformProperty);
      BindingOperations.ClearBinding(this, TransformProperty);
    }

    #endregion

    #region Events Handlers

    protected virtual void OnDiameterChanged()
    {
      var length = Diameter * 1.25;
      var width = Diameter * 0.12;
      var diameter = Diameter * 1.65;
      var innerDiameter = Diameter * 1.5;
      var rotateLength = Diameter * 0.1;

      foreach (var translateManipulator in Children.OfType<TranslateManipulator>())
      {
        translateManipulator.Length = length;
        translateManipulator.Diameter = width;
      }

      foreach (var rotateManipulator in Children.OfType<RotateManipulator>())
      {
        rotateManipulator.Diameter = diameter;
        rotateManipulator.InnerDiameter = innerDiameter;
        rotateManipulator.Length = rotateLength;
      }
    }

    #endregion

    #region Methods

    private double GetLargestValue(double[] source)
    {
      double largest = 0;
      foreach (var value in source)
      {
        if (value > largest)
        {
          largest = value;
        }
      }
      return largest;
    }

    #endregion
  }
}
