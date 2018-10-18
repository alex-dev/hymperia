using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  /// <summary>Classe de base de tous les groupes de <see cref="Manipulator"/> utilisés.</summary>
  public abstract class CombinedManipulator : ModelVisual3D
  {
    #region Properties

    /// <summary>Le <see cref="Transform3D"/> appliqué à la cible du manipulateur.</summary>
    [NotNull]
    public Transform3D TargetTransform
    {
      get => (Transform3D)GetValue(TransformProperty);
      set => SetValue(TransformProperty, value);
    }

    #endregion

    #region Constructors

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
      Justification = @"The call is known and needed to perform proper initialization.")]
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

    /// <summary>Génère les <see cref="Manipulator"/> et des <see cref="Action{Manipulator}"/> pour les lier au <see cref="CombinedManipulator"/>.</summary>
    /// <returns>
    ///   Un <see cref="IEnumerable{Tuple{Manipulator, Action{Manipulator}}}"/> de <see cref="Manipulator"/> et
    ///   d'<see cref="Action{Manipulator}"/> pour le lier.
    /// </returns>
    [NotNull]
    [ItemNotNull]
    protected abstract IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators();

    #endregion

    #region Binding to Source

    /// <summary>
    ///   Lie la <paramref name="source"/> au <see cref="CombinedManipulator"/> pour appliquer à <paramref name="source"/> les
    ///   transformations du <see cref="CombinedManipulator"/>.
    /// </summary>
    public abstract void Bind([NotNull] ModelVisual3D source);
    /// <summary>Délie le <see cref="ModelVisual3D"/> déjà lié au <see cref="CombinedManipulator"/>.</summary>
    public abstract void Unbind();

    #endregion
  }
}
