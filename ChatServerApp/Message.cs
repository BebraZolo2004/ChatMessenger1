using System;

[Serializable]
public class Message
{
    public string Author { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    public string Receiver { get; set; }

    public byte[] FileData { get; set; }
    public string FileName { get; set; }

    public Message()
    {
        Timestamp = DateTime.Now;
    }
}