using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  public abstract class CombinedManipulator : ModelVisual3D
  {
    #region Attributes

    [CanBeNull]
    public Transform3D TargetTransform
    {
      get => (Transform3D)GetValue(TransformProperty);
      set => SetValue(TransformProperty, value);
    }

    public Vector3D Offset
    {
      get => Children.Cast<Manipulator>().First().Offset;
      set
      {
        foreach (Manipulator manipulator in Children)
        {
          manipulator.Offset = value;
        }
      }
    }

    public Point3D Position
    {
      get => ((Manipulator)Children.First()).Position;
      set
      {
        foreach (Manipulator manipulator in Children)
        {
          switch (manipulator)
          {
            case TranslateManipulator translate:
              translate.Position = value; break;
            case RotateManipulator rotate:
              rotate.Pivot = value; break;
          }
        }
      }
    }

    #endregion

    #region Constructors

    protected CombinedManipulator()
    {
      var binding = new Binding("Transform") { Source = this };

      foreach (var (manipulator, binder) in GenerateManipulators())
      {
        BindingOperations.SetBinding(manipulator, Manipulator.TargetTransformProperty, binding);
        binder(manipulator);
        Children.Add(manipulator);
      }
    }

    [NotNull]
    [ItemNotNull]
    protected abstract IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators();

    #endregion

    #region Binding to Source

    public abstract void Bind([NotNull] ModelVisual3D source);
    public abstract void Unbind();

    #endregion
  }
}
