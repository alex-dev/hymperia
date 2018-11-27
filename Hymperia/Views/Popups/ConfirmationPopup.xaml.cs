using System;
using System.Windows;
using System.Windows.Controls;
using Prism.Interactivity.InteractionRequest;

namespace Hymperia.Facade.Views.Popups
{
  public partial class ConfirmationPopup : UserControl, IInteractionRequestAware
  {
    public Action FinishInteraction { get; set; }
    public INotification Notification { get; set; }

    public ConfirmationPopup()
    {
      InitializeComponent();
    }

    private void Ok(object sender, RoutedEventArgs e)
    {
      if (Notification is IConfirmation confirmation)
        confirmation.Confirmed = true;

      FinishInteraction?.Invoke();
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      if (Notification is IConfirmation confirmation)
        confirmation.Confirmed = false;

      FinishInteraction?.Invoke();
    }
  }
}
