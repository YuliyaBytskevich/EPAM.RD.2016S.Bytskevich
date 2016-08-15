using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage.Services
{
    //[Serializable]
    public class ServiceState: IXmlSerializable
    {
        public string Identifier { get; private set; }
        public string XmlPath { get; private set; }
        public int LastGeneratedId { get; set; }
        public IUserStorage Repository { get; private set; }

        public ServiceState() { }

        public ServiceState(string identifier, string xmlPath, int lastGeneratedId, IUserStorage repository)
        {
            Identifier = identifier;
            XmlPath = xmlPath;
            LastGeneratedId = lastGeneratedId;
            Repository = repository;
        }

        public void SetTargerRepository(IUserStorage repository)
        {
            Repository = repository;
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
                    Identifier = reader.Value;
                }
                if (reader.Name == "XmlPath" && reader.IsStartElement())
                {
                    reader.Read();
                    XmlPath = reader.Value;
                }
                if (reader.Name == "LastGeneratedId" && reader.IsStartElement())
                {
                    reader.Read();
                    LastGeneratedId = int.Parse(reader.Value);
                }
                if (reader.Name == "Repository" && reader.IsStartElement())
                {
                    reader.Read();
                    Repository = new InMemoryUserStorage();
                    if (Repository == null)
                        Console.WriteLine("FAIL !!!");
                    Repository.ReadXml(reader);
                }
            }

        }

        public void WriteXml(XmlWriter writer)
        {
            //XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
            //XmlSerializer intSserializer = new XmlSerializer(typeof(int));
            //XmlSerializer repositorySerializer = new XmlSerializer(typeof(IUserStorage));
            //stringSerializer.Serialize(writer, Identifier);
            //stringSerializer.Serialize(writer, XmlPath);      
            //intSserializer.Serialize(writer, LastGeneratedId);
            //repositorySerializer.Serialize(writer, Repository);
            writer.WriteElementString("Identifier", Identifier);
            writer.WriteElementString("XmlPath", XmlPath);
            writer.WriteElementString("LastGeneratedId", LastGeneratedId.ToString());
            Repository.WriteXml(writer);
            //writer.write
        }
    }
}
