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

    public class ConquestWallObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestWallInfo Info;

        public ConquestObject Conquest;

        public Wall Wall;


        public ConquestWallObject()
        {

        }
        public ConquestWallObject(PlayerObject owner, string name)
        {

        }
        public ConquestWallObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            if (Wall != null) Health = Wall.HP;
            writer.Write(Index);
            writer.Write(Wall.Health);
        }


        public void Spawn()
        {
            if (Wall != null) Wall.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;

            if (monsterInfo.AI != 82) return;

            Wall = (Wall)MonsterObject.GetMonster(monsterInfo);

            if (Wall == null) return;

            Wall.Conquest = Conquest;
            Wall.WallIndex = Index;

            Wall.Spawn(Conquest.ConquestMap, Info.Location);

            if (Health == 0)
                Wall.Die();
            else
                Wall.SetHP(Health);

            Wall.CheckDirection();
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (Wall != null)
            {
                cost = Info.RepairCost / (Wall.MaxHP / (Wall.MaxHP - Wall.HP));
            }

            return cost;
        }

        public void Repair()
        {
            if (Wall == null)
            {
                Spawn();
                return;
            }

            if (Wall.Dead)
                Spawn();
            else
                Wall.HP = Wall.MaxHP;

            Wall.CheckDirection();
        }
    }
}