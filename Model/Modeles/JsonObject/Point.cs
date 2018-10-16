namespace Hymperia.Model.Modeles.JsonObject
{
  public class Point
  {
    public static Point Center => new Point(0, 0, 0);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }
  }
}
