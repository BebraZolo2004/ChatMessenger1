using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public static class ChatLogger
{
    private static string filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chatlog.json");

    private static readonly object locker = new object();

    public static void Save(Message message)
    {
        if (message == null)
            return;

        lock (locker)
        {
            try
            {
                List<Message> list = Load();
                list.Add(message);

                File.WriteAllText(filePath,
                    JsonConvert.SerializeObject(list, Formatting.Indented));
            }
            catch { }
        }
    }

    public static List<Message> Load()
    {
        try
        {
            if (!File.Exists(filePath))
                return new List<Message>();

            string json = File.ReadAllText(filePath);

            var list = JsonConvert.DeserializeObject<List<Message>>(json);
            return list ?? new List<Message>();
        }
        catch
        {
            return new List<Message>();
        }
    }
}