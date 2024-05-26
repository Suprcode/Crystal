using NAudio.Wave;

namespace Client.MirSounds.Libraries {
    internal class CachedSound {
        public int Index { get; private set; }
        public long ExpireTime { get; set; }
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public CachedSound(int index, string fileName) {
            Index = index;

            fileName = Path.Combine(Settings.SoundPath, fileName);
            string fileType = Path.GetExtension(fileName);

            // attempt to find file
            if(String.IsNullOrEmpty(fileType)) {
                foreach(String ext in SoundManager.SupportedFileTypes) {
                    if(File.Exists($"{fileName}{ext}")) {
                        fileName = $"{fileName}{ext}";
                        fileType = ext;

                        break;
                    }
                }
            }

            if(SoundManager.SupportedFileTypes.Contains(fileType) &&
               File.Exists(fileName)) {
                using(AudioFileReader audioFileReader = new AudioFileReader(fileName)) {
                    WaveFormat = audioFileReader.WaveFormat;
                    List<float> wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                    float[] readBuffer =
                        new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }

                    AudioData = wholeFile.ToArray();
                }
            }
        }
    }
}
