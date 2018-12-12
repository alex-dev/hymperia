using Hymperia.Facade.Titles;
using Prism.Mvvm;
using Prism.Titles;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    public string Title
    {
      get => title;
      set => SetProperty(ref title, value);
    }

    public FenetrePrincipaleViewModel(ITitleAggregator titles)
    {
      var title_ = titles.GetTitle<MainWindowTitle>();
      title_.TitleChanged += OnTitleChanged;
      title = title_.Title;
    }

    private void OnTitleChanged(ITitle sender, TitleChangedEventArgs e) => Title = e.Title;

    private string title;
  }
}
