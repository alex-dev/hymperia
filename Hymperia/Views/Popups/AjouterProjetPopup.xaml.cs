using System;
using System.Windows;
using System.Windows.Controls;
using Prism.Interactivity.InteractionRequest;

namespace Hymperia.Facade.Views.Popups
{
  /// <summary>
  /// Logique d'interaction pour AjouterProjet.xaml
  /// </summary>
  public partial class AjouterProjetPopup : UserControl, IInteractionRequestAware
  {
    public Action FinishInteraction { get; set; }
    public INotification Notification { get; set; }

    public AjouterProjetPopup()
    {
      InitializeComponent();
    }

    private void Confirm(object sender, RoutedEventArgs e)
    {
      if (Notification is IConfirmation confirmation)
      {
        confirmation.Confirmed = !string.IsNullOrWhiteSpace(confirmation.Content?.ToString());
      }

      FinishInteraction?.Invoke();
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      if (Notification is IConfirmation confirmation)
      {
        confirmation.Confirmed = false;
      }

      FinishInteraction?.Invoke();
    }
  }
}
