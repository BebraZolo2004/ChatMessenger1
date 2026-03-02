using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServerApp
{
    public partial class Form1 : Form
    {
        private TcpListener listener;
        private bool isRunning = false;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            if (isRunning)
                return;

            try
            {
                string ip = txtIP.Text;
                int port = int.Parse(txtPort.Text);

                listener = new TcpListener(IPAddress.Parse(ip), port);
                listener.Start();

                isRunning = true;
                btnStartServer.Enabled = false;

                MessageBox.Show("Сервер запущен!");

                while (isRunning)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            MessageBox.Show("Клиент подключился");
            await Task.CompletedTask;
        }
    }
}