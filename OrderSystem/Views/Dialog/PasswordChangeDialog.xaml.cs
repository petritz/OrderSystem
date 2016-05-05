using OrderSystem.Data;
using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OrderSystem.Enums;

namespace OrderSystem.Views.Dialog
{
    /// <summary>
    /// Interaktionslogik für PasswordChangeDialog.xaml
    /// </summary>
    public partial class PasswordChangeDialog : Window
    {
        public PasswordChangeDialog()
        {
            InitializeComponent();
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = tbPassword.Password;
                string passwordRepeat = tbPasswordRepeat.Password;

                if (!password.Equals(passwordRepeat))
                {
                    throw new Exception("Du musst das gleiche Passwort eingeben.");
                }

                UserModel model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);

                if (!model.PasswordCheck(password))
                {
                    throw new Exception(
                        "Passwörter müssen mehr wie 8 Zeichen haben, mindestens einen Großbuchstaben, einen Kleinbuchstaben und eine Zahl. Es sind keine Sonderzeichen zugelassen.");
                }

                if (!model.ChangePassword(Session.Instance.CurrentUserId, password))
                {
                    throw new Exception("Passwort konnte nicht geändert werden.");
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}