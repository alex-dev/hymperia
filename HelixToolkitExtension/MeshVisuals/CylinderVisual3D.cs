using System.Windows;
using System.Windows.Media.Media3D;

namespace HelixToolkit.Wpf
{
  /// <summary>A visual element that shows a cylinder.</summary>
  public class CylinderVisual3D : MeshElement3D
  {
    #region Dependency Properties

    /// <seealso cref="Diameter"/>
    public static readonly DependencyProperty DiameterProperty =
      DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(CylinderVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

    /// <seealso cref="InnerDiameter"/>
    public static readonly DependencyProperty InnerDiameterProperty =
      DependencyProperty.Register(nameof(InnerDiameter), typeof(double), typeof(CylinderVisual3D), new UIPropertyMetadata(0.0, GeometryChanged));

    /// <seealso cref="Diameter"/>
    public static readonly DependencyProperty HeightProperty =
      DependencyProperty.Register(nameof(Height), typeof(double), typeof(CylinderVisual3D), new UIPropertyMetadata(1.0, GeometryChanged));

    /// <seealso cref="Origin"/>
    public static readonly DependencyProperty OriginProperty =
      DependencyProperty.Register(nameof(Origin), typeof(Point3D), typeof(CylinderVisual3D), new UIPropertyMetadata(new Point3D(0, 0, 0), GeometryChanged));

    /// <seealso cref="ThetaDiv"/>
    public static readonly DependencyProperty ThetaDivProperty =
      DependencyProperty.Register(nameof(ThetaDiv), typeof(int), typeof(PipeVisual3D), new UIPropertyMetadata(36, GeometryChanged));

    #endregion

    #region Properties

    /// <summary>Cylinder outer diameter.</summary>
    public double Diameter
    {
      get => (double)GetValue(DiameterProperty);
      set => SetValue(DiameterProperty, value);
    }

    /// <summary>Cylinder inner diameter.</summary>
    public double InnerDiameter
    {
      get => (double)GetValue(InnerDiameterProperty);
      set => SetValue(InnerDiameterProperty, value);
    }

    /// <summary>Cylinder height.</summary>
    public double Height
    {
      get => (double)GetValue(HeightProperty);
      set => SetValue(HeightProperty, value);
    }

    /// <summary>Cylinder origin.</summary>
    public Point3D Origin
    {
      get => (Point3D)GetValue(OriginProperty);
      set => SetValue(OriginProperty, value);
    }


    /// <summary>Cylinder theta div.</summary>
    public int ThetaDiv
    {
      get => (int)GetValue(ThetaDivProperty);
      set => SetValue(ThetaDivProperty, value);
    }

    #endregion

    /// <summary>
    /// Do the tessellation and return the <see cref="MeshGeometry3D" />.
    /// </summary>
    /// <returns>
    /// A triangular mesh geometry.
    /// </returns>
    protected override MeshGeometry3D Tessellate()
    {
      var builder = new MeshBuilder(false, true);
      builder.AddCylinder(Origin, Height, InnerDiameter, Diameter, ThetaDiv);
      return builder.ToMesh();
    }
  }
}
