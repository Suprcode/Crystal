using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Server.MirEnvir;
using System.Drawing;
using Server.MirDatabase;
using Server.MirObjects.Monsters;

namespace Server.MirObjects
{

    public class ConquestArcherObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public bool Alive;

        public ConquestArcherInfo Info;

        public ConquestObject Conquest;

        public ConquestArcher ArcherMonster;


        public ConquestArcherObject()
        {

        }
        public ConquestArcherObject(PlayerObject owner, string name)
        {

        }
        public ConquestArcherObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Alive = reader.ReadBoolean();
        }
        public void Save(BinaryWriter writer)
        {
            if (ArcherMonster == null || ArcherMonster.Dead) Alive = false;
            else Alive = true;
            writer.Write(Index);
            writer.Write(Alive);
        }


        public void Spawn(bool Revive = false)
        {
            if (Revive) Alive = true;

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 80) return;

            ArcherMonster = (ConquestArcher)MonsterObject.GetMonster(monsterInfo);

            if (ArcherMonster == null) return;

            ArcherMonster.Conquest = Conquest;
            ArcherMonster.ArcherIndex = Index;

            if (Alive)
                ArcherMonster.Spawn(Conquest.ConquestMap, Info.Location);
            else
            {
                ArcherMonster.Spawn(Conquest.ConquestMap, Info.Location);
                ArcherMonster.Die();
                ArcherMonster.DeadTime = Envir.Time;
            }
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (ArcherMonster == null || ArcherMonster.Dead)
                cost = Info.RepairCost;

            return cost;
        }
    }
}