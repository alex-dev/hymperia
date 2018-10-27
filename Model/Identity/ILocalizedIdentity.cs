namespace Hymperia.Model.Identity
{
  public interface ILocalizedIdentity<T> where T : IIdentity
  {
    string StringKey { get; }
    string CultureKey { get; }
  }
}
