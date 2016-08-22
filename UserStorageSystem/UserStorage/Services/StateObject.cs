namespace UserStorage.Services
{
    using System.Net.Sockets;

    public class StateObject
    {
        public const int BufferSize = 1024;

        public Socket WorkSocket { get; set; } = null;

        public byte[] Buffer { get; set; } = new byte[BufferSize];
    }
}
