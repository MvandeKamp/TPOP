using GameStateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TPOP
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Player player;
        public MainWindow()
        {
            Networking.Connect("192.168.178.24", "7777");
            InitializeComponent();

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
            Networking.LoginAccount(createPlayer());
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
            Networking.CreateAccount(createPlayer());
        }
        private void ConnectToServer()
        {
            
        }
        private Player createPlayer()
        {
            return player = new Player(txtUsername.Text, txtPassword.Text);
        }
    }
}
