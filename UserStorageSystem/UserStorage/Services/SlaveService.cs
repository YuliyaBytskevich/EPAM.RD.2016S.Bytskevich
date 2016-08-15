using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace UserStorage.Services
{
    public class SlaveService : UserStorageService
    {
        public int Port { get; }
        private static readonly Logger logger = LogManager.GetLogger("*");
        private ManualResetEvent allDone = new ManualResetEvent(false);
        

        public SlaveService(string serviceIdentifier, string xmlPath, int port, IUserStorage storage): base(serviceIdentifier, xmlPath, storage)
        {
            Port = port;
        }

        public override void Add(User user)
        {
            logger.Trace(State.Identifier + " : ADD [slave service] operation called... ");
            logger.Error(State.Identifier + ": Slave service is not allowed to call ADD operation");
            throw new ForbiddenOperationException("Slave service is not allowed to call ADD operation");
        }

        public override void Delete(User user)
        {
            logger.Trace(State.Identifier + " : DELETE [slave service] operation called... ");
            logger.Error(State.Identifier + ": Slave service is not allowed to call DELETE operation");
            throw new ForbiddenOperationException("Slave service is not allowed to call DELETE operation");
        }

        public override int SearchForUser(params Func<User, bool>[] predicates)
        {
            slaveCollectionsAreEnable.WaitOne();
            return base.SearchForUser(predicates);
        }

        public override IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates)
        {
            slaveCollectionsAreEnable.WaitOne();
            return base.SearchForUsers(predicates);
        }

        public void EnableNetworkConnection()
        {
            Thread listeningThred = new Thread(RunListeningSocket);
            listeningThred.Start();
        }

        private void RunListeningSocket()
        {
            byte[] bytes = new Byte[1024];
            IPHostEntry ipHostInfo = Dns.Resolve("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    allDone.Reset();
                    //slaveCollectionIsEnable.Reset();
                    //Console.WriteLine(State.Identifier + ": вызывано несигнальное состояние slaveCollectionIsEnable.Reset()");
                    listener.BeginAccept(AcceptCallback, listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            List<byte> messageInBytes = new List<byte>();
            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.workSocket;
            int numOfRecievedBytes = handler.EndReceive(ar);
            if (numOfRecievedBytes > 0)
            {
                messageInBytes.AddRange(state.buffer.Take(numOfRecievedBytes));
                if (numOfRecievedBytes < StateObject.BufferSize)
                {
                    ServiceMessage recievedMessage = DeserializeMessage(messageInBytes.ToArray());
                    if (recievedMessage.Operation == Operation.Add)
                    {
                        State.Repository.Add(recievedMessage.ChangingData);
                    }
                    else if (recievedMessage.Operation == Operation.Remove)
                    {
                        State.Repository.Delete(recievedMessage.ChangingData);
                    }
                    logger.Info(State.Identifier + ": RECIEVED MESSAGE: " + recievedMessage.Operation + " | " +
                                recievedMessage.ChangingData.Id + " " + recievedMessage.ChangingData.FirstName + " " +
                                recievedMessage.ChangingData.LastName);
                    slaveCollectionsAreEnable.Set();
                   // Console.WriteLine(State.Identifier + ": вызывано СИГНАЛЬНОЕ состояние slaveCollectionIsEnable.Set()");
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback),
                        state);
                }
            }
        }

        private static ServiceMessage DeserializeMessage(byte[] messageInBytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(messageInBytes, 0, messageInBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (ServiceMessage) formatter.Deserialize(stream);
        }
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
