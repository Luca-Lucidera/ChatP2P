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
    /// Logica di interazione per Comunicazione1.xaml
    /// </summary>
    public partial class Comunicazione1 : Window
    {
        string username;
        const int port = 2003;
        UdpClient receivingClient;
        UdpClient sendingClient;
        Thread receivingThread;
        public Comunicazione1(string username)
        {
            InitializeComponent();
            this.username = username;
            receivingClient = new UdpClient(port);

            //thread per ricevere i dati in background => non bloccante
            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.SetApartmentState(ApartmentState.STA);
            receivingThread.Start();

        }

        private void tryConnect_Click(object sender, RoutedEventArgs e)
        {
            sendingClient = new UdpClient(txt_ip.Text, port);
            string toSend = "c;" + username; //Primo peer vuole instaurare la connessione
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }
        private void Receiver()//funzione che viene eseguita in parallelo dal thread di ricezione (receivingClient)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                string ipRicevuto = endPoint.Address.ToString();
                //MessageBox.Show(message);
                if (message[0] == 'c')//lo fa il secondo peer
                {
                    //il secondo peer riceve C e il nome utente di chi vuole connettersi
                    if (MessageBox.Show("Vuoi stabilire la connessione?", "Richiesta di connessione", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        string daRitornare = "y;" + username; //Il secondo peer invia y = yes e il suo Username
                        sendData(ipRicevuto, daRitornare); //invia il y;username al primo peer
                        /*
                            col fatto che il secondo peer ha accettato la connessione
                            chiude questa finestra e aprirà la finestra per dialogare con
                            l'altro peer
                        */
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            sendingClient.Close();
                            receivingClient.Close();
                            Messaggi m = new Messaggi(ipRicevuto, username);
                            m.Show();
                            this.Hide();
                        }));
                    }
                    else
                    {
                        //se il secondo peer rifiuta la connessione manda una n = no
                        sendData(ipRicevuto, "n");
                    }
                }
                else if (message[0] == 'y')//questo lo eseguirà il primo peer nella terza fase
                {
                    //visto che il primo peer ha ricevuto il y, saprà che la connessione è stata accettata
                    //e aprirà la pagina di dialogo
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        sendingClient.Close();
                        receivingClient.Close();
                        Messaggi m = new Messaggi(ipRicevuto, username);
                        m.Show();
                        this.Hide();
                    }));
                    break; //il break in questo caso serve per far finire il thread della ricezione dei messaggi
                }
            }

        }

        private void sendData(string ip, string messaggio) //funzione per inviare dei dati
        {
            sendingClient = new UdpClient(ip, port);
            string toSend = messaggio;
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }
    }
}
