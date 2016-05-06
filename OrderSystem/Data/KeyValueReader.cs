using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OrderSystem.Data
{
    /// <summary>
    /// This class is supposed to read key-value xml files
    /// </summary>
    public class KeyValueReader
    {
        private readonly string filename;

        /// <summary>
        /// Read from the specified filename
        /// </summary>
        /// <param name="filename">The filename</param>
        public KeyValueReader(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Read the Xml Document from the file
        /// </summary>
        /// <returns>The Xml Document</returns>
        public XmlDocument Read()
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            reader.MoveToContent();
            document.Load(reader);
            reader.Close();
            return document;
        }
    }
}
