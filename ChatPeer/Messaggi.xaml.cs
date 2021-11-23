using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatPeer
{
    /// <summary>
    /// Logica di interazione per Messaggi.xaml
    /// </summary>
    public partial class Messaggi : Window
    {
        const int port = 2003;
        UdpClient receivingClient;
        UdpClient sendingClient;
        Thread receivingThread;
        string ip;
        public Messaggi(string ip)
        {
            InitializeComponent();
            this.ip = ip;
            //sendingClient = new UdpClient(ipDestinatario, port);
            receivingClient = new UdpClient(port);

            //thread per ricevere i dati in background => non bloccante
            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }
        private void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            string ipRicevuto = endPoint.Address.ToString();
            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                if (message[0] == 'm')
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        txt_all.Text += message.Substring(2,message.Length - 2) + "\n";

                    }));
                }
                else if (message[0] == 'e')
                {
                    //fa qualcosa per chiudere la connessione
                }
            }
        }
        private void sendData(string ip, string messaggio)
        {
            sendingClient = new UdpClient(ip, port);
            string toSend = messaggio;
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            sendData(ip, String.Format("m;{0}", txt_messaggio.Text));
        }
    }
}
