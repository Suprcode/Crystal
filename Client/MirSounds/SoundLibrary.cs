using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.DirectX.DirectSound;

namespace Client.MirSounds
{

    class SoundLibrary : IDisposable
    {
        public int Index;

        private List<SecondaryBuffer> _bufferList;

        private MemoryStream _mStream;
        private bool _loop;

        public SoundLibrary(int index, string fileName, bool loop)
        {
            Index = index;

            fileName = Path.Combine(Settings.SoundPath, fileName);
            if (!File.Exists(fileName)) return;

            _mStream = new MemoryStream(File.ReadAllBytes(fileName));

            _loop = loop;

            _bufferList = new List<SecondaryBuffer>();

            Play();
        }

        public void Play()
        {
            if (_mStream == null) return;

            _mStream.Seek(0, SeekOrigin.Begin);

            if (_loop)
            {
                if (_bufferList.Count == 0)
                    _bufferList.Add(new SecondaryBuffer(_mStream, new BufferDescription { BufferBytes = (int)_mStream.Length, ControlVolume = true }, SoundManager.Device) { Volume = SoundManager.Vol });
                else if (_bufferList[0] == null || _bufferList[0].Disposed)
                    _bufferList[0] = new SecondaryBuffer(_mStream, new BufferDescription { BufferBytes = (int)_mStream.Length, ControlVolume = true }, SoundManager.Device) { Volume = SoundManager.Vol };

                if (!_bufferList[0].Status.Playing)
                    _bufferList[0].Play(0, BufferPlayFlags.Looping);
            }
            else
            {
                for (int i = _bufferList.Count - 1; i >= 0; i--)
                {
                    if (_bufferList[i] == null || _bufferList[i].Disposed)
                    {
                        _bufferList.RemoveAt(i);
                        continue;
                    }

                    if (!_bufferList[i].Status.Playing)
                    {
                        _bufferList[i].Play(0, BufferPlayFlags.Default);
                        return;
                    }
                }

                if (_bufferList.Count >= Settings.SoundOverLap) return;

                SecondaryBuffer buffer = new SecondaryBuffer(_mStream, new BufferDescription { BufferBytes = (int)_mStream.Length, ControlVolume = true }, SoundManager.Device) { Volume = SoundManager.Vol };
                buffer.Play(0, BufferPlayFlags.Default);
                _bufferList.Add(buffer);
            }

        }
        public void Stop()
        {
            if (_bufferList == null || _bufferList.Count == 0) return;

            if (_loop)
                _bufferList[0].Dispose();
            else
            {
                for (int i = 0; i < _bufferList.Count; i++)
                    _bufferList[i].Dispose();
                _bufferList.Clear();
            }
        }

        public void Dispose()
        {
            if (_mStream != null)
                _mStream.Dispose();
            _mStream = null;

            if (_bufferList != null)
            for (int i = 0; i < _bufferList.Count; i++)
                _bufferList[i].Dispose();
            _bufferList = null;

            _loop = false;
        }

        public void SetVolume(int vol)
        {
            if (vol <= -3000) vol = -10000;

            for (int i = 0; i < _bufferList.Count; i++)
                if (_bufferList[i] != null && !_bufferList[i].Disposed)
                {
                    _bufferList[i].Volume = vol;
                }
        }
    }
}
