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

        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Введите корректный порт!");
                return;
            }

            string ip = txtIP.Text.Trim();

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Введите IP!");
                return;
            }

            btnStartServer.Enabled = false;
            MessageBox.Show("Сервер запущен!");

            await server.Start(ip, port);
        }
    }
}