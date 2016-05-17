using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OrderSystem.Data;
using OrderSystem.Enums;
using OrderSystem.Models;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// Interaktionslogik für AdminCreditPage.xaml
    /// </summary>
    public partial class AdminCreditPage : AppPage
    {
        private ObservableCollection<Credit> creditTable;
        private CreditModel creditModel; 

        public AdminCreditPage()
        {
        }

        public override void LoadView()
        {
            InitializeComponent();
            LoadedView = true;
        }

        public override void LoadResources()
        {
            LoadMembers();
            LoadCredits();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadCredits();
        }

        private void LoadMembers()
        {
            creditTable = new ObservableCollection<Credit>();
            dgCredits.DataContext = this;

            creditModel = (CreditModel) ModelRegistry.Get(ModelIdentifier.Credit);
        }

        private void LoadCredits()
        {
            creditTable.Clear();

            foreach (Credit credit in creditModel.GetAllOpenCredits())
            {
                creditTable.Add(credit);
            }
        }

        public ObservableCollection<Credit> CreditTable
        {
            get { return creditTable; }
        }

        private void OnDeleteCreditRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                Credit row = ((FrameworkElement)sender).DataContext as Credit;

                MessageBoxResult result = MessageBox.Show("Willst du die Guthaben-Anfrage wirklich löschen?",
                    "Guthaben-Anfrage löschen", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!creditModel.Delete(row.Id))
                    {
                        throw new Exception("Guthaben-Anfrage konnte nicht gelöscht werden.");
                    }
                    
                    ReloadResources();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnAcceptCreditRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                Credit row = ((FrameworkElement)sender).DataContext as Credit;
                if (!creditModel.Accept(row.Id))
                {
                    throw new Exception("Guthaben-Anfrage konnte nicht akzeptiert werden.");
                }

                ReloadResources();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
