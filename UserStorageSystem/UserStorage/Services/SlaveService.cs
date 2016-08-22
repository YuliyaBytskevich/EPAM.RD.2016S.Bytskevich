namespace UserStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using NLog;
    using Predicates;
    using Repositories;
    using UserEntity;

    public class SlaveService : UserStorageService
    {
        private static readonly Logger Logger = LogManager.GetLogger("*");
        private ManualResetEvent allDone = new ManualResetEvent(false);

        public SlaveService()
        {          
        }

        public SlaveService(string serviceIdentifier, string xmlPath, int port, IUserStorage storage) : base(serviceIdentifier, xmlPath, storage)
        {
            this.Port = port;
        }

        public int Port { get; }

        public override void Add(User user)
        {
            Logger.Trace(State.Identifier + " : ADD [slave service] operation called... ");
            Logger.Error(State.Identifier + ": Slave service is not allowed to call ADD operation");
            throw new ForbiddenOperationException("Slave service is not allowed to call ADD operation");
        }

        public override void Delete(User user)
        {
            Logger.Trace(State.Identifier + " : DELETE [slave service] operation called... ");
            Logger.Error(State.Identifier + ": Slave service is not allowed to call DELETE operation");
            throw new ForbiddenOperationException("Slave service is not allowed to call DELETE operation");
        }

        public override int SearchForUser(params IPredicate[] predicates)
        {
            CollectionIsEnabled.WaitOne();
            return base.SearchForUser(predicates);
        }

        public override List<int> SearchForUsers(params IPredicate[] predicates)
        {
            CollectionIsEnabled.WaitOne();
            return base.SearchForUsers(predicates);
        }

        public void EnableNetworkConnection()
        {
            Thread listeningThred = new Thread(this.RunListeningSocket);
            listeningThred.Start();
        }

        private static ServiceMessage DeserializeMessage(byte[] messageInBytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Write(messageInBytes, 0, messageInBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return (ServiceMessage)formatter.Deserialize(stream);
        }

        private void RunListeningSocket()
        {
            byte[] bytes = new byte[1024];
            IPHostEntry hostInfo = Dns.GetHostEntry("localhost");
            IPAddress address = hostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(address, this.Port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    this.allDone.Reset();
                    listener.BeginAccept(this.AcceptCallback, listener);
                    this.allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.StackTrace);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            this.allDone.Set();
            CollectionIsEnabled.Reset();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, this.ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            List<byte> messageInBytes = new List<byte>();
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;
            int numOfRecievedBytes = handler.EndReceive(ar);
            if (numOfRecievedBytes > 0)
            {
                messageInBytes.AddRange(state.Buffer.Take(numOfRecievedBytes));
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

                    Logger.Info(State.Identifier + ": RECIEVED MESSAGE: " + recievedMessage.Operation + " | " +
                                recievedMessage.ChangingData.Id + " " + recievedMessage.ChangingData.FirstName + " " +
                                recievedMessage.ChangingData.LastName);
                    CollectionIsEnabled.Set();
                }
                else
                {
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(this.ReadCallback), state);
                }
            }
        }
    }
}
