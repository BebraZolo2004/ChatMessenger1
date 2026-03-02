using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class ChatLogger
{
    private static string filePath = "chatlog.json";

    public static void Save(Message message)
    {
        List<Message> list = Load();

        list.Add(message);

        File.WriteAllText(filePath,
            JsonConvert.SerializeObject(list, Formatting.Indented));
    }

    public static List<Message> Load()
    {
        if (!File.Exists(filePath))
            return new List<Message>();

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Message>>(json);
    }
}