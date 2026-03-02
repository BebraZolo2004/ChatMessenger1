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
                client = new TcpClient();
                await client.ConnectAsync(txtIP.Text, int.Parse(txtPort.Text));

                stream = client.GetStream();

                MessageBox.Show("Подключено к серверу!");

                _ = Task.Run(() => ReceiveMessages());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }
        private async Task ReceiveMessages()
        {
            byte[] buffer = new byte[4096];
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                while (sb.ToString().Contains("\n"))
                {
                    string fullMessage = sb.ToString();
                    int index = fullMessage.IndexOf("\n");

                    string json = fullMessage.Substring(0, index);
                    sb.Remove(0, index + 1);

                    Message msg = JsonConvert.DeserializeObject<Message>(json);
                    if (msg == null) continue;

                    Invoke(new Action(() =>
                    {
                        if (msg.FileData != null && msg.FileData.Length > 0)
                        {
                            string path = System.IO.Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                msg.FileName);

                            System.IO.File.WriteAllBytes(path, msg.FileData);

                            txtChatHistory.AppendText(
                                $"[{msg.Timestamp:HH:mm}] Получен файл: {msg.FileName}\r\n");
                        }
                        else
                        {
                            txtChatHistory.AppendText(
                                $"[{msg.Timestamp:HH:mm}] {msg.Author}: {msg.Text}\r\n");
                        }
                    }));
                }
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

            string json = JsonConvert.SerializeObject(msg) + "\n";
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

                string json = JsonConvert.SerializeObject(msg) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);

                stream.Write(data, 0, data.Length);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var history = ChatLogger.Load();

            foreach (var msg in history)
            {
                txtChatHistory.AppendText(
                    $"[{msg.Timestamp:HH:mm}] {msg.Author}: {msg.Text}\r\n");
            }

        }
    }
}
