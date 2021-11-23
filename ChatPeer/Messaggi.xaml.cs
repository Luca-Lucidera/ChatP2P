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
        string ip;
        public Messaggi(string ip)
        {
            InitializeComponent();
            this.ip = ip;
        }
    }
}
