using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage
{
    [Serializable]
    public struct VisaRecord: IXmlSerializable
    {
        public string Country { get; private set; }
        public DateTime DateOfStarting { get; private set; }
        public DateTime DateOfEnding { get; private set; }

        public VisaRecord(string country, DateTime dateOfStarting, DateTime dateOfEnding)
        {
            Country = country;
            DateOfStarting = dateOfStarting;
            DateOfEnding = dateOfEnding;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            //reader.ReadToDescendant("Country");
            Country = reader.ReadElementContentAsString();
            //reader.ReadToDescendant("DateOfStartinguntry");
            DateOfStarting = DateTime.Parse(reader.ReadElementContentAsString());
            //reader.ReadToDescendant("DateOfStarting");
            DateOfEnding = DateTime.Parse(reader.ReadElementContentAsString());
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Visa");
            writer.WriteElementString("Country", Country);
            writer.WriteElementString("DateOfStarting", DateOfStarting.ToString());
            writer.WriteElementString("DateOfEnding", DateOfEnding.ToString());
            writer.WriteEndElement();
        }
    }
}
