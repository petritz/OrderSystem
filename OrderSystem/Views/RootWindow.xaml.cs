using OrderSystem.Data;
using OrderSystem.Database;
using OrderSystem.Events;
using OrderSystem.Models;
using OrderSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Threading;
using OrderSystem.Enums;

namespace OrderSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage currentPage;
        private MainPage.MainEventHandler mainHandler;

        private DateTime date;
        private string now;
        private bool needLogin;

        // Init

        public MainWindow()
        {
            InitializeComponent();

            timer_Tick(null, null);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            lbConnection.Content = DAL.Instance.Connection.Database;
            
            //Call async and wait for response
            ReadSession();
            InitPage();
            UpdateUser();
        }

        // Function

        private void InitPage()
        {
            mainHandler = new MainPage.MainEventHandler(sender_DefaultEvent);

            if (needLogin)
            {
                currentPage = new Login();
            }
            else
            {
                currentPage = new RootApp();
            }

            currentPage.DefaultEvent += mainHandler;
            frame.Navigate(currentPage);
        }

        private void UpdateUser()
        {
            try
            {
                if(Session.Instance == null)
                {
                    throw new Exception();
                }

                UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
                User user = model.GetUser(Session.Instance.CurrentUserId);

                if(user == null)
                {
                    throw new Exception();
                }

                lbFirstname.Content = user.Firstname;
                lbLastname.Content = user.Lastname;
            }
            catch (Exception)
            {
                lbFirstname.Content = "";
                lbLastname.Content = "";
            }
        }

        private void ReadSession()
        {
            string email = Storage.Instance.Get("email");
            string password = Storage.Instance.Get("password");

            if(email == null || password == null)
            {
                needLogin = true;
            }
            else
            {
                UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
                if (model.LoginMd5(email, password))
                {
                    needLogin = false;
                    Session.CreateSession(model.GetUserId(email), email).Save(password);
                }
                else
                {
                    needLogin = true;
                    Session.DeleteSession();
                }
            }
        }

        // Events

        private void sender_DefaultEvent(object sender, MainEventArgs e)
        {
            if(e is LoginEventArgs)
            {
                LoginEventArgs login = (LoginEventArgs)e;
                if (login.Success)
                {
                    currentPage.DefaultEvent -= mainHandler;
                    currentPage = new RootApp();
                    currentPage.DefaultEvent += mainHandler;
                    frame.Navigate(currentPage);
                    //load other page
                    UpdateUser();
                }
            }
            else if(e is RegisterEventArgs)
            {
                RegisterEventArgs register = (RegisterEventArgs)e;
                if (register.Success)
                {
                    currentPage.DefaultEvent -= mainHandler;
                    currentPage = new RootApp();
                    currentPage.DefaultEvent += mainHandler;
                    frame.Navigate(currentPage);
                    //load other page
                    UpdateUser();
                }
            }
            else if(e is LogoutEventArgs)
            {
                //Logout
                needLogin = true;
                currentPage.DefaultEvent -= mainHandler;
                InitPage();
                UpdateUser();
            }
            else if(e is RedirectEventArgs)
            {
                RedirectEventArgs redirect = (RedirectEventArgs)e;
                string view = redirect.View;

                currentPage.DefaultEvent -= mainHandler;

                //TODO: make better
                if (view.Equals("login"))
                {
                    currentPage = new Login();
                }
                else if (view.Equals("register"))
                {
                    currentPage = new Register();
                }

                currentPage.DefaultEvent += mainHandler;
                frame.Navigate(currentPage);
                UpdateUser();
            }
        }

        // Properties

        public string Now
        {
            get
            {
                return now;
            }

            private set
            {
                now = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Now"));
            }

        }

        public Brush Primary
        {
            get
            {
                return Configuration.Instance.Primary;
            }
        }

        // Events

        void timer_Tick(object sender, EventArgs e)
        {
            date = DateTime.Now;
            Now = date.ToShortDateString() + " " + date.ToLongTimeString();
        }

    }
}
