using OrderSystem.Data;
using OrderSystem.Events;
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
using OrderSystemLibrary.Enums;
using OrderSystemLibrary.Helper;
using OrderSystemLibrary.Models;

namespace OrderSystem.Views
{
    /// <summary>
    /// The login page
    /// </summary>
    public partial class Login : MainPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            string email = tbUsername.Text;
            string password = tbPassword.Password;

            //TODO: make async
            UserModel model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);
            if (model.Login(email, password))
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
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            base.OnEvent(new RedirectEventArgs("register"));
        }
    }
}