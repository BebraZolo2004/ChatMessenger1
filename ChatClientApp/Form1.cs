using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace ChatClientApp
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        string username;

        public Form1()
        {
            InitializeComponent();

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                username = txtUsername.Text;

                client = new TcpClient();
                await client.ConnectAsync(txtIP.Text, int.Parse(txtPort.Text));

                stream = client.GetStream();

                Task.Run(ReceiveMessages);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }
        private async Task ReceiveMessages()
        {
            byte[] buffer = new byte[4096];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0) break;

                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Message msg = JsonConvert.DeserializeObject<Message>(json);

                Invoke(new Action(() =>
                {
                    txtChatHistory.AppendText(
                        $"[{msg.Timestamp:HH:mm}] {msg.Author}: {msg.Text}\r\n");
                }));
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected) return;

            Message msg = new Message()
            {
                Author = username,
                Text = txtMessage.Text,
                Timestamp = DateTime.Now,
                Receiver = lstUsers.SelectedItem?.ToString()
            };

            string json = JsonConvert.SerializeObject(msg);
            byte[] data = Encoding.UTF8.GetBytes(json);

            await stream.WriteAsync(data, 0, data.Length);

            txtMessage.Clear();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                byte[] fileData = System.IO.File.ReadAllBytes(dialog.FileName);

                Message msg = new Message()
                {
                    Author = username,
                    FileData = fileData,
                    FileName = System.IO.Path.GetFileName(dialog.FileName),
                    Receiver = lstUsers.SelectedItem?.ToString()
                };

                string json = JsonConvert.SerializeObject(msg);
                byte[] data = Encoding.UTF8.GetBytes(json);

                stream.Write(data, 0, data.Length);
            }
        }
    }
}
