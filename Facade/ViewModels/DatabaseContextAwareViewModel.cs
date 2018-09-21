using Hymperia.Model;

namespace Hymperia.Facade.ViewModels
{
  public abstract class DatabaseContextAwareViewModel : DisposableViewModel
  {
    protected DatabaseContext Context { get; private set; }

    public DatabaseContextAwareViewModel(DatabaseContext context)
    {
      Context = context;
    }

    protected override void DisposeResource()
    {
      Context.Dispose();
    }
  }
}
