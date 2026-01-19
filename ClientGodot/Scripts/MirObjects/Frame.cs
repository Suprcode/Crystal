using System;
using System.IO;

namespace ClientGodot.Scripts.MirObjects
{
    public struct Frame
    {
        public short Start, Count, Skip, Interval;
        public sbyte EffectStart, EffectCount, EffectSkip, EffectInterval;
        public short Reverse;
        public sbyte Blend;

        public Frame(BinaryReader reader)
        {
            Start = reader.ReadInt16();
            Count = reader.ReadInt16();
            Skip = reader.ReadInt16();
            Interval = reader.ReadInt16();
            EffectStart = reader.ReadSByte();
            EffectCount = reader.ReadSByte();
            EffectSkip = reader.ReadSByte();
            EffectInterval = reader.ReadSByte();
            Reverse = reader.ReadInt16();
            Blend = reader.ReadSByte();
        }
    }
}
