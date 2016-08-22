namespace UserStorage.Services
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Repositories;

    [Serializable]
    public class ServiceState : IXmlSerializable
    {
        public ServiceState()
        {           
        }

        public ServiceState(string identifier, string xmlPath, int lastGeneratedId, IUserStorage repository)
        {
            this.Identifier = identifier;
            this.XmlPath = xmlPath;
            this.LastGeneratedId = lastGeneratedId;
            this.Repository = repository;
        }

        public string Identifier { get; private set; }

        public string XmlPath { get; private set; }

        public int LastGeneratedId { get; set; }

        public IUserStorage Repository { get; private set; }

        public void SetTargerRepository(IUserStorage repository)
        {
            this.Repository = repository;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "Identifier" && reader.IsStartElement())
                {
                    reader.Read();
                    this.Identifier = reader.Value;
                }

                if (reader.Name == "XmlPath" && reader.IsStartElement())
                {
                    reader.Read();
                    this.XmlPath = reader.Value;
                }

                if (reader.Name == "LastGeneratedId" && reader.IsStartElement())
                {
                    reader.Read();
                    this.LastGeneratedId = int.Parse(reader.Value);
                }

                if (reader.Name == "Repository" && reader.IsStartElement())
                {
                    reader.Read();
                    this.Repository = new InMemoryUserStorage();
                    if (this.Repository == null)
                    {
                        Console.WriteLine("FAIL !!!");
                    }
                        
                    this.Repository.ReadXml(reader);
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Identifier", this.Identifier);
            writer.WriteElementString("XmlPath", this.XmlPath);
            writer.WriteElementString("LastGeneratedId", this.LastGeneratedId.ToString());
            this.Repository.WriteXml(writer);
        }
    }
}
