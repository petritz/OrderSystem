using System.Windows.Media;

namespace OrderSystem.Data
{
    /// <summary>
    /// Configuration class that stores constants
    /// </summary>
    public class Configuration
    {
        private static Configuration _instance;
        private readonly Brush primary;
        private readonly string database;
        private readonly string storageFile;

        private Configuration()
        {
            primary = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C3E50"));
            database =
                "server=######;uid=######;database=######;persistsecurityinfo=True;allowuservariables=True;Pwd=######;Convert Zero Datetime=True";
            storageFile = "storage.prop";
        }

        /// <summary>
        /// Get the primary color of the application
        /// </summary>
        public Brush Primary
        {
            get { return primary; }
        }

        /// <summary>
        /// Get the database string
        /// </summary>
        public string Database
        {
            get { return database; }
        }

        /// <summary>
        /// Get the filename for the storage
        /// </summary>
        public string StorageFile
        {
            get { return storageFile; }
        }

        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static Configuration Instance
        {
            get
            {
                if (_instance == null) _instance = new Configuration();
                return _instance;
            }
        }
    }
}