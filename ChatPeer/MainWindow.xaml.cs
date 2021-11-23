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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatPeer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void txt_nome_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!txt_nome.Text.Equals(""))
                btn_insNome.IsEnabled = true;
            else
                btn_insNome.IsEnabled = false;
        }

        private void btn_insNome_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Comunicazione1 c = new Comunicazione1(txt_nome.Text);
            c.Show();
        }
    }
}
