namespace Client.MirSounds
{
    public interface ISoundLibrary
    {
        int Index { get; set; }
        long ExpireTime { get; set; }

        bool IsPlaying();
        void Play();
        void Stop();
        void SetVolume(int vol);

        void Dispose();
    }
}
