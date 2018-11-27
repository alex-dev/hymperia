/*
 * Auteur : Antoine Mailhot
 * Date de création : 27 novembre 2018
 */
using System;
using System.Windows;
using System.Windows.Controls;
using Prism.Interactivity.InteractionRequest;

namespace Hymperia.Facade.Views.Popups
{
  public partial class InformationPopup : UserControl, IInteractionRequestAware
  {
    public Action FinishInteraction { get; set; }
    public INotification Notification { get; set; }

    public InformationPopup()
    {
      InitializeComponent();
    }

    private void Ok(object sender, RoutedEventArgs e) => FinishInteraction?.Invoke();
  }
}
