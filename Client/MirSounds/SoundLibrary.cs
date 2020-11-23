using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

namespace Client.MirSounds
{
    public class SoundLibrary : IDisposable
    {
        public int Index;

        private List<SecondarySoundBuffer> _bufferList;
        private SoundStream _stream;

        private bool _loop;

        private SoundBufferDescription _desc;
        private readonly byte[] _data;

        public SoundLibrary(int index, string fileName, bool loop)
        {
            Index = index;

            fileName = Path.Combine(Settings.SoundPath, fileName);
            if (!File.Exists(fileName)) return;

            _stream = new SoundStream(File.OpenRead(fileName));

            _desc = new SoundBufferDescription
            {
                BufferBytes = (int)_stream.Length,
                Flags = BufferFlags.ControlVolume | BufferFlags.ControlPan | BufferFlags.GlobalFocus,
                Format = _stream.Format
            };

            _data = new byte[_desc.BufferBytes];
            _stream.Read(_data, 0, (int)_stream.Length);

            _loop = loop;

            _bufferList = new List<SecondarySoundBuffer>();

            Play();
        }

        public void Play()
        {
            if (_stream == null) return;

            if (_loop)
            {
                if (_bufferList.Count == 0)
                {
                    SecondarySoundBuffer buffer = new SecondarySoundBuffer(SoundManager.Device, _desc) { Volume = SoundManager.Vol };
                    buffer.Write(_data, 0, LockFlags.None);
                    _bufferList.Add(buffer);
                }
                else if (_bufferList[0] == null || _bufferList[0].IsDisposed)
                {
                    SecondarySoundBuffer buffer = new SecondarySoundBuffer(SoundManager.Device, _desc) { Volume = SoundManager.Vol };
                    buffer.Write(_data, 0, LockFlags.None);
                    _bufferList[0] = buffer;
                }

                if ((BufferStatus)_bufferList[0].Status != BufferStatus.Playing)
                    _bufferList[0].Play(0, PlayFlags.Looping);
            }
            else
            {
                for (int i = _bufferList.Count - 1; i >= 0; i--)
                {
                    if (_bufferList[i] == null || _bufferList[i].IsDisposed)
                    {
                        _bufferList.RemoveAt(i);
                        continue;
                    }

                    if ((BufferStatus)_bufferList[i].Status != BufferStatus.Playing)
                    {
                        _bufferList[i].Volume = SoundManager.Vol;
                        _bufferList[i].Play(0, 0);
                        return;
                    }
                }

                if (_bufferList.Count >= Settings.SoundOverLap) return;

                SecondarySoundBuffer buffer = new SecondarySoundBuffer(SoundManager.Device, _desc) { Volume = SoundManager.Vol, Pan = 0 };
                buffer.Write(_data, 0, LockFlags.None);
                buffer.Play(0, 0);
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
            if (_stream != null)
                _stream.Dispose();
            _stream = null;

            if (_bufferList != null)
                for (int i = 0; i < _bufferList.Count; i++)
                    _bufferList[i].Dispose();
            _bufferList = null;

            _loop = false;
        }

        public void SetVolume(int vol)
        {
            if (_bufferList == null) return;
            if (vol <= -3000) vol = -10000;

            for (int i = 0; i < _bufferList.Count; i++)
                if (_bufferList[i] != null && !_bufferList[i].IsDisposed)
                {
                    _bufferList[i].Volume = vol;
                }
        }
    }
}
