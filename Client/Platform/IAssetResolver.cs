using System.IO;

namespace Client.Platform
{
    public interface IAssetResolver
    {
        string Resolve(string path);
        bool Exists(string path);
        byte[] ReadAllBytes(string path);
        Stream OpenRead(string path);
        string ResolveSound(string path); // Returns transcoded path if needed (.wma -> .ogg)
    }
}
