using System.Collections.Generic;
using System.IO;

public class Notice
{
    public string Title = string.Empty;
    public string Message = string.Empty;

    public Notice() { }

    public Notice(BinaryReader reader)
    {
        Title = reader.ReadString();
        Message = reader.ReadString();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Title);
        writer.Write(Message);
    }
}