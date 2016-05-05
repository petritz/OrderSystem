using OrderSystem.Data;
using OrderSystem.Events;
using OrderSystem.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OrderSystem.Enums;

namespace OrderSystem.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : MainPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            swActivity.Visibility = Visibility.Visible;
            swActivity.IsSpinning = true;

            string email = tbUsername.Text;
            string password = tbPassword.Password;

            //TODO: make async
            UserModel model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);
            if(model.Login(email, password))
            {
                lbMessage.Content = "Erfolgreich eingeloggt.";
                Session.CreateSession(model.GetUserId(email), email).Save(HashHelper.CreateMD5(password));
                base.OnEvent(new LoginEventArgs(true));
            }
            else
            {
                lbMessage.Content = "Es wurden falsche Logindaten eingegeben.";
                tbPassword.Focus();
                base.OnEvent(new LoginEventArgs(false));
            }

            swActivity.Visibility = Visibility.Hidden;
            swActivity.IsSpinning = false;
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            base.OnEvent(new RedirectEventArgs("register"));
        }
    }
}
