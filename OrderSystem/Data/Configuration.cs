using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OrderSystem
{
    public class Configuration : INotifyPropertyChanged
    {
        private static Configuration instance;
        private Brush primary;
        private string database;
        private string storageFile;
        public event PropertyChangedEventHandler PropertyChanged;

        // Init

        static Configuration()
        {
        }

        private Configuration()
        {
            primary = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C3E50"));
            database = "server=#########;user id=####;database=#####;persistsecurityinfo=True;allowuservariables=True;Pwd=######;Convert Zero Datetime=True";
            storageFile = "storage.prop";
        }

        // Functions



        // Properties

        public Brush Primary
        {
            get { return primary; }
            set { primary = value; PropertyChanged(this, new PropertyChangedEventArgs("Primary")); }
        }

        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        public string StorageFile
        {
            get { return storageFile; }
            set { storageFile = value; }
        }

        public static Configuration Instance
        {
            get
            {
                if (instance == null) instance = new Configuration();
                return instance;
            }
        }
    }
}
