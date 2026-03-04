using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
            this.FormClosing += Form1_FormClosing;

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                username = txtUsername.Text.Trim();

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Введите имя пользователя!");
                    return;
                }

                client = new TcpClient();
                await client.ConnectAsync(txtIP.Text, int.Parse(txtPort.Text));

                stream = client.GetStream();

                MessageBox.Show("Подключено к серверу!");

                var joinMessage = new Message
                {
                    Author = username,
                    Text = "",
                    Timestamp = DateTime.Now
                };

                string json = JsonConvert.SerializeObject(joinMessage) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                await stream.WriteAsync(data, 0, data.Length);

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

            while (client != null && client.Connected)
            {
                int bytesRead;

                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch
                {
                    break;
                }

                if (bytesRead <= 0)
                    continue;

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                while (true)
                {
                    string data = sb.ToString();
                    int index = data.IndexOf("\n");

                    if (index < 0)
                        break;

                    string json = data.Substring(0, index);
                    sb.Remove(0, index + 1);

                    Message msg;

                    try
                    {
                        msg = JsonConvert.DeserializeObject<Message>(json);
                    }
                    catch
                    {
                        continue;
                    }

                    if (msg == null)
                        continue;

                    if (!string.IsNullOrEmpty(msg.Text) && msg.Text.StartsWith("__USERLIST__"))
                    {
                        Invoke(new Action(() =>
                        {
                            lstUsers.Items.Clear();

                            string usersData = msg.Text.Replace("__USERLIST__", "");

                            if (!string.IsNullOrWhiteSpace(usersData))
                            {
                                foreach (var user in usersData.Split('|'))
                                {
                                    if (!string.IsNullOrWhiteSpace(user))
                                        lstUsers.Items.Add(user);
                                }
                            }
                        }));

                        continue;
                    }

                    Invoke(new Action(() =>
                    {
                        if (!string.IsNullOrEmpty(msg.Text))
                        {
                            txtChatHistory.AppendText(
                                $"[{msg.Timestamp:HH:mm}] {(string.IsNullOrEmpty(msg.Author) ? "Unknown" : msg.Author)}: {msg.Text}\r\n");
                        }
                        else if (msg.FileData != null && msg.FileData.Length > 0)
                        {
                            string path = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                msg.FileName ?? "file.dat");

                            try
                            {
                                File.WriteAllBytes(path, msg.FileData);
                            }
                            catch { }

                            txtChatHistory.AppendText(
                                $"[{msg.Timestamp:HH:mm}] Получен файл: {msg.FileName}\r\n");
                        }
                    }));
                }
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Нет подключения к серверу!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
                return;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Имя пользователя не задано!");
                return;
            }

            Message msg = new Message()
            {
                Author = username,
                Text = txtMessage.Text,
                Timestamp = DateTime.Now,
                Receiver = lstUsers.SelectedItem?.ToString()
            };

            try
            {
                string json = JsonConvert.SerializeObject(msg) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);

                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                }
            }
            catch { }
        }
    }
}
