using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.Extensions;
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
    protected override IEnumerable<Manipulator> GenerateManipulators()
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

        return (from manipulator in Generate()
                select manipulator).DeferredForEach(BindTo);
    }

    #region Manipulator Size Bindings

    protected void BindTo([NotNull] Manipulator manipulator)
    {
      switch (manipulator)
      {
        case TranslateManipulator translate:
          BindToManipulator(translate); break;
        case RotateManipulator rotate:
          BindToManipulator(rotate); break;
      }
    }

    #endregion

    #endregion

    #region Binding from Source

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
