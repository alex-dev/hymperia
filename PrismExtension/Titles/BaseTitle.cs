using System;

namespace Prism.Titles
{
  public abstract class BaseTitle : ITitle
  {
    public event TitleChangedEventHandler TitleChanged;

    public string Title
    {
      get => title;
      protected set
      {
        if (title.Equals(value, StringComparison.CurrentCultureIgnoreCase))
          return;

        title = value;
        RaiseTitleChanged();
      }
    }

    protected void RaiseTitleChanged() => TitleChanged?.Invoke(this, new TitleChangedEventArgs(Title));

    public sealed override string ToString() => Title;

    private string title;
  }
}
