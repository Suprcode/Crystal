using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared
{
    public class LogNotice
    {
        public string Title = string.Empty;
        public string LogString = string.Empty;
        public string Link = string.Empty;
        public LogNotice() { }

        public LogNotice(BinaryReader reader)
        {
            Title = reader.ReadString();
            LogString = reader.ReadString();
            Link = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Title);
            writer.Write(LogString);
            writer.Write(Link);
        }
    }

}