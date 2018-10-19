using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  /// <summary>Manipulateur de déplacement.</summary>
  public class MovementManipulator : CombinedManipulator
  {
    #region Constructors

    /// inheritdoc/>
    [NotNull]
    [ItemNotNull]
    protected override IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators()
    {
      IEnumerable<Manipulator> Generate()
      {
        yield return new RotateManipulator { Axis = new Vector3D(1, 0, 0), Color = Colors.Red };
        yield return new RotateManipulator { Axis = new Vector3D(0, 1, 0), Color = Colors.Green };
        yield return new RotateManipulator { Axis = new Vector3D(0, 0, 1), Color = Colors.Blue };
        yield return new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red };
        yield return new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green };
        yield return new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue };
      }

      return from manipulator in Generate()
             select Tuple.Create<Manipulator, Action<Manipulator>>(manipulator, BindToManipulator);
    }

    #region Manipulator Size Bindings

    private void BindToManipulator([NotNull] Manipulator manipulator)
    {
      switch (manipulator)
      {
        case TranslateManipulator translate:
          BindToTranslateManipulator(translate); break;
        case RotateManipulator rotate:
          BindToRotationManipulator(rotate); break;
      }
    }

    #endregion

    #endregion

    #region Binding to Source

    /// inheritdoc/>
    public override void Bind([NotNull] ModelVisual3D source)
    {
      base.Bind(source);
      SetBinding(TransformProperty, new Binding(nameof(source.Transform)) { Source = source, Mode = BindingMode.TwoWay });
    }

    /// inheritdoc/>
    public override void Unbind()
    {
      base.Unbind();
      ClearBinding(TransformProperty);
    }

    #endregion
  }
}
