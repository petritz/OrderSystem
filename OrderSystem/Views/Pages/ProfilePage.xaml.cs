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

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// Interaktionslogik für ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User user;
        private readonly UserModel model;

        public ProfilePage()
        {
            InitializeComponent();
            model = (UserModel)ModelRegistry.Get("user");
            user = model.GetUser(Session.Instance.CurrentUserId);
            OnLoad();
        }

        private void OnLoad()
        {
            lbEmail.Content = user.Email;
            lbFirstname.Content = user.Firstname;
            lbLastname.Content = user.Lastname;
            lbAdmin.Content = user.Admin ? "Ja" : "Nein";
        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            PasswordChangeDialog dialog = new PasswordChangeDialog();
            if(dialog.ShowDialog() == true)
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
