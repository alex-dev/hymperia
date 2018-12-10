namespace Prism.Titles
{
  public interface ITitle
  {
    event TitleChangedEventHandler TitleChanged;

    string Title { get; }
  }
}
