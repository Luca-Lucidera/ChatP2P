using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatPeer
{
    internal class Connettore
    {
        private UdpClient client;
        public string ipDestinatario { set; get; }
        public Connettore()
        {
            client = new UdpClient();
        }
        public void sendData(string data)
        {
            byte[] b = Encoding.ASCII.GetBytes("192.168.1.201");
            IPAddress ipdest = new IPAddress(b);
            IPEndPoint ip = new IPEndPoint(ipdest, 2003);
            byte[] bufferData = Encoding.ASCII.GetBytes(data);
            client.Send(bufferData, bufferData.Length,ip);
        }
        public string reciveData()
        {
            
            IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] dataReceived = client.Receive(ref riceveEP);
            String risposta = Encoding.ASCII.GetString(dataReceived);
            return risposta;
        }
    }
}
