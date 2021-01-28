namespace Client.MirSounds
{
    public interface ISoundLibrary
    {
        int Index { get; set; }
        void Play();
        void Stop();
        void SetVolume(int vol);

        void Dispose();
    }
}
