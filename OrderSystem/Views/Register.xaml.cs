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
    /// The Register page
    /// </summary>
    public partial class Register : MainPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            base.OnEvent(new RedirectEventArgs("login"));
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            try
            {
                swActivity.Visibility = Visibility.Visible;
                swActivity.IsSpinning = true;

                string firstname = tbFirstname.Text;
                string lastname = tbLastname.Text;
                string email = tbEmail.Text;
                string password = tbPassword.Password;
                string passwordRepeat = tbPasswordRepeat.Password;

                if (!password.Equals(passwordRepeat))
                {
                    throw new Exception("Du musst das gleiche Passwort eingeben.");
                }

                //TODO: make async
                UserModel model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);

                if (!model.PasswordCheck(password))
                {
                    throw new Exception(
                        "Passwörter müssen mehr als 8 Zeichen haben, mindestens einen Großbuchstaben, einen Kleinbuchstaben und eine Zahl. Es sind keine Sonderzeichen zugelassen.");
                }

                if (model.Register(firstname, lastname, email, password))
                {
                    lbMessage.Text = "Erfolgreich registriert.";
                    Session.CreateSession(model.GetUserId(email), email).Save(HashHelper.CreateMD5(password));
                    base.OnEvent(new RegisterEventArgs(true));
                }
                else
                {
                    lbMessage.Text = "Du konntest nicht registriert werden.";
                    tbPassword.Focus();
                    base.OnEvent(new RegisterEventArgs(false));
                }
            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }
            finally
            {
                swActivity.Visibility = Visibility.Hidden;
                swActivity.IsSpinning = false;
            }
        }
    }
}