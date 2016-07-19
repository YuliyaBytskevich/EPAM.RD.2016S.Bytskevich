using System;
using System.IO;
using System.Net;
using System.Text;

namespace FileStreams
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Arguments: <source> <destination>");
                return;
            }
            string source = args[0];
            string destin = args[1];

            ByteCopy(source, destin);
            BlockCopy(source, destin);
            LineCopy(source, destin);
            MemoryBufferCopy(source, destin);
            WebClient();
            Console.ReadLine();
        }

        public static void ByteCopy(string source, string destin)
        {
            int bytesCounter = 0;
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.Create, FileAccess.Write))
            {
                int b;
                while ((b = sourceStream.ReadByte()) != -1) 
                {
                    bytesCounter++;
                    destinStream.WriteByte((byte)b);
                }
            }
            Console.WriteLine("ByteCopy() done. Total bytes: {0}", bytesCounter);
        }

        public static void BlockCopy(string source, string destin)
        {
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                do
                {
                    bytesRead = sourceStream.Read(buffer, 0, 1024);
                    Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                    destinStream.Write(buffer, 0, bytesRead);
                }
                while (bytesRead == buffer.Length);
            }
        }

        public static void LineCopy(string source, string destin)
        {
            int linesCount = 0;
            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var destinStream = new FileStream(destin, FileMode.Create, FileAccess.Write))
            {
                using (var streamReader = new StreamReader(sourceStream))
                using (var streamWriter = new StreamWriter(destinStream))
                {
                    string line;
                    while (true)
                    {
                        linesCount++;
                        if ((line = streamReader.ReadLine()) == null) 
                        {
                            break;
                        }
                        streamWriter.WriteLine(line); 

                    }
                }
            }
            Console.WriteLine("LineCopy(): {0} lines.", linesCount);
        }

        public static void MemoryBufferCopy(string source, string destin)
        {
            var stringBuilder = new StringBuilder();
            string content;
            using (var textReader = (TextReader)new StreamReader(new FileStream(source, FileMode.Open)))
            {
                content = textReader.ReadToEnd();
            }
            using (var sourceReader = new StringReader(content)) 
            using (var sourceWriter = new StringWriter(stringBuilder)) 
            {
                string line = null;
                do
                {
                    line = sourceReader.ReadLine();
                    if (line != null)
                    {
                        sourceWriter.WriteLine(line); 
                    }

                } while (line != null);
            }
            Console.WriteLine("MemoryBufferCopy(): chars in StringBuilder {0}", stringBuilder.Length);
            const int blockSize = 1024;
            using (var stringReader = new StringReader(stringBuilder.ToString()))
            using (var memoryStream = new MemoryStream(blockSize))
            using (var streamWriter = new StreamWriter(memoryStream)) 
            using (var destinStream = new FileStream(destin, FileMode.Create, FileAccess.Write)) 
            {
                char[] buffer = new char[blockSize];
                int bytesRead;
                do
                {
                    bytesRead = stringReader.Read(buffer, 0, blockSize);            
                    streamWriter.Write(buffer, 0, bytesRead); 
                    destinStream.Write(memoryStream.GetBuffer(), 0, bytesRead);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
                while (bytesRead == blockSize);
            }
        }

        public static void WebClient()
        {
            WebClient webClient = new WebClient();
            using (var stream = webClient.OpenRead("http://google.com"))
            {
                Console.WriteLine("WebClient(): CanRead = {0}", stream.CanRead);
                Console.WriteLine("WebClient(): CanWrite = {0}", stream.CanWrite); 
                Console.WriteLine("WebClient(): CanSeek = {0}", stream.CanSeek); 
                using (var streamReader = new StreamReader(stream))
                using (var destinStream = new StreamWriter(new FileStream("google_request.txt", FileMode.Create, FileAccess.Write)))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null)
                    {
                        destinStream.WriteLine(line);
                    }
                }
            }
        }
    }
}