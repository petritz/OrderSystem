using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OrderSystem.Exceptions;

namespace OrderSystem.Data
{
    /// <summary>
    /// This class is supposed to write out key-value xml files
    /// </summary>
    public class KeyValueWriter
    {
        private readonly string filename;

        private XmlDocument document;
        private XmlNode root;

        /// <summary>
        /// The filename to write to
        /// </summary>
        /// <param name="filename"></param>
        public KeyValueWriter(string filename)
        {
            this.filename = filename;
            document = new XmlDocument();

            root = document.CreateElement("keyvalue");
            XmlAttribute version = document.CreateAttribute("version");
            version.InnerText = "1.0";
            root.Attributes.Append(version);
            document.AppendChild(root);
        }

        /// <summary>
        /// Sets the dictionary to append to the root
        /// </summary>
        /// <param name="dict">The dictionary to set</param>
        public void SetDict(XmlNode dict)
        {
            if (dict == null)
            {
                throw new KeyValueException("The dictionary cannot be null.");
            }
            if (!dict.Name.Equals("dictionary"))
            {
                throw new KeyValueException("The name must be `dictionary`.");
            }
            if (root.HasChildNodes)
            {
                throw new KeyValueException("Dictionary was already created.");
            }

            root.AppendChild(dict);
        }

        /// <summary>
        /// Writes the XmlDocument to the file
        /// </summary>
        public void Write()
        {
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            document.Save(writer);
            writer.Close();
        }
    }
}
