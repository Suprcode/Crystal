using NAudio.Wave;

namespace Client.MirSounds.Libraries {
    internal class OneShotProvider : ISampleProvider {
        private readonly CachedSound cachedSound;
        private long position;

        public OneShotProvider(CachedSound cachedSound) {
            this.cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count) {
            long availableSamples = cachedSound.AudioData.Length - position;
            long samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat => cachedSound.WaveFormat;
    }
}
