using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using UserStorage.Services;
using UserStorage.Configurations;

namespace UserStorage
{
    public class MasterService : UserStorageService
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent masterCollectionIsEnable = new ManualResetEvent(true);
        private static Logger logger = LogManager.GetLogger("*");
        private List<int> slavePorts = new List<int>();


        public MasterService(string serviceIdentifier, string xmlPath, IUserStorage storage) : base(serviceIdentifier, xmlPath, storage) { }
        
        public override void Add(User user)
        {
            slaveCollectionsAreEnable.Reset();
            masterCollectionIsEnable.WaitOne();
            masterCollectionIsEnable.Reset();
            logger.Trace(State.Identifier + " : ADD [master service] operation called... ");
            try
            {
                user.Id = State.Repository.Add(user);
                State.LastGeneratedId = user.Id;
                logger.Trace(State.Identifier + ": New user is added successfully. New user ID = " +
                             State.LastGeneratedId + "\n");
                foreach (var port in slavePorts)
                {
                    SendMessageViaSocket(port, new ServiceMessage(user, Operation.Add));
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                masterCollectionIsEnable.Set();
            }
        }

        public override void Delete(User user)
        {
            slaveCollectionsAreEnable.Reset();
            masterCollectionIsEnable.WaitOne();
            masterCollectionIsEnable.Reset();
            logger.Trace(State.Identifier + " : DELETE [master service] operation called... ");
            try
            {
                int id = user.Id;
                State.Repository.Delete(user);
                logger.Trace(State.Identifier + ": User with id " + id + "is deleted successfully.\n");
                foreach (var port in slavePorts)
                {
                    SendMessageViaSocket(port, new ServiceMessage(user, Operation.Remove));
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                masterCollectionIsEnable.Set();
            }
        }

        public void RegisterPortForSlaveService(int newSlavePort)
        {
            slavePorts.Add(newSlavePort);
        }

        public override int SearchForUser(params Func<User, bool>[] predicates)
        {
            masterCollectionIsEnable.WaitOne();
            return base.SearchForUser(predicates);
        }

        public override IEnumerable<int> SearchForUsers(params Func<User, bool>[] predicates)
        {
            masterCollectionIsEnable.WaitOne();
            return base.SearchForUsers(predicates);
        }

        private void SendMessageViaSocket(int targetPort, ServiceMessage message)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve("localhost");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, targetPort);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, ConnectCallback, client);
                connectDone.WaitOne();
                Send(client, message);
                sendDone.WaitOne();
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
        }

        private  void ConnectCallback(IAsyncResult ar)
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

        private  void Send(Socket client, ServiceMessage message)
        {
            byte[] byteData = SerializeMessage(message);
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
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

        private byte[] SerializeMessage(ServiceMessage message)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, message);
            byte[] msg = stream.ToArray();       
            return msg;
        }
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
