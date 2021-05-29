using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;

namespace Server.MirObjects.Monsters
{
    /// <summary>
    /// Attack1 - Basic melee attack
    /// Attack2 - Slam Attack (DC * 3)
    /// AttackRange1 - Lightning Group Attack (find all targets within 1 range of target and hit)
    /// Special Action - Gains a Lightning Magic Shield at various stages of HP (80-70% / 50-40% / 20-0%)
    /// Special Action 2 - Hits EVERYONE in range with a thunderbolt periodically (this is only when Energy Shield is up)
    /// Summons Slaves periodically
    /// </summary>

    class GeneralJinmYo : MonsterObject
    {
        public long spawnTime;        
        public int shieldUpDuration = Settings.Second * 30;        
        public long thunderAttackTime = Math.Max(Envir.Time + Envir.Random.Next(2000), Envir.Time + Envir.Random.Next(4000));

        protected virtual byte AttackRange
        {
            get
            {
                return 12;
            }
        }

        protected internal GeneralJinmYo(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            /* Energy Shield Logic:
               When mob gets to certain health percentages (i.e. 80% / 60% / 40% / 20%) active Energy Shield.
               Whilst Energy Shield is active the following happen:
                    - This monsters damage taken is reduced (50% reduction?)
                    - Every 'x' amount of seconds all players in range are attacked with a thunderbolt
             */
            bool stage1Bubble = HP < Stats[Stat.HP] / 10 * 8 && this.HP > Stats[Stat.HP] / 10 * 7;
            bool stage2Bubble = HP < Stats[Stat.HP] / 10 * 5 && this.HP > Stats[Stat.HP] / 10 * 4;
            bool stage3Bubble = this.HP < Stats[Stat.HP] / 10 * 2 && this.HP > 1;

            if (stage1Bubble == true || stage2Bubble == true || stage3Bubble == true)
            {

                if (Target != null)
                {
                    BuffType type = BuffType.GeneralJimnyoShield;

                    int addedStat1 = 100;
                    int addedStat2 = 100;

                    if (this.Stats[Stat.MaxAC] + addedStat1 > 65535)
                    {
                        addedStat1 = 0;
                    }
                    if (this.Stats[Stat.MinAC] + addedStat2 > 65535)
                    {
                        addedStat2 = 0;
                    }

                    var stats = new Stats
                    {
                        [Stat.MaxAC] = this.Stats[Stat.MaxAC] + addedStat1,
                        [Stat.MinAC] = this.Stats[Stat.MinAC] + addedStat2
                    };
                    AddBuff(type, this, shieldUpDuration, stats, visible: true);
                    //Target.ReceiveChat($"BUFF { type } ACTIVATED! ", ChatType.System2);                                        

                    if (Envir.Time > thunderAttackTime)
                    {
                        MassThunderAttack();
                    }

                    OperateTime = 0;                    
                }
            }


            if (!ranged)
            {
                if (Envir.Random.Next(9) != 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;
                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * 3;
                    if (damage == 0) return;
                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.AC, true);
                    ActionList.Add(action);
                }
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });
                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;
                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MACAgility, true);
                ActionList.Add(action);
            }

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            // After first 60 seconds: spawn mobs, then every 60 seconds after, spawn more mobs.
            if (Target != null && Envir.Time > spawnTime)
            {
                SpawnSlaves();
                spawnTime = Envir.Time + 60000;
            }

            base.ProcessAI();
        }

        public void MassThunderAttack()
        {
            // Whilst Energy Shield is up, attack all players every few seconds.
            if (Envir.Time > thunderAttackTime)
            {
                SpellObject spellObj = null;

                List<MapObject> targets = FindAllTargets(AttackRange, Target.CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    Target = targets[Envir.Random.Next(targets.Count)];

                    Target = targets[i];

                    if (Target.IsAttackTarget(this))
                    {
                        spellObj = new SpellObject
                        {
                            Spell = Spell.GeneralJinmyoThunder,
                            Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinMC], Stats[Stat.MaxMC])),
                            ExpireTime = Envir.Time + (1000),
                            TickSpeed = 500,
                            Caster = null,
                            CurrentLocation = Target.CurrentLocation,
                            CurrentMap = CurrentMap,
                            Direction = MirDirection.Up
                        };

                        DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + 2000, spellObj);
                        CurrentMap.ActionList.Add(action);
                    }
                }
            }
        }

        public override void Spawned()
        {
            // Begin coutndown timer
            spawnTime = Envir.Time + 60000;

            base.Spawned();
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool slamDamage = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            Target.Attacked(this, damage, DefenceType.AC);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool aoe = (bool)data[3];            

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (aoe)
            {
                List<MapObject> targets = FindAllTargets(2, Target.CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    Target = targets[i];
                    if (Target.IsAttackTarget(this))
                        Target.Attacked(this, damage, DefenceType.AC);
                }
            }
        }

        private void SpawnSlaves()
        {
            int count = Math.Min(3, 6 - SlaveList.Count);

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(4))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.GeneralJinmyoMob1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.GeneralJinmyoMob2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.GeneralJinmyoMob3));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.GeneralJinmyoMob4));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }

        protected override void ProcessTarget()
        {
            if (CurrentMap.Players.Count == 0) return;

            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }
    }

}

