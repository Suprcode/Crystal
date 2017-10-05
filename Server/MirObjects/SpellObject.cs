using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class SpellObject : MapObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Spell; }
        }

        public override string Name { get; set; }
        public override int CurrentMapIndex { get; set; }
        public override Point CurrentLocation { get; set; }
        public override MirDirection Direction { get; set; }
        public override ushort Level { get; set; }
        public override bool Blocking
        {
            get
            {
                return false;
            }
        }

        public long TickTime, StartTime;
        public PlayerObject Caster;
        public int Value, TickSpeed;
        public Spell Spell;
        public Point CastLocation;
        public bool Show, Decoration;

        //ExplosiveTrap
        public int ExplosiveTrapID;
        public int ExplosiveTrapCount;
        public bool DetonatedTrap;

        //Portal
        public Map ExitMap;
        public Point ExitCoord;

        public override uint Health
        {
            get { throw new NotSupportedException(); }
        }
        public override uint MaxHealth
        {
            get { throw new NotSupportedException(); }
        }


        public override void Process()
        {
            if (Decoration) return;

            if (Caster != null && Caster.Node == null) Caster = null;

            if (Envir.Time > ExpireTime || ((Spell == Spell.FireWall || Spell == Spell.Portal || Spell == Spell.ExplosiveTrap || Spell == Spell.Reincarnation) && Caster == null) || (Spell == Spell.TrapHexagon && Target != null) || (Spell == Spell.Trap && Target != null))
            {
                if (Spell == Spell.TrapHexagon && Target != null || Spell == Spell.Trap && Target != null)
                {
                    MonsterObject ob = (MonsterObject)Target;

                    if (Envir.Time < ExpireTime && ob.ShockTime != 0) return;
                }

                if (Spell == Spell.Reincarnation && Caster != null)
                {
                    Caster.ReincarnationReady = true;
                    Caster.ReincarnationExpireTime = Envir.Time + 6000;
                }

                CurrentMap.RemoveObject(this);
                Despawn();
                return;
            }

            if (Spell == Spell.Reincarnation && !Caster.ActiveReincarnation)
            {
                CurrentMap.RemoveObject(this);
                Despawn();
                return;
            }

            if (Spell == Spell.ExplosiveTrap && FindObject(Caster.ObjectID, 20) == null && Caster != null)
            {
                CurrentMap.RemoveObject(this);
                Despawn();
                return;
            }

            if (Envir.Time < TickTime) return;
            TickTime = Envir.Time + TickSpeed;

            Cell cell = CurrentMap.GetCell(CurrentLocation);
            for (int i = 0; i < cell.Objects.Count; i++)
                ProcessSpell(cell.Objects[i]);

            if ((Spell == Spell.MapLava) || (Spell == Spell.MapLightning)) Value = 0;
        }
        public void ProcessSpell(MapObject ob)
        {
            if (Envir.Time < StartTime) return;
            switch (Spell)
            {
                case Spell.FireWall:
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;

                    if (!ob.IsAttackTarget(Caster)) return;
                    ob.Attacked(Caster, Value, DefenceType.MAC, false);
                    break;
                case Spell.Healing: //SafeZone
                    if (ob.Race != ObjectType.Player && (ob.Race != ObjectType.Monster || ob.Master == null || ob.Master.Race != ObjectType.Player)) return;
                    if (ob.Dead || ob.HealAmount != 0 || ob.PercentHealth == 100) return;

                    ob.HealAmount += 25;
                    Broadcast(new S.ObjectEffect {ObjectID = ob.ObjectID, Effect = SpellEffect.Healing});
                    break;
                case Spell.PoisonCloud:
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;              

                    if (!ob.IsAttackTarget(Caster)) return;
                    ob.Attacked(Caster, Value, DefenceType.MAC, false);
                    if (!ob.Dead)
                    ob.ApplyPoison(new Poison
                        {
                            Duration = 15,
                            Owner = Caster,
                            PType = PoisonType.Green,
                            TickSpeed = 2000,
                            Value = Value/20
                        }, Caster, false, false);
                    break;
                case Spell.Blizzard:
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;
                    if (Caster != null && Caster.ActiveBlizzard == false) return;
                    if (!ob.IsAttackTarget(Caster)) return;
                    ob.Attacked(Caster, Value, DefenceType.MAC, false);
                    if (!ob.Dead && Envir.Random.Next(8) == 0)
                        ob.ApplyPoison(new Poison
                        {
                            Duration = 5 + Envir.Random.Next(Caster.Freezing),
                            Owner = Caster,
                            PType = PoisonType.Slow,
                            TickSpeed = 2000,
                        }, Caster);
                    break;
                case Spell.MeteorStrike:
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;
                    if (Caster != null && Caster.ActiveBlizzard == false) return;
                    if (!ob.IsAttackTarget(Caster)) return;
                    ob.Attacked(Caster, Value, DefenceType.MAC, false);
                    break;
                case Spell.ExplosiveTrap:
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;
                    if (!ob.IsAttackTarget(Caster)) return;
                    if (DetonatedTrap) return;//make sure explosion happens only once
                    DetonateTrapNow();
                    ob.Attacked(Caster, Value, DefenceType.MAC, false);
                    break;
                case Spell.MapLava:
                    if (ob is PlayerObject)
                    {
                        PlayerObject pOb = (PlayerObject)ob;
                        if (pOb.Account.AdminAccount && pOb.Observer)
                            return;
                    }
                    break;
                case Spell.MapLightning:
                    if (ob is PlayerObject)
                    {
                        PlayerObject pOb = (PlayerObject)ob;
                        if (pOb.Account.AdminAccount && pOb.Observer)
                            return;
                    }
                    break;
                case Spell.MapQuake1:
                case Spell.MapQuake2:
                    if (Value == 0) return;
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) return;
                    if (ob.Dead) return;
                    ob.Struck(Value, DefenceType.MAC);
                    break;

                case Spell.Portal:
                    if (ob.Race != ObjectType.Player) return;
                    if (Caster != ob && (Caster == null || (Caster.GroupMembers == null) || (!Caster.GroupMembers.Contains((PlayerObject)ob)))) return;

                    if (ExitMap == null) return;

                    MirDirection dir = ob.Direction;

                    Point newExit = Functions.PointMove(ExitCoord, dir, 1);

                    if (!ExitMap.ValidPoint(newExit)) return;

                    ob.Teleport(ExitMap, newExit, false);

                    Value = Value - 1;

                    if(Value < 1)
                    {
                        ExpireTime = Envir.Time;
                        return;
                    }
                    
                    break;
            }
        }

        public void DetonateTrapNow()
        {
            DetonatedTrap = true;
            Broadcast(GetInfo());
            ExpireTime = Envir.Time + 1000;
        }

        public override void SetOperateTime()
        {
            long time = Envir.Time + 2000;

            if (TickTime < time && TickTime > Envir.Time)
                time = TickTime;

            if (OwnerTime < time && OwnerTime > Envir.Time)
                time = OwnerTime;

            if (ExpireTime < time && ExpireTime > Envir.Time)
                time = ExpireTime;

            if (PKPointTime < time && PKPointTime > Envir.Time)
                time = PKPointTime;

            if (LastHitTime < time && LastHitTime > Envir.Time)
                time = LastHitTime;

            if (EXPOwnerTime < time && EXPOwnerTime > Envir.Time)
                time = EXPOwnerTime;

            if (BrownTime < time && BrownTime > Envir.Time)
                time = BrownTime;

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (ActionList[i].Time >= time && ActionList[i].Time > Envir.Time) continue;
                time = ActionList[i].Time;
            }

            for (int i = 0; i < PoisonList.Count; i++)
            {
                if (PoisonList[i].TickTime >= time && PoisonList[i].TickTime > Envir.Time) continue;
                time = PoisonList[i].TickTime;
            }

            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].ExpireTime >= time && Buffs[i].ExpireTime > Envir.Time) continue;
                time = Buffs[i].ExpireTime;
            }


            if (OperateTime <= Envir.Time || time < OperateTime)
                OperateTime = time;
        }

        public override void Process(DelayedAction action)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            throw new NotSupportedException();
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            throw new NotSupportedException();
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(PlayerObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            throw new NotSupportedException();
        }
        public override void ReceiveChat(string text, ChatType type)
        {
            throw new NotSupportedException();
        }

        public override Packet GetInfo()
        {
            switch (Spell)
            {
                case Spell.Healing:
                    return null;
                case Spell.PoisonCloud:
                case Spell.Blizzard:
                case Spell.MeteorStrike:
                    if (!Show)
                        return null;

                    return new S.ObjectSpell
                    {
                        ObjectID = ObjectID,
                        Location = CastLocation,
                        Spell = Spell,
                        Direction = Direction
                    };
                case Spell.ExplosiveTrap:
                    return new S.ObjectSpell
                    {
                        ObjectID = ObjectID,
                        Location = CurrentLocation,
                        Spell = Spell,
                        Direction = Direction,
                        Param = DetonatedTrap
                    };

                default:
                    return new S.ObjectSpell
                    {
                        ObjectID = ObjectID,
                        Location = CurrentLocation,
                        Spell = Spell,
                        Direction = Direction
                    };
            }

        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            throw new NotSupportedException();
        }
        public override void Die()
        {
            throw new NotSupportedException();
        }
        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            throw new NotSupportedException();
        }
        public override void SendHealth(PlayerObject player)
        {
            throw new NotSupportedException();
        }
        public override void Despawn()
        {
            base.Despawn();

            if (Spell == Spell.Reincarnation && Caster != null && Caster.Node != null)
            {
                Caster.ActiveReincarnation = false;
                Caster.Enqueue(new S.CancelReincarnation { });
            }

            if (Spell == Spell.ExplosiveTrap && Caster != null)
                Caster.ExplosiveTrapDetonated(ExplosiveTrapID, ExplosiveTrapCount);

            if (Spell == Spell.Portal && Caster != null)
            {
                if (Caster.PortalObjectsArray[0] == this)
                {
                    Caster.PortalObjectsArray[0] = null;

                    if (Caster.PortalObjectsArray[1] != null)
                    {
                        Caster.PortalObjectsArray[1].ExpireTime = 0;
                        Caster.PortalObjectsArray[1].Process();
                    }
                }
                else
                {
                    Caster.PortalObjectsArray[1] = null;
                }
            }
        }

        public override void BroadcastInfo()
        {
            if ((Spell != Spell.ExplosiveTrap) || (Caster == null))
                base.BroadcastInfo();
            Packet p;
            if (CurrentMap == null) return;

            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];
                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                {
                    if ((Caster == null) || (player == null)) continue;
                    if ((player == Caster) || (player.IsFriendlyTarget(Caster)))
                    {
                        p = GetInfo();
                        if (p != null)
                            player.Enqueue(p);
                    }
                }
            }
        }
    }
}
