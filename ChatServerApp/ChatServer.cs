using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServer
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();
    private Dictionary<TcpClient, string> users = new Dictionary<TcpClient, string>();

    public async Task Start(string ip, int port)
    {
        listener = new TcpListener(IPAddress.Parse(ip), port);
        listener.Start();

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            clients.Add(client);

            _ = HandleClient(client);
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        NetworkStream stream = null;

        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[4096];
            StringBuilder sb = new StringBuilder();

            while (client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                    break;

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                while (true)
                {
                    string full = sb.ToString();
                    int index = full.IndexOf("\n");

                    if (index < 0)
                        break;

                    string json = full.Substring(0, index);
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

                    if (!users.ContainsKey(client))
                    {
                        users[client] = msg.Author;

                        await Broadcast(new Message
                        {
                            Author = "Server",
                            Text = $"{msg.Author} вошёл в чат"
                        });

                        await BroadcastUserList();
                    }

                    await Broadcast(msg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client error: " + ex.Message);
        }
        finally
        {
            if (users.ContainsKey(client))
            {
                string userName = users[client];
                users.Remove(client);

                await Broadcast(new Message
                {
                    Author = "Server",
                    Text = $"{userName} вышел из чата"
                });

                await BroadcastUserList();
            }

            clients.Remove(client);

            try { client.Close(); } catch { }
        }
    }

    private async Task Broadcast(Message message)
    {
        Console.WriteLine("SAVE TEST");
        ChatLogger.Save(message);


        if (message == null)
            return;

        if (string.IsNullOrWhiteSpace(message.Author))
            message.Author = "Unknown";

        ChatLogger.Save(message);

        string json = JsonConvert.SerializeObject(message) + "\n";
        byte[] data = Encoding.UTF8.GetBytes(json);

        List<TcpClient> disconnected = new List<TcpClient>();

        foreach (var client in clients)
        {
            try
            {
                await client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch
            {
                disconnected.Add(client);
            }
        }

        foreach (var dc in disconnected)
        {
            clients.Remove(dc);
            dc.Close();
        }
    }
    private async Task BroadcastUserList()
    {
        var userListMessage = new Message
        {
            Author = "Server",
            Text = "__USERLIST__" + string.Join("|", users.Values),
            Timestamp = DateTime.Now
        };

        string json = JsonConvert.SerializeObject(userListMessage) + "\n";
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