using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ChatServer
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();
    private Dictionary<TcpClient, string> users = new Dictionary<TcpClient, string>();

    public async void Start(string ip, int port)
    {
        listener = new TcpListener(IPAddress.Parse(ip), port);
        listener.Start();

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            clients.Add(client);

            _ = Task.Run(() => HandleClient(client));
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                while (sb.ToString().Contains("\n"))
                {
                    string full = sb.ToString();
                    int index = full.IndexOf("\n");

                    string json = full.Substring(0, index);
                    sb.Remove(0, index + 1);

                    Message msg = JsonConvert.DeserializeObject<Message>(json);
                    if (msg == null) continue;

                    if (!users.ContainsKey(client))
                    {
                        users[client] = msg.Author;

                        await Broadcast(new Message
                        {
                            Author = "Server",
                            Text = $"{msg.Author} вошёл в чат"
                        });
                    }

                    await Broadcast(msg); // ВСЕМ включая отправителя
                }
            }
        }
        catch { }

        if (users.ContainsKey(client))
        {
            string user = users[client];
            users.Remove(client);

            await Broadcast(new Message
            {
                Author = "Server",
                Text = $"{user} вышел из чата"
            });
        }

        clients.Remove(client);
        client.Close();
    }

    private async Task Broadcast(Message message)
    {
        ChatLogger.Save(message);

        string json = JsonConvert.SerializeObject(message) + "\n";
        byte[] data = Encoding.UTF8.GetBytes(json);

        foreach (var client in clients)
        {
            try
            {
                await client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch { }
        }
    }
}