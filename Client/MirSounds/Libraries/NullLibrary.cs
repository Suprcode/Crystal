using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirSounds
{
    public class NullLibrary : ISoundLibrary, IDisposable
    {
        public int Index { get; set; }
        public NullLibrary(int index, string fileName, bool loop)
        {
            Index = index;
        }
        public void Dispose()
        {
            
        }

        public void Play()
        {
            
        }

        public void SetVolume(int vol)
        {

        }

        public void Stop()
        {

        }
    }
}
