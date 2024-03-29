﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;

namespace HelixViewport3DTest
{
  public partial class PlaceRandomShapes : UserControl
  {
    private Random Random { get; set; }

    private IDictionary<string, Func<MeshElement3D>> Generator { get; set; }

    public IEnumerable<string> Keys { get => Generator.Keys; }

    public PlaceRandomShapes()
    {
      Random = new Random();
      Generator = new Dictionary<string, Func<MeshElement3D>>
      {
        { "Cube", CreateCube },
        { "Elipsoid", CreateEllipsoid },
        { "Flèche", CreateArrow },
        { "Prisme rectangulaire", CreateBox },
        { "Sphère", CreateSphere },
        { "Cylindre", CreatePipe },
        { "Cone", CreateTruncatedCone }
      };

      InitializeComponent();
      lstboxKeys.SelectAll();
    }

    private void Viewport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var generator = (from string key in lstboxKeys.SelectedItems
                       select Generator[key]).ToArray();
      viewport.Children.Add(generator[Random.Next() % generator.Length]());
    }

    /// <remarks>
    ///   <list type="bullet">
    ///     <item><see cref="ArrowVisual3D.Diameter"/>: Le diamètre du cylindre de la flèche.</item>
    ///     <item><see cref="ArrowVisual3D.Direction"/>: La direction de la flèche permettant de trouver <see cref="ArrowVisual3D.Point2"/>.</item>
    ///     <item>
    ///       <see cref="ArrowVisual3D.HeadLength"/>: La longueur de la tête de la flèche relativement à <see cref="ArrowVisual3D.Diameter"/>.
    ///       Une valeur supérieure à 1 crée une tête plus longue que le cylindre.
    ///     </item>
    ///     <item><see cref="ArrowVisual3D.Origin"/>: L'origine de la flèche.</item>
    ///     <item><see cref="ArrowVisual3D.Point1"/>: Le début de la flèche. <seealso cref="ArrowVisual3D.Origin"/></item>
    ///     <item><see cref="ArrowVisual3D.Point2"/>: La fin de la flèche.</item>
    ///     <item><see cref="ArrowVisual3D.ThetaDiv"/>: La moitié du nombre de segment utilisés pour former le cylindre de la flèche.</item>
    ///   </list>
    /// </remarks>
    private MeshElement3D CreateArrow()
    {
      return new ArrowVisual3D
      {
        Diameter = Random.Next(1, 3),
        HeadLength = Random.NextDouble(),
        Origin = viewport.CursorPosition ?? default,
        Direction = new Vector3D(Random.Next(0, 5), Random.Next(0, 5), Random.Next(0, 5))
      };
    }

    private MeshElement3D CreateBox()
    {
      return new BoxVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        Height = Random.Next(1, 5),
        Length = Random.Next(1, 5),
        Width = Random.Next(1, 5)
      };
    }

    /// <remarks>
    ///   <seealso cref="BoxVisual3D"/> pour une forme pouvant représenter un cube lorsque <see cref="BoxVisual3D.Height"/>,
    ///   <see cref="BoxVisual3D.Length"/> et <see cref="BoxVisual3D.Width"/> sont égaux.
    /// </remarks>
    private MeshElement3D CreateCube()
    {
      return new CubeVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        SideLength = Random.Next(1, 5)
      };
    }

    /// <remarks>
    ///   <list type="bullet">
    ///     <item><see cref="SphereVisual3D.PhiDiv"/>: Le nombre de segments équatoriaux utilisés pour former l'équateur de la sphère.</item>
    ///     <item><see cref="SphereVisual3D.ThetaDiv"/>: La moitié du nombre de segments polaires utilisés pour former l'axe polaire de la sphère.</item>
    ///   </list>
    /// </remarks>
    private MeshElement3D CreateEllipsoid()
    {
      return new EllipsoidVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        RadiusX = Random.Next(1, 5),
        RadiusY = Random.Next(1, 5),
        RadiusZ = Random.Next(1, 5)
      };

    }

    /// <remarks>
    ///   <list type="bullet">
    ///     <item><see cref="SphereVisual3D.PhiDiv"/>: Le nombre de segments équatoriaux utilisés pour former l'équateur de la sphère.</item>
    ///     <item><see cref="SphereVisual3D.ThetaDiv"/>: La moitié du nombre de segments polaires utilisés pour former l'axe polaire de la sphère.</item>
    ///   </list>
    ///   <seealso cref="SpherePhiThetaExploration"/> pour une explication visuelle.
    ///   <seealso cref="EllipsoidVisual3D"/> pour une forme pouvant créer une sphère lorsque <see cref="EllipsoidVisual3D.RadiusX"/>,
    ///   <see cref="EllipsoidVisual3D.RadiusY"/> et <see cref="EllipsoidVisual3D.RadiusZ"/> sont égaux.
    /// </remarks>
    private MeshElement3D CreateSphere()
    {
      return new SphereVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        Radius = Random.Next(1, 5),
        ThetaDiv = ThetaDiv()
      };
    }

    /// <remarks>
    ///   <list type="bullet">
    ///     <item><see cref="PipeVisual3D.Diameter"/>: Diamètre extérieur du cylindre</item>
    ///     <item><see cref="PipeVisual3D.InnerDiameter"/>: Diamètre intérieur du cylindre</item>
    ///     <item><see cref="PipeVisual3D.Point1"/>: Base du cylindre</item>
    ///     <item><see cref="PipeVisual3D.Point2"/>: Dessus du cylindre</item>
    ///     <item><see cref="PipeVisual3D.ThetaDiv"/>: Le moitié du nombre de segments utilisés pour former le(s) côté(s) du cylindre.(Permet plusieurs sortes de bases comme triangulaire, pentagonale, etc)</item>
    ///   </list>
    /// </remarks>
    private MeshElement3D CreatePipe()
    {
      return new PipeVisual3D
      {
        Diameter = Random.Next(1, 3),
        InnerDiameter = Random.Next(0, 3),
        Point1 = viewport.CursorPosition ?? default,
        Point2 = new Point3D(viewport.CursorPosition.Value.X + Random.Next(-5, 5), viewport.CursorPosition.Value.Y + Random.Next(-5, 5), viewport.CursorPosition.Value.Z + Random.Next(-5, 5)),
        ThetaDiv = ThetaDiv()
      };
    }

    /// <remarks>
    ///   <list type="bullet">
    ///     <item><see cref="TruncatedConeVisual3D.Origin"/>: Point central à la base du cône</item>
    ///     <item>
    ///       <see cref="TruncatedConeVisual3D.ThetaDiv"/>: La moitié du nombre de segment utilisés pour former le(s) côté(s) du cône.(Permet techniquement de faire des pyramides ayant des bases différentes)
    ///       <seealso cref="PipeVisual3D.ThetaDiv"/>: Fonctionne selon le même principe  
    ///     </item>
    ///     <item><see cref="TruncatedConeVisual3D.Height"/>: Hauteur du cône</item>
    ///     <item><see cref="TruncatedConeVisual3D.BaseRadius"/>: Longueur d'un rayon du cône</item>
    ///     <item><see cref="TruncatedConeVisual3D.TopRadius"/>: Longueur d'un rayon au haut du cône pour un haut plat</item>
    ///   </list>
    /// </remarks>
    private MeshElement3D CreateTruncatedCone()
    {
      return new TruncatedConeVisual3D
      {
        Origin = viewport.CursorPosition ?? default,
        ThetaDiv = ThetaDiv(),
        Height = Random.Next(1, 5),
        BaseRadius = Random.Next(1, 5),
        /*TopRadius = Random.Next(0, 3)*/
      };
    }

    private int ThetaDiv()
    {
      return Random.Next(4, 50);
    }

    private int PhiDiv()
    {
      return Random.Next(3, 50);
    }
  }
}
