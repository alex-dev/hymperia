using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Hymperia.HelixViewport3DTest.Manipulators
{
  public class MovementManipulator : ModelVisual3D
  {
    #region Dependancy

    public static readonly DependencyProperty DiameterProperty;

    /// <summary>
    /// The target transform property.
    /// </summary>
    public static readonly DependencyProperty TargetTransformProperty;

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

    /// <summary>
    /// The rotate x manipulator.
    /// </summary>
    private readonly RotateManipulator RotateXManipulator;

    /// <summary>
    /// The rotate y manipulator.
    /// </summary>
    private readonly RotateManipulator RotateYManipulator;

    /// <summary>
    /// The rotate z manipulator.
    /// </summary>
    private readonly RotateManipulator RotateZManipulator;

    /// <summary>
    /// The translate x manipulator.
    /// </summary>
    private readonly TranslateManipulator TranslateXManipulator;

    /// <summary>
    /// The translate y manipulator.
    /// </summary>
    private readonly TranslateManipulator TranslateYManipulator;

    /// <summary>
    /// The translate z manipulator.
    /// </summary>
    private readonly TranslateManipulator TranslateZManipulator;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///   Initializes a new instance of the <see cref = "CombinedManipulator" /> class.
    /// </summary>
    public MovementManipulator()
    {
      TranslateXManipulator = new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red };
      this.TranslateYManipulator = new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green };
      this.TranslateZManipulator = new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue };
      this.RotateXManipulator = new RotateManipulator { Axis = new Vector3D(1, 0, 0), Color = Colors.Red };
      this.RotateYManipulator = new RotateManipulator { Axis = new Vector3D(0, 1, 0), Color = Colors.Green };
      this.RotateZManipulator = new RotateManipulator { Axis = new Vector3D(0, 0, 1), Color = Colors.Blue };

      Children.Add(TranslateXManipulator);


      BindingOperations.SetBinding(
          this,
          TransformProperty,
          new Binding("TargetTransform") { Source = this });

      BindingOperations.SetBinding(
          this.TranslateXManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
      BindingOperations.SetBinding(
          this.TranslateYManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
      BindingOperations.SetBinding(
          this.TranslateZManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
      BindingOperations.SetBinding(
          this.RotateXManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
      BindingOperations.SetBinding(
          this.RotateYManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
      BindingOperations.SetBinding(
          this.RotateZManipulator,
          Manipulator.TargetTransformProperty,
          new Binding("TargetTransform") { Source = this });
    }

    #endregion

    #region Public Properties
    /// <summary>
    ///   Gets or sets the diameter.
    /// </summary>
    /// <value>The diameter.</value>
    public double Diameter
    {
      get
      {
        return (double)this.GetValue(DiameterProperty);
      }

      set
      {
        this.SetValue(DiameterProperty, value);
      }
    }

    /// <summary>
    ///   Gets or sets the target transform.
    /// </summary>
    /// <value>The target transform.</value>
    public Transform3D TargetTransform
    {
      get
      {
        return (Transform3D)this.GetValue(TargetTransformProperty);
      }

      set
      {
        this.SetValue(TargetTransformProperty, value);
      }
    }

    /// <summary>
    ///   Gets or sets the offset of the visual (this vector is added to the Position point).
    /// </summary>
    /// <value>The offset.</value>
    public Vector3D Offset
    {
      get { return TranslateXManipulator.Offset; }
      set
      {
        TranslateXManipulator.Offset = value;
        TranslateYManipulator.Offset = value;
        TranslateZManipulator.Offset = value;
        RotateXManipulator.Offset = value;
        RotateYManipulator.Offset = value;
        RotateZManipulator.Offset = value;
      }
    }

    /// <summary>
    ///   Gets or sets the position of the manipulator.
    /// </summary>
    /// <value>The position.</value>
    public Point3D Position
    {
      get { return TranslateXManipulator.Position; }
      set
      {
        TranslateXManipulator.Position = value;
        TranslateYManipulator.Position = value;
        TranslateZManipulator.Position = value;
        RotateXManipulator.Position = value;
        RotateYManipulator.Position = value;
        RotateZManipulator.Position = value;
      }
    }

    /// <summary>
    ///   Gets or sets the pivot point of the manipulator.
    /// </summary>
    /// <value>The position.</value>
    public Point3D Pivot
    {
      get { return TranslateXManipulator.Position; }
      set
      {
        TranslateXManipulator.Position = value;
        TranslateYManipulator.Position = value;
        TranslateZManipulator.Position = value;
        RotateXManipulator.Pivot = value;
        RotateYManipulator.Pivot = value;
        RotateZManipulator.Pivot = value;
      }
    }
    #endregion

    #region Methods
    protected virtual void OnDiameterChanged()
    {
      TranslateXManipulator.Length = 
        TranslateYManipulator.Length =
        TranslateZManipulator.Length = Diameter * 0.75;
      TranslateXManipulator.Diameter =
        TranslateYManipulator.Diameter =
        TranslateZManipulator.Diameter = Diameter * 0.2;

      RotateXManipulator.Diameter =
        RotateYManipulator.Diameter =
        RotateZManipulator.Diameter = Diameter * 0.6;
      RotateXManipulator.InnerDiameter =
        RotateYManipulator.InnerDiameter =
        RotateZManipulator.InnerDiameter = Diameter * 0.55;
      RotateXManipulator.Length =
        RotateYManipulator.Length =
        RotateZManipulator.Length = Diameter * 0.1;

    }
    #endregion

    #region Public Bindig Methods

    /// <summary>
    /// Binds this manipulator to a given Visual3D.
    /// </summary>
    /// <param name="source">Source Visual3D which receives the manipulator transforms.</param>
    public virtual void Bind(ModelVisual3D source)
    {
      
      BindingOperations.SetBinding(this, TargetTransformProperty, new Binding("Transform") { Source = source });
      BindingOperations.SetBinding(this, TransformProperty, new Binding("Transform") { Source = source });
    }

    /// <summary>
    /// Releases the binding of this manipulator.
    /// </summary>
    public virtual void UnBind()
    {
      BindingOperations.ClearBinding(this, TargetTransformProperty);
      BindingOperations.ClearBinding(this, TransformProperty);
    }

    #endregion
  }
}
