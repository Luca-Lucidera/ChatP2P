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
        string mioUsername;
        string altroClient;
        UdpClient receivingClient;
        UdpClient sendingClient;
        Thread receivingThread;
        string ip;
        public Messaggi(string ip, string username, string altroClient)
        {
            InitializeComponent();
            this.ip = ip;
            this.mioUsername = username;
            this.altroClient = altroClient;
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
                if (message[0] == 'm')//esegue il secondo peer riceve un messaggio
                {
                    //metto in "tutto" il nome del primo peer che mi ha inviato il messaggio, e poi il messaggio
                    string tutto = String.Format("{0} : {1}", altroClient, message.Substring(2, message.Length - 2)) + "\n";
                    
                    //Dispatcher per modificare la grafica
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        txt_all.Text += tutto; //vado a visualizzare chi ha mandato il messaggio e il messaggio stesso

                    }));
                }
                else if (message[0] == 'e')//se il peer riceve "e" allora deve chiudere la connessione 
                {
                    //Avverte che il primo peer ha chiuso la connessione
                    MessageBox.Show("L'altro client ha chiuso la connessione!");
                    //Dispatcher per modificare il thread grafico
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        sendingClient.Close(); //chiudo il peer che manda i dati
                        receivingClient.Close(); //chiudo il peer che riceve i dati
                        Comunicazione1 m = new Comunicazione1(mioUsername);
                        m.Show();
                        this.Hide();
                    }));
                    break; //break serve per far terminare il Thread
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

        //invio del messaggio e visualizzazione
        private void button_Click(object sender, RoutedEventArgs e)
        {
            txt_all.Text += mioUsername + ": " + txt_messaggio.Text + "\n";//visualizzo il messaggio
            sendData(ip, String.Format("m;{0}", txt_messaggio.Text));//invio i dati del messaggio all'altro peer
        }

        //torna alla schermata delle connessioni
        private void btn_indietro_Click(object sender, RoutedEventArgs e)
        {
            sendData(ip, "e");//invio il messaggio per finire la connessione 
            receivingThread.Abort();//visto che non sono nel thread lo faccio finire con un eccezione volontaria
            Dispatcher.BeginInvoke((Action)(() =>
            {
                receivingClient.Close(); //termino il recivingClient
                Comunicazione1 m = new Comunicazione1(mioUsername); //ripasso alla pagina per avviare una connessione il mio username
                m.Show();
                this.Hide();
            }));
        }
    }
}
