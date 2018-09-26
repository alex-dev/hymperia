namespace Hymperia.Model.Modeles.JsonObject
{
  public class Quaternion
  {
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double W { get; set; }

    public Quaternion(double x, double y, double z, double w)
    {
      X = x;
      Y = y;
      Z = z;
      W = w;
    }
  }
}
