namespace UserStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using NLog;
    using Predicates;
    using Repositories;
    using UserEntity;

    public class MasterService : UserStorageService
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static Logger logger = LogManager.GetLogger("*");
        private List<int> slavePorts = new List<int>();

        public MasterService()
        {          
        }

        public MasterService(string serviceIdentifier, string xmlPath, IUserStorage storage) : base(serviceIdentifier, xmlPath, storage)
        {           
        }
        
        public override void Add(User user)
        {
            CollectionIsEnabled.WaitOne();
            CollectionIsEnabled.Reset();
            logger.Trace(State.Identifier + " : ADD [master service] operation called... ");
            try
            {
                user.Id = State.Repository.Add(user);
                State.LastGeneratedId = user.Id;
                logger.Trace(State.Identifier + ": New user is added successfully. New user ID = " +
                             State.LastGeneratedId + "\n");
                foreach (var port in this.slavePorts)
                {
                    this.SendMessageViaSocket(port, new ServiceMessage(user, Operation.Add));
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                CollectionIsEnabled.Set();
            }
        }

        public override void Delete(User user)
        {
            CollectionIsEnabled.WaitOne();
            CollectionIsEnabled.Reset();
            logger.Trace(State.Identifier + " : DELETE [master service] operation called... ");
            try
            {
                int id = user.Id;
                State.Repository.Delete(user);
                logger.Trace(State.Identifier + ": User with id " + id + "is deleted successfully.\n");
                foreach (var port in this.slavePorts)
                {
                    this.SendMessageViaSocket(port, new ServiceMessage(user, Operation.Remove));
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                CollectionIsEnabled.Set();
            }
        }

        public void RegisterPortForSlaveService(int newSlavePort)
        {
            this.slavePorts.Add(newSlavePort);
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

       private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
        }

        private void SendMessageViaSocket(int targetPort, ServiceMessage message)
        {
            try
            {
                IPHostEntry hostInfo = Dns.GetHostEntry("localhost");
                IPAddress address = hostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, targetPort);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, this.ConnectCallback, client);
                connectDone.WaitOne();
                this.Send(client, message);
                sendDone.WaitOne();
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                connectDone.Set();
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
        }

        private void Send(Socket client, ServiceMessage message)
        {
            byte[] byteData = this.SerializeMessage(message);
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private byte[] SerializeMessage(ServiceMessage message)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, message);
            byte[] msg = stream.ToArray();       
            return msg;
        }
    }
}
