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

    public class ConquestSiegeObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestSiegeInfo Info;

        public ConquestObject Conquest;

        public SabukGate Gate;


        public ConquestSiegeObject()
        {

        }
        public ConquestSiegeObject(PlayerObject owner, string name)
        {

        }
        public ConquestSiegeObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            //if (Gate != null) Health = Gate.HP; - needs adding
            writer.Write(Index);
            writer.Write(Health);
        }


        public void Spawn()
        {
            if (Gate != null) Gate.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 72) return;

            Gate = (SabukGate)MonsterObject.GetMonster(monsterInfo);

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

            if (Gate.Dead) Gate.Revive(Gate.MaxHP, false);
            else
                Gate.HP = Gate.MaxHP;

            Gate.CheckDirection();


        }
    }
}