using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Views.Dialog;
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

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// Interaktionslogik für ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : AppPage
    {
        private User user;
        private UserModel model;

        public ProfilePage()
        {
        }

        public override void LoadView()
        {
            InitializeComponent();
            LoadedView = true;
        }

        public override void LoadResources()
        {
            model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);
            OnLoad();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            OnLoad();
        }

        private void OnLoad()
        {
            user = model.GetUser(Session.Instance.CurrentUserId);
            lbEmail.Content = user.Email;
            lbFirstname.Content = user.Firstname;
            lbLastname.Content = user.Lastname;
            lbAdmin.Content = user.Admin ? "Ja" : "Nein";
        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            PasswordChangeDialog dialog = new PasswordChangeDialog();
            if (dialog.ShowDialog() == true)
            {
                Console.WriteLine("Password changed");
                MessageBox.Show("Das Passwort wurde geändert.");
            }
            else
            {
                Console.WriteLine("Password not changed");
            }
        }
    }
}