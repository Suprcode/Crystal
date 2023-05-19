using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Client.MirSounds.Libraries
{
    internal class NAudioLibrary : ISoundLibrary, IDisposable
    {
        public int Index { get; set; }
        public long ExpireTime { get; set; }

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        private int _unscaledVolume;
        private string _fileName;
        private bool _loop;
        private bool _isDisposing;

        public static NAudioLibrary TryCreate(int index, string fileName, int volume, bool loop)
        {
            fileName = Path.Combine(Settings.SoundPath, fileName);
            string fileType = Path.GetExtension(fileName);

            // attempt to find file
            if (String.IsNullOrEmpty(fileType))
            {
                foreach (String ext in SoundManager.SupportedFileTypes)
                {
                    if (File.Exists($"{fileName}{ext}"))
                    {
                        fileName = $"{fileName}{ext}";
                        fileType = ext;

                        break;
                    }
                }
            }

            if (SoundManager.SupportedFileTypes.Contains(fileType) &&
                File.Exists(fileName))
            {
                return new NAudioLibrary(index, fileName, volume, loop);
            }
            else
            {
                return null;
            }
        }

        public NAudioLibrary(int index, string fileName, int volume, bool loop)
        {
            Index = index;
            _loop = loop;
            _fileName = fileName;

            Play(volume);
        }

        public bool IsPlaying()
        {
            return outputDevice.PlaybackState == PlaybackState.Playing;
        }

        public void Play(int volume)
        {
            if (outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                return;
            }

            ExpireTime = CMain.Time + Settings.SoundCleanMinutes * 60 * 1000;

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += (_,_) => OutputDevice_PlaybackStopped();
            }

            if (audioFile == null)
            {
                audioFile = new AudioFileReader(_fileName);
                outputDevice.Init(audioFile);
            }

            // loop or sound already cached
            if (outputDevice?.PlaybackState == PlaybackState.Stopped)
            {
                audioFile.Seek(0, SeekOrigin.Begin);
            }

            outputDevice.Volume = ScaleVolume(volume);
            outputDevice.Play();
        }

        public void SetVolume(int vol)
        {
            outputDevice.Volume = ScaleVolume(vol);
        }

        public void Stop()
        {
            Dispose();
        }

        private void OutputDevice_PlaybackStopped()
        {
            if (_loop &&
                !_isDisposing)
            {
                    Play(_unscaledVolume);
            }
        }

        public void Dispose()
        {
            _isDisposing = true;

            outputDevice?.Dispose();
            audioFile?.Dispose();
        }

        private float ScaleVolume(int volume)
        {
            _unscaledVolume = volume;

            float scaled = 0.0f + (float)(volume - 0) / (100 - 0) * (1.0f - 0.0f);
            return scaled;
        }
    }
}
