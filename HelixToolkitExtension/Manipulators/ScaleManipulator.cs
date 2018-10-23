using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using JetBrains.Annotations;

namespace HelixToolkit.Wpf
{
  /// <summary>Represents a visual element that contains a manipulator that can scale along an axis.</summary>
  public class ScaleManipulator : Manipulator
  {
    #region Dependency Properties

    /// <summary>Identifies the <see cref="Diameter"/> dependency property.</summary>
    public static readonly DependencyProperty DiameterProperty =
      DependencyProperty.Register(
        nameof(Diameter),
        typeof(double),
        typeof(ScaleManipulator),
        new UIPropertyMetadata(0.2, UpdateGeometry));

    /// <summary>Identifies the <see cref="Direction"/> dependency property.</summary>
    public static readonly DependencyProperty DirectionProperty =
      DependencyProperty.Register(
        nameof(Direction),
        typeof(Vector3D),
        typeof(ScaleManipulator),
        new UIPropertyMetadata(UpdateGeometry));

    /// <summary>Identifies the <see cref="Length"/> dependency property.</summary>
    public static readonly DependencyProperty LengthProperty =
      DependencyProperty.Register(
        nameof(Length),
        typeof(double),
        typeof(ScaleManipulator),
        new UIPropertyMetadata(2.0, UpdateGeometry));

    public static readonly DependencyProperty BindLengthToValueProperty =
      DependencyProperty.Register(
        nameof(BindLengthToValue),
        typeof(bool),
        typeof(ScaleManipulator),
        new UIPropertyMetadata(true));

    #endregion

    #region Properties

    /// <summary>Diameter of the manipulator arrow.</summary>
    public double Diameter
    {
      get => (double)GetValue(DiameterProperty);
      set => SetValue(DiameterProperty, value);
    }

    /// <summary>Direction of the scaling.</summary>
    public Vector3D Direction
    {
      get => (Vector3D)GetValue(DirectionProperty);
      set => SetValue(DirectionProperty, value);
    }

    /// <summary>Length of the manipulator arrow.</summary>
    public double Length
    {
      get => (double)GetValue(LengthProperty);
      set => SetValue(LengthProperty, value);
    }

    public bool BindLengthToValue
    {
      get => (bool)GetValue(BindLengthToValueProperty);
      set => SetValue(BindLengthToValueProperty, value);
    }

    #endregion

    /// <summary>Updates the geometry.</summary>
    protected override void UpdateGeometry()
    {
      var mesh = new MeshBuilder(false, false);
      var center = new Point3D(0, 0, 0);

      var direction = Direction;
      direction.Normalize();

      var point = center + (direction * Length);
      mesh.AddArrow(center, point, Diameter);

      Model.Geometry = mesh.ToMesh();
    }

    /// <summary>Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
    /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
    protected override void OnMouseDown([NotNull] MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      var direction = ToWorld(Direction);
      var up = Vector3D.CrossProduct(Camera.LookDirection, direction);

      var hitPlaneOrigin = ToWorld(Position);
      HitPlaneNormal = Vector3D.CrossProduct(up, direction);

      var nearest = GetNearestPoint(e.GetPosition(ParentViewport), hitPlaneOrigin, HitPlaneNormal);

      if (nearest is Point3D point)
      {
        lastpoint = ToLocal(point);
      }
    }

    /// <summary>Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
    /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseMove([NotNull] MouseEventArgs e)
    {
      base.OnMouseMove(e);

      if (IsMouseCaptured)
      {
        var hitPlaneOrigin = ToWorld(Position);
        var nearest = GetNearestPoint(e.GetPosition(ParentViewport), hitPlaneOrigin, HitPlaneNormal);

        if (!(nearest is Point3D point))
        {
          return;
        }

        var delta = Vector3D.DotProduct(ToLocal(point) - lastpoint, Direction);
        Value += delta;

        if (BindLengthToValue)
        {
          Length += delta;
        }

        lastpoint = ToLocal(point);
      }
    }

    /// <summary>Gets the nearest point on the scaling axis.</summary>
    /// <param name="position">The position (in screen coordinates).</param>
    /// <param name="hitPlaneOrigin">The hit plane origin (world coordinate system).</param>
    /// <param name="hitPlaneNormal">The hit plane normal (world coordinate system).</param>
    /// <returns>The nearest point (world coordinates) or null if no point could be found.</returns>
    private Point3D? GetNearestPoint(Point position, Point3D hitPlaneOrigin, Vector3D hitPlaneNormal)
    {
      var hpp = GetHitPlanePoint(position, hitPlaneOrigin, hitPlaneNormal);
      var ray = new Ray3D(ToWorld(Position), ToWorld(Direction));

      return hpp is Point3D point ? (Point3D?)ray.GetNearest(point) : null;
    }

    #region Private Fields

    /// <summary>The last point.</summary>
    private Point3D lastpoint;

    #endregion
  }
}
