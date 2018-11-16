/**
 * Auteur : Antoine Mailhot
 * Date de création : 9 novembre 2018
 */

using System.Windows;
using System.Windows.Controls;
using Hymperia.Facade.ViewModelInterface;

namespace Hymperia.Facade.Views
{
  public partial class Inscription : UserControl
  {
    public Inscription()
    {
      InitializeComponent();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e) =>
      ((IPasswordHolder)DataContext).PasswordIsValid =
        txtPassword.Password == txtVerification.Password;
  }
}
