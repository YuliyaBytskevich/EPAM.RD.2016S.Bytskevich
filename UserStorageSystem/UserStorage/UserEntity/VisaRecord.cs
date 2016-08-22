namespace UserStorage.UserEntity
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    [DataContract]
    public struct VisaRecord : IXmlSerializable
    {
        public VisaRecord(string country, DateTime dateOfStarting, DateTime dateOfEnding)
        {
            this.Country = country;
            this.DateOfStarting = dateOfStarting;
            this.DateOfEnding = dateOfEnding;
        }

        [DataMember]
        public string Country { get; private set; }
        [DataMember]
        public DateTime DateOfStarting { get; private set; }
        [DataMember]
        public DateTime DateOfEnding { get; private set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this.Country = reader.ReadElementContentAsString();
            this.DateOfStarting = DateTime.Parse(reader.ReadElementContentAsString());
            this.DateOfEnding = DateTime.Parse(reader.ReadElementContentAsString());
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Visa");
            writer.WriteElementString("Country", this.Country);
            writer.WriteElementString("DateOfStarting", this.DateOfStarting.ToString());
            writer.WriteElementString("DateOfEnding", this.DateOfEnding.ToString());
            writer.WriteEndElement();
        }
    }
}
