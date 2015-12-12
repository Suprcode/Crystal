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

    public class ConquestGateObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestGateInfo Info;

        public ConquestObject Conquest;

        public Gate Gate;


        public ConquestGateObject()
        {

        }
        public ConquestGateObject(PlayerObject owner, string name)
        {

        }
        public ConquestGateObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            if (Gate != null) Health = Gate.HP;
            writer.Write(Index);
            writer.Write(Health);
        }


        public void Spawn()
        {
            if (Gate != null) Gate.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 81) return;

            Gate = (Gate)MonsterObject.GetMonster(monsterInfo);

            if (Gate == null) return;

            Gate.Conquest = Conquest;
            Gate.GateIndex = Index;

            Gate.Spawn(Conquest.ConquestMap, Info.Location);

            if (Health == 0)
                Gate.Die();
            else
                Gate.SetHP(Health);

            Gate.CheckDirection();
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (Gate != null)
            {
                if (Info.RepairCost != 0 && Gate.MaxHP != Gate.HP)
                cost = Info.RepairCost / (Gate.MaxHP / (Gate.MaxHP - Gate.HP));
            }
            return cost;
        }

        public void Repair()
        {
            if (Gate == null)
            {
                Spawn();
                return;
            }

            if (Gate.Dead)
                Spawn();
            else
                Gate.HP = Gate.MaxHP;

            Gate.CheckDirection();
          

        }
    }
}