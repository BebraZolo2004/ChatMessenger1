using System;
using System.Windows.Forms;

namespace ChatServerApp
{
    public partial class Form1 : Form
    {
        private ChatServer server = new ChatServer();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            server.Start(txtIP.Text, int.Parse(txtPort.Text));
            btnStartServer.Enabled = false;
            MessageBox.Show("Сервер запущен!");
        }
    }
}