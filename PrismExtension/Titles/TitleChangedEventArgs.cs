using System;

namespace Prism.Titles
{
  public class TitleChangedEventArgs : EventArgs
  {
    public string Title { get; }

    public TitleChangedEventArgs(string title)
    {
      Title = title;
    }
  }
}
