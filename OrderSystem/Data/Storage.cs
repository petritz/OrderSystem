using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    /// <summary>
    /// The storage class can save data persistent. It saves the data in key-value files. The data is read automatically on the first Instance access.
    /// </summary>
    public class Storage
    {
        private static Storage instance;

        private Dictionary<string, string> list;
        private readonly string filename;

        private Storage()
        {
            filename = Configuration.Instance.StorageFile;
            Reload();
        }

        /// <summary>
        /// Gets the associated value to the key. If no value is found, the default value is returned.
        /// </summary>
        /// <param name="key">The key to find the value</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value</returns>
        public string Get(string key, string defaultValue)
        {
            return Get(key) ?? defaultValue;
        }

        /// <summary>
        /// Gets the associated value to the key. If no value is found, null is returned.
        /// </summary>
        /// <param name="key">The key to find the value</param>
        /// <returns>The value</returns>
        public string Get(string key)
        {
            return (list.ContainsKey(key)) ? (list[key]) : (null);
        }

        /// <summary>
        /// Set a key-value pair. If the pair already existed it will be overwritten. At this point nothing is saved in the file! You need to call .Save() for that
        /// </summary>
        /// <param name="key">The key to save</param>
        /// <param name="value">The value to save</param>
        public void Set(string key, string value)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, value);
            }
            else
            {
                list[key] = value;
            }
        }

        /// <summary>
        /// Removes a key-value pair. At this point nothing is changed in the file! You need to call .Save() for that
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (list.ContainsKey(key))
            {
                list.Remove(key);
            }
        }

        /// <summary>
        /// Save the changed data to the file.
        /// </summary>
        public void Save()
        {
            if (!System.IO.File.Exists(filename))
            {
                System.IO.File.Create(filename);
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);

            foreach (string property in list.Keys.ToArray())
            {
                if (!string.IsNullOrWhiteSpace(list[property]))
                {
                    file.WriteLine(property + "=" + list[property]);
                }
            }

            file.Close();
        }

        /// <summary>
        /// Reload the data from the file. Everything that has been changed and not saved, will be lost.
        /// </summary>
        public void Reload()
        {
            list = new Dictionary<string, string>();

            if (System.IO.File.Exists(filename))
            {
                LoadFromFile();
            }
            else
            {
                System.IO.File.Create(filename);
            }
        }

        /// <summary>
        /// Loads the key-value pairs from the file.
        /// </summary>
        private void LoadFromFile()
        {
            foreach (string line in System.IO.File.ReadAllLines(filename))
            {
                if ((!string.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore duplicates
                        list.Add(key, value);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// The instance to the storage
        /// </summary>
        public static Storage Instance
        {
            get
            {
                if (instance == null) instance = new Storage();
                return instance;
            }
        }
    }
}