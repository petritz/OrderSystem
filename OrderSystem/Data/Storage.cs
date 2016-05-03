using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public class Storage
    {
        private static Storage instance;

        private Dictionary<string, string> list;
        private string filename;

        // Init

        static Storage()
        {

        }

        private Storage()
        {
            this.filename = Configuration.Instance.StorageFile;
            Reload();
        }

        // Functions

        public string Get(string key, string defaultValue)
        {
            return Get(key) ?? defaultValue;
        }

        public string Get(string key)
        {
            return (list.ContainsKey(key)) ? (list[key]) : (null);
        }

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

        public void Remove(string key)
        {
            if (list.ContainsKey(key))
            {
                list.Remove(key);
            }
        }

        public void Save()
        {
            if (!System.IO.File.Exists(filename))
            {
                System.IO.File.Create(filename);
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            
            foreach(string property in list.Keys.ToArray())
            {
                if (!string.IsNullOrWhiteSpace(list[property]))
                {
                    file.WriteLine(property + "=" + list[property]);
                }
            }

            file.Close();
        }

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

        private void LoadFromFile()
        {
            foreach(string line in System.IO.File.ReadAllLines(filename))
            {
                if((!string.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore duplicates
                        list.Add(key, value);
                    }
                    catch { }
                }
            }
        }

        // Properties

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
