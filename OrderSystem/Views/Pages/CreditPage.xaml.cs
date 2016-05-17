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
    /// The credit page
    /// </summary>
    public partial class CreditPage : AppPage
    {
        private ObservableCollection<Credit> creditTable;
        private CreditModel creditModel; 

        public CreditPage()
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
            LoadCurrentCredit();
            LoadCredits();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadCurrentCredit();
            LoadCredits();
        }

        private void LoadMembers()
        {
            creditTable = new ObservableCollection<Credit>();
            dgCredit.DataContext = this;

            creditModel = (CreditModel) ModelRegistry.Get(ModelIdentifier.Credit);
        }

        private void LoadCurrentCredit()
        {
            lbCredit.Content = string.Format("€ {0,00}", creditModel.GetCurrentCredit(Session.Instance.CurrentUserId));
        }

        private void LoadCredits()
        {
            creditTable.Clear();

            foreach (Credit credit in creditModel.GetPendingCredits(Session.Instance.CurrentUserId))
            {
                creditTable.Add(credit);
            }
        }

        public ObservableCollection<Credit> CreditTable
        {
            get { return creditTable; }
        }

        private void OnChargeCredit(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal price = duPrice.Value ?? 0;

                if (price <= 0)
                {
                    throw new Exception("Der Betrag muss größer als 0 sein.");
                }

                if (!creditModel.AddCredit(price, Session.Instance.CurrentUserId))
                {
                    throw new Exception("Guthaben konnte nicht erstellt werden.");
                }

                ReloadResources();
                duPrice.Value = 0;
                MessageBox.Show("Bitte bezahl das Geld bei einem Administrator, damit das Guthaben aufgeladen wird.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnDeleteOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                Credit row = ((FrameworkElement)sender).DataContext as Credit;
                if (row.Status == CreditStatus.Deleted)
                {
                    throw new Exception("Das Guthaben wurde bereits gelöscht.");
                }

                if (!creditModel.Delete(row.Id))
                {
                    throw new Exception("Das Guthaben konnte nicht gelöscht werden.");
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
