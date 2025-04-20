using System.Drawing;
﻿using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects.Monsters;
using System.Diagnostics.Eventing.Reader;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class MonsterObject : MapObject
    {
        public static MonsterObject GetMonster(MonsterInfo info)
        {
            if (info == null) return null;

            switch (info.AI)
            {
                case 1:
                case 2:
                    return new Deer(info);
                case 3:
                    return new Tree(info);
                case 4:
                    //Common AI: 1 Line Attack with Poison
                    return new SpittingSpider(info);
                case 5:
                    return new CannibalPlant(info);
                case 6:
                    return new Guard(info);
                case 7:
                    return new CaveMaggot(info);
                case 8:
                    //Common AI: 1 Range Projectile Attack with Fear
                    return new AxeSkeleton(info);
                case 9:
                    return new HarvestMonster(info);
                case 10:
                    //Common AI: 1 Magic Attack
                    return new FlamingWooma(info);
                case 11:
                    return new WoomaTaurus(info);
                case 12:
                    return new BugBagMaggot(info);
                case 13:
                    return new RedMoonEvil(info);
                case 14:
                    return new EvilCentipede(info);
                case 15:
                    return new ZumaMonster(info);
                case 16:
                    return new RedThunderZuma(info);
                case 17:
                    return new ZumaTaurus(info);
                case 18:
                    return new Shinsu(info);
                case 19:
                    return new KingScorpion(info);
                case 20:
                    return new DarkDevil(info);
                case 21:
                    return new IncarnatedGhoul(info);
                case 22:
                    return new IncarnatedZT(info);
                case 23:
                    return new BoneFamiliar(info);
                case 24:
                    return new DigOutZombie(info);
                case 25:
                    return new RevivingZombie(info);
                case 26:
                    return new ShamanZombie(info);
                case 27:
                    return new Khazard(info);
                case 28:
                    return new ToxicGhoul(info);
                case 29:
                    //Common AI: 1 Line Attack
                    return new BoneSpearman(info);
                case 30:
                    return new BoneLord(info);
                case 31:
                    //Common AI: 2 Magic Attacks, 1 Close, 1 Range
                    return new RightGuard(info);
                case 32:
                    //Common AI: 2 Magic Attacks, 1 Close, 1 Range Projectile
                    return new LeftGuard(info);
                case 33:
                    return new MinotaurKing(info);
                case 34:
                    return new FrostTiger(info); //Effect 0/1
                case 35:
                    //Common AI: 1 Line Attack
                    return new SandWorm(info);
                case 36:
                    return new Yimoogi(info);
                case 37:
                    return new CrystalSpider(info);
                case 38:
                    return new HolyDeva(info);
                case 39:
                    return new RootSpider(info);
                case 40:
                    return new BombSpider(info);
                case 41:
                case 42:
                    return new YinDevilNode(info);
                case 43:
                    return new OmaKing(info);
                case 44:
                    //Common AI: 2 Attacks, 1 Close, 1 Line Attack
                    return new BlackFoxman(info);
                case 45:
                    return new RedFoxman(info);
                case 46:
                    return new WhiteFoxman(info);
                case 47:
                    return new TrapRock(info);
                case 48:
                    return new GuardianRock(info);
                case 49:
                    return new ThunderElement(info);
                case 50:
                    return new GreatFoxSpirit(info);
                case 51:
                    //Common AI: 2 Physical Attacks, 1 Close, 1 Range
                    return new HedgeKekTal(info);
                case 52:
                    return new EvilMir(info);
                case 53:
                    return new EvilMirBody(info);
                case 54:
                    return new DragonStatue(info);
                case 55:
                    return new HumanWizard(info);
                case 56:
                    return new Trainer(info);
                case 57:
                    return new TownArcher(info);
                case 58:
                    return new Guard(info);
                case 59:
                    return new HumanAssassin(info);
                case 60:
                    return new VampireSpider(info); //TODO - Clean up
                case 61:
                    return new SpittingToad(info);
                case 62:
                    return new SnakeTotem(info);
                case 63:
                    return new CharmedSnake(info);
                case 64:
                    return new IntelligentCreatureObject(info);
                case 65:
                    //Common AI: 2 Close attacks with WeakerTeleport
                    return new MutatedManworm(info);
                case 66:
                    //Common AI: 2 Close Attacks
                    return new CrazyManworm(info);
                case 67:
                    return new DarkDevourer(info);
                case 68:
                    return new Football(info);
                case 69:
                    return new PoisonHugger(info);
                case 70:
                    return new Hugger(info);
                case 71:
                    return new Behemoth(info);
                case 72:
                    return new FinialTurtle(info);
                case 73:
                    return new TurtleKing(info);
                case 74:
                    return new LightTurtle(info);
                case 75:
                    return new WitchDoctor(info);
                case 76:
                    //Common AI: 2 Close Attacks, 1 Normal, 1 Halfmoon
                    return new HellSlasher(info);
                case 77:
                    //Common AI: 2 Close Attacks, 1 Normal, 1 Fullmoon
                    return new HellPirate(info);
                case 78:
                    return new HellCannibal(info);
                case 79:
                    return new HellKeeper(info);
                case 80:
                    return new ConquestArcher(info);
                case 81:
                    return new Gate(info);
                case 82:
                    return new Wall(info);
                case 83:
                    return new Tornado(info);
                case 84:
                    return new WingedTigerLord(info);
                case 85:
                    return new FlamingMutant(info);
                case 86:
                    return new ManectricClaw(info);
                case 87:
                    return new ManectricBlest(info);
                case 88:
                    return new ManectricKing(info);
                case 89:
                    return new IcePillar(info);
                case 90:
                    return new TrollBomber(info);
                case 91:
                    return new TrollKing(info);
                case 92:
                    //Common AI: 2 Attacks with Fear, 1 Normal, 1 Long Line
                    return new FlameSpear(info);
                case 93:
                    //Common AI: 2 Magic Attacks with Fear, 1 Close, 1 Range AOE
                    return new FlameMage(info);
                case 94:
                    //Common AI: 2 Magic Attacks with Fear, 1 Close, 1 Close AOE
                    return new FlameScythe(info);
                case 95:
                    return new FlameAssassin(info);
                case 96:
                    return new FlameQueen(info);
                case 97:
                    return new HellKnight(info);
                case 98:
                    return new HellLord(info);
                case 99:
                    return new HellBomb(info);
                case 100:
                    //Common AI: 1 Magic Line Attack with Poison
                    return new VenomSpider(info);
                case 101:
                    return new AncientBringer(info);
                case 102:
                    return new IceGuard(info);
                case 103:
                    return new ElementGuard(info);
                case 104:
                    return new DemonGuard(info);
                case 105:
                    return new KingGuard(info);
                case 106:
                    return new DeathCrawler(info);
                case 107:
                    //Common AI: 2 Magic Attacks with Rush, 1 Close, 1 Range
                    return new BurningZombie(info);
                case 108:
                    return new MudZombie(info);
                case 109:
                    return new HardenRhino(info);
                case 110:
                    return new DemonWolf(info); //Effect 0/1
                case 111:
                    return new WhiteMammoth(info);
                case 112:
                    //Common AI: 2 Close attacks
                    return new DarkBeast(info); //Effect 0/1
                case 113:
                    return new ArcherGuard(info);
                case 114:
                    //Common AI: 1 Close attack with WeakerTeleport
                    return new Mandrill(info);
                case 115:
                    return new SandSnail(info);
                case 116:
                    return new BlackHammerCat(info);
                case 117:
                    return new StrayCat(info);
                case 118:
                    return new CatShaman(info);
                case 119:
                    return new Jar1(info);
                case 120:
                    return new Jar2(info);
                case 121:
                    return new SeedingsGeneral(info);
                case 122:
                    return new RestlessJar(info);
                case 123:
                    return new GeneralMeowMeow(info);
                case 124:
                    return new Armadillo(info);
                case 125:
                    return new ArmadilloElder(info);
                case 126:
                    return new TucsonMage(info);
                case 127:
                    return new TucsonWarrior(info);
                case 128:
                    return new TucsonEgg(info); //Effect 0/1
                case 129:
                    return new SwampWarrior(info);
                case 130:
                    return new CannibalTentacles(info);
                case 131:
                    return new TucsonGeneral(info);
                case 132:
                    return new GasToad(info);
                case 133:
                    return new Mantis(info);
                case 134:
                    return new AssassinBird(info);
                case 135:
                    return new StoningStatue(info);
                case 136:
                    return new FlyingStatue(info);
                case 137:
                    return new RhinoPriest(info);
                case 138:
                    return new ElephantMan(info);
                case 139:
                    return new StoneGolem(info);
                case 140:
                    return new EarthGolem(info);
                case 141:
                    return new TreeGuardian(info);
                case 142:
                    return new TreeQueen(info);
                case 143:
                    return new PeacockSpider(info);
                case 144:
                    return new OmaCannibal(info);
                case 145:
                    //Common AI: 2 Attacks, 1 Close, 1 Close AOE
                    return new OmaBlest(info);
                case 146:
                    //Common AI: 1 Halfmoon Attack
                    return new OmaSlasher(info);
                case 147:
                    return new OmaMage(info);
                case 148:
                    return new OmaWitchDoctor(info);
                case 149:
                    return new PowerBead(info); //Effect 0/1/2
                case 150:
                    return new DarkOmaKing(info);
                case 151:
                    return new CaveStatue(info);
                case 152:
                    return new PlagueCrab(info);
                case 153:
                    return new CreeperPlant(info);
                case 154:
                    return new Nadz(info);
                case 155:
                    return new AvengingSpirit(info);
                case 156:
                    return new AvengingWarrior(info);
                case 157:
                    return new AxePlant(info);
                case 158:
                    //Common AI: None With Attack On Death
                    return new WoodBox(info);
                case 159:
                    return new DarkCaptain(info);
                case 160:
                    //Common AI: 1 Range Attack with Fear
                    return new BlueSoul(info);
                case 161:
                    return new SackWarrior(info);
                case 162:
                    return new KingHydrax(info);
                case 163:
                    return new HornedMage(info);
                case 164:
                    return new HornedArcher(info); //Effect 0/1
                case 165:
                    return new HornedWarrior(info);
                case 166:
                    return new FloatingRock(info);
                case 167:
                    return new ScalyBeast(info);
                case 168:
                    return new WereTiger(info);
                case 169:
                    return new HornedSorceror(info);
                case 170:
                    return new BoulderSpirit(info);
                case 171:
                    return new HornedCommander(info);

                //case 172: MoonSunLightningStone

                case 173:
                    return new TurtleGrass(info);
                case 174:
                    return new ManTree(info);
                case 175:
                    return new ChieftainArcher(info);

                //case 176: ChieftainSword

                case 177:
                    return new FrozenKnight(info);
                case 178:
                    return new IcePhantom(info); //TODO
                case 179:
                    return new SnowWolf(info);
                case 180:
                    return new SnowWolfKing(info);
                case 181:
                    return new WaterDragon(info);
                case 182:
                    return new BlackTortoise(info);

                //case 183: Manticore

                case 184:
                    return new DragonWarrior(info); //TODO

                //case 185: DragonArcher

                case 186:
                    return new Kirin(info);
                case 187:
                    return new FrozenMiner(info);
                case 188:
                    return new FrozenAxeman(info);
                case 189:
                    return new FrozenMagician(info);
                case 190:
                    return new SnowYeti(info);
                case 191:
                    return new IceCrystalSoldier(info);
                case 192:
                    return new DarkWraith(info);

                //case 193: CrystalBeast
                //case 194: RedOrb
                //case 195: FatalLotus

                case 196:
                    return new AntCommander(info);


                // Sanjian

                case 197:
                    return new GlacierSnail(info);
                case 198:
                    return new FurbolgWarrior(info);
                case 199:
                    return new FurbolgArcher(info);
                case 200:
                    return new FurbolgCommander(info);
                case 201:
                    return new FurbolgGuard(info);
                case 202:
                    return new GlacierBeast(info);
                case 203:
                    return new GlacierWarrior(info);



                case 210:
                    return new HoodedSummonerScrolls(info);
                case 211:
                    return new HoodedSummoner(info);
                case 212:
                    return new PurpleFaeFlower(info);
                case 213:
                    return new Siege(info); //TODO

                case 214:
                    return new SepWarrior(info); //TODO
                case 215:
                    return new SepWizard(info); //TODO
                case 216:
                    return new SepTaoist(info); //TODO
                case 217:
                    return new SepAssassin(info); //TODO
                case 218:
                    return new SepArcher(info); //TODO
                case 219:
                    return new SepHighWarrior(info); //TODO
                case 220:
                    return new SepHighWizard(info); //TODO
                case 221:
                    return new SepHighTaoist(info); //TODO
                case 222:
                    return new SepHighAssassin(info); //TODO
                case 223:
                    return new SepHighArcher(info); //TODO

                case 255://Skill 
                    return new StoneTrap(info);

                default:
                    return new MonsterObject(info);
            }
        }

        public override ObjectType Race
        {
            get { return ObjectType.Monster; }
        }

        public MonsterInfo Info;
        public MapRespawn Respawn;

        public override string Name
        {
            get { return Master == null ? Info.GameName : string.Format("{0}({1})", Info.GameName, Master.Name); }
            set { throw new NotSupportedException(); }
        }

        public override int CurrentMapIndex { get; set; }
        public override Point CurrentLocation { get; set; }
        public override sealed MirDirection Direction { get; set; }
        public override ushort Level
        {
            get { return Info.Level; }
            set { throw new NotSupportedException(); }
        }

        public override sealed AttackMode AMode
        {
            get
            {
                return base.AMode;
            }
            set
            {
                base.AMode = value;
            }
        }
        public override sealed PetMode PMode
        {
            get
            {
                return base.PMode;
            }
            set
            {
                base.PMode = value;
            }
        }

        public override int Health
        {
            get { return HP; }
        }

        public override int MaxHealth
        {
            get { return Stats[Stat.HP]; }
        }

        public int HealthPercent
        { 
            get 
            { 
                return (Health * 100) / MaxHealth; 
            } 
        }

        public int HP;

        public ushort MoveSpeed;

        public virtual uint Experience
        {
            get { return Info.Experience; }
        }
        public int DeadDelay
        {
            get
            {
                switch (Info.AI)
                {
                    case 64:
                        return 0;
                    case 81:
                    case 82:
                        return int.MaxValue;
                    case 252:
                        return 5000;
                    default:
                        return 180000;
                }
            }
        }
        public const int RegenDelay = 10000, EXPOwnerDelay = 5000, AloneDelay = 3000, SearchDelay = 3000, RoamDelay = 1000, HealDelay = 600, RevivalDelay = 2000;
        public long ActionTime, MoveTime, AttackTime, RegenTime, DeadTime, AloneTime, SearchTime, RoamTime, HealTime;
        public long ShockTime, RageTime, HallucinationTime;
        public bool BindingShotCenter, PoisonStopRegen = true;

        protected bool Alone = false, Stacking = false;

        public byte PetLevel;
        public uint PetExperience;
        public byte MaxPetLevel;
        public long TameTime;
        public bool DieNextTurn;

        public int RoutePoint;
        public bool Waiting;
        public bool GMMade;
        
        public List<MonsterObject> SlaveList = new List<MonsterObject>();
        public List<RouteInfo> Route = new List<RouteInfo>();

        public override bool Blocking
        {
            get
            {
                return !Dead;
            }
        }
        protected virtual bool CanRegen
        {
            get { return Envir.Time >= RegenTime; }
        }
        protected virtual bool CanMove
        {
            get
            {
                return 
                    !Dead && 
                    Envir.Time > MoveTime && 
                    Envir.Time > ActionTime && 
                    Envir.Time > ShockTime &&
                    (Master == null || Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.Both || Master.PMode == PetMode.FocusMasterTarget) && 
                    !CurrentPoison.HasFlag(PoisonType.Paralysis) && 
                    !CurrentPoison.HasFlag(PoisonType.LRParalysis) &&
                    !CurrentPoison.HasFlag(PoisonType.Frozen) &&
                    (!CurrentPoison.HasFlag(PoisonType.Stun) || (Info.Light == 10 || Info.Light == 5));
            }
        }
        protected virtual bool CanAttack
        {
            get
            {
                return 
                    !Dead &&
                    Envir.Time > AttackTime &&
                    Envir.Time > ActionTime &&
                    (Master == null || Master.PMode == PetMode.AttackOnly || Master.PMode == PetMode.Both || Master.PMode == PetMode.FocusMasterTarget) &&
                    !CurrentPoison.HasFlag(PoisonType.Paralysis) &&
                    !CurrentPoison.HasFlag(PoisonType.LRParalysis) &&
                    !CurrentPoison.HasFlag(PoisonType.Dazed) &&
                    !CurrentPoison.HasFlag(PoisonType.Frozen) &&
                    (!CurrentPoison.HasFlag(PoisonType.Stun) || (Info.Light == 10 || Info.Light == 5));
            }
        }
        protected internal MonsterObject(MonsterInfo info)
        {
            Info = info;

            Stats = new Stats();

            Undead = Info.Undead;
            AutoRev = info.AutoRev;
            CoolEye = info.CoolEye > Envir.Random.Next(100);
            Direction = (MirDirection)Envir.Random.Next(8);

            AMode = AttackMode.All;
            PMode = PetMode.Both;

            RegenTime = Envir.Random.Next(RegenDelay) + Envir.Time;
            SearchTime = Envir.Random.Next(SearchDelay) + Envir.Time;
            RoamTime = Envir.Random.Next(RoamDelay) + Envir.Time;
        }
        public bool Spawn(Map temp, Point location)
        {
            if (!temp.ValidPoint(location)) return false;

            CurrentMap = temp;
            CurrentLocation = location;

            CurrentMap.AddObject(this);

            RefreshAll();
            SetHP(Stats[Stat.HP]);

            Spawned();
            Envir.MonsterCount++;
            CurrentMap.MonsterCount++;
            return true;
        }
        public bool Spawn(MapRespawn respawn)
        {
            Respawn = respawn;

            if (Respawn.Map == null) return false;
            if (Respawn.WalkableCells == null || Respawn.WalkableCells.Count == 0) return false;

            var spawnPoint = Respawn.WalkableCells[Envir.Random.Next(Respawn.WalkableCells.Count)];

            CurrentLocation = spawnPoint;

            respawn.Map.AddObject(this);

            CurrentMap = respawn.Map;

            if (Respawn.Route.Count > 0)
                Route.AddRange(Respawn.Route);

            RefreshAll();
            SetHP(Stats[Stat.HP]);

            Spawned();
            Respawn.Count++;
            respawn.Map.MonsterCount++;
            Envir.MonsterCount++;
            return true;
        }

        public override void Spawned()
        {
            ActionTime = Envir.Time + 2000;

            if (Info.HasSpawnScript && (Envir.MonsterNPC != null))
            {
                Envir.MonsterNPC.Call(this,string.Format("[@_SPAWN({0})]",Info.Index));
            }

            base.Spawned();
        }

        protected virtual void RefreshBase()
        {
            Stats.Clear();

            Stats.Add(Info.Stats);

            MoveSpeed = Info.MoveSpeed;
            AttackSpeed = Info.AttackSpeed;
        }

        public virtual void RefreshAll()
        {
            RefreshBase();

            Stats[Stat.HP] += PetLevel * 20;
            Stats[Stat.MinAC] += PetLevel * 2;
            Stats[Stat.MaxAC] += PetLevel * 2;
            Stats[Stat.MinMAC] += PetLevel * 2;
            Stats[Stat.MaxMAC] += PetLevel * 2;
            Stats[Stat.MinDC] += PetLevel;
            Stats[Stat.MaxDC] += PetLevel;

            if (Info.Name == Settings.SkeletonName || Info.Name == Settings.ShinsuName || Info.Name == Settings.AngelName)
            {
                MoveSpeed = (ushort)Math.Min(ushort.MaxValue, (Math.Max(ushort.MinValue, MoveSpeed - MaxPetLevel * 130)));
                AttackSpeed = (ushort)Math.Min(ushort.MaxValue, (Math.Max(ushort.MinValue, AttackSpeed - MaxPetLevel * 70)));
            }

            if (MoveSpeed < 400) MoveSpeed = 400;
            if (AttackSpeed < 400) AttackSpeed = 400;

            RefreshBuffs();
        }

        protected virtual void RefreshBuffs()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buff buff = Buffs[i];

                Stats.Add(buff.Stats);

                switch (buff.Type)
                {
                    case BuffType.SwiftFeet:
                        MoveSpeed = (ushort)Math.Max(ushort.MinValue, MoveSpeed + 100);
                        break;
                }
            }
        }
        public virtual void RefreshNameColour(bool send = true)
        {
            if (ShockTime < Envir.Time) BindingShotCenter = false;

            Color colour = Color.White;

            switch (PetLevel)
            {
                case 1:
                    colour = Color.Aqua;
                    break;
                case 2:
                    colour = Color.Aquamarine;
                    break;
                case 3:
                    colour = Color.LightSeaGreen;
                    break;
                case 4:
                    colour = Color.SlateBlue;
                    break;
                case 5:
                    colour = Color.SteelBlue;
                    break;
                case 6:
                    colour = Color.Blue;
                    break;
                case 7:
                    colour = Color.Navy;
                    break;
            }

            if (Envir.Time < ShockTime)
                colour = Color.Peru;
            else if (Envir.Time < RageTime)
                colour = Color.Red;
            else if (Envir.Time < HallucinationTime)
                colour = Color.MediumOrchid;

            if (colour == NameColour || !send) return;

            NameColour = colour;

            Broadcast(new S.ObjectColourChanged { ObjectID = ObjectID, NameColour = NameColour });
        }

        public void SetHP(int amount)
        {
            if (HP == amount) return;

            HP = amount <= Stats[Stat.HP] ? amount : Stats[Stat.HP];

            if (!Dead && HP == 0) Die();

            //  HealthChanged = true;
            BroadcastHealthChange();
        }
        public virtual void ChangeHP(int amount)
        {
            if (HP + amount > Stats[Stat.HP])
                amount = Stats[Stat.HP] - HP;

            if (amount == 0) return;

            HP += amount;

            if (HP < 0) HP = 0;

            if (!Dead && HP == 0) Die();

            // HealthChanged = true;
            BroadcastHealthChange();
        }

        //use this so you can have mobs take no/reduced poison damage
        public virtual void PoisonDamage(int amount, MapObject Attacker)
        {
            ChangeHP(amount);
        }


        public override bool Teleport(Map temp, Point location, bool effects = true, byte effectnumber = 0)
        {
            if (temp == null || !temp.ValidPoint(location)) return false;

            CurrentMap.RemoveObject(this);
            if (effects) Broadcast(new S.ObjectTeleportOut { ObjectID = ObjectID, Type = effectnumber });
            Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

            CurrentMap.MonsterCount--;

            CurrentMap = temp;
            CurrentLocation = location;

            CurrentMap.MonsterCount++;

            InTrapRock = false;

            CurrentMap.AddObject(this);
            BroadcastInfo();

            if (effects) Broadcast(new S.ObjectTeleportIn { ObjectID = ObjectID, Type = effectnumber });

            BroadcastHealthChange();

            return true;
        }


        public override void Die()
        {
            if (Dead) return;

            HP = 0;
            Dead = true;

            DeadTime = Envir.Time + DeadDelay;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            if (Info.HasDieScript && (Envir.MonsterNPC != null))
            {
                Envir.MonsterNPC.Call(this, string.Format("[@_DIE({0})]", Info.Index));
            }

            if (EXPOwner != null && EXPOwner.Node != null && Master == null && (EXPOwner.Race == ObjectType.Player || EXPOwner.Race == ObjectType.Hero))
            {
                EXPOwner.WinExp(Experience, Level);

                if (EXPOwner.Race != ObjectType.Hero)
                {
                    PlayerObject playerObj = (PlayerObject)EXPOwner;
                    playerObj.CheckGroupQuestKill(Info);
                }

            }

            if (Respawn != null)
                Respawn.Count--;

            if (Master == null && EXPOwner != null)
                Drop();

            Master = null;

            PoisonList.Clear();
            Envir.MonsterCount--;
            if (CurrentMap != null)
            CurrentMap.MonsterCount--;
        }

        public MapObject GetAttacker(MapObject attacker)
        {
            return attacker switch
            {
                HeroObject hero => hero.Owner,
                _ => attacker
            };
        }

        public void Revive(int hp, bool effect)
        {
            if (!Dead) return;

            SetHP(hp);

            Dead = false;
            ActionTime = Envir.Time + RevivalDelay;

            Broadcast(new S.ObjectRevived { ObjectID = ObjectID, Effect = effect });

            if (Respawn != null)
                Respawn.Count++;

            Envir.MonsterCount++;
            CurrentMap.MonsterCount++;
        }

        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            if (!Info.CanPush) return 0;
            //if (!CanMove) return 0; //stops mobs that can't move (like cannibalplants) from being pushed

            int result = 0;
            MirDirection reverse = Functions.ReverseDirection(dir);
            for (int i = 0; i < distance; i++)
            {
                Point location = Functions.PointMove(CurrentLocation, dir, 1);

                if (!CurrentMap.ValidPoint(location)) return result;

                Cell cell = CurrentMap.GetCell(location);

                bool stop = false;
                if (cell.Objects != null)
                    for (int c = 0; c < cell.Objects.Count; c++)
                    {
                        MapObject ob = cell.Objects[c];
                        if (!ob.Blocking) continue;
                        stop = true;
                    }
                if (stop) break;

                CurrentMap.GetCell(CurrentLocation).Remove(this);

                Direction = reverse;
                RemoveObjects(dir, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(dir, 1);

                Broadcast(new S.ObjectPushed { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                result++;
            }

            ActionTime = Envir.Time + 300 * result;
            MoveTime = Envir.Time + 500 * result;

            if (result > 0)
            {
                Cell cell = CurrentMap.GetCell(CurrentLocation);

                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell) continue;
                    SpellObject ob = (SpellObject)cell.Objects[i];

                    ob.ProcessSpell(this);
                    //break;
                }
            }

            return result;
        }

        protected virtual void Drop()
        {
            if (CurrentMap.Info.NoDropMonster)
                return;

            for (int i = 0; i < Info.Drops.Count; i++)
            {
                DropInfo drop = Info.Drops[i];

                var reward = drop.AttemptDrop(EXPOwner?.Stats[Stat.ItemDropRatePercent] ?? 0, EXPOwner?.Stats[Stat.GoldDropRatePercent] ?? 0);

                if (reward != null)
                {
                    if (reward.Gold > 0)
                    {
                        DropGold(reward.Gold);
                    }

                    foreach (var dropItem in reward.Items)
                    {
                        UserItem item = Envir.CreateDropItem(dropItem);

                        if (item == null) continue;

                        if (GMMade)
                        {
                            item.GMMade = true;
                        }

                        if (EXPOwner != null && EXPOwner.Race == ObjectType.Player)
                        {
                            PlayerObject ob = (PlayerObject)EXPOwner;

                            if (ob.CheckGroupQuestItem(item))
                            {
                                continue;
                            }
                        }

                        if (drop.QuestRequired) continue;
                        if (!DropItem(item)) return;
                    }
                }
            }
        }

        protected virtual bool DropItem(UserItem item)
        {
            if (CurrentMap.Info.NoDropMonster)
                return false;

            ItemObject ob = new ItemObject(this, item)
            {
                Owner = EXPOwner,
                OwnerTime = Envir.Time + Settings.Minute,
            };

            if (!item.Info.GlobalDropNotify)
                return ob.Drop(Settings.DropRange);

            foreach (var player in Envir.Players)
            {
                player.ReceiveChat($"{Name} has dropped {item.FriendlyName}.", ChatType.System2);
            }

            return ob.Drop(Settings.DropRange);
        }

        protected virtual bool DropGold(uint gold)
        {
            if (EXPOwner != null && EXPOwner.CanGainGold(gold) && !Settings.DropGold)
            {
                EXPOwner.WinGold(gold);
                return true;
            }

            uint count = gold / Settings.MaxDropGold == 0 ? 1 : gold / Settings.MaxDropGold + 1;
            for (int i = 0; i < count; i++)
            {
                ItemObject ob = new ItemObject(this, i != count - 1 ? Settings.MaxDropGold : gold % Settings.MaxDropGold)
                {
                    Owner = EXPOwner,
                    OwnerTime = Envir.Time + Settings.Minute,
                };

                ob.Drop(Settings.DropRange);
            }

            return true;
        }

        public override void Process()
        {
            base.Process();

            RefreshNameColour();

            if (Target != null && (Target.CurrentMap != CurrentMap || !Target.IsAttackTarget(this) || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Globals.DataRange)))
                Target = null;

            for (int i = SlaveList.Count - 1; i >= 0; i--)
                if (SlaveList[i].Dead || SlaveList[i].Node == null)
                    SlaveList.RemoveAt(i);

            if (Dead && Envir.Time >= DeadTime)
            {
                CurrentMap.RemoveObject(this);
                if (Master != null)
                {
                    Master.Pets.Remove(this);
                    Master = null;
                }

                Despawn();
                return;
            }

            if (Master != null && TameTime > 0 && Envir.Time >= TameTime)
            {
                Master.Pets.Remove(this);
                Master = null;
                Broadcast(new S.ObjectName { ObjectID = ObjectID, Name = Name });
            }

            ProcessAI();

            ProcessBuffs();
            ProcessRegen();
            ProcessPoison();
        }

        public override void SetOperateTime()
        {
            long time = Envir.Time + 2000;

            if (AloneTime < time && AloneTime > Envir.Time)
                time = AloneTime;

            if (DeadTime < time && DeadTime > Envir.Time)
                time = DeadTime;

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

            if (SearchTime < time && SearchTime > Envir.Time)
                time = SearchTime;

            if (RoamTime < time && RoamTime > Envir.Time)
                time = RoamTime;

            if (ShockTime < time && ShockTime > Envir.Time)
                time = ShockTime;

            if (RegenTime < time && RegenTime > Envir.Time && Health < MaxHealth)
                time = RegenTime;

            if (RageTime < time && RageTime > Envir.Time)
                time = RageTime;

            if (HallucinationTime < time && HallucinationTime > Envir.Time)
                time = HallucinationTime;

            if (ActionTime < time && ActionTime > Envir.Time)
                time = ActionTime;

            if (MoveTime < time && MoveTime > Envir.Time)
                time = MoveTime;

            if (AttackTime < time && AttackTime > Envir.Time)
                time = AttackTime;

            if (HealTime < time && HealTime > Envir.Time && HealAmount > 0)
                time = HealTime;

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
                if (Buffs[i].NextTime >= time && Buffs[i].NextTime > Envir.Time) continue;
                time = Buffs[i].NextTime;
            }

            if (OperateTime <= Envir.Time || time < OperateTime)
                OperateTime = time;
        }

        public override void Process(DelayedAction action)
        {
            switch (action.Type)
            {
                case DelayedType.Damage:
                    CompleteAttack(action.Params);
                    break;
                case DelayedType.RangeDamage:
                    CompleteRangeAttack(action.Params);
                    break;
                case DelayedType.Die:
                    CompleteDeath(action.Params);
                    break;
                case DelayedType.Recall:
                    PetRecall();
                    break;
                case DelayedType.SpellEffect:
                    CompleteSpellEffect(action.Params);
                    break;
            }
        }

        public void PetRecall()
        {
            if (Master == null) return;
            if (!Teleport(Master.CurrentMap, Master.Back))
                Teleport(Master.CurrentMap, Master.CurrentLocation);
        }
        protected virtual void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);
        }

        protected virtual void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);
        }

        protected virtual void CompleteDeath(IList<object> data)
        {
            throw new NotImplementedException();
        }

        protected virtual void CompleteSpellEffect(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            SpellEffect effect = (SpellEffect)data[1];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            S.ObjectEffect p = new S.ObjectEffect { ObjectID = target.ObjectID, Effect = effect };
            CurrentMap.Broadcast(p, target.CurrentLocation);
        }

        protected virtual void ProcessRegen()
        {
            if (Dead) return;

            int healthRegen = 0;

            if (CanRegen)
            {
                RegenTime = Envir.Time + RegenDelay;


                if (HP < Stats[Stat.HP])
                    healthRegen += (int)(Stats[Stat.HP] * 0.022F) + 1;
            }


            if (Envir.Time > HealTime)
            {
                HealTime = Envir.Time + HealDelay;

                if (HealAmount > 5)
                {
                    healthRegen += 5;
                    HealAmount -= 5;
                }
                else
                {
                    healthRegen += HealAmount;
                    HealAmount = 0;
                }
            }

            if (healthRegen > 0)
            {
                ChangeHP(healthRegen);
                BroadcastDamageIndicator(DamageType.Hit, healthRegen);
            }
            if (HP == Stats[Stat.HP]) HealAmount = 0;
        }
        protected virtual void ProcessPoison()
        {
            PoisonType type = PoisonType.None;
            ArmourRate = 1F;
            DamageRate = 1F;

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (Dead) return;

                Poison poison = PoisonList[i];
                if (poison.Owner != null && poison.Owner.Node == null)
                {
                    if (poison.PType == PoisonType.Slow)
                    {
                        MoveSpeed = Info.MoveSpeed;
                        AttackSpeed = Info.AttackSpeed;
                        AttackTime = Envir.Time + AttackSpeed;
                    }
                    PoisonList.RemoveAt(i);
                    continue;
                }

                if (Envir.Time > poison.TickTime)
                {
                    poison.Time++;
                    poison.TickTime = Envir.Time + poison.TickSpeed;

                    if (poison.Time >= poison.Duration)
                    {
                        if (poison.PType == PoisonType.Slow)
                        {
                            MoveSpeed = Info.MoveSpeed;
                            AttackSpeed = Info.AttackSpeed;
                            AttackTime = Envir.Time + AttackSpeed;
                        }
                        PoisonList.RemoveAt(i);
                        continue;
                    }

                    if (poison.PType == PoisonType.Green || poison.PType == PoisonType.Bleeding)
                    {
                        if (EXPOwner == null || EXPOwner.Dead)
                        {
                            EXPOwner = poison.Owner;
                            EXPOwnerTime = Envir.Time + EXPOwnerDelay;
                        }
                        else if (EXPOwner == poison.Owner)
                            EXPOwnerTime = Envir.Time + EXPOwnerDelay;

                        if (poison.PType == PoisonType.Bleeding)
                        {
                            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Bleeding, EffectType = 0 });
                        }

                        //ChangeHP(-poison.Value);
                        PoisonDamage(-poison.Value, poison.Owner);
                        BroadcastDamageIndicator(DamageType.Hit, -poison.Value);
                        if (PoisonStopRegen)
                            RegenTime = Envir.Time + RegenDelay;
                        if (poison.Owner != null && Target == null)
                            Target = poison.Owner;
                    }

                    if (poison.PType == PoisonType.DelayedExplosion)
                    {
                        if (Envir.Time > ExplosionInflictedTime) ExplosionInflictedStage++;

                        if (!ProcessDelayedExplosion(poison))
                        {
                            ExplosionInflictedStage = 0;
                            ExplosionInflictedTime = 0;

                            if (Dead) break; //temp to stop crashing

                            PoisonList.RemoveAt(i);
                            continue;
                        }
                    }
                }

                switch (poison.PType)
                {
                    case PoisonType.Red:
                        ArmourRate -= 0.5F;
                        break;
                    case PoisonType.Stun:
                        DamageRate += 0.5F;
                        break;
                    case PoisonType.Blindness:
                        break;
                    case PoisonType.Slow:
                        MoveSpeed = (ushort)Math.Min(3500, MoveSpeed + 100);
                        AttackSpeed = (ushort)Math.Min(3500, AttackSpeed + 100);

                        if (poison.Time >= poison.Duration)
                        {
                            MoveSpeed = Info.MoveSpeed;
                            AttackSpeed = Info.AttackSpeed;
                            //Reset the Attack time
                            AttackTime = Envir.Time + AttackSpeed;
                        }
                        break;
                }
                type |= poison.PType;
                /*
                if ((int)type < (int)poison.PType)
                    type = poison.PType;
                 */
            }


            if (type == CurrentPoison) return;

            CurrentPoison = type;
            Broadcast(new S.ObjectPoisoned { ObjectID = ObjectID, Poison = type });
        }

        private bool ProcessDelayedExplosion(Poison poison)
        {
            if (Dead) return false;

            if (ExplosionInflictedStage == 0)
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 0 });
                return true;
            }
            if (ExplosionInflictedStage == 1)
            {
                if (Envir.Time > ExplosionInflictedTime)
                    ExplosionInflictedTime = poison.TickTime + 3000;
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 1 });
                return true;
            }
            if (ExplosionInflictedStage == 2)
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 2 });
                if (poison.Owner != null)
                {
                    switch (poison.Owner.Race)
                    {
                        case ObjectType.Player:
                            PlayerObject caster = (PlayerObject)poison.Owner;
                            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time, poison.Owner, caster.GetMagic(Spell.DelayedExplosion), poison.Value, this.CurrentLocation);
                            CurrentMap.ActionList.Add(action);
                            //Attacked((PlayerObject)poison.Owner, poison.Value, DefenceType.MAC, false);
                            break;
                        case ObjectType.Monster://this is in place so it could be used by mobs if one day someone chooses to
                            Attacked((MonsterObject)poison.Owner, poison.Value, DefenceType.MAC);
                            break;
                    }
                    LastHitter = poison.Owner;
                }
                return false;
            }
            return false;
        }

        private void ProcessBuffs()
        {
            bool refresh = false;
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = Buffs[i];

                if (buff.NextTime > Envir.Time) continue;

                if (!buff.Paused && buff.StackType != BuffStackType.Infinite)
                {
                    var change = Envir.Time - buff.LastTime;
                    buff.ExpireTime -= change;
                }

                buff.LastTime = Envir.Time;
                buff.NextTime = Envir.Time + 1000;

                if ((buff.ExpireTime > 0 || buff.StackType == BuffStackType.Infinite) && !buff.FlagForRemoval) continue;

                Buffs.RemoveAt(i);

                if (buff.Info.Visible)
                {
                    Broadcast(new S.RemoveBuff { Type = buff.Type, ObjectID = ObjectID });
                }

                switch (buff.Type)
                {
                    case BuffType.Hiding:
                    case BuffType.MoonLight:
                    case BuffType.DarkBody:
                        if (!HasAnyBuffs(buff.Type, BuffType.ClearRing, BuffType.Hiding, BuffType.MoonLight, BuffType.DarkBody))
                        {
                            Hidden = false;
                        }
                        if (buff.Type == BuffType.MoonLight || buff.Type == BuffType.DarkBody)
                        {
                            if (!HasAnyBuffs(buff.Type, BuffType.MoonLight, BuffType.DarkBody))
                            {
                                Sneaking = false;
                            }
                            break;
                        }
                        break;
                }

                ProcessBuffEnd(buff);

                refresh = true;
            }

            if (refresh) RefreshAll();
        }

        protected virtual void ProcessBuffEnd(Buff buff)
        {

        }

        protected virtual void ProcessAI()
        {
            if (Dead) return;

            if (DieNextTurn)
            {
                Die();
                return;
            }

            if (Master != null)
            {
                if (Master.PMode == PetMode.Both || Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.FocusMasterTarget)
                {
                    if (!Functions.InRange(CurrentLocation, Master.CurrentLocation, Globals.DataRange) || CurrentMap != Master.CurrentMap)
                        PetRecall();
                }

                if (Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.None)
                    Target = null;
            }

            CheckAlone();

            if (!Alone || Settings.MonsterProcessWhenAlone)
            {
                ProcessStacking();

                ProcessSearch();
                ProcessRoam();
                ProcessTarget();
            }
        }

        protected virtual void CheckAlone()
        {
            if (Envir.Time < AloneTime) return;

            AloneTime = Envir.Time + AloneDelay;

            if (Route.Count > 0)
            {
                Alone = false;
                return;
            }

            if (CurrentMap.Players.Count == 0)
            {
                Alone = true;
                return;
            }

            for (int i = 0; i < CurrentMap.Players.Count; i++)
            {
                if (Functions.InRange(CurrentLocation, CurrentMap.Players[i].CurrentLocation, Globals.DataRange * 2))
                {
                    Alone = false;
                    return;
                }
            }

            Alone = true;
        }

        protected virtual void ProcessStacking()
        {
            //Stacking or Infront of master - Move
            Stacking = CheckStacked();

            if (CanMove && ((Master != null && Master.Front == CurrentLocation) || Stacking))
            {
                //Walk Randomly
                if (!Walk(Direction))
                {
                    MirDirection dir = Direction;

                    switch (Envir.Random.Next(3)) // favour Clockwise
                    {
                        case 0:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.NextDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                        default:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.PreviousDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                    }
                }

                return;
            }
        }

        protected virtual void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;
            if (Master != null && (Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.None || Master.PMode == PetMode.FocusMasterTarget)) return;

            SearchTime = Envir.Time + SearchDelay;

            if (Target == null || Envir.Random.Next(3) == 0)
                FindTarget();
        }

        protected virtual void ProcessRoam()
        {
            if (Target != null || Envir.Time < RoamTime) return;

            if (ProcessRoute()) return;

            if (Master != null)
            {
                MoveTo(Master.Back);
                return;
            }

            RoamTime = Envir.Time + RoamDelay;

            if (Envir.Random.Next(10) != 0) return;

            switch (Envir.Random.Next(3)) //Face Walk
            {
                case 0:
                    Turn((MirDirection)Envir.Random.Next(8));
                    break;
                default:
                    Walk(Direction);
                    break;
            }
        }

        protected virtual void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        protected virtual bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected virtual void FindTarget()
        {
            Map Current = CurrentMap;

            for (int d = 0; d <= Info.ViewRange; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= Current.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= Current.Width) break;
                        Cell cell = Current.Cells[x, y];
                        if (cell.Objects == null || !cell.Valid) continue;
                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Hero:

                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level)) continue;
                                    if (this is TrapRock && ob.InTrapRock) continue;

                                    if (ob.Race == ObjectType.Monster && 
                                        ob is StoneTrap)
                                    {
                                        if (Target is null || 
                                            (Target is not null &&
                                            Target is not StoneTrap))
                                        {
                                            Target = ob;
                                        }
                                        
                                        return;
                                    }
                                    else
                                    {
                                        Target ??= ob;
                                    }
                                    continue;
                                    
                                case ObjectType.Player:

                                    if (Target != null)
                                    {
                                        continue;
                                    }

                                    PlayerObject playerob = (PlayerObject)ob;
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (playerob.GMGameMaster || ob.Hidden && (!CoolEye || Level < ob.Level) || Envir.Time < HallucinationTime) continue;

                                    Target = ob;

                                    if (Master != null)
                                    {
                                        for (int j = 0; j < playerob.Pets.Count; j++)
                                        {
                                            MonsterObject pet = playerob.Pets[j];

                                            if (!pet.IsAttackTarget(this)) continue;
                                            Target = pet;
                                            break;
                                        }
                                    }
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        protected virtual bool ProcessRoute()
        {
            if (Route.Count < 1) return false;

            RoamTime = Envir.Time + 500;

            if (CurrentLocation == Route[RoutePoint].Location)
            {
                if (Route[RoutePoint].Delay > 0 && !Waiting)
                {
                    Waiting = true;
                    RoamTime = Envir.Time + RoamDelay + Route[RoutePoint].Delay;
                    return true;
                }

                Waiting = false;
                RoutePoint++;
            }

            if (RoutePoint > Route.Count - 1) RoutePoint = 0;

            if (!CurrentMap.ValidPoint(Route[RoutePoint].Location)) return true;

            MoveTo(Route[RoutePoint].Location);

            return true;
        }

        protected virtual void MoveTo(Point location)
        {
            if (CurrentLocation == location) return;

            bool inRange = Functions.InRange(location, CurrentLocation, 1);

            if (inRange)
            {
                if (!CurrentMap.ValidPoint(location)) return;
                Cell cell = CurrentMap.GetCell(location);
                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking) continue;
                        return;
                    }
            }

            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, location);

            if (Walk(dir)) return;

            switch (Envir.Random.Next(2)) //No favour
            {
                case 0:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.NextDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
                default:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.PreviousDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
            }
        }

        public virtual void Turn(MirDirection dir)
        {
            if (!CanMove) return;

            Direction = dir;

            InSafeZone = CurrentMap.GetSafeZone(CurrentLocation) != null;

            Cell cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                //break;
            }

            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }

        public virtual bool Walk(MirDirection dir)
        {
            if (!CanMove) return false;

            Point location = Functions.PointMove(CurrentLocation, dir, 1);

            if (!CurrentMap.ValidPoint(location)) return false;

            Cell cell = CurrentMap.GetCell(location);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking || Race == ObjectType.Creature) continue;

                    return false;
                }

            CurrentMap.GetCell(CurrentLocation).Remove(this);

            Direction = dir;
            RemoveObjects(dir, 1);
            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, 1);

            if (Hidden)
            {
                RemoveBuff(BuffType.Hiding);
            }

            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + 300;
            MoveTime = Envir.Time + MoveSpeed;
            if (MoveTime > AttackTime)
                AttackTime = MoveTime;

            InSafeZone = CurrentMap.GetSafeZone(CurrentLocation) != null;

            Broadcast(new S.ObjectWalk { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                //break;
            }

            return true;
        }
        protected virtual void Attack()
        {
            if (BindingShotCenter) ReleaseBindingShot();

            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

        public void ReleaseBindingShot()
        {
            if (!BindingShotCenter) return;

            ShockTime = 0;
            Broadcast(GetInfo());//update clients in range (remove effect)
            BindingShotCenter = false;

            //the centertarget is escaped so make all shocked mobs awake (3x3 from center)
            Point place = CurrentLocation;
            for (int y = place.Y - 1; y <= place.Y + 1; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = place.X - 1; x <= place.X + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    Cell cell = CurrentMap.GetCell(x, y);
                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject targetob = cell.Objects[i];
                        if (targetob == null || targetob.Node == null || targetob.Race != ObjectType.Monster) continue;
                        if (((MonsterObject)targetob).ShockTime == 0) continue;

                        //each centerTarget has its own effect which needs to be cleared when no longer shocked
                        if (((MonsterObject)targetob).BindingShotCenter) ((MonsterObject)targetob).ReleaseBindingShot();
                        else ((MonsterObject)targetob).ShockTime = 0;

                        break;
                    }
                }
            }
        }

        public bool FindNearby(int distance)
        {
            for (int d = 0; d <= distance; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;
                        if (!CurrentMap.ValidPoint(x, y)) continue;
                        Cell cell = CurrentMap.GetCell(x, y);
                        if (cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level)) continue;
                                    if (ob.Race == ObjectType.Player)
                                    {
                                        PlayerObject player = ((PlayerObject)ob);
                                        if (player.GMGameMaster) continue;
                                    }
                                    return true;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }

            return false;
        }
        public bool FindFriendsNearby(int distance)
        {
            for (int d = 0; d <= distance; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;
                        if (!CurrentMap.ValidPoint(x, y)) continue;
                        Cell cell = CurrentMap.GetCell(x, y);
                        if (cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    if (ob == this || ob.Dead) continue;
                                    if (ob.IsAttackTarget(this)) continue;
                                    if (ob.Race == ObjectType.Player)
                                    {
                                        PlayerObject player = ((PlayerObject)ob);
                                        if (player.GMGameMaster) continue;
                                    }
                                    return true;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }

            return false;
        }

        protected List<MapObject> FindAllFriends(int dist, Point location, bool needSight = true, bool ownAI = true)
        {
            List<MapObject> targets = new List<MapObject>();
            for (int d = 0; d <= dist; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];

                            if (ob == this) continue;

                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    if (ob.Dead) continue;
                                    if (!ownAI && ob.Race == ObjectType.Monster && ((MonsterObject)ob).Info.AI == Info.AI) continue;
                                    if (!ob.IsFriendlyTarget(this)) continue;
                                    if (ob.Master != Master) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level) && needSight) continue;
                                    targets.Add(ob);
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            return targets;
        }

        public List<MapObject> FindAllNearby(int dist, Point location, bool needSight = true)
        {
            List<MapObject> targets = new List<MapObject>();
            for (int d = 0; d <= dist; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    targets.Add(ob);
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            return targets;
        }

        protected List<MapObject> FindAllTargets(int dist, Point location, bool needSight = true)
        {
            List<MapObject> targets = new List<MapObject>();
            for (int d = 0; d <= dist; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level) && needSight) continue;
                                    if (ob.Race == ObjectType.Player)
                                    {
                                        PlayerObject player = ((PlayerObject)ob);
                                        if (player.GMGameMaster) continue;
                                    }
                                    targets.Add(ob);
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            return targets;
        }

        public override bool IsAttackTarget(HumanObject attacker)
        {
            if (attacker == null || attacker.Node == null) return false;
            if (Dead) return false;
            if (Master == null) return true;

            if (attacker.Race == ObjectType.Hero)
                attacker = ((HeroObject)attacker).Owner;

            if (attacker.AMode == AttackMode.Peace) return false;
            if (Master == attacker) return attacker.AMode == AttackMode.All;
            if (Master.Race == ObjectType.Player && (attacker.InSafeZone || InSafeZone)) return false;

            switch (attacker.AMode)
            {
                case AttackMode.Group:
                    return Master.GroupMembers == null || !Master.GroupMembers.Contains(attacker);
                case AttackMode.Guild:
                    {
                        if (!(Master is PlayerObject)) return false;
                        PlayerObject master = (PlayerObject)Master;
                        return master.MyGuild == null || master.MyGuild != attacker.MyGuild;
                    }
                case AttackMode.EnemyGuild:
                    {
                        if (!(Master is PlayerObject)) return false;
                        PlayerObject master = (PlayerObject)Master;
                        return (master.MyGuild != null && attacker.MyGuild != null) && master.MyGuild.IsEnemy(attacker.MyGuild);
                    }
                case AttackMode.RedBrown:
                    return Master.PKPoints >= 200 || Envir.Time < Master.BrownTime;
                default:
                    return true;
            }
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            if (attacker == null || attacker.Node == null) return false;
            if (Dead || attacker == this) return false;
            if (attacker.Race == ObjectType.Creature) return false;

            if (attacker.Info.AI == 6 || attacker.Info.AI == 113) // Guard
            {
                if (Info.AI != 1 && Info.AI != 2 && Info.AI != 3 && (Master == null || Master.PKPoints >= 200)) //Not Dear/Hen/Tree/Pets or Red Master 
                    return true;
            }
            else if (attacker.Info.AI == 58) // Tao Guard - attacks Pets
            {
                if (Info.AI != 1 && Info.AI != 2 && Info.AI != 3 && (Master == null || Master.AMode != AttackMode.Peace)) //Not Dear/Hen/Tree or Peaceful Master
                    return true;
            }
            else if (Master != null) //Pet Attacked
            {
                if (attacker.Master == null) //Wild Monster
                    return true;

                //Pet Vs Pet
                if (Master == attacker.Master)
                    return false;

                if (Envir.Time < ShockTime) //Shocked
                    return false;

                if (Master.Race == ObjectType.Player && attacker.Master.Race == ObjectType.Player && (Master.InSafeZone || attacker.Master.InSafeZone)) return false;

                switch (attacker.Master.AMode)
                {
                    case AttackMode.Group:
                        if (Master.GroupMembers != null && Master.GroupMembers.Contains((PlayerObject)attacker.Master)) return false;
                        break;
                    case AttackMode.Guild:
                        break;
                    case AttackMode.EnemyGuild:
                        break;
                    case AttackMode.RedBrown:
                        if (attacker.Master.PKPoints < 200 || Envir.Time > attacker.Master.BrownTime) return false;
                        break;
                    case AttackMode.Peace:
                        return false;
                }

                for (int i = 0; i < Master.Pets.Count; i++)
                    if (Master.Pets[i].EXPOwner == attacker.Master) return true;

                for (int i = 0; i < attacker.Master.Pets.Count; i++)
                {
                    MonsterObject ob = attacker.Master.Pets[i];
                    if (ob == Target || ob.Target == this) return true;
                }

                return Master.LastHitter == attacker.Master;
            }
            else if (attacker.Master != null) //Pet Attacking Wild Monster
            {
                if (Envir.Time < ShockTime) //Shocked
                    return false;

                for (int i = 0; i < attacker.Master.Pets.Count; i++)
                {
                    MonsterObject ob = attacker.Master.Pets[i];
                    if (ob == Target || ob.Target == this) return true;
                }

                if (Target == attacker.Master)
                    return true;
            }

            if (Envir.Time < attacker.HallucinationTime) return true;

            return Envir.Time < attacker.RageTime;
        }
        public override bool IsFriendlyTarget(HumanObject ally)
        {
            if (Master == null) return false;
            if (Master == ally) return true;

            switch (ally.AMode)
            {
                case AttackMode.Group:
                    return Master.GroupMembers != null && Master.GroupMembers.Contains(ally);
                case AttackMode.Guild:
                    return false;
                case AttackMode.EnemyGuild:
                    return true;
                case AttackMode.RedBrown:
                    return Master.PKPoints < 200 & Envir.Time > Master.BrownTime;
            }
            return true;
        }

        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            if (Master != null) return false;
            if (ally.Race != ObjectType.Monster) return false;
            if (ally.Master != null) return false;

            return true;
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            if (Target == null && attacker.IsAttackTarget(this))
            {
                Target = attacker;
            }

            var armour = GetArmour(type, attacker, out bool hit);

            if (!hit)
                return 0;

            armour = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(armour * ArmourRate))));
            damage = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(damage * DamageRate))));

            if (damageWeapon)
                attacker.DamageWeapon();
            damage += attacker.Stats[Stat.AttackBonus];

            if (armour >= damage)
            {
                BroadcastDamageIndicator(DamageType.Miss);
                return 0;
            }

            if (Envir.Random.Next(100) < (attacker.Stats[Stat.CriticalRate] * Settings.CriticalRateWeight))
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Critical });
                damage = Math.Min(int.MaxValue, damage + (int)Math.Floor(damage * (((double)attacker.Stats[Stat.CriticalDamage] / (double)Settings.CriticalDamageWeight) * 10)));
                BroadcastDamageIndicator(DamageType.Critical);
            }

            if (Target != this && attacker.IsAttackTarget(this))
            {
                if (attacker.Info.MentalState == 2)
                {
                    if (Functions.MaxDistance(CurrentLocation, attacker.CurrentLocation) < (8 - attacker.Info.MentalStateLvl))
                        Target = attacker;
                }
                else
                    Target = attacker;
            }

            if (BindingShotCenter) ReleaseBindingShot();
            ShockTime = 0;

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

                PoisonList.RemoveAt(i);
                OperateTime = 0;
            }

            if (Master != null && Master != attacker && (Master.Race != ObjectType.Hero || Master.Race == ObjectType.Hero && attacker != ((HeroObject)Master).Owner))
                if (Envir.Time > Master.BrownTime && Master.PKPoints < 200)
                    attacker.BrownTime = Envir.Time + Settings.Minute;

            if (EXPOwner == null || EXPOwner.Dead)
            {
                EXPOwner = GetAttacker(attacker);
            }

            if (EXPOwner == attacker)
                EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            ushort levelOffset = (ushort)(Level > attacker.Level ? 0 : Math.Min(10, attacker.Level - Level));

            ApplyNegativeEffects(attacker, type, levelOffset);

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            if (attacker.Stats[Stat.HPDrainRatePercent] > 0 && damageWeapon)
            {
                attacker.HpDrain += Math.Max(0, ((float)(damage - armour) / 100) * attacker.Stats[Stat.HPDrainRatePercent]);
                if (attacker.HpDrain > 2)
                {
                    int hpGain = (int)Math.Floor(attacker.HpDrain);
                    attacker.ChangeHP(hpGain);
                    attacker.HpDrain -= hpGain;
                }
            }

            attacker.GatherElement();

            if (attacker.Info.Mentor != 0 && attacker.Info.IsMentor)
            {
                if (attacker.HasBuff(BuffType.Mentor, out _))
                {
                    CharacterInfo mentee = Envir.GetCharacterInfo(attacker.Info.Mentor);
                    PlayerObject player = Envir.GetPlayer(mentee.Name);
                    if (player != null && player.CurrentMap == attacker.CurrentMap && Functions.InRange(player.CurrentLocation, attacker.CurrentLocation, Globals.DataRange) && !player.Dead)
                    {
                        if (GroupMembers != null && GroupMembers.Contains(player))
                            damage += (int)Math.Round((double)(damage * attacker.Stats[Stat.MentorDamageRatePercent]) / 100);
                    }
                }
            }

            if (Master != null && Master != attacker && Master.Race == ObjectType.Player && Envir.Time > Master.BrownTime && Master.PKPoints < 200 && !((PlayerObject)Master).AtWar(attacker))
            {
                attacker.BrownTime = Envir.Time + Settings.Minute;
            }

            for (int i = 0; i < attacker.Pets.Count; i++)
            {
                MonsterObject ob = attacker.Pets[i];

                if (IsAttackTarget(ob) && (ob.Target == null)) ob.Target = this;
            }

            BroadcastDamageIndicator(DamageType.Hit, armour - damage);

            ChangeHP(armour - damage);
            return damage - armour;
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (Target == null && attacker.IsAttackTarget(this))
                Target = attacker;


            var armour = GetArmour(type, attacker, out bool hit);
            if (!hit)
                return 0;

            armour = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(armour * ArmourRate))));
            damage = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(damage * DamageRate))));

            if (armour >= damage)
            {
                BroadcastDamageIndicator(DamageType.Miss);
                return 0;
            }

            if (Target != this && attacker.IsAttackTarget(this))
                Target = attacker;

            if (BindingShotCenter) ReleaseBindingShot();
            ShockTime = 0;

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

                PoisonList.RemoveAt(i);
                OperateTime = 0;
            }

            if (attacker.Info.AI == 6 || attacker.Info.AI == 58 || attacker.Info.AI == 113)
                EXPOwner = null;

            else if (attacker.Master != null)
            {
                if (attacker.CurrentMap != attacker.Master.CurrentMap || !Functions.InRange(attacker.CurrentLocation, attacker.Master.CurrentLocation, Globals.DataRange))
                    EXPOwner = null;
                else
                {

                    if (EXPOwner == null || EXPOwner.Dead)
                        EXPOwner = attacker.Master switch
                        {
                            HeroObject hero => hero.Owner,
                            _ => attacker.Master
                        };

                    if (EXPOwner == attacker.Master)
                        EXPOwnerTime = Envir.Time + EXPOwnerDelay;
                }

            }

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            BroadcastDamageIndicator(DamageType.Hit, armour - damage);

            ChangeHP(armour - damage);
            return damage - armour;
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            int armour = 0;

            switch (type)
            {
                case DefenceType.ACAgility:
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.MACAgility:
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.Agility:
                    break;
            }

            armour = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(armour * ArmourRate))));
            damage = (int)Math.Max(int.MinValue, (Math.Min(int.MaxValue, (decimal)(damage * DamageRate))));

            if (armour >= damage) return 0;
            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = 0, Direction = Direction, Location = CurrentLocation });

            ChangeHP(armour - damage);
            return damage - armour;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            if (p.Owner != null && p.Owner.IsAttackTarget(this) && Target == null)
                Target = p.Owner;

            if (Master != null && p.Owner != null && p.Owner.Race == ObjectType.Player && p.Owner != Master)
            {
                if (Envir.Time > Master.BrownTime && Master.PKPoints < 200)
                    p.Owner.BrownTime = Envir.Time + Settings.Minute;
            }

            if (!ignoreDefence && (p.PType == PoisonType.Green))
            {
                int armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);

                if (p.Value < armour)
                    p.PType = PoisonType.None;
                else
                    p.Value -= armour;
            }

            if (p.PType == PoisonType.None) return;

            for (int i = 0; i < PoisonList.Count; i++)
            {
                if (PoisonList[i].PType != p.PType) continue;
                if ((PoisonList[i].PType == PoisonType.Green) && (PoisonList[i].Value > p.Value)) return;//cant cast weak poison to cancel out strong poison
                if ((PoisonList[i].PType != PoisonType.Green) && ((PoisonList[i].Duration - PoisonList[i].Time) > p.Duration)) return;//cant cast 1 second poison to make a 1minute poison go away!
                if (p.PType == PoisonType.DelayedExplosion) return;
                if ((PoisonList[i].PType == PoisonType.Frozen) || (PoisonList[i].PType == PoisonType.Slow) || (PoisonList[i].PType == PoisonType.Paralysis)|| (PoisonList[i].PType == PoisonType.LRParalysis)) return;//prevents mobs from being perma frozen/slowed
                PoisonList[i] = p;
                return;
            }

            if (p.PType == PoisonType.DelayedExplosion)
            {
                ExplosionInflictedTime = Envir.Time + 4000;
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion });
            }
            else if (p.PType == PoisonType.Dazed)
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Stunned, Time = (uint)(p.Duration * p.TickSpeed) });
            }
            else if (p.PType == PoisonType.Blindness)
            {
                var stats = new Stats
                {
                    [Stat.Accuracy] = p.Value * -1
                };

                AddBuff(BuffType.Blindness, Caster, (int)(p.Duration * p.TickSpeed), stats);
            }

            PoisonList.Add(p);
        }

        public override Buff AddBuff(BuffType type, MapObject owner, int duration, Stats stats, bool refreshStats = true, bool updateOnly = false, params int[] values)
        {
            Buff b = base.AddBuff(type, owner, duration, stats, refreshStats, updateOnly, values);

            var packet = new S.AddBuff
            {
                Buff = b.ToClientBuff(),
            };

            if (b.Info.Visible) Broadcast(packet);

            if (refreshStats)
            {
                RefreshAll();
            }

            return b;
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Info.Image,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                ShockTime = (ShockTime > 0 ? ShockTime - Envir.Time : 0),
                BindingShotCenter = BindingShotCenter,
                Buffs = Buffs.Where(d => d.Info.Visible).Select(e => e.Type).ToList()
            };
        }

        public override void ReceiveChat(string text, ChatType type)
        {
            throw new NotSupportedException();
        }

        public void RemoveObjects(MirDirection dir, int count)
        {
            switch (dir)
            {
                case MirDirection.Up:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpRight:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Right:
                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownRight:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Down:
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownLeft:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Left:
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpLeft:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
            }
        }
        public void AddObjects(MirDirection dir, int count)
        {
            switch (dir)
            {
                case MirDirection.Up:
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpRight:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Right:
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownRight:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Down:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownLeft:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Left:
                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpLeft:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                if (ob.Race != ObjectType.Player) continue;
                                ob.Add(this);
                            }
                        }
                    }
                    break;
            }
        }

        public override void Add(HumanObject player)
        {
            player.Enqueue(GetInfo());
            SendHealth(player);
        }

        public override void SendHealth(HumanObject player)
        {
            if (!player.IsMember(Master) && !(player.IsMember(EXPOwner) && AutoRev) && Envir.Time > RevTime) return;
            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            player.Enqueue(new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time });
        }

        public void PetExp(uint amount)
        {
            if (PetLevel >= MaxPetLevel) return;

            if (Info.Name == Settings.SkeletonName || Info.Name == Settings.ShinsuName || Info.Name == Settings.AngelName)
                amount *= 3;

            PetExperience += amount;

            if (PetExperience < (PetLevel + 1) * 20000) return;

            PetExperience = (uint)(PetExperience - ((PetLevel + 1) * 20000));
            PetLevel++;
            RefreshAll();
            OperateTime = 0;
            BroadcastHealthChange();
        }
        public override void Despawn()
        {
            SlaveList.Clear();
            base.Despawn();
        }


        // MONSTER AI ATTACKS \\\
        protected virtual void PoisonTarget(MapObject target, int chanceToPoison, long poisonDuration, PoisonType poison, long poisonTickSpeed = 1000, bool noResist = false, bool ignoreDefence = true)
        {
            int value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= target.Stats[Stat.PoisonResist])
            {
                if (Envir.Random.Next(chanceToPoison) == 0)
                {
                    target.ApplyPoison(new Poison { Owner = this, Duration = poisonDuration, PType = poison, Value = value, TickSpeed = poisonTickSpeed }, this, noResist, ignoreDefence);
                }
            }
        }

        protected virtual void TriangleAttack(int damage, int distance, int limitWidth = -1, int additionalDelay = 500, DefenceType defenceType = DefenceType.ACAgility, bool push = false)
        {
            List<Point> points = new List<Point>();

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (!CurrentMap.ValidPoint(target)) continue;

                points.Add(target);

                if (distance > 1)
                {
                    Point left = target;
                    Point right = target;

                    var offset = i - 1;

                    for (int l = 1; l <= offset; l++)
                    {
                        if (limitWidth > -1 && l > limitWidth) break;

                        left = Functions.Left(left, Direction);
                        if (!CurrentMap.ValidPoint(left)) continue;
                        points.Add(left);
                    }

                    for (int r = 1; r <= offset; r++)
                    {
                        if (limitWidth > -1 && r > limitWidth) break;

                        right = Functions.Right(right, Direction);
                        if (!CurrentMap.ValidPoint(right)) continue;
                        points.Add(right);
                    }
                }
            }

            foreach (var point in points)
            {
                Cell cell = CurrentMap.GetCell(point);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player || ob.Race == ObjectType.Hero)
                    {
                        if (!ob.IsAttackTarget(this)) continue;

                        if (push)
                        {
                            var dir = Functions.DirectionFromPoint(CurrentLocation, ob.CurrentLocation);

                            ob.Pushed(this, dir, distance - 1);
                        }

                        int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + additionalDelay; //50 MS per Step

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);

                        ActionList.Add(action);
                    }
                    else continue;

                    break;
                }
            }
        }

        protected virtual void LineAttack(int damage, int distance, int additionalDelay = 500, DefenceType defenceType = DefenceType.ACAgility, bool push = false)
        {
            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (!CurrentMap.ValidPoint(target)) continue;

                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player || ob.Race == ObjectType.Hero)
                    {
                        if (!ob.IsAttackTarget(this)) continue;

                        if (push)
                        {
                            ob.Pushed(this, Direction, distance - 1);
                        }

                        int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + additionalDelay; //50 MS per Step
                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);
                        ActionList.Add(action);
                    }
                    else continue;

                    break;
                }
            }
        }

        protected virtual void WideLineAttack(int damage, int distance, int additionalDelay = 500, DefenceType defenceType = DefenceType.ACAgility, bool push = false, int width = 3)
        {
            if (width <= 2)
            {
                width = 3;
            }

            var even = width % 2 == 0;

            if (even)
            {
                width--;
            }

            var startPoints = new List<Point>
            {
                CurrentLocation 
            };

            var half = (width - 1) / 2;

            var leftLoc = CurrentLocation;
            var rightLoc = CurrentLocation;

            for (int j = 0; j < half; j++)
            {
                leftLoc = Functions.Left(leftLoc, Direction);
                rightLoc = Functions.Right(rightLoc, Direction);

                startPoints.Add(leftLoc);
                startPoints.Add(rightLoc);
            }

            for (int j = 0; j < startPoints.Count; j++)
            {
                var point = startPoints[j];

                for (int i = 1; i <= distance; i++)
                {
                    Point target = Functions.PointMove(point, Direction, i);

                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player || ob.Race == ObjectType.Hero)
                        {
                            if (!ob.IsAttackTarget(this)) continue;

                            if (push)
                            {
                                ob.Pushed(this, Direction, distance - 1);
                            }

                            int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + additionalDelay; //50 MS per Step
                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);
                            ActionList.Add(action);
                        }
                        else continue;

                        break;
                    }
                }
            }
        }

        protected virtual void HalfmoonAttack(int damage, int delay = 500, DefenceType defenceType = DefenceType.ACAgility)
        {
            MirDirection dir = Functions.PreviousDir(Direction);

            for (int i = 0; i < 4; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, dir, 1);
                dir = Functions.NextDir(dir);

                if (!CurrentMap.ValidPoint(target)) continue;

                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster && ob.Race != ObjectType.Hero) continue;
                    if (!ob.IsAttackTarget(this)) continue;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);
                    ActionList.Add(action);
                    break;
                }
            }
        }

        // Sanjian
        protected virtual void ThreeQuarterMoonAttack(int damage, int delay = 500, DefenceType defenceType = DefenceType.ACAgility)
        {
            MirDirection dir = Functions.PreviousDir(Direction);

            for (int i = 0; i < 6; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, dir, 1);
                dir = Functions.NextDir(dir);

                if (!CurrentMap.ValidPoint(target)) continue;

                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster && ob.Race != ObjectType.Hero) continue;
                    if (!ob.IsAttackTarget(this)) continue;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);
                    ActionList.Add(action);
                    break;
                }
            }
        }
        protected virtual void JumpBack(int distance)
        {
            MirDirection jumpDir = Functions.ReverseDirection(Direction);

            Point location = new Point();

            for (int i = 0; i < distance; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);
                if (!CurrentMap.ValidPoint(location)) return;
            }

            for (int i = 0; i < distance; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);

                CurrentMap.GetCell(CurrentLocation).Remove(this);
                RemoveObjects(jumpDir, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(jumpDir, 1);
            }

            Broadcast(new S.ObjectBackStep { ObjectID = ObjectID, Direction = Direction, Location = location, Distance = distance });
        }

        protected virtual void FullmoonAttack(int damage, int delay = 500, DefenceType defenceType = DefenceType.ACAgility, int pushDistance = -1, int distance = 1)
        {
            MirDirection dir = Direction;

            bool pushed = false;

            for (int j = 1; j <= distance; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    dir = Functions.NextDir(dir);
                    Point point = Functions.PointMove(CurrentLocation, dir, j);

                    if (!CurrentMap.ValidPoint(point)) continue;

                    Cell cell = CurrentMap.GetCell(point);

                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster && ob.Race != ObjectType.Hero) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        if (pushDistance > 0 && !pushed)
                        {
                            ob.Pushed(this, Direction, pushDistance);
                            pushed = true;
                        }

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType);
                        ActionList.Add(action);
                        break;
                    }
                }
            }     
        }
    
        protected virtual void ProjectileAttack(int damage, DefenceType type = DefenceType.ACAgility, int additionalDelay = 500)
        {
            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + additionalDelay;

            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, Target, damage, type);
            ActionList.Add(action);
        }

        protected virtual void SinglePushAttack(int damage, DefenceType type = DefenceType.AC, int delay = 500, int pushDistance = 3)
        {
            //Repulsion - (utilises DelayedAction so player is hit at end of push)
            //need to put Damage Stats (DC/MC/SC) on mob for it to push
            int levelGap = 5;
            int mobLevel = this.Level;
            int targetLevel = Target.Level;

            if ((targetLevel <= mobLevel + levelGap))
            {
                if (Target.Pushed(this, Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation), pushDistance) > 0)
                {
                    AttackTime = Envir.Time + AttackSpeed + 300;
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, type);
                    ActionList.Add(action);
                }
                else
                {
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, type);
                    ActionList.Add(action);
                }
            }
        }
    }
}
