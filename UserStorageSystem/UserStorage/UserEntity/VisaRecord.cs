using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage.UserEntity
{
    [Serializable]
    [DataContract]
    public struct VisaRecord: IXmlSerializable
    {
        [DataMember]
        public string Country { get; private set; }
        [DataMember]
        public DateTime DateOfStarting { get; private set; }
        [DataMember]
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
            Country = reader.ReadElementContentAsString();
            DateOfStarting = DateTime.Parse(reader.ReadElementContentAsString());
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
