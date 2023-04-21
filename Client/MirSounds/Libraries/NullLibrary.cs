namespace Client.MirSounds
{
    public class NullLibrary : ISoundLibrary, IDisposable
    {
        public int Index { get; set; }
        public long ExpireTime { get; set; }

        public NullLibrary(int index, string fileName, bool loop)
        {
            Index = index;
        }
        public void Dispose()
        {
            
        }

        public bool IsPlaying()
        {
            return false;
        }

        public void Play(int volume)
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
