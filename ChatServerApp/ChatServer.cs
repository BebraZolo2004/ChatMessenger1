using System;
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
        string username = "";

        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Message msg = JsonConvert.DeserializeObject<Message>(json);

                if (!users.ContainsKey(client))
                {
                    users[client] = msg.Author;

                    await Broadcast(new Message
                    {
                        Author = "Server",
                        Text = $"{msg.Author} вошёл в чат"
                    }, null);
                }

                await Broadcast(msg, client);
            }
        }
        catch { }

        finally
        {
            if (users.ContainsKey(client))
            {
                string user = users[client];
                users.Remove(client);

                await Broadcast(new Message
                {
                    Author = "Server",
                    Text = $"{user} вышел из чата"
                }, null);
            }

            clients.Remove(client);
            client.Close();
        }
    }

    private async Task Broadcast(Message message, TcpClient sender)
    {
        string json = JsonConvert.SerializeObject(message);
        byte[] data = Encoding.UTF8.GetBytes(json);

        foreach (var client in clients)
        {
            if (client == sender) continue;

            try
            {
                await client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch { }
        }
    }
}