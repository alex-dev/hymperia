using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private string title;

    #endregion

    public string Title
    {
      get => title;
      private set => SetProperty(ref title, value);
    }

    #endregion

    public FenetrePrincipaleViewModel()
    {
      Title = "Hymperia";
    }
  }
}
