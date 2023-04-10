using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirControls;
using S = ServerPackets;
using C = ClientPackets;
using Client.MirScenes.Dialogs;
using System.Reflection;

namespace Client.MirObjects
{
    public class PlayerObject : MapObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Player; }
        }

        public override bool Blocking
        {
            get { return !Dead; }
        }

        public MirGender Gender;
        public MirClass Class;
        public byte Hair;
        public ushort Level;

        public MLibrary WeaponLibrary1, WeaponEffectLibrary1, WeaponLibrary2, HairLibrary, WingLibrary, MountLibrary;
        public int Armour, Weapon, WeaponEffect, ArmourOffSet, HairOffSet, WeaponOffSet, WingOffset, MountOffset;

        public int DieSound, FlinchSound, AttackSound;


        public FrameSet Frames;
        public Frame Frame, WingFrame;
        public int FrameIndex, FrameInterval, EffectFrameIndex, EffectFrameInterval, SlowFrameIndex;
        public byte SkipFrameUpdate = 0;

        public bool HasClassWeapon
        {
            get
            {
                switch (Weapon / Globals.ClassWeaponCount)
                {
                    default:
                        return Class == MirClass.Wizard || Class == MirClass.Warrior || Class == MirClass.Taoist;
                    case 1:
                        return Class == MirClass.Assassin;
                    case 2:
                        return Class == MirClass.Archer;
                }
            }
        }

        public bool HasFishingRod
        {
            get
            {
                return Globals.FishingRodShapes.Contains(Weapon);
            }
        }

        public Spell Spell;
        public byte SpellLevel;
        public bool Cast;
        public uint TargetID;
        public List<uint> SecondaryTargetIDs;
        public Point TargetPoint;

        public bool MagicShield;
        public Effect ShieldEffect;

        public bool ElementalBarrier;
        public Effect ElementalBarrierEffect;

        public byte WingEffect;
        private short StanceDelay = 2500;

        //ArcherSpells - Elemental system
        public bool ElementalBuff;
        public bool Concentrating;
        public InterruptionEffect ConcentratingEffect;
        public bool ConcentrateInterrupted;
        public bool HasElements;
        public bool ElementCasted;
        public int ElementEffect;//hold orb count for player(object) load
        public int ElementsLevel;
        public int ElementOrbMax;
        //Elemental system END

        public SpellEffect CurrentEffect;

        public bool RidingMount, Sprint, FastRun, Fishing, FoundFish;
        public long StanceTime, MountTime, FishingTime;
        public long BlizzardStopTime, ReincarnationStopTime, SlashingBurstTime;

        public short MountType = -1, TransformType = -1;

        public string GuildName;
        public string GuildRankName;

        public Point FishingPoint;

        public LevelEffects LevelEffects;

        public PlayerObject() { }
        public PlayerObject(uint objectID) : base(objectID)
        {
            Frames = FrameSet.Player;
        }

        public void Load(S.ObjectPlayer info)
        {
            Name = info.Name;
            NameColour = info.NameColour;
            GuildName = info.GuildName;
            GuildRankName = info.GuildRankName;
            Class = info.Class;
            Gender = info.Gender;
            Level = info.Level;

            CurrentLocation = info.Location;
            MapLocation = info.Location;
            GameScene.Scene.MapControl.AddObject(this);

            Direction = info.Direction;
            Hair = info.Hair;

            Weapon = info.Weapon;
			WeaponEffect = info.WeaponEffect;
			Armour = info.Armour;
            Light = info.Light;

            Poison = info.Poison;

            Dead = info.Dead;
            Hidden = info.Hidden;

            WingEffect = info.WingEffect;
            CurrentEffect = info.Effect;

            MountType = info.MountType;
            RidingMount = info.RidingMount;

            Fishing = info.Fishing;

            TransformType = info.TransformType;

            SetLibraries();

            if (Dead) ActionFeed.Add(new QueuedAction { Action = MirAction.Dead, Direction = Direction, Location = CurrentLocation });
            if (info.Extra) Effects.Add(new Effect(Libraries.Magic2, 670, 10, 800, this));

            ElementEffect = (int)info.ElementOrbEffect;
            ElementsLevel = (int)info.ElementOrbLvl;
            ElementOrbMax = (int)info.ElementOrbMax;

            Buffs = info.Buffs;

            LevelEffects = info.LevelEffects;

            ProcessBuffs();

            SetAction();

            SetEffects();
        }
        public void Update(S.PlayerUpdate info)
        {
            Weapon = info.Weapon;
			WeaponEffect = info.WeaponEffect;
			Armour = info.Armour;
            Light = info.Light;
            WingEffect = info.WingEffect;

            SetLibraries();
            SetEffects();
        }

        public override bool ShouldDrawHealth()
        {
            if (GroupDialog.GroupList.Contains(Name) || this == User)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessBuffs()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                AddBuffEffect(Buffs[i]);
            }
        }

        public void MountUpdate(S.MountUpdate info)
        {
            MountType = info.MountType;
            RidingMount = info.RidingMount;

            QueuedAction action = new QueuedAction { Action = MirAction.Standing, Direction = Direction, Location = CurrentLocation };
            ActionFeed.Insert(0, action);

            MountTime = CMain.Time;

            if (MountType < 0)
                GameScene.Scene.MountDialog.Hide();

            SetLibraries();
            SetEffects();

            PlayMountSound();
        }

        public void FishingUpdate(S.FishingUpdate p)
        {
            if (Fishing != p.Fishing)
            {
                MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, p.FishingPoint);

                if (p.Fishing)
                {        
                    QueuedAction action = new QueuedAction { Action = MirAction.FishingCast, Direction = dir, Location = CurrentLocation };
                    ActionFeed.Add(action);
                }
                else
                {
                    QueuedAction action = new QueuedAction { Action = MirAction.FishingReel, Direction = dir, Location = CurrentLocation };
                    ActionFeed.Add(action);
                }

                Fishing = p.Fishing;
                SetLibraries();
            }

            if (!HasFishingRod)
            {
                GameScene.Scene.FishingDialog.Hide();
            }          

            FishingPoint = p.FishingPoint;
            FoundFish = p.FoundFish;
        }


        public virtual void SetLibraries()
        {
            //fishing broken
            //10
            //11
            //12
            //13

            //almost all broken
            //20 - black footballer - 791
            //21 - red footballer - 791
            //22 - blue footballer - 791
            //23 - green footballer - 791
            //24 - red2 footballer - 791

            bool altAnim = false;

            bool showMount = true;
            bool showFishing = true;

            if (TransformType > -1)
            {
                #region Transform
                
                switch (TransformType)
                {
                    case 4:
                    case 5:
                    case 7:
                    case 8:                
                    case 26:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                        showFishing = false;
                        break;
                    case 6:
                    case 9:
                        showMount = false;
                        showFishing = false;
                        break;
                    default:
                        break;
                }

                switch (CurrentAction)
                {
                    case MirAction.Standing:
                    case MirAction.Jump:
                        Frames.TryGetValue(MirAction.Standing, out Frame);
                        break;
                    case MirAction.Walking:
                    case MirAction.WalkingBow:
                        Frames.TryGetValue(MirAction.Walking, out Frame);
                        break;
                    case MirAction.Running:
                    case MirAction.RunningBow:
                        Frames.TryGetValue(MirAction.Running, out Frame);
                        break;
                    case MirAction.Attack1:
                    case MirAction.Attack2:
                    case MirAction.Attack3:
                    case MirAction.Attack4:
                    case MirAction.AttackRange1:
                    case MirAction.AttackRange2:
                    case MirAction.AttackRange3:
                        Frames.TryGetValue(MirAction.Attack1, out Frame);
                        break;
                }

                if (MountType > 6 && RidingMount)
                {
                    ArmourOffSet = -416;
                    BodyLibrary = TransformType < Libraries.TransformMounts.Length ? Libraries.TransformMounts[TransformType] : Libraries.TransformMounts[0];
                }
                else
                {
                    ArmourOffSet = 0;
                    BodyLibrary = TransformType < Libraries.Transform.Length ? Libraries.Transform[TransformType] : Libraries.Transform[0];
                }

                HairLibrary = null;
                WeaponLibrary1 = null;
                WeaponLibrary2 = null;

                if (TransformType == 19)
                {
                    WingEffect = 2;
                    WingLibrary = WingEffect - 1 < Libraries.TransformEffect.Length ? Libraries.TransformEffect[WingEffect - 1] : null;
                }
                else
                {
                    WingLibrary = null;
                }

                HairOffSet = 0;
                WeaponOffSet = 0;
                WingOffset = 0;
                MountOffset = 0;

                #endregion
            }
            else
            {

                switch (Class)
                {
                    #region Archer
                    case MirClass.Archer:

                        #region WeaponType
                        if (HasClassWeapon)
                        {
                            switch (CurrentAction)
                            {
                                case MirAction.Walking:
                                case MirAction.Running:
                                case MirAction.AttackRange1:
                                case MirAction.AttackRange2:
                                    altAnim = true;
                                    break;
                            }
                        }

                        if (CurrentAction == MirAction.Jump) altAnim = true;

                        #endregion

                        #region Armours
                        if (altAnim)
                        {
                            BodyLibrary = Armour < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour] : Libraries.ARArmours[0];
                            HairLibrary = Hair < Libraries.ARHair.Length ? Libraries.ARHair[Hair] : null;
                        }
                        else
                        {
                            BodyLibrary = Armour < Libraries.CArmours.Length ? Libraries.CArmours[Armour] : Libraries.CArmours[0];
                            HairLibrary = Hair < Libraries.CHair.Length ? Libraries.CHair[Hair] : null;
                        }
                        #endregion

                        #region Weapons
                        if (HasClassWeapon)
                        {
                            int Index = Weapon - 200;

                            if (altAnim)
                                WeaponLibrary2 = Index < Libraries.ARWeaponsS.Length ? Libraries.ARWeaponsS[Index] : null;
                            else
                                WeaponLibrary2 = Index < Libraries.ARWeapons.Length ? Libraries.ARWeapons[Index] : null;

                            WeaponLibrary1 = null;
                        }
                        else
                        {
							if (Weapon >= 0)
							{
								WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
								if (WeaponEffect > 0)
									WeaponEffectLibrary1 = WeaponEffect < Libraries.CWeaponEffect.Length ? Libraries.CWeaponEffect[WeaponEffect] : null;
								else
									WeaponEffectLibrary1 = null;

                                WeaponLibrary2 = null;
                            }
							else
							{
								WeaponLibrary1 = null;
								WeaponEffectLibrary1 = null;
								WeaponLibrary2 = null;
							}
						}
                        #endregion

                        #region WingEffects
                        if (WingEffect > 0 && WingEffect < 100)
                        {
                            if (altAnim)
                                WingLibrary = (WingEffect - 1) < Libraries.ARHumEffect.Length ? Libraries.ARHumEffect[WingEffect - 1] : null;
                            else
                                WingLibrary = (WingEffect - 1) < Libraries.CHumEffect.Length ? Libraries.CHumEffect[WingEffect - 1] : null;
                        }
                        #endregion

                        #region Offsets
                        ArmourOffSet = Gender == MirGender.Male ? 0 : altAnim ? 352 : 808;
                        HairOffSet = Gender == MirGender.Male ? 0 : altAnim ? 352 : 808;
                        WeaponOffSet = Gender == MirGender.Male ? 0 : altAnim ? 352 : 416;
                        WingOffset = Gender == MirGender.Male ? 0 : altAnim ? 352 : 840;
                        MountOffset = 0;
                        #endregion

                        break;
                    #endregion


                    #region Assassin
                    case MirClass.Assassin:

                        #region WeaponType
                        if (HasClassWeapon || Weapon < 0)
                        {
                            switch (CurrentAction)
                            {
                                case MirAction.Standing:
                                case MirAction.Stance:
                                case MirAction.Walking:
                                case MirAction.Running:
                                case MirAction.Die:
                                case MirAction.Struck:
                                case MirAction.Attack1:
                                case MirAction.Attack2:
                                case MirAction.Attack3:
                                case MirAction.Attack4:
                                case MirAction.Sneek:
                                case MirAction.Spell:
                                case MirAction.DashAttack:
                                    altAnim = true;
                                    break;
                            }
                        }
                        #endregion

                        #region Armours
                        if (altAnim)
                        {
                            BodyLibrary = Armour < Libraries.AArmours.Length ? Libraries.AArmours[Armour] : Libraries.AArmours[0];
                            HairLibrary = Hair < Libraries.AHair.Length ? Libraries.AHair[Hair] : null;
                        }
                        else
                        {
                            BodyLibrary = Armour < Libraries.CArmours.Length ? Libraries.CArmours[Armour] : Libraries.CArmours[0];
                            HairLibrary = Hair < Libraries.CHair.Length ? Libraries.CHair[Hair] : null;
                        }
                        #endregion

                        #region Weapons
                        if (HasClassWeapon)
                        {
                            int Index = Weapon - 100;

                            WeaponLibrary1 = Index < Libraries.AWeaponsL.Length ? Libraries.AWeaponsR[Index] : null;
                            WeaponLibrary2 = Index < Libraries.AWeaponsR.Length ? Libraries.AWeaponsL[Index] : null;
                        }
                        else
                        {
							if (Weapon >= 0)
							{
								WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
								if (WeaponEffect > 0)
									WeaponEffectLibrary1 = WeaponEffect < Libraries.CWeaponEffect.Length ? Libraries.CWeaponEffect[WeaponEffect] : null;
								else
									WeaponEffectLibrary1 = null;

                                WeaponLibrary2 = null;
                            }
							else
							{
								WeaponLibrary1 = null;
								WeaponEffectLibrary1 = null;
								WeaponLibrary2 = null;
							}
						}
                        #endregion

                        #region WingEffects
                        if (WingEffect > 0 && WingEffect < 100)
                        {
                            if (altAnim)
                                WingLibrary = (WingEffect - 1) < Libraries.AHumEffect.Length ? Libraries.AHumEffect[WingEffect - 1] : null;
                            else
                                WingLibrary = (WingEffect - 1) < Libraries.CHumEffect.Length ? Libraries.CHumEffect[WingEffect - 1] : null;
                        }
                        #endregion

                        #region Offsets
                        ArmourOffSet = Gender == MirGender.Male ? 0 : altAnim ? 512 : 808;
                        HairOffSet = Gender == MirGender.Male ? 0 : altAnim ? 512 : 808;
                        WeaponOffSet = Gender == MirGender.Male ? 0 : altAnim ? 512 : 416;
                        WingOffset = Gender == MirGender.Male ? 0 : altAnim ? 544 : 840;
                        MountOffset = 0;
                        #endregion

                        break;
                    #endregion


                    #region Others
                    case MirClass.Warrior:
                    case MirClass.Taoist:
                    case MirClass.Wizard:

                        #region Armours
                        BodyLibrary = Armour < Libraries.CArmours.Length ? Libraries.CArmours[Armour] : Libraries.CArmours[0];
                        HairLibrary = Hair < Libraries.CHair.Length ? Libraries.CHair[Hair] : null;
						#endregion

						#region Weapons

						if (Weapon >= 0)
						{
							WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
							if (WeaponEffect > 0)
								WeaponEffectLibrary1 = WeaponEffect < Libraries.CWeaponEffect.Length ? Libraries.CWeaponEffect[WeaponEffect] : null;
							else
								WeaponEffectLibrary1 = null;
						}
						else
						{
							WeaponLibrary1 = null;
							WeaponEffectLibrary1 = null;
							WeaponLibrary2 = null;
						}

						#endregion

						#region WingEffects
						if (WingEffect > 0 && WingEffect < 100)
                        {
                            WingLibrary = (WingEffect - 1) < Libraries.CHumEffect.Length ? Libraries.CHumEffect[WingEffect - 1] : null;
                        }
                        #endregion

                        #region Offsets
                        ArmourOffSet = Gender == MirGender.Male ? 0 : 808;
                        HairOffSet = Gender == MirGender.Male ? 0 : 808;
                        WeaponOffSet = Gender == MirGender.Male ? 0 : 416;
                        WingOffset = Gender == MirGender.Male ? 0 : 840;
                        MountOffset = 0;
                        #endregion

                        break;
                    #endregion
                }
            }

            #region Common
            //Harvest
            if (CurrentAction == MirAction.Harvest && TransformType < 0)
            {
                WeaponLibrary1 = 1 < Libraries.CWeapons.Length ? Libraries.CWeapons[1] : null;
            }

            //Mounts
            if (MountType > -1 && RidingMount && showMount)
            {
                MountLibrary = MountType < Libraries.Mounts.Length ? Libraries.Mounts[MountType] : null;
            }
            else
            {
                MountLibrary = null;
            }

            //Fishing
            if (HasFishingRod && showFishing)
            {
                if (CurrentAction == MirAction.FishingCast || CurrentAction == MirAction.FishingWait || CurrentAction == MirAction.FishingReel)
                {
                    WeaponLibrary1 = 0 < Libraries.Fishing.Length ? Libraries.Fishing[Weapon - 49] : null;
                    WeaponLibrary2 = null;
                    WeaponOffSet = -632;
                }
            }

            DieSound = Gender == MirGender.Male ? SoundList.MaleDie : SoundList.FemaleDie;
            FlinchSound = Gender == MirGender.Male ? SoundList.MaleFlinch : SoundList.FemaleFlinch;
            #endregion
        }

        public virtual void SetEffects()
        {
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                if (Effects[i] is SpecialEffect) Effects[i].Remove();
            }

            if (RidingMount) return;

            if (CurrentEffect == SpellEffect.MagicShieldUp)
            {
                if (ShieldEffect != null)
                {
                    ShieldEffect.Clear();
                    ShieldEffect.Remove();
                }

                MagicShield = true;
                Effects.Add(ShieldEffect = new Effect(Libraries.Magic, 3890, 3, 600, this) { Repeat = true });
            }

            if (WingEffect >= 100)
            {
                switch(WingEffect)
                {
                    case 100: //Oma King Robe effect
                        Effects.Add(new SpecialEffect(Libraries.Effect, 352, 33, 3600, this, true, false, 0) { Repeat = true });
                        break;
                }
            }

			long delay = 5000;

            if (LevelEffects == LevelEffects.None) return;

            //Effects dependant on flags
            if (LevelEffects.HasFlag(LevelEffects.BlueDragon))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1210, 20, 3200, this, true, true, 1) { Repeat = true });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1240, 32, 4200, this, true, false, 1) { Repeat = true };
                effect.SetStart(CMain.Time);
                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.RedDragon))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 990, 20, 3200, this, true, true, 1) { Repeat = true });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1020, 32, 4200, this, true, false, 1) { Repeat = true, Delay = delay };
                effect.SetStart(CMain.Time + delay);
                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.Mist))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 296, 32, 3600, this, true, false, 1) { Repeat = true });
            }

            if (LevelEffects.HasFlag(LevelEffects.Rebirth1))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1271, 36, 3600, this, true, false, 1) { Repeat = true });


            }

            if (LevelEffects.HasFlag(LevelEffects.Rebirth2))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1308, 38, 3600, this, true, false, 1) { Repeat = true });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1347, 39, 3600, this, true, true, 1) { Repeat = true, Delay = delay };
                effect.SetStart(CMain.Time + delay);
                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.Rebirth3))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1409, 19, 3600, this, true, false, 1) { Repeat = true, Delay = delay });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1444, 25, 4600, this, true, true, 1) { Repeat = true };

                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.NewBlue))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1574, 31, 3600, this, true, false, 1) { Repeat = true, Delay = delay });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1621, 24, 3600, this, true, true, 1) { Repeat = true };

                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.YellowDragon))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1486, 32, 4600, this, true, false, 1) { Repeat = true, Delay = delay });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1534, 24, 4600, this, true, true, 1) { Repeat = true };

                Effects.Add(effect);
            }

            if (LevelEffects.HasFlag(LevelEffects.Phoenix))
            {
                Effects.Add(new SpecialEffect(Libraries.Effect, 1663, 26, 3600, this, true, false, 1) { Repeat = true, Delay = delay });
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1705, 21, 3600, this, true, true, 1) { Repeat = true };

                Effects.Add(effect);
            }
        }

        public override void Process()
        {
            bool update = CMain.Time >= NextMotion || GameScene.CanMove;

            if (this == User)
            {
                if (CMain.Time - GameScene.LastRunTime > 899)
                    GameScene.CanRun = false;
            }

            SkipFrames = this != User && ActionFeed.Count > 1;

            ProcessFrames();

            if (Frame == null)
            {
                DrawFrame = 0;
                DrawWingFrame = 0;
            }
            else
            {
                DrawFrame = Frame.Start + (Frame.OffSet * (byte)Direction) + FrameIndex;
                DrawWingFrame = Frame.EffectStart + (Frame.EffectOffSet * (byte)Direction) + EffectFrameIndex;
            }

            #region Moving OffSet

            switch (CurrentAction)
            {
                case MirAction.Walking:
                case MirAction.Running:
                case MirAction.MountWalking:
                case MirAction.MountRunning:
                case MirAction.Pushed:
                case MirAction.DashL:
                case MirAction.DashR:
                case MirAction.Sneek:
                case MirAction.Jump:
                case MirAction.DashAttack:
                    if (Frame == null)
                    {
                        OffSetMove = Point.Empty;
                        Movement = CurrentLocation;
                        break;
                    }

                    var i = 0;
                    if (CurrentAction == MirAction.MountRunning) i = 3;
                    else if (CurrentAction == MirAction.Running) 
                        i = (Sprint && !Sneaking ? 3 : 2);
                    else i = 1;

                    if (CurrentAction == MirAction.Jump) i = -JumpDistance;
                    if (CurrentAction == MirAction.DashAttack) i = JumpDistance;

                    Movement = Functions.PointMove(CurrentLocation, Direction, CurrentAction == MirAction.Pushed ? 0 : -i);

                    int count = Frame.Count;
                    int index = FrameIndex;

                    if (CurrentAction == MirAction.DashR || CurrentAction == MirAction.DashL)
                    {
                        count = 3;
                        index %= 3;
                    }

                    switch (Direction)
                    {
                        case MirDirection.Up:
                            OffSetMove = new Point(0, (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.UpRight:
                            OffSetMove = new Point((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Right:
                            OffSetMove = new Point((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), 0);
                            break;
                        case MirDirection.DownRight:
                            OffSetMove = new Point((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Down:
                            OffSetMove = new Point(0, (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.DownLeft:
                            OffSetMove = new Point((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Left:
                            OffSetMove = new Point((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), 0);
                            break;
                        case MirDirection.UpLeft:
                            OffSetMove = new Point((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                    }

                    OffSetMove = new Point(OffSetMove.X % 2 + OffSetMove.X, OffSetMove.Y % 2 + OffSetMove.Y);
                    break;
                default:
                    OffSetMove = Point.Empty;
                    Movement = CurrentLocation;
                    break;
            }

            #endregion


            DrawY = Movement.Y > CurrentLocation.Y ? Movement.Y : CurrentLocation.Y;

            DrawLocation = new Point((Movement.X - User.Movement.X + MapControl.OffSetX) * MapControl.CellWidth, (Movement.Y - User.Movement.Y + MapControl.OffSetY) * MapControl.CellHeight);
            DrawLocation.Offset(GlobalDisplayLocationOffset);

            if (this != User)
            {
                DrawLocation.Offset(User.OffSetMove);
                DrawLocation.Offset(-OffSetMove.X, -OffSetMove.Y);
            }

            if (BodyLibrary != null && update)
            {
                FinalDrawLocation = DrawLocation.Add(BodyLibrary.GetOffSet(DrawFrame));
                DisplayRectangle = new Rectangle(DrawLocation, BodyLibrary.GetTrueSize(DrawFrame));
            }

            for (int i = 0; i < Effects.Count; i++)
                Effects[i].Process();

            Color colour = DrawColour;
            DrawColour = Color.White;
            if (Poison != PoisonType.None)
            {
                
                if (Poison.HasFlag(PoisonType.Green))
                    DrawColour = Color.Green;
                if (Poison.HasFlag(PoisonType.Red))
                    DrawColour = Color.Red;
                if (Poison.HasFlag(PoisonType.Bleeding))
                    DrawColour = Color.DarkRed;
                if (Poison.HasFlag(PoisonType.Slow))
                    DrawColour = Color.Purple;
                if (Poison.HasFlag(PoisonType.Stun) || Poison.HasFlag(PoisonType.Dazed))
                    DrawColour = Color.Yellow;
                if (Poison.HasFlag(PoisonType.Blindness))
                    DrawColour = Color.MediumVioletRed;
                if (Poison.HasFlag(PoisonType.Frozen))
                    DrawColour = Color.Blue;
                if (Poison.HasFlag(PoisonType.Paralysis) || Poison.HasFlag(PoisonType.LRParalysis))
                    DrawColour = Color.Gray;             
                if (Poison.HasFlag(PoisonType.DelayedExplosion))
                    DrawColour = Color.Orange;
            }


            if (colour != DrawColour) GameScene.Scene.MapControl.TextureValid = false;
        }
        public virtual void SetAction()
        {
            if (NextAction != null && !GameScene.CanMove)
            {
                switch (NextAction.Action)
                {
                    case MirAction.Walking:
                    case MirAction.Running:
                    case MirAction.MountWalking:
                    case MirAction.MountRunning:
                    case MirAction.Pushed:
                    case MirAction.DashL:
                    case MirAction.DashR:
                    case MirAction.Sneek:
                    case MirAction.Jump:
                    case MirAction.DashAttack:
                        return;
                }
            }

            if (User == this && CMain.Time < MapControl.NextAction)// && CanSetAction)
            {
                //NextMagic = null;
                return;
            }


            if (ActionFeed.Count == 0)
            {
                CurrentAction = MirAction.Standing;

                CurrentAction = CMain.Time > BlizzardStopTime ? CurrentAction : MirAction.Stance2;
                //CurrentAction = CMain.Time > SlashingBurstTime ? CurrentAction : MirAction.Lunge;

                if (RidingMount)
                {
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            CurrentAction = MirAction.MountStanding;
                            break;
                        case MirAction.Walking:
                            CurrentAction = MirAction.MountWalking;
                            break;
                        case MirAction.Running:
                            CurrentAction = MirAction.MountRunning;
                            break;
                        case MirAction.Struck:
                            CurrentAction = MirAction.MountStruck;
                            break;
                        case MirAction.Attack1:
                            CurrentAction = MirAction.MountAttack;
                            break;
                    }
                }

                if (CurrentAction == MirAction.Standing)
                {
                    if (Class == MirClass.Archer && HasClassWeapon)
                        CurrentAction = MirAction.Standing;
                    else
                        CurrentAction = CMain.Time > StanceTime ? MirAction.Standing : MirAction.Stance;
                }

                if (Fishing) CurrentAction = MirAction.FishingWait;

                Frames.TryGetValue(CurrentAction, out Frame);
                FrameIndex = 0;
                EffectFrameIndex = 0;

                if (MapLocation != CurrentLocation)
                {
                    GameScene.Scene.MapControl.RemoveObject(this);
                    MapLocation = CurrentLocation;
                    GameScene.Scene.MapControl.AddObject(this);
                }

                if (Frame == null) return;

                FrameInterval = Frame.Interval;
                EffectFrameInterval = Frame.EffectInterval;

                SetLibraries();
            }
            else
            {
                QueuedAction action = ActionFeed[0];
                ActionFeed.RemoveAt(0);


                CurrentAction = action.Action;

                if (RidingMount)
                {
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            CurrentAction = MirAction.MountStanding;
                            break;
                        case MirAction.Walking:
                            CurrentAction = MirAction.MountWalking;
                            break;
                        case MirAction.Running:
                            CurrentAction = MirAction.MountRunning;
                            break;
                        case MirAction.Struck:
                            CurrentAction = MirAction.MountStruck;
                            break;
                        case MirAction.Attack1:
                            CurrentAction = MirAction.MountAttack;
                            break;
                    }
                }

                CurrentLocation = action.Location;
                MirDirection olddirection = Direction;
                Direction = action.Direction;

                Point temp;
                switch (CurrentAction)
                {
                    case MirAction.Walking:
                    case MirAction.Running:
                    case MirAction.MountWalking:
                    case MirAction.MountRunning:
                    case MirAction.Pushed:
                    case MirAction.DashL:
                    case MirAction.DashR:
                    case MirAction.Sneek:
                        var steps = 0;
                        if (CurrentAction == MirAction.MountRunning) steps = 3;
                        else if (CurrentAction == MirAction.Running) steps = (Sprint && !Sneaking ? 3 : 2);
                        else steps = 1;

                        temp = Functions.PointMove(CurrentLocation, Direction, CurrentAction == MirAction.Pushed ? 0 : -steps);

                        break;
                    case MirAction.Jump:
                    case MirAction.DashAttack:
                        temp = Functions.PointMove(CurrentLocation, Direction, JumpDistance);
                        break;
                    default:
                        temp = CurrentLocation;
                        break;
                }

                temp = new Point(action.Location.X, temp.Y > CurrentLocation.Y ? temp.Y : CurrentLocation.Y);

                if (MapLocation != temp)
                {
                    GameScene.Scene.MapControl.RemoveObject(this);
                    MapLocation = temp;
                    GameScene.Scene.MapControl.AddObject(this);
                }


                bool ArcherLayTrap = false;

                switch (CurrentAction)
                {
                    case MirAction.Pushed:
                        if (this == User)
                            MapControl.InputDelay = CMain.Time + 500;
                        Frames.TryGetValue(MirAction.Walking, out Frame);
                        break;
                    case MirAction.DashL:
                    case MirAction.DashR:
                        Frames.TryGetValue(MirAction.Running, out Frame);
                        break;
                    case MirAction.DashAttack:
                        Frames.TryGetValue(MirAction.DashAttack, out Frame);
                        break;
                    case MirAction.DashFail:
                        Frames.TryGetValue(RidingMount ? MirAction.MountStanding : MirAction.Standing, out Frame);
                        //Frames.TryGetValue(MirAction.Standing, out Frame);
                        //CanSetAction = false;
                        break;
                    case MirAction.Jump:
                        Frames.TryGetValue(MirAction.Jump, out Frame);
                        break;
                    case MirAction.Attack1:
                        switch (Class)
                        {
                            case MirClass.Archer:
                                Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                            case MirClass.Assassin:
                                if(GameScene.User.DoubleSlash)
                                    Frames.TryGetValue(MirAction.Attack1, out Frame);
                                else if (CMain.Shift)
                                    Frames.TryGetValue(CMain.Random.Next(100) >= 20 ? (CMain.Random.Next(100) > 40 ? MirAction.Attack1 : MirAction.Attack4) : (CMain.Random.Next(100) > 10 ? MirAction.Attack2 : MirAction.Attack3), out Frame);
                                else
                                    Frames.TryGetValue(CMain.Random.Next(100) >= 40 ? MirAction.Attack1 : MirAction.Attack4, out Frame);
                                break;
                            default:
                                if (CMain.Shift && TargetObject == null)
                                    Frames.TryGetValue(CMain.Random.Next(100) >= 20 ? MirAction.Attack1 : MirAction.Attack3, out Frame);
                                else
                                    Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        break;
                    case MirAction.Attack4:
                        Spell = (Spell)action.Params[0];
                        Frames.TryGetValue(Spell == Spell.TwinDrakeBlade || Spell == Spell.FlamingSword ? MirAction.Attack1 : CurrentAction, out Frame);
                        break;
                    case MirAction.Spell:
                        Spell = (Spell)action.Params[0];
                        switch (Spell)
                        {
                            case Spell.ShoulderDash:
                                Frames.TryGetValue(MirAction.Running, out Frame);
                                CurrentAction = MirAction.DashL;
                                Direction = olddirection;
                                CurrentLocation = Functions.PointMove(CurrentLocation, Direction, 1);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2500;
                                    GameScene.SpellTime = CMain.Time + 2500; //Spell Delay

                                    Network.Enqueue(new C.Magic { ObjectID = GameScene.User.ObjectID, Spell = Spell, Direction = Direction, });
                                }
                                break;
                            case Spell.BladeAvalanche:
                                Frames.TryGetValue(MirAction.Attack3, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2500;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                    MapControl.InputDelay = CMain.Time + 1000;
                                }
                                break;
                            case Spell.SlashingBurst:
                                 Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2000; // 80%
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.CounterAttack:
                                Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    GameScene.AttackTime = CMain.Time + User.AttackSpeed;
                                    MapControl.NextAction = CMain.Time + 100; // 80%
                                    GameScene.SpellTime = CMain.Time + 100; //Spell Delay
                                }
                                break;
                            case Spell.PoisonSword:
                                Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2000; // 80%
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.HeavenlySword:
                                Frames.TryGetValue(MirAction.Attack2, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1200;
                                    GameScene.SpellTime = CMain.Time + 1200; //Spell Delay
                                }
                                break;
                            case Spell.CrescentSlash:
                                Frames.TryGetValue(MirAction.Attack3, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2500;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.FlashDash:
                                {
                                    int sLevel = (byte)action.Params[3];

                                    GetFlashDashDistance(sLevel);

                                    if (JumpDistance != 0)
                                    {
                                        Frames.TryGetValue(MirAction.DashAttack, out Frame);
                                        CurrentAction = MirAction.DashAttack;
                                        CurrentLocation = Functions.PointMove(CurrentLocation, Direction, JumpDistance);
                                    }
                                    else
                                    {
                                        Frames.TryGetValue(CMain.Random.Next(100) >= 40 ? MirAction.Attack1 : MirAction.Attack4, out Frame);
                                    }

                                    if (this == User)
                                    {
                                        MapControl.NextAction = CMain.Time;
                                        GameScene.SpellTime = CMain.Time + 250; //Spell Delay
                                        if (JumpDistance != 0) Network.Enqueue(new C.Magic { Spell = Spell, Direction = Direction });
                                    }
                                }
                                break;
                            case Spell.StraightShot:
                                Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.DoubleShot:                          
                                Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 500; //Spell Delay
                                }
                                break;
                            case Spell.ExplosiveTrap:
                                Frames.TryGetValue(MirAction.Harvest, out Frame);
                                CurrentAction = MirAction.Harvest;
                                ArcherLayTrap = true;
                                if (this == User)
                                {
                                    uint targetID = (uint)action.Params[1];
                                    Point location = (Point)action.Params[2];
                                    Network.Enqueue(new C.Magic { ObjectID = GameScene.User.ObjectID, Spell = Spell, Direction = Direction, TargetID = targetID, Location = location });
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.DelayedExplosion:
                                Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.BackStep:
                                {
                                    int sLevel = (byte)action.Params[3];
                                    GetBackStepDistance(sLevel);
                                    Frames.TryGetValue(MirAction.Jump, out Frame);
                                    CurrentAction = MirAction.Jump;
                                    CurrentLocation = Functions.PointMove(CurrentLocation, Functions.ReverseDirection(Direction), JumpDistance);
                                    if (this == User)
                                    {
                                        MapControl.NextAction = CMain.Time + 800;
                                        GameScene.SpellTime = CMain.Time + 2500; //Spell Delay
                                        Network.Enqueue(new C.Magic { ObjectID = GameScene.User.ObjectID, Spell = Spell, Direction = Direction });
                                    }
                                    break;
                                }
                            case Spell.ElementalShot:
                                if (HasElements && !ElementCasted)
                                {
                                    Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                    CurrentAction = MirAction.AttackRange2;
                                    if (this == User)
                                    {
                                        MapControl.NextAction = CMain.Time + 1000;
                                        GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                    }
                                }
                                else Frames.TryGetValue(CurrentAction, out Frame);
                                if (ElementCasted) ElementCasted = false;
                                break;
                            case Spell.BindingShot:
                            case Spell.VampireShot:
                            case Spell.PoisonShot:
                            case Spell.CrippleShot:
                            case Spell.NapalmShot:
                            case Spell.SummonVampire:
                            case Spell.SummonToad:
                            case Spell.SummonSnakes:
                            case Spell.Stonetrap:
                                Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1000; //Spell Delay
                                }
                                break;
                            default:
                                Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        
                        break;
                    default:
                        Frames.TryGetValue(CurrentAction, out Frame);
                        break;

                }

                //ArcherTest - Need to check for bow weapon only
                if (Class == MirClass.Archer && HasClassWeapon)
                {
                    switch (CurrentAction)
                    {
                        case MirAction.Walking:
                            Frames.TryGetValue(MirAction.WalkingBow, out Frame);
                            break;
                        case MirAction.Running:
                            Frames.TryGetValue(MirAction.RunningBow, out Frame);
                            break;
                    }
                }

                //Assassin sneekyness
                if (Class == MirClass.Assassin && Sneaking && (CurrentAction == MirAction.Walking || CurrentAction == MirAction.Running))
                {
                    Frames.TryGetValue(MirAction.Sneek, out Frame);
                }

                SetLibraries();

                FrameIndex = 0;
                EffectFrameIndex = 0;
                Spell = Spell.None;
                SpellLevel = 0;
                //NextMagic = null;

                ClientMagic magic;

                if (Frame == null) return;

                FrameInterval = Frame.Interval;
                EffectFrameInterval = Frame.EffectInterval;

                if (this == User)
                {
                    switch (CurrentAction)
                    {
                        case MirAction.DashFail:
                            //CanSetAction = false;
                            break;
                        case MirAction.Standing:
                        case MirAction.MountStanding:
                            Network.Enqueue(new C.Turn { Direction = Direction });
                            MapControl.NextAction = CMain.Time + 2500;
                            GameScene.CanRun = false;
                            break;
                        case MirAction.Walking:
                        case MirAction.MountWalking:
                        case MirAction.Sneek:
                            GameScene.LastRunTime = CMain.Time;
                            Network.Enqueue(new C.Walk { Direction = Direction });
                            GameScene.Scene.MapControl.FloorValid = false;
                            GameScene.CanRun = true;
                            MapControl.NextAction = CMain.Time + 2500;
                            break;
                        case MirAction.Running:
                        case MirAction.MountRunning:
                            GameScene.LastRunTime = CMain.Time;
                            Network.Enqueue(new C.Run { Direction = Direction });
                            GameScene.Scene.MapControl.FloorValid = false;
                            MapControl.NextAction = CMain.Time + (Sprint ? 1000 : 2500);
                            break;
                        case MirAction.Pushed:
                            GameScene.LastRunTime = CMain.Time;
                            GameScene.Scene.MapControl.FloorValid = false;
                            MapControl.InputDelay = CMain.Time + 500;
                            GameScene.CanRun = false;
                            GameScene.CanMove = false;
                            break;
                        case MirAction.DashL:
                        case MirAction.DashR:
                        case MirAction.Jump:
                        case MirAction.DashAttack:
                            GameScene.LastRunTime = CMain.Time;
                            GameScene.Scene.MapControl.FloorValid = false;
                            GameScene.CanRun = false;
                            //CanSetAction = false;
                            break;
                        case MirAction.Mine:
                            Network.Enqueue(new C.Attack { Direction = Direction, Spell = Spell.None });
                            GameScene.AttackTime = CMain.Time + (1400 - Math.Min(370, (User.Level * 14)));
                            MapControl.NextAction = CMain.Time + 2500;
                            break;
                        case MirAction.Attack1:
                        case MirAction.MountAttack:

                            if (!RidingMount)
                            {
                                if (GameScene.User.Slaying && TargetObject != null)
                                    Spell = Spell.Slaying;

                                if (GameScene.User.Thrusting && GameScene.Scene.MapControl.HasTarget(Functions.PointMove(CurrentLocation, Direction, 2)))
                                    Spell = Spell.Thrusting;

                                if (GameScene.User.HalfMoon)
                                {
                                    if (TargetObject != null || GameScene.Scene.MapControl.CanHalfMoon(CurrentLocation, Direction))
                                    {
                                        magic = User.GetMagic(Spell.HalfMoon);
                                        if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                            Spell = Spell.HalfMoon;
                                    }
                                }

                                if (GameScene.User.CrossHalfMoon)
                                {
                                    if (TargetObject != null || GameScene.Scene.MapControl.CanCrossHalfMoon(CurrentLocation))
                                    {
                                        magic = User.GetMagic(Spell.CrossHalfMoon);
                                        if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                            Spell = Spell.CrossHalfMoon;
                                    }
                                }

                                if (GameScene.User.DoubleSlash)
                                {
                                    magic = User.GetMagic(Spell.DoubleSlash);
                                    if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                        Spell = Spell.DoubleSlash;
                                }


                                if (GameScene.User.TwinDrakeBlade && TargetObject != null)
                                {
                                    magic = User.GetMagic(Spell.TwinDrakeBlade);
                                    if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                        Spell = Spell.TwinDrakeBlade;
                                }

                                if (GameScene.User.FlamingSword)
                                {
                                    if (TargetObject != null)
                                    {
                                        magic = User.GetMagic(Spell.FlamingSword);
                                        if (magic != null)
                                        {
                                            Spell = Spell.FlamingSword;
                                            magic.CastTime = CMain.Time;
                                        }
                                    }
                                }
                            }

                            Network.Enqueue(new C.Attack { Direction = Direction, Spell = Spell });

                            if (Spell == Spell.Slaying)
                                GameScene.User.Slaying = false;
                            if (Spell == Spell.TwinDrakeBlade)
                                GameScene.User.TwinDrakeBlade = false;
                            if (Spell == Spell.FlamingSword)
                                GameScene.User.FlamingSword = false;

                            magic = User.GetMagic(Spell);

                            if (magic != null) SpellLevel = magic.Level;

                            GameScene.AttackTime = CMain.Time + User.AttackSpeed;
                            MapControl.NextAction = CMain.Time + 2500;
                            break;
                        case MirAction.Attack2:
                            //Network.Enqueue(new C.Attack2 { Direction = Direction });
                            break;
                        case MirAction.Attack3:
                            //Network.Enqueue(new C.Attack3 { Direction = Direction });
                            break;
                        //case MirAction.Attack4:
                        //    GameScene.AttackTime = CMain.Time;// + User.AttackSpeed;
                        //    MapControl.NextAction = CMain.Time;
                        //    break;

                        case MirAction.AttackRange1:
                            {
                                GameScene.AttackTime = CMain.Time + User.AttackSpeed + 200;

                                uint targetID = (uint)action.Params[0];
                                Point location = (Point)action.Params[1];

                                Network.Enqueue(new C.RangeAttack { Direction = Direction, Location = CurrentLocation, TargetID = targetID, TargetLocation = location });
                            }
                            break;
                        case MirAction.AttackRange2:
                        case MirAction.Spell:
                            {
                                Spell = (Spell)action.Params[0];
                                uint targetID = (uint)action.Params[1];
                                Point location = (Point)action.Params[2];

                                Network.Enqueue(new C.Magic { ObjectID = GameScene.User.ObjectID, Spell = Spell, Direction = Direction, TargetID = targetID, Location = location });

                                if (Spell == Spell.FlashDash)
                                {
                                    GameScene.SpellTime = CMain.Time + 250;
                                    MapControl.NextAction = CMain.Time;
                                }
                                else
                                {
                                    GameScene.SpellTime = Spell == Spell.FlameField ? CMain.Time + 2500 : CMain.Time + 1800;
                                    MapControl.NextAction = CMain.Time + 2500;
                                }
                            }
                            break;                         
                        case MirAction.Harvest:
                            if (ArcherLayTrap)
                            {
                                ArcherLayTrap = false;
                                SoundManager.PlaySound(20000 + 124 * 10);
                            }
                            else
                            {
                                Network.Enqueue(new C.Harvest { Direction = Direction });
                                MapControl.NextAction = CMain.Time + 2500;
                            }
                            break;

                    }
                }


                switch (CurrentAction)
                {
                    case MirAction.Pushed:
                        FrameIndex = Frame.Count - 1;
                        EffectFrameIndex = Frame.EffectCount - 1;
                        GameScene.Scene.Redraw();
                        break;
                    case MirAction.DashL:
                    case MirAction.Jump:
                        FrameIndex = 0;
                        EffectFrameIndex = 0;
                        GameScene.Scene.Redraw();
                        break;
                    case MirAction.DashR:
                        FrameIndex = 3;
                        EffectFrameIndex = 3;
                        GameScene.Scene.Redraw();
                        break;
                    case MirAction.Walking:
                    case MirAction.Running:
                    case MirAction.MountWalking:
                    case MirAction.MountRunning:
                    case MirAction.Sneek:
                        GameScene.Scene.Redraw();
                        break;
                    case MirAction.DashAttack:
                        //FrameIndex = 0;
                        //EffectFrameIndex = 0;
                        GameScene.Scene.Redraw();

                        if (IsDashAttack())
                        {
                            action = new QueuedAction { Action = MirAction.Attack4, Direction = Direction, Location = CurrentLocation, Params = new List<object>() };
                            action.Params.Add(Spell.FlashDash);
                            ActionFeed.Insert(0, action);
                        }
                        break;
                    case MirAction.Attack1:
                        if (this != User)
                        {
                            Spell = (Spell)action.Params[0];
                            SpellLevel = (byte)action.Params[1];
                        }

                        switch (Spell)
                        {
                            case Spell.Slaying:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + (Gender == MirGender.Male ? 0 : 1));
                                break;
                            case Spell.DoubleSlash:
                                FrameInterval = (int)(FrameInterval * 0.46f); //46% Animation Speed
                                EffectFrameInterval = (int)(EffectFrameInterval * 0.46f);

                                action = new QueuedAction { Action = MirAction.Attack4, Direction = Direction, Location = CurrentLocation, Params = new List<object>() };
                                action.Params.Add(Spell);
                                ActionFeed.Insert(0, action);

                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;
                            case Spell.Thrusting:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;
                            case Spell.HalfMoon:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            case Spell.TwinDrakeBlade:
                                //FrameInterval = FrameInterval * 9 / 10; //70% Faster Animation
                                //EffectFrameInterval = EffectFrameInterval * 9 / 10;
                                //action = new QueuedAction { Action = MirAction.Attack4, Direction = Direction, Location = CurrentLocation, Params = new List<object>() };
                                //action.Params.Add(Spell);
                                //ActionFeed.Insert(0, action);
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            case Spell.CrossHalfMoon:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            case Spell.FlamingSword:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                break;

                            
                        }
                        break;
                    case MirAction.Attack4:
                        Spell = (Spell)action.Params[0];
                        switch (Spell)
                        {
                            case Spell.DoubleSlash:
                                FrameInterval = (int)(FrameInterval * 0.46f); //46% Animation Speed
                                EffectFrameInterval = (int)(EffectFrameInterval * 0.46f);

                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                break;
                            case Spell.TwinDrakeBlade:
                                FrameInterval = FrameInterval * 9 / 10; //80% Animation Speed
                                EffectFrameInterval = EffectFrameInterval * 9 / 10;
                                break;
                            case Spell.FlashDash:
                                int attackDelay = (User.AttackSpeed - 120) <= 300 ? 300 : (User.AttackSpeed - 120);

                                float attackRate = (float)(attackDelay / 300F * 10F);
                                FrameInterval = FrameInterval * (int)attackRate / 20;
                                EffectFrameInterval = EffectFrameInterval * (int)attackRate / 20;
                                break;
                        }
                        break;
                    case MirAction.Struck:
                    case MirAction.MountStruck:
                        uint attackerID = (uint)action.Params[0];
                        StruckWeapon = -2;
                        for (int i = 0; i < MapControl.Objects.Count; i++)
                        {
                            MapObject ob = MapControl.Objects[i];
                            if (ob.ObjectID != attackerID) continue;
                            if (ob.Race != ObjectType.Player) break;
                            PlayerObject player = ((PlayerObject)ob);
                            StruckWeapon = player.Weapon;
                            if (player.Class != MirClass.Assassin || StruckWeapon == -1) break;
                            StruckWeapon = 1;
                            break;
                        }

                        PlayStruckSound();
                        PlayFlinchSound();
                        break;
                    case MirAction.AttackRange1: //ArcherTest - Assign Target for other users
                        if (this != User)
                        {
                            TargetID = (uint)action.Params[0];
                            TargetPoint = (Point)action.Params[1];
                            Spell = (Spell)action.Params[2];
                        }
                        break;
                    case MirAction.AttackRange2:
                    case MirAction.Spell:
                        if (this != User)
                        {
                            Spell = (Spell)action.Params[0];
                            TargetID = (uint)action.Params[1];
                            TargetPoint = (Point)action.Params[2];
                            Cast = (bool)action.Params[3];
                            SpellLevel = (byte)action.Params[4];
                            SecondaryTargetIDs = (List<uint>)action.Params[5];
                        }

                        switch (Spell)
                        {
                            #region FireBall

                            case Spell.FireBall:
                                Effects.Add(new Effect(Libraries.Magic, 0, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Healing

                            case Spell.Healing:
                                Effects.Add(new Effect(Libraries.Magic, 200, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Repulsion

                            case Spell.Repulsion:
                                Effects.Add(new Effect(Libraries.Magic, 900, 6, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ElectricShock

                            case Spell.ElectricShock:
                                Effects.Add(new Effect(Libraries.Magic, 1560, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Poisoning

                            case Spell.Poisoning:
                                Effects.Add(new Effect(Libraries.Magic, 600, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region GreatFireBall

                            case Spell.GreatFireBall:
                                Effects.Add(new Effect(Libraries.Magic, 400, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region HellFire

                            case Spell.HellFire:
                                Effects.Add(new Effect(Libraries.Magic, 920, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ThunderBolt

                            case Spell.ThunderBolt:
                                Effects.Add(new Effect(Libraries.Magic2, 20, 3, 300, this));
                                break;

                            #endregion

                            #region SoulFireBall

                            case Spell.SoulFireBall:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region SummonSkeleton

                            case Spell.SummonSkeleton:
                                Effects.Add(new Effect(Libraries.Magic, 1500, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion
                            #region StormEscape
                            case Spell.StormEscape:
                                Effects.Add(new Effect(Libraries.Magic3, 590, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;
                            #endregion
                            #region Teleport

                            case Spell.Teleport:
                                Effects.Add(new Effect(Libraries.Magic, 1590, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Blink

                            case Spell.Blink:
                                Effects.Add(new Effect(Libraries.Magic, 1590, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Hiding

                            case Spell.Hiding:
                                Effects.Add(new Effect(Libraries.Magic, 1520, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Haste

                            case Spell.Haste:
                                Effects.Add(new Effect(Libraries.Magic2, 2140 + (int)Direction * 10, 6, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Fury

                            case Spell.Fury:
                                Effects.Add(new Effect(Libraries.Magic3, 200, 8, 8 * FrameInterval, this));
                                Effects.Add(new Effect(Libraries.Magic3, 187, 10, 10 * FrameInterval, this));
                                //i don't know sound
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ImmortalSkin
                            case Spell.ImmortalSkin:
                                Effects.Add(new Effect(Libraries.Magic3, 550, 17, Frame.Count * FrameInterval * 4, this));
                                Effects.Add(new Effect(Libraries.Magic3, 570, 5, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;
                            #endregion

                            #region FireBang

                            case Spell.FireBang:
                                Effects.Add(new Effect(Libraries.Magic, 1650, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region FireWall

                            case Spell.FireWall:
                                Effects.Add(new Effect(Libraries.Magic, 1620, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region HealingCircle

                            case Spell.HealingCircle:
                                Effects.Add(new Effect(Libraries.Magic3, 620, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region MoonMist

                            case Spell.MoonMist:
                                MapControl.Effects.Add(new Effect(Libraries.Magic3, 680, 25, 1800, CurrentLocation));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region TrapHexagon

                            case Spell.TrapHexagon:
                                Effects.Add(new Effect(Libraries.Magic, 1380, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region EnergyRepulsor

                            case Spell.EnergyRepulsor:
                                Effects.Add(new Effect(Libraries.Magic2, 190, 6, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region FireBurst

                            case Spell.FireBurst:
                                Effects.Add(new Effect(Libraries.Magic2, 2320, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region FlameDisruptor

                            case Spell.FlameDisruptor:
                                Effects.Add(new Effect(Libraries.Magic2, 130, 6, Frame.Count * FrameInterval, this));
                                break;

                            #endregion

                            #region SummonShinsu

                            case Spell.SummonShinsu:
                                Effects.Add(new Effect(Libraries.Magic2, 0, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region UltimateEnchancer

                            case Spell.UltimateEnhancer:
                                Effects.Add(new Effect(Libraries.Magic2, 160, 15, 1000, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region FrostCrunch

                            case Spell.FrostCrunch:
                                Effects.Add(new Effect(Libraries.Magic2, 400, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Purification

                            case Spell.Purification:
                                Effects.Add(new Effect(Libraries.Magic2, 600, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region FlameField

                            case Spell.FlameField:
                                MapControl.Effects.Add(new Effect(Libraries.Magic2, 910, 23, 1800, CurrentLocation));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Trap

                            case Spell.Trap:
                                Effects.Add(new Effect(Libraries.Magic2, 2340, 11, 11 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region MoonLight

                            case Spell.MoonLight:
                                Effects.Add(new Effect(Libraries.Magic2, 2380, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region SwiftFeet

                            case Spell.SwiftFeet:
                                Effects.Add(new Effect(Libraries.Magic2, 2440, 16, 16 * EffectFrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region LightBody

                            case Spell.LightBody:
                                Effects.Add(new Effect(Libraries.Magic2, 2470, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion


                            #region PoisonSword

                            case Spell.PoisonSword:
                                Effects.Add(new Effect(Libraries.Magic2, 2490 + ((int)Direction * 10), 10, Frame.Count * FrameInterval + 500, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region DarkBody

                            case Spell.DarkBody:
                                Effects.Add(new Effect(Libraries.Magic2, 2580, 10, 10 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ThunderStorm

                            case Spell.ThunderStorm:
                                MapControl.Effects.Add(new Effect(Libraries.Magic, 1680, 10, Frame.Count * FrameInterval, CurrentLocation));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region MassHealing

                            case Spell.MassHealing:
                                Effects.Add(new Effect(Libraries.Magic, 1790, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region IceStorm

                            case Spell.IceStorm:
                                Effects.Add(new Effect(Libraries.Magic, 3840, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region MagicShield

                            case Spell.MagicShield:
                                Effects.Add(new Effect(Libraries.Magic, 3880, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region TurnUndead

                            case Spell.TurnUndead:
                                Effects.Add(new Effect(Libraries.Magic, 3920, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region MagicBooster

                            case Spell.MagicBooster:
                                Effects.Add(new Effect(Libraries.Magic3, 80, 9, 9 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region PetEnhancer

                            case Spell.PetEnhancer:
                                Effects.Add(new Effect(Libraries.Magic3, 200, 8, 8 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Revelation

                            case Spell.Revelation:
                                Effects.Add(new Effect(Libraries.Magic, 3960, 20, 1200, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ProtectionField

                            case Spell.ProtectionField:
                                Effects.Add(new Effect(Libraries.Magic2, 1520, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Rage

                            case Spell.Rage:
                                Effects.Add(new Effect(Libraries.Magic2, 1510, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion


                            #region Vampirism

                            case Spell.Vampirism:
                                Effects.Add(new Effect(Libraries.Magic2, 1040, 7, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region LionRoar, BattleCry

                            case Spell.LionRoar:
                            case Spell.BattleCry:
                                Effects.Add(new Effect(Libraries.Magic2, 710, 20, 1200, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + (Gender == MirGender.Male ? 0 : 1));
                                break;

                            #endregion

                            #region TwinDrakeBlade

                            case Spell.TwinDrakeBlade:
                                Effects.Add(new Effect(Libraries.Magic2, 210, 6, 500, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Entrapment

                            case Spell.Entrapment:
                                Effects.Add(new Effect(Libraries.Magic2, 990, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region BladeAvalanche

                            case Spell.BladeAvalanche:
                                Effects.Add(new Effect(Libraries.Magic2, 740 + (int)Direction * 20, 15, 15 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region SlashingBurst

                            case Spell.SlashingBurst:
                                //MapControl.Effects.Add(new Effect(Libraries.Magic2, 1700 + (int)Direction * 10, 9, 9 * FrameInterval, CurrentLocation));
                                Effects.Add(new Effect(Libraries.Magic2, 1700 + (int)Direction * 10, 9, 9 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                SlashingBurstTime = CMain.Time + 2000;
                                break;

                            #endregion

                            #region CounterAttack

                            case Spell.CounterAttack:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 5);
                                Effects.Add(new Effect(Libraries.Magic, 3480 + (int)Direction * 10, 10, 10 * FrameInterval, this));
                                Effects.Add(new Effect(Libraries.Magic3, 140, 2, 2 * FrameInterval, this));
                                break;

                            #endregion

                            #region CrescentSlash

                            case Spell.CrescentSlash:
                                Effects.Add(new Effect(Libraries.Magic2, 2620 + (int)Direction * 20, 20, 20 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + (Gender == MirGender.Male ? 0 : 1));

                               
                                break;

                            #endregion

                            #region FlashDash

                            case Spell.FlashDash:
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + (Gender == MirGender.Male ? 0 : 1));
                                int attackDelay = (User.AttackSpeed - 120) <= 300 ? 300 : (User.AttackSpeed - 120);

                                float attackRate = (float)(attackDelay / 300F * 10F);
                                FrameInterval = FrameInterval * (int)attackRate / 20;
                                EffectFrameInterval = EffectFrameInterval * (int)attackRate / 20;
                                break;
                            #endregion

                            #region Mirroring

                            case Spell.Mirroring:
                                Effects.Add(new Effect(Libraries.Magic2, 650, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region Blizzard

                            case Spell.Blizzard:
                                Effects.Add(new Effect(Libraries.Magic2, 1540, 8, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                BlizzardStopTime = CMain.Time + 3000;
                                break;

                            #endregion

                            #region MeteorStrike

                            case Spell.MeteorStrike:
                                Effects.Add(new Effect(Libraries.Magic2, 1590, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                BlizzardStopTime = CMain.Time + 3000;
                                break;

                            #endregion

                            #region Reincarnation

                            case Spell.Reincarnation:
                                ReincarnationStopTime = CMain.Time + 6000;
                                break;

                            #endregion

                            #region HeavenlySword

                            case Spell.HeavenlySword:
                                Effects.Add(new Effect(Libraries.Magic2, 2230 + ((int)Direction * 10), 8, 800, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                break;

                            #endregion

                            #region ElementalBarrier

                            case Spell.ElementalBarrier:
                                if (HasElements && !ElementalBarrier)
                                {
                                    Effects.Add(new Effect(Libraries.Magic3, 1880, 8, Frame.Count * FrameInterval, this));
                                    SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                }
                                break;

                            #endregion

                            #region PoisonShot
                            case Spell.PoisonShot:
                                Effects.Add(new Effect(Libraries.Magic3, 2300, 8, 1000, this));
                                break;
                            #endregion

                            #region OneWithNature
                            case Spell.OneWithNature:
                                MapControl.Effects.Add(new Effect(Libraries.Magic3, 2710, 8, 1200, CurrentLocation));
                                SoundManager.PlaySound(20000 + 139 * 10);
                                break;
                            #endregion


                            #region FireBounce

                            case Spell.FireBounce:
                                Effects.Add(new Effect(Libraries.Magic, 400, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10);
                                break;

                            #endregion

                            #region MeteorShower

                            case Spell.MeteorShower:
                                Effects.Add(new Effect(Libraries.Magic, 400, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10);
                                break;

                            #endregion

                        }


                        break;
                    case MirAction.Dead:
                        GameScene.Scene.Redraw();
                        GameScene.Scene.MapControl.SortObject(this);
                        if (MouseObject == this) MouseObjectID = 0;
                        if (TargetObject == this) TargetObjectID = 0;
                        if (MagicObject == this) MagicObjectID = 0;
                        DeadTime = CMain.Time;
                        break;

                }

            }

            GameScene.Scene.MapControl.TextureValid = false;

            NextMotion = CMain.Time + FrameInterval;
            NextMotion2 = CMain.Time + EffectFrameInterval;

            if (ElementalBarrier)
            {
                switch (CurrentAction)
                {
                    case MirAction.Struck:
                    case MirAction.MountStruck:
                        if (ElementalBarrierEffect != null)
                        {
                            ElementalBarrierEffect.Clear();
                            ElementalBarrierEffect.Remove();
                        }

                        Effects.Add(ElementalBarrierEffect = new Effect(Libraries.Magic3, 1910, 5, 600, this));
                        ElementalBarrierEffect.Complete += (o, e) => Effects.Add(ElementalBarrierEffect = new Effect(Libraries.Magic3, 1890, 16, 3200, this) { Repeat = true });
                        break;
                    default:
                        if (ElementalBarrierEffect == null)
                            Effects.Add(ElementalBarrierEffect = new Effect(Libraries.Magic3, 1890, 16, 3200, this) { Repeat = true });
                        break;
                }
            }

            if (MagicShield)
            {
                switch (CurrentAction)
                {
                    case MirAction.Struck:
                    case MirAction.MountStruck:
                        if (ShieldEffect != null)
                        {
                            ShieldEffect.Clear();
                            ShieldEffect.Remove();
                        }

                        Effects.Add(ShieldEffect = new Effect(Libraries.Magic, 3900, 3, 600, this));
                        ShieldEffect.Complete += (o, e) => Effects.Add(ShieldEffect = new Effect(Libraries.Magic, 3890, 3, 600, this) { Repeat = true });
                        break;
                    default:
                        if (ShieldEffect == null)
                            Effects.Add(ShieldEffect = new Effect(Libraries.Magic, 3890, 3, 600, this) { Repeat = true });
                        break;
                }
            }

        }

        public virtual void ProcessFrames()
        {
            if (Frame == null) return;

            switch (CurrentAction)
            {
                case MirAction.Walking:
                case MirAction.Running:
                case MirAction.MountWalking:
                case MirAction.MountRunning:
                case MirAction.Sneek:
                case MirAction.DashAttack:
                    if (!GameScene.CanMove) return;
                    

                    GameScene.Scene.MapControl.TextureValid = false;

                    if (this == User) GameScene.Scene.MapControl.FloorValid = false;
                    //if (CMain.Time < NextMotion) return;
                    if (SkipFrames) UpdateFrame();



                    if (UpdateFrame(false) >= Frame.Count)
                    {


                        FrameIndex = Frame.Count - 1;
                        SetAction();
                    }
                    else
                    {
                        if (this == User)
                        {
                            if (FrameIndex == 1 || FrameIndex == 4)
                                PlayStepSound();
                        }
                        //NextMotion += FrameInterval;
                    }

                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        if (this == User) GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;
                 case MirAction.Jump:
                    if (!GameScene.CanMove) return;
                    GameScene.Scene.MapControl.TextureValid = false;
                    if (this == User) GameScene.Scene.MapControl.FloorValid = false;
                    if (SkipFrames) UpdateFrame();
                    if (UpdateFrame() >= Frame.Count)
                    {
                        FrameIndex = Frame.Count - 1;
                        SetAction();
                    }
                    else
                    {
                        if (FrameIndex == 1)
                            SoundManager.PlaySound(20000 + 127 * 10 + (Gender == MirGender.Male ? 5 : 6));
                        if (FrameIndex == 7)
                            SoundManager.PlaySound(20000 + 127 * 10 + 7);
                    }
                    //Backstep wingeffect
                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        if (this == User) GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;
                case MirAction.DashL:
                    if (!GameScene.CanMove) return;

                    GameScene.Scene.MapControl.TextureValid = false;

                    if (this == User) GameScene.Scene.MapControl.FloorValid = false;
                    if (UpdateFrame() >= 3)
                    {
                        FrameIndex = 2;
                        SetAction();
                    }

                    if (UpdateFrame2() >= 3) EffectFrameIndex = 2;
                    break;
                case MirAction.DashR:
                    if (!GameScene.CanMove) return;

                    GameScene.Scene.MapControl.TextureValid = false;

                    if (this == User) GameScene.Scene.MapControl.FloorValid = false;

                    if (UpdateFrame() >= 6)
                    {
                        FrameIndex = 5;
                        SetAction();
                    }

                    if (UpdateFrame2() >= 6) EffectFrameIndex = 5;
                    break;
                case MirAction.Pushed:
                    if (!GameScene.CanMove) return;

                    GameScene.Scene.MapControl.TextureValid = false;

                    if (this == User) GameScene.Scene.MapControl.FloorValid = false;

                    FrameIndex -= 2;
                    EffectFrameIndex -= 2;

                    if (FrameIndex < 0)
                    {
                        FrameIndex = 0;
                        SetAction();
                    }

                    if (FrameIndex < 0) EffectFrameIndex = 0;
                    break;

                case MirAction.Standing:
                case MirAction.MountStanding:
                case MirAction.DashFail:
                case MirAction.Harvest:
                case MirAction.Stance:
                case MirAction.Stance2:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }

                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;  


                case MirAction.FishingCast:             
                case MirAction.FishingReel:
                case MirAction.FishingWait:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    switch (CurrentAction)
                                    {
                                        case MirAction.FishingCast:
                                            SoundManager.PlaySound(SoundList.FishingThrow);
                                            ((MirAnimatedButton)GameScene.Scene.FishingStatusDialog.FishButton).Visible = false;
                                            break;
                                        case MirAction.FishingReel:
                                            SoundManager.PlaySound(SoundList.FishingPull);
                                            break;
                                        case MirAction.FishingWait:
                                            if (FoundFish)
                                            {
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 671, 6, 720, FishingPoint) { Light = 0 });
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 665, 6, 720, FishingPoint) { Light = 0 });

                                                SoundManager.PlaySound(SoundList.Fishing);
                                                Effects.Add(new Effect(Libraries.Prguse, 1350, 2, 720, this) { Light = 0, Blend = false });

                                                ((MirAnimatedButton)GameScene.Scene.FishingStatusDialog.FishButton).Visible = true;
                                            }
                                            else
                                            {
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 650, 6, 720, FishingPoint) { Light = 0 });
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 640, 6, 720, FishingPoint) { Light = 0 });
                                                ((MirAnimatedButton)GameScene.Scene.FishingStatusDialog.FishButton).Visible = false;
                                            }

                                            ((MirAnimatedButton)GameScene.Scene.FishingStatusDialog.FishButton).AnimationCount = FoundFish ? 10 : 1;
                                            break;
                                    }
                                    break;
                            }
                            NextMotion += FrameInterval;
                        }
                    }

                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;     

                case MirAction.Attack1:
                case MirAction.Attack2:
                case MirAction.Attack3:
                case MirAction.Attack4:
                case MirAction.MountAttack:
                case MirAction.Mine:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            //if (ActionFeed.Count == 0)
                            //    ActionFeed.Add(new QueuedAction { Action = MirAction.Stance, Direction = Direction, Location = CurrentLocation });

                            StanceTime = CMain.Time + StanceDelay;
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            if (FrameIndex == 1) PlayAttackSound();
                            NextMotion += FrameInterval;
                        }
                    }

                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;

                case MirAction.AttackRange1:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            if (FrameIndex == 1) PlayAttackSound();
                            Missile missile;
                            switch (FrameIndex)
                            {
                                case 6:
                                    switch (Spell)
                                    {
                                        case Spell.Focus:
                                            Effects.Add(new Effect(Libraries.Magic3, 2730, 10, Frame.Count * FrameInterval, this));
                                            SoundManager.PlaySound(20000 + 121 * 10 + 5);
                                            break;
                                    }

                                    break;
                                case 5:
                                    missile = CreateProjectile(1030, Libraries.Magic3, true, 5, 30, 5);
                                    StanceTime = CMain.Time + StanceDelay;
                                    SoundManager.PlaySound(20000 + 121 * 10);
                                    if (missile.Target != null)
                                    {
                                        missile.Complete += (o, e) =>
                                        {
                                            SoundManager.PlaySound(20000 + 121 * 10 + 2);
                                        };
                                    }
                                    break;
                            }

                            NextMotion += FrameInterval;
                        }
                    }

                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;

                case MirAction.AttackRange2:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            if (Cast)
                            {
                                MapObject ob = MapControl.GetObject(TargetID);

                                Missile missile;
                                switch (Spell)
                                {
                                    case Spell.StraightShot:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 0);
                                        missile = CreateProjectile(1210, Libraries.Magic3, true, 5, 30, 5);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic3, 1370, 7, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.StraightShot * 10 + 2);
                                            };
                                        }
                                        break;
                                }


                                Cast = false;
                            }

                            StanceTime = CMain.Time + StanceDelay;
                            FrameIndex = Frame.Count - 1;
                            SetAction();

                        }
                        else
                        {
                            NextMotion += FrameInterval;

                            Missile missile;

                            switch(Spell)
                            {
                                case Spell.DoubleShot:
                                    switch (FrameIndex)
                                    {
                                        case 7:
                                        case 5:
                                            missile = CreateProjectile(1030, Libraries.Magic3, true, 5, 30, 5);//normal arrow
                                            StanceTime = CMain.Time + StanceDelay;
                                            SoundManager.PlaySound(20000 + 121 * 10);
                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    SoundManager.PlaySound(20000 + 121 * 10 + 2);
                                                };
                                            }
                                            break;
                                    }
                                    break;
                                case Spell.ElementalShot:
                                    if (HasElements && !ElementCasted)
                                        switch (FrameIndex)
                                        {
                                            case 7:
                                                missile = CreateProjectile(1690, Libraries.Magic3, true, 6, 30, 4);//elemental arrow
                                                StanceTime = CMain.Time + StanceDelay;
                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 2);//sound M128-2
                                                    };
                                                }
                                                break;
                                            case 1:
                                                Effects.Add(new Effect(Libraries.Magic3, 1681, 5, Frame.Count * FrameInterval, this));
                                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 0);//sound M128-0
                                                break;
                                        }
                                    break;
                                case Spell.BindingShot:
                                case Spell.SummonVampire:
                                case Spell.SummonToad:
                                case Spell.SummonSnakes:
                                    switch (FrameIndex)
                                    {
                                        case 7:
                                            SoundManager.PlaySound(20000 + 121 * 10);
                                            missile = CreateProjectile(2750, Libraries.Magic3, true, 5, 10, 5);
                                            StanceTime = CMain.Time + StanceDelay;
                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 7);//sound M130-7
                                                };
                                            }
                                            break;
                                    }
                                    break;
                                case Spell.Stonetrap:
                                    switch (FrameIndex)
                                    {
                                        case 7:
                                            SoundManager.PlaySound(20000 + 121 * 10);
                                            missile = CreateProjectile(2750, Libraries.Magic3, true, 5, 20, 5);
                                            StanceTime = CMain.Time + StanceDelay;
                                            missile.Explode = true;

                                            missile.Complete += (o, e) =>
                                            {
                                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 7);//sound M130-7
                                            };

                                            break;
                                    }
                                    break;
                                case Spell.DelayedExplosion:
                                    switch (FrameIndex)
                                    {
                                        case 5:
                                            missile = CreateProjectile(1030, Libraries.Magic3, true, 5, 30, 5);//normal arrow
                                            StanceTime = CMain.Time + StanceDelay;
                                            SoundManager.PlaySound(20000 + 121 * 10);
                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    SoundManager.PlaySound(20000 + 121 * 10 + 2);
                                                };
                                            }
                                            break;
                                    }
                                    break;
                                case Spell.VampireShot:
                                case Spell.PoisonShot:
                                case Spell.CrippleShot:
                                    MapObject ob = MapControl.GetObject(TargetID);
                                    Effect eff;
                                    int exFrameStart = 0;
                                    if (Spell == Spell.PoisonShot) exFrameStart = 200;
                                    if (Spell == Spell.CrippleShot) exFrameStart = 400;
                                    switch (FrameIndex)
                                    {
                                        case 7:
                                            SoundManager.PlaySound(20000 + ((Spell == Spell.CrippleShot) ? 136 : 121) * 10);//M136-0
                                            missile = CreateProjectile(1930 + exFrameStart, Libraries.Magic3, true, 5, 10, 5);
                                            StanceTime = CMain.Time + StanceDelay;
                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    if (ob != null)
                                                    {
                                                        if (Spell == Spell.CrippleShot)
                                                        {
                                                            int exIdx = 0;
                                                            if (this == User)
                                                            {
                                                                if (GameScene.Scene.BuffsDialog.Buffs.Any(x => x.Type == BuffType.VampireShot)) exIdx = 20;
                                                                if (GameScene.Scene.BuffsDialog.Buffs.Any(x => x.Type == BuffType.PoisonShot)) exIdx = 10;
                                                            }
                                                            else
                                                            {
                                                                if (Buffs.Any(x => x == BuffType.VampireShot)) exIdx = 20;
                                                                if (Buffs.Any(x => x == BuffType.PoisonShot)) exIdx = 10;
                                                            }

                                                            ob.Effects.Add(eff = new Effect(Libraries.Magic3, 2490 + exIdx, 7, 1000, ob));
                                                            SoundManager.PlaySound(20000 + 136 * 10 + 5 + (exIdx / 10));//sound M136-5/7

                                                            if (exIdx == 20)
                                                                eff.Complete += (o1, e1) =>
                                                                {
                                                                    SoundManager.PlaySound(20000 + 45 * 10 + 2);//sound M45-2
                                                                    Effects.Add(new Effect(Libraries.Magic3, 2100, 8, 1000, this));
                                                                };
                                                        }

                                                        if (Spell == Spell.VampireShot || Spell == Spell.PoisonShot)
                                                        {
                                                            ob.Effects.Add(eff = new Effect(Libraries.Magic3, 2090 + exFrameStart, 6, 1000, ob));
                                                            SoundManager.PlaySound(20000 + (133 + (exFrameStart / 100)) * 10 + 2);//sound M133-2 or M135-2
                                                            if (Spell == Spell.VampireShot)
                                                                eff.Complete += (o1, e1) =>
                                                                {
                                                                    SoundManager.PlaySound(20000 + 45 * 10 + 2);//sound M45-2
                                                                    Effects.Add(new Effect(Libraries.Magic3, 2100 + exFrameStart, 8, 1000, this));
                                                                };
                                                        }
                                                    }
                                                    //SoundManager.PlaySound(20000 + 121 * 10 + 2);//sound M121-2
                                                };
                                            }
                                            break;
                                    }
                                    break;
                                case Spell.NapalmShot:
                                    switch (FrameIndex)
                                    {
                                        case 7:
                                            SoundManager.PlaySound(20000 + 138 * 10);//M138-0
                                            missile = CreateProjectile(2530, Libraries.Magic3, true, 6, 50, 4);
                                            StanceTime = CMain.Time + StanceDelay;
                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    SoundManager.PlaySound(20000 + 138 * 10 + 2);//M138-2
                                                    MapControl.Effects.Add(new Effect(Libraries.Magic3, 2690, 10, 1000, TargetPoint));
                                                };
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;

                case MirAction.Struck:
                case MirAction.MountStruck:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;
                case MirAction.Spell:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            if (Cast)
                            {

                                MapObject ob = MapControl.GetObject(TargetID);

                                Missile missile;
                                Effect effect;
                                switch (Spell)
                                {
                                    #region FireBall

                                    case Spell.FireBall:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        missile = CreateProjectile(10, Libraries.Magic, true, 6, 30, 4);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic, 170, 10, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 2);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region GreatFireBall

                                    case Spell.GreatFireBall:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        missile = CreateProjectile(410, Libraries.Magic, true, 6, 30, 4);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic, 570, 10, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 2);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region Healing

                                    case Spell.Healing:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 370, 10, 800, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic, 370, 10, 800, ob));
                                        break;

                                    #endregion

                                    #region ElectricShock

                                    case Spell.ElectricShock:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 1570, 10, 1000, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic, 1570, 10, 1000, ob));
                                        break;
                                    #endregion

                                    #region Poisoning

                                    case Spell.Poisoning:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        if (ob != null)
                                            ob.Effects.Add(new Effect(Libraries.Magic, 770, 10, 1000, ob));
                                        break;
                                    #endregion

                                    #region HellFire

                                    case Spell.HellFire:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);

                                        
                                        Point dest = CurrentLocation;
                                        for (int i = 0; i < 4; i++)
                                        {
                                            dest = Functions.PointMove(dest, Direction, 1);
                                            if (!GameScene.Scene.MapControl.ValidPoint(dest)) break;
                                            effect = new Effect(Libraries.Magic, 930, 6, 500, dest) { Rate = 0.7F };

                                            effect.SetStart(CMain.Time + i * 50);
                                            MapControl.Effects.Add(effect);
                                        }

                                        if (SpellLevel == 3)
                                        {
                                            MirDirection dir = (MirDirection)(((int)Direction + 1) % 8);
                                            dest = CurrentLocation;
                                            for (int r = 0; r < 4; r++)
                                            {
                                                dest = Functions.PointMove(dest, dir, 1);
                                                if (!GameScene.Scene.MapControl.ValidPoint(dest)) break;
                                                effect = new Effect(Libraries.Magic, 930, 6, 500, dest) { Rate = 0.7F };

                                                effect.SetStart(CMain.Time + r * 50);
                                                MapControl.Effects.Add(effect);
                                            }

                                            dir = (MirDirection)(((int)Direction - 1 + 8) % 8);
                                            dest = CurrentLocation;
                                            for (int r = 0; r < 4; r++)
                                            {
                                                dest = Functions.PointMove(dest, dir, 1);
                                                if (!GameScene.Scene.MapControl.ValidPoint(dest)) break;
                                                effect = new Effect(Libraries.Magic, 930, 6, 500, dest) { Rate = 0.7F };

                                                effect.SetStart(CMain.Time + r * 50);
                                                MapControl.Effects.Add(effect);
                                            }
                                        }
                                        break;

                                    #endregion

                                    #region ThunderBolt

                                    case Spell.ThunderBolt:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);

                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic2, 10, 5, 400, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic2, 10, 5, 400, ob));
                                        break;

                                    #endregion

                                    #region SoulFireBall

                                    case Spell.SoulFireBall:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic, 1360, 10, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.SoulFireBall * 10 + 2);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region EnergyShield

                                    case Spell.EnergyShield:

                                        //Effects.Add(new Effect(Libraries.Magic2, 1880, 9, Frame.Count * FrameInterval, this));
                                        //SoundManager.PlaySound(20000 + (ushort)Spell * 9);
                                        break;

                                    #endregion

                                    #region FireBang

                                    case Spell.FireBang:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        MapControl.Effects.Add(new Effect(Libraries.Magic, 1660, 10, 1000, TargetPoint));
                                        break;

                                    #endregion

                                    #region MassHiding

                                    case Spell.MassHiding:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                        {
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 1540, 10, 800, TargetPoint));
                                            SoundManager.PlaySound(20000 + (ushort)Spell.MassHiding * 10 + 1);
                                        };
                                        break;

                                    #endregion

                                    #region SoulShield

                                    case Spell.SoulShield:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                        {
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 1320, 15, 1200, TargetPoint));
                                            SoundManager.PlaySound(20000 + (ushort)Spell.SoulShield * 10 + 1);
                                        };
                                        break;

                                    #endregion

                                    #region BlessedArmour

                                    case Spell.BlessedArmour:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                        {
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 1340, 15, 1200, TargetPoint));
                                            SoundManager.PlaySound(20000 + (ushort)Spell.BlessedArmour * 10 + 1);
                                        };
                                        break;

                                    #endregion

                                    #region FireWall

                                    case Spell.FireWall:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        break;

                                    #endregion


                                    #region HealingCircle

                                    case Spell.HealingCircle:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        MapControl.Effects.Add(new Effect(Libraries.Magic3, 620, 10, 1200, TargetPoint));
                                        break;

                                    #endregion

                                    #region MassHealing

                                    case Spell.MassHealing:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        MapControl.Effects.Add(new Effect(Libraries.Magic, 1800, 10, 1000, TargetPoint));
                                        break;

                                    #endregion

                                    #region IceStorm

                                    case Spell.IceStorm:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        MapControl.Effects.Add(new Effect(Libraries.Magic, 3850, 20, 1300, TargetPoint));
                                        break;

                                    #endregion

                                    #region TurnUndead

                                    case Spell.TurnUndead:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 3930, 15, 1000, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic, 3930, 15, 1000, ob));
                                        break;
                                    #endregion

                                    #region IceThrust

                                    case Spell.IceThrust:

                                        Point location = Functions.PointMove(CurrentLocation, Direction, 1);

                                        MapControl.Effects.Add(new Effect(Libraries.Magic2, 1790 + (int)Direction * 10, 10, 10 * FrameInterval, location));
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        break;

                                    #endregion

                                    #region Revelation

                                    case Spell.Revelation:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic, 3990, 10, 1000, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic, 3990, 10, 1000, ob));
                                        break;
                                    #endregion

                                    #region FlameDisruptor

                                    case Spell.FlameDisruptor:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);

                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic2, 140, 10, 600, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic2, 140, 10, 600, ob));
                                        break;

                                    #endregion

                                    #region CatTongue
                                    case Spell.CatTongue:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        missile = CreateProjectile(260, Libraries.Magic3, true, 6, 30, 4);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic3, 420, 8, 800, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.CatTongue * 10 + 1);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region FrostCrunch

                                    case Spell.FrostCrunch:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        missile = CreateProjectile(410, Libraries.Magic2, true, 4, 30, 6);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic2, 570, 8, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.FrostCrunch * 10 + 2);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region Purification

                                    case Spell.Purification:
                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic2, 620, 10, 800, TargetPoint));
                                        else
                                            ob.Effects.Add(new Effect(Libraries.Magic2, 620, 10, 800, ob));
                                        break;

                                    #endregion

                                    #region Curse

                                    case Spell.Curse:
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                        {
                                            MapControl.Effects.Add(new Effect(Libraries.Magic2, 950, 24, 2000, TargetPoint));
                                            SoundManager.PlaySound(20000 + (ushort)Spell.Curse * 10);
                                        };
                                        break;

                                    #endregion

                                    #region Hallucination

                                    case Spell.Hallucination:
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 48, 7);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic2, 1110, 10, 1000, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region Lightning

                                    case Spell.Lightning:
                                        Effects.Add(new Effect(Libraries.Magic, 970 + (int)Direction * 20, 6, Frame.Count * FrameInterval, this));
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        break;

                                    #endregion

                                    #region Vampirism

                                    case Spell.Vampirism:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);

                                        if (ob == null)
                                            MapControl.Effects.Add(new Effect(Libraries.Magic2, 1060, 20, 1000, TargetPoint));
                                        else
                                        {
                                            ob.Effects.Add(effect = new Effect(Libraries.Magic2, 1060, 20, 1000, ob));
                                            effect.Complete += (o, e) =>
                                            {
                                                SoundManager.PlaySound(20000 + (ushort)Spell.Vampirism * 10 + 2);
                                                Effects.Add(new Effect(Libraries.Magic2, 1090, 10, 500, this));
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region PoisonCloud

                                    case Spell.PoisonCloud:
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                            {
                                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                            };

                                        break;

                                    #endregion

                                    #region Blizzard

                                    case Spell.Blizzard:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        //BlizzardFreezeTime = CMain.Time + 3000;
                                        break;

                                    #endregion

                                    #region MeteorStrike

                                    case Spell.MeteorStrike:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 2);
                                        //BlizzardFreezeTime = CMain.Time + 3000;
                                        break;

                                    #endregion

                                    #region Reincarnation

                                    case Spell.Reincarnation:
                                        ReincarnationStopTime = 0;
                                        break;

                                    #endregion

                                    #region SummonHolyDeva

                                    case Spell.SummonHolyDeva:
                                        Effects.Add(new Effect(Libraries.Magic, 1500, 10, Frame.Count * FrameInterval, this));
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                        break;

                                    #endregion

                                    #region UltimateEnhancer

                                    case Spell.UltimateEnhancer:
                                        if (ob != null && ob != User)
                                            ob.Effects.Add(new Effect(Libraries.Magic2, 160, 15, 1000, ob));
                                        break;

                                    #endregion

                                    #region Plague

                                    case Spell.Plague:
                                        //SoundManager.PlaySound(20000 + (ushort)Spell.SoulShield * 10);
                                        missile = CreateProjectile(1160, Libraries.Magic, true, 3, 30, 7);
                                        missile.Explode = true;

                                        missile.Complete += (o, e) =>
                                        {
                                            MapControl.Effects.Add(new Effect(Libraries.Magic3, 110, 10, 1200, TargetPoint));
                                            SoundManager.PlaySound(20000 + (ushort)Spell.Plague * 10 + 3);
                                        };
                                        break;

                                    #endregion

                                    #region TrapHexagon

                                    case Spell.TrapHexagon:
                                        if (ob != null)
                                        SoundManager.PlaySound(20000 + (ushort)Spell.TrapHexagon * 10 + 1);
                                        break;

                                    #endregion

                                    #region Trap

                                    case Spell.Trap:
                                        if (ob != null)
                                            SoundManager.PlaySound(20000 + (ushort)Spell.Trap * 10 + 1);
                                        break;

                                    #endregion

                                    #region CrescentSlash

                                    case Spell.CrescentSlash:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 2);
                                        break;

                                    #endregion

                                    #region NapalmShot

                                    case Spell.NapalmShot:

                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        MapControl.Effects.Add(new Effect(Libraries.Magic3, 1660, 10, 1000, TargetPoint));
                                        break;

                                    #endregion


                                    #region FireBounce

                                    case Spell.FireBounce:
                                        SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 1);

                                        missile = CreateProjectile(410, Libraries.Magic, true, 6, 30, 4);

                                        if (missile.Target != null)
                                        {
                                            missile.Complete += (o, e) =>
                                            {
                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                missile.Target.Effects.Add(new Effect(Libraries.Magic, 570, 10, 600, missile.Target));
                                                SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 2);
                                            };
                                        }
                                        break;

                                    #endregion

                                    #region MeteorShower

                                    case Spell.MeteorShower:

                                        SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 1);

                                        var targetIDs = new List<uint> { TargetID };

                                        if (SecondaryTargetIDs != null)
                                        {
                                            targetIDs.AddRange(SecondaryTargetIDs);
                                        }

                                        foreach (var targetID in targetIDs)
                                        {
                                            missile = CreateProjectile(410, Libraries.Magic, true, 6, 30, 4, targetID: targetID);

                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    var sender = (Missile)o;

                                                    if (sender.Target.CurrentAction == MirAction.Dead) return;
                                                    sender.Target.Effects.Add(new Effect(Libraries.Magic, 570, 10, 600, sender.Target));
                                                    SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 2);
                                                };
                                            }
                                        }
                                        
                                        break;

                                    #endregion

                                }


                                Cast = false;
                            }
                            //if (ActionFeed.Count == 0)
                            //    ActionFeed.Add(new QueuedAction { Action = MirAction.Stance, Direction = Direction, Location = CurrentLocation });

                            StanceTime = CMain.Time + StanceDelay;
                            FrameIndex = Frame.Count - 1;
                            SetAction();

                        }
                        else
                        {
                            NextMotion += FrameInterval;

                        }
                    }
                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;
                case MirAction.Die:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            ActionFeed.Clear();
                            ActionFeed.Add(new QueuedAction { Action = MirAction.Dead, Direction = Direction, Location = CurrentLocation });
                            SetAction();
                        }
                        else
                        {
                            if (FrameIndex == 1)
                                PlayDieSound();

                            NextMotion += FrameInterval;
                        }
                    }
                    if (WingEffect > 0 && CMain.Time >= NextMotion2)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame2();

                        if (UpdateFrame2() >= Frame.EffectCount)
                            EffectFrameIndex = Frame.EffectCount - 1;
                        else
                            NextMotion2 += EffectFrameInterval;
                    }
                    break;
                case MirAction.Dead:
                    break;
                case MirAction.Revive:
                    if (CMain.Time >= NextMotion)
                    {
                        GameScene.Scene.MapControl.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            ActionFeed.Clear();
                            ActionFeed.Add(new QueuedAction { Action = MirAction.Standing, Direction = Direction, Location = CurrentLocation });
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;

            }

            if (this == User) return;

            if ((CurrentAction == MirAction.Standing || CurrentAction == MirAction.MountStanding || CurrentAction == MirAction.Stance || CurrentAction == MirAction.Stance2 || CurrentAction == MirAction.DashFail) && NextAction != null)
                SetAction();
            //if Revive and dead set action

        }
        public int UpdateFrame(bool skip = true)
        {
            if (Frame == null) return 0;
            if (Poison.HasFlag(PoisonType.Slow) && !skip)
            {
                SkipFrameUpdate++;
                if (SkipFrameUpdate == 2)
                    SkipFrameUpdate = 0;
                else
                    return FrameIndex;
            }
            if (Frame.Reverse) return Math.Abs(--FrameIndex);

            return ++FrameIndex;
        }

        public int UpdateFrame2()
        {
            if (Frame == null) return 0;

            if (Frame.Reverse) return Math.Abs(--EffectFrameIndex);

            return ++EffectFrameIndex;
        }


        public override Missile CreateProjectile(int baseIndex, MLibrary library, bool blend, int count, int interval, int skip, int lightDistance = 6, bool direction16 = true, Color? lightColour = null, uint targetID = 0)
        {
            if (targetID == 0)
            {
                targetID = TargetID;
            }

            MapObject ob = MapControl.GetObject(targetID);

            var targetPoint = TargetPoint;

            if (ob != null) targetPoint = ob.CurrentLocation;

            int duration = Functions.MaxDistance(CurrentLocation, targetPoint) * 50;

            Missile missile = new Missile(library, baseIndex, duration / interval, duration, this, targetPoint)
            {
                Target = ob,
                Interval = interval,
                FrameCount = count,
                Blend = blend,
                Skip = skip,
                Light = lightDistance,
                LightColour = lightColour == null ? Color.White : (Color)lightColour
            };

            Effects.Add(missile);

            return missile;
        }

        //Rebuild
        public void PlayStepSound()
        {
            int x = CurrentLocation.X - CurrentLocation.X % 2;
            int y = CurrentLocation.Y - CurrentLocation.Y % 2;
            if (GameScene.Scene.MapControl.M2CellInfo[x, y].FrontIndex > 199) return; //prevents any move sounds on non mir2 maps atm
            if (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleIndex > 199) return; //prevents any move sounds on non mir2 maps atm
            if (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex > 199) return; //prevents any move sounds on non mir2 maps atm

            int moveSound;

            if (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex > 99 && GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex < 199) //shanda tiles
            {
                PlayShandaStepSound(x, y, out moveSound);
            }
            else if (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex > 199 && GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex < 299) //mir3 tiles
            {
                PlayWemadeMir3StepSound(x, y, out moveSound);
            }
            else //wemade tiles
            {
                PlayWemadeStepSound(x, y, out moveSound);
            }

            if (RidingMount) moveSound = SoundList.MountWalkL;

            if (CurrentAction == MirAction.Running) moveSound += 2;
            if (FrameIndex == 4) moveSound++;

            SoundManager.PlaySound(moveSound);
        }
        private void PlayWemadeStepSound(int x, int y, out int moveSound)
        {
            int index = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackImage & 0x1FFFF) - 1;
            //index = (GameScene.Scene.MapControl.M2CellInfo[x, y].FrontIndex - 2) * 10000 + index;

            if (index >= 0 && index <= 10000)
            {
                if ((index >= 330 && index <= 349) || (index >= 450 && index <= 454) || (index >= 550 && index <= 554) ||
                    (index >= 750 && index <= 754) || (index >= 950 && index <= 954) || (index >= 1250 && index <= 1254) ||
                    (index >= 1400 && index <= 1424) || (index >= 1455 && index <= 1474) || (index >= 1500 && index <= 1524) ||
                    (index >= 1550 && index <= 1574))
                    moveSound = SoundList.WalkLawnL;
                else if ((index >= 250 && index <= 254) || (index >= 1005 && index <= 1009) || (index >= 1050 && index <= 1054) ||
                    (index >= 1060 && index <= 1064) || (index >= 1450 && index <= 1454) || (index >= 1650 && index <= 1654))
                    moveSound = SoundList.WalkRoughL;
                else if ((index >= 605 && index <= 609) || (index >= 650 && index <= 654) || (index >= 660 && index <= 664) ||
                    (index >= 2000 && index <= 2049) || (index >= 3025 && index <= 3049) || (index >= 2400 && index <= 2424) ||
                    (index >= 4625 && index <= 4649) || (index >= 4675 && index <= 4678))
                    moveSound = SoundList.WalkStoneL;
                else if ((index >= 1825 && index <= 1924) || (index >= 2150 && index <= 2174) || (index >= 3075 && index <= 3099) ||
                    (index >= 3325 && index <= 3349) || (index >= 3375 && index <= 3399))
                    moveSound = SoundList.WalkCaveL;
                else if (index == 3230 || index == 3231 || index == 3246 || index == 3277 || (index >= 3780 && index <= 3799))
                    moveSound = SoundList.WalkWoodL;
                else if (index >= 3825 && index <= 4434)
                    switch (index % 25)
                    {
                        case 0:
                            moveSound = SoundList.WalkWoodL;
                            break;
                        default:
                            moveSound = SoundList.WalkGroundL;
                            break;
                    }
                else if ((index >= 2075 && index <= 2099) || (index >= 2125 && index <= 2149))
                    moveSound = SoundList.WalkRoomL;
                else if (index >= 1800 && index <= 1824)
                    moveSound = SoundList.WalkWaterL;
                else moveSound = SoundList.WalkGroundL;

                if ((index >= 825 && index <= 1349) && (index - 825) / 25 % 2 == 0) moveSound = SoundList.WalkStoneL;
                if ((index >= 1375 && index <= 1799) && (index - 1375) / 25 % 2 == 0) moveSound = SoundList.WalkCaveL;
                if (index == 1385 || index == 1386 || index == 1391 || index == 1392) moveSound = SoundList.WalkWoodL;

                index = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleImage & 0x7FFF) - 1;
                if (index >= 0 && index <= 115)
                    moveSound = SoundList.WalkGroundL;
                else if (index >= 120 && index <= 124)
                    moveSound = SoundList.WalkLawnL;

                index = (GameScene.Scene.MapControl.M2CellInfo[x, y].FrontImage & 0x7FFF) - 1;
                if ((index >= 221 && index <= 289) || (index >= 583 && index <= 658) || (index >= 1183 && index <= 1206) ||
                    (index >= 7163 && index <= 7295) || (index >= 7404 && index <= 7414))
                    moveSound = SoundList.WalkStoneL;
                else if ((index >= 3125 && index <= 3267) || (index >= 3757 && index <= 3948) || (index >= 6030 && index <= 6999))
                    moveSound = SoundList.WalkWoodL;
                if (index >= 3316 && index <= 3589)
                    moveSound = SoundList.WalkRoomL;
            }
            else
                moveSound = SoundList.WalkGroundL;
        }

        private void PlayShandaStepSound(int x, int y, out int moveSound)
        {
            int index = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackImage & 0x1FFFF) - 1;
            //index = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex - 100) * 100000 + index;

            var tt = GameScene.Scene.MapControl.M2CellInfo[x, y];

            //CMain.SendDebugMessage(string.Format("BackImage : {0}. BackIndex : {1}. MiddleImage : {2}. MiddleIndex {3}", tt.BackImage, tt.BackIndex, tt.MiddleImage, tt.MiddleIndex));

            #region Back Tiles (Tiles)
            switch (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex)
            {
                case 100:
                    {
                        //Tiles1
                        if ((index >= 0 && index <= 39) || (index >= 50 && index <= 74) || (index >= 80 && index <= 84) || (index >= 100 && index <= 204) ||
                            (index >= 210 && index <= 249) || (index >= 255 && index <= 259) || (index >= 265 && index <= 329) || (index >= 350 && index <= 404) ||
                            (index >= 410 && index <= 449) || (index >= 455 && index <= 459) || (index >= 465 && index <= 504) ||  (index >= 510 && index <= 549) ||
                            (index >= 555 && index <= 559) || (index >= 565 && index <= 574) || (index >= 581 && index <= 581) || (index >= 586 && index <= 587) ||
                            (index >= 600 && index <= 604) || (index >= 610 && index <= 614) || (index >= 615 && index <= 649) || (index >= 655 && index <= 659) || 
                            (index >= 665 && index <= 704) || (index >= 710 && index <= 749) || (index >= 755 && index <= 759) || (index >= 765 && index <= 824) || 
                            (index >= 850 && index <= 874) || (index >= 900 && index <= 924) || (index >= 950 && index <= 974) || (index >= 1100 && index <= 1124) || 
                            (index >= 1150 && index <= 1174) || (index >= 1200 && index <= 1204) || (index >= 1210 && index <= 1214) || (index >= 1255 && index <= 1259) || 
                            (index >= 1300 && index <= 1324) || (index >= 1340 && index <= 1374) || (index >= 1560 && index <= 1604) || (index >= 1610 && index <= 1614) || 
                            (index >= 1655 && index <= 1659) || (index >= 1665 && index <= 1674) || (index >= 1700 && index <= 1724) || (index >= 1750 && index <= 1774) ||
                            (index >= 1850 && index <= 1874) || (index >= 2050 && index <= 2074) || (index >= 2100 && index <= 2124) || (index >= 2150 && index <= 2174) ||
                            (index >= 2200 && index <= 2224) ||(index >= 2250 && index <= 2274) ||(index >= 2300 && index <= 2324) ||(index >= 2350 && index <= 2374) ||
                            (index >= 2500 && index <= 2524) ||(index >= 2750 && index <= 2774) ||(index >= 3250 && index <= 3274) ||(index >= 3300 && index <= 3349) ||
                            (index >= 3375 && index <= 3404) ||(index >= 3425 && index <= 3449) ||(index >= 3475 && index <= 3499) ||(index >= 3525 && index <= 3549) ||
                            (index >= 3575 && index <= 3599) ||(index >= 3625 && index <= 3649) ||(index >= 3675 && index <= 3749) ||(index >= 3775 && index <= 3780) ||
                            (index >= 4475 && index <= 4624) ||(index >= 4680 && index <= 4824) ||(index >= 6225 && index <= 7924) ||(index >= 11079 && index <= 11079) ||
                            (index >= 11083 && index <= 11084) ||(index >= 11120 && index <= 11127) ||(index >= 11129 && index <= 11130) ||(index >= 11132 && index <= 11133) ||
                            (index >= 11135 && index <= 11136) ||(index >= 13770 && index <= 14019) ||(index >= 14170 && index <= 14219) ||(index >= 14820 && index <= 14869) ||
                            (index >= 20470 && index <= 21619) ||(index >= 25000 && index <= 31724))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 205 && index <= 209) ||(index >= 260 && index <= 264) ||(index >= 330 && index <= 349) ||(index >= 405 && index <= 409) ||
                            (index >= 460 && index <= 464) ||(index >= 505 && index <= 509) ||(index >= 560 && index <= 564) ||(index >= 575 && index <= 580) ||
                            (index >= 582 && index <= 585) ||(index >= 588 && index <= 599) ||(index >= 705 && index <= 709) ||(index >= 750 && index <= 754) ||
                            (index >= 760 && index <= 764) ||(index >= 1205 && index <= 1209) ||(index >= 1215 && index <= 1224) ||(index >= 1250 && index <= 1254) ||
                            (index >= 1260 && index <= 1274) ||(index >= 1400 && index <= 1424) ||(index >= 1455 && index <= 1459) ||(index >= 1500 && index <= 1524) ||
                            (index >= 1550 && index <= 1574) ||(index >= 2555 && index <= 2559) ||(index >= 2605 && index <= 2609) ||(index >= 2655 && index <= 2659) ||
                            (index >= 2705 && index <= 2709) ||(index >= 10320 && index <= 10324) ||(index >= 10329 && index <= 10329) ||(index >= 10334 && index <= 10334) ||
                            (index >= 10372 && index <= 10372) ||(index >= 10381 && index <= 10381) ||(index >= 10420 && index <= 10422) ||(index >= 10425 && index <= 10424) ||
                            (index >= 10429 && index <= 10429) ||(index >= 10470 && index <= 10470) ||(index >= 10472 && index <= 10472) ||(index >= 10475 && index <= 10475) ||
                            (index >= 10479 && index <= 10519) ||(index >= 10530 && index <= 10530) ||(index >= 10535 && index <= 10535) ||(index >= 10539 && index <= 10539) ||
                            (index >= 10623 && index <= 10624) ||(index >= 10628 && index <= 10629) ||(index >= 10634 && index <= 10634) ||(index >= 10639 && index <= 10639) ||
                            (index >= 10644 && index <= 10669) ||(index >= 10673 && index <= 10677) ||(index >= 10680 && index <= 10681) ||(index >= 10684 && index <= 10685) ||
                            (index >= 10688 && index <= 10690) ||(index >= 10721 && index <= 10723) ||(index >= 10725 && index <= 10727) ||(index >= 10729 && index <= 10731) ||
                            (index >= 10733 && index <= 10770) ||(index >= 10772 && index <= 10772) ||(index >= 10775 && index <= 10775) ||(index >= 10779 && index <= 10779) ||
                            (index >= 10784 && index <= 10784) ||(index >= 10835 && index <= 10836) ||(index >= 10840 && index <= 10869) ||(index >= 10874 && index <= 10874) ||
                            (index >= 10890 && index <= 10890) ||(index >= 10920 && index <= 10929) ||(index >= 10931 && index <= 10934) ||(index >= 10936 && index <= 10939) ||
                            (index >= 10941 && index <= 10970) ||(index >= 10974 && index <= 10974) ||(index >= 10977 && index <= 11019) ||(index >= 11021 && index <= 11021) ||
                            (index >= 11023 && index <= 11027) ||(index >= 11029 && index <= 11033) ||(index >= 11037 && index <= 11038) ||(index >= 11070 && index <= 11071) ||
                            (index >= 11075 && index <= 11078) ||(index >= 11080 && index <= 11082) ||(index >= 11086 && index <= 11089) ||(index >= 11091 && index <= 11119) ||
                            (index >= 11128 && index <= 11128) ||(index >= 11131 && index <= 11131) ||(index >= 11134 && index <= 11134) ||(index >= 11137 && index <= 11171) ||
                            (index >= 11175 && index <= 11177) ||(index >= 11180 && index <= 11182) ||(index >= 11185 && index <= 11219) ||(index >= 11223 && index <= 11224) ||
                            (index >= 11227 && index <= 11229) ||(index >= 11232 && index <= 11291) ||(index >= 11296 && index <= 11299) ||(index >= 11305 && index <= 11308) ||
                            (index >= 11314 && index <= 11317) ||(index >= 11322 && index <= 11327) ||(index >= 11332 && index <= 11369))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 250 && index <= 254) ||(index >= 450 && index <= 454) ||(index >= 550 && index <= 554) ||(index >= 1000 && index <= 1024) ||
                            (index >= 1050 && index <= 1074) ||(index >= 1450 && index <= 1454) ||(index >= 1460 && index <= 1464) ||(index >= 1465 && index <= 1474) ||
                            (index >= 1605 && index <= 1609) ||(index >= 1615 && index <= 1624) ||(index >= 1650 && index <= 1654) ||(index >= 1660 && index <= 1664))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 2800 && index <= 2824) ||(index >= 3130 && index <= 3130) ||(index >= 3137 && index <= 3138) ||(index >= 3177 && index <= 3178) ||
                            (index >= 3189 && index <= 3190) ||(index >= 3230 && index <= 3231) ||(index >= 3240 && index <= 3242) ||(index >= 3246 && index <= 3246) ||
                            (index >= 3276 && index <= 3278) ||(index >= 3280 && index <= 3299) ||(index >= 5178 && index <= 5179) ||(index >= 5182 && index <= 5185) ||
                            (index >= 5188 && index <= 5191) ||(index >= 5194 && index <= 5195) ||(index >= 5780 && index <= 6060) ||(index >= 6080 && index <= 6084) ||
                            (index >= 6145 && index <= 6224) ||(index >= 14020 && index <= 14169) ||(index >= 14220 && index <= 14819))
                            moveSound = SoundList.WalkWoodL;
                        else if ((index >= 825 && index <= 849) ||(index >= 875 && index <= 899) ||(index >= 925 && index <= 949) ||(index >= 975 && index <= 999) ||
                            (index >= 1025 && index <= 1049) ||(index >= 1075 && index <= 1099) ||(index >= 1125 && index <= 1149) ||(index >= 1175 && index <= 1199) ||
                            (index >= 1225 && index <= 1249) ||(index >= 1275 && index <= 1299) ||(index >= 1325 && index <= 1339) ||(index >= 2025 && index <= 2049) ||
                            (index >= 2075 && index <= 2099) ||(index >= 2125 && index <= 2149) ||(index >= 2900 && index <= 2924) ||(index >= 3781 && index <= 4474) ||
                            (index >= 6085 && index <= 6144))
                            moveSound = SoundList.WalkWoodL;
                        else if ((index >= 40 && index <= 49) ||(index >= 75 && index <= 79) ||(index >= 85 && index <= 99) ||(index >= 1375 && index <= 1399) ||
                            (index >= 1425 && index <= 1449) ||(index >= 1475 && index <= 1499) ||(index >= 1525 && index <= 1549) ||(index >= 1575 && index <= 1559) ||
                            (index >= 1625 && index <= 1649) ||(index >= 1675 && index <= 1699) ||(index >= 1725 && index <= 1749) ||(index >= 1775 && index <= 1799) ||
                            (index >= 1825 && index <= 1849) ||(index >= 1875 && index <= 2024) ||(index >= 2175 && index <= 2199) ||(index >= 2225 && index <= 2249) ||
                            (index >= 2275 && index <= 2299) ||(index >= 2325 && index <= 2349) ||(index >= 2375 && index <= 2499) ||(index >= 2525 && index <= 2549) ||
                            (index >= 2575 && index <= 2599) ||(index >= 2625 && index <= 2649) ||(index >= 2675 && index <= 2699) ||(index >= 2725 && index <= 2749) ||
                            (index >= 2775 && index <= 2799) ||(index >= 2825 && index <= 2849) ||(index >= 2875 && index <= 2899) ||(index >= 2925 && index <= 2994) ||
                            (index >= 2995 && index <= 2999) ||(index >= 3025 && index <= 3129) ||(index >= 3131 && index <= 3136) ||(index >= 3139 && index <= 3176) ||
                            (index >= 3179 && index <= 3188) ||(index >= 3191 && index <= 3229) ||(index >= 3232 && index <= 3239) ||(index >= 3243 && index <= 3245) ||
                            (index >= 3247 && index <= 3249) ||(index >= 3275 && index <= 3275) ||(index >= 3279 && index <= 3279) ||(index >= 3350 && index <= 3374) ||
                            (index >= 3450 && index <= 3474) ||(index >= 3500 && index <= 3524) ||(index >= 3550 && index <= 3574) ||(index >= 3600 && index <= 3624) ||
                            (index >= 3650 && index <= 3674) ||(index >= 3750 && index <= 3774) ||(index >= 4825 && index <= 5177) ||(index >= 5180 && index <= 5181) ||
                            (index >= 5186 && index <= 5187) ||(index >= 5192 && index <= 5193) ||(index >= 5196 && index <= 5779) ||(index >= 7925 && index <= 10305) ||
                            (index >= 11370 && index <= 13769) ||(index >= 14870 && index <= 20469) ||(index >= 21620 && index <= 24999))
                            moveSound = SoundList.WalkCaveL;
                        else if ((index >= 605 && index <= 609) ||(index >= 650 && index <= 654) ||(index >= 660 && index <= 664) ||(index >= 2550 && index <= 2554) ||
                            (index >= 2560 && index <= 2564) ||(index >= 2565 && index <= 2574) ||(index >= 2600 && index <= 2604) ||(index >= 2610 && index <= 2624) ||
                            (index >= 2650 && index <= 2654) ||(index >= 2660 && index <= 2674) ||(index >= 2700 && index <= 2704) ||(index >= 2710 && index <= 2724) ||
                            (index >= 2850 && index <= 2874) ||(index >= 3405 && index <= 3424) ||(index >= 4625 && index <= 4679) ||(index >= 6075 && index <= 6079) ||
                            (index >= 10325 && index <= 10328) ||(index >= 10330 && index <= 10333) ||(index >= 10335 && index <= 10371) ||(index >= 10373 && index <= 10380) ||
                            (index >= 10382 && index <= 10419) ||(index >= 10423 && index <= 10424) ||(index >= 10426 && index <= 10428) ||(index >= 10430 && index <= 10469) ||
                            (index >= 10471 && index <= 10471) ||(index >= 10473 && index <= 10474) ||(index >= 10476 && index <= 10478) ||(index >= 10520 && index <= 10529) ||
                            (index >= 10531 && index <= 10534) ||(index >= 10536 && index <= 10538) ||(index >= 10540 && index <= 10622) ||(index >= 10625 && index <= 10627) ||
                            (index >= 10630 && index <= 10633) ||(index >= 10635 && index <= 10638) ||(index >= 10640 && index <= 10643) ||(index >= 10670 && index <= 10672) ||
                            (index >= 10771 && index <= 10771) ||(index >= 10773 && index <= 10774) ||(index >= 10776 && index <= 10778) ||(index >= 10780 && index <= 10783) ||
                            (index >= 10785 && index <= 10834) ||(index >= 10837 && index <= 10839) ||(index >= 10870 && index <= 10873) ||(index >= 10875 && index <= 10889) ||
                            (index >= 10891 && index <= 10919) ||(index >= 10930 && index <= 10930) ||(index >= 10935 && index <= 10935) ||(index >= 10940 && index <= 10940) ||
                            (index >= 10971 && index <= 10973) ||(index >= 10975 && index <= 10976) ||(index >= 11020 && index <= 11020) ||(index >= 11022 && index <= 11022) ||
                            (index >= 11028 && index <= 11028) ||(index >= 11072 && index <= 11074))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 1800 && index <= 1824) || (index >= 3000 && index <= 3024) ||(index >= 10678 && index <= 10679) ||
                            (index >= 10682 && index <= 10683) ||(index >= 10686 && index <= 10687) ||(index >= 10691 && index <= 10720) ||
                            (index >= 10724 && index <= 10724) ||(index >= 10728 && index <= 10728) ||(index >= 10732 && index <= 10732) ||
                            (index >= 11034 && index <= 11036) ||(index >= 11039 && index <= 11069) ||(index >= 11085 && index <= 11085) ||
                            (index >= 11090 && index <= 11090) ||(index >= 11172 && index <= 11174) ||(index >= 11178 && index <= 11179) ||
                            (index >= 11183 && index <= 11184) ||(index >= 11220 && index <= 11222) ||(index >= 11225 && index <= 11226) ||
                            (index >= 11230 && index <= 11231) ||(index >= 11292 && index <= 11295) ||(index >= 11300 && index <= 11304) ||
                            (index >= 11309 && index <= 11313) ||(index >= 11318 && index <= 11321) ||(index >= 11328 && index <= 11331))
                            moveSound = SoundList.WalkWaterL;
                        else
                            moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 101:
                    {
                        //Tiles2
                        if ((index >= 0 && index <= 74) || (index >= 79 && index <= 83) || (index >= 94 && index <= 204) || (index >= 209 && index <= 213) ||
                            (index >= 209 && index <= 213) || (index >= 224 && index <= 249) || (index >= 255 && index <= 258) || (index >= 274 && index <= 329) ||
                            (index >= 350 && index <= 404) || (index >= 424 && index <= 449) || (index >= 455 && index <= 459) || (index >= 474 && index <= 504) ||
                            (index >= 509 && index <= 513) || (index >= 524 && index <= 549) || (index >= 555 && index <= 559) || (index >= 565 && index <= 573) ||
                            (index >= 594 && index <= 704) || (index >= 709 && index <= 713) || (index >= 724 && index <= 748) || (index >= 755 && index <= 759) ||
                            (index >= 774 && index <= 904) || (index >= 909 && index <= 923) || (index >= 974 && index <= 1004) || (index >= 1055 && index <= 1058) ||
                            (index >= 1064 && index <= 1204) || (index >= 1209 && index <= 1213) || (index >= 1300 && index <= 1373) || (index >= 1574 && index <= 1604) ||
                            (index >= 1609 && index <= 1649) || (index >= 1655 && index <= 1659) || (index >= 1674 && index <= 1704) || (index >= 1709 && index <= 1723) ||
                            (index >= 1755 && index <= 1758) || (index >= 1765 && index <= 1799) || (index >= 2050 && index <= 2523) || (index >= 2724 && index <= 2849) ||
                            (index >= 2950 && index <= 2999) || (index >= 3150 && index <= 3804) || (index >= 3809 && index <= 3849) || (index >= 3855 && index <= 3858) ||
                            (index >= 4475 && index <= 4794) || (index >= 5024 && index <= 5054) || (index >= 5059 && index <= 5299))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 205 && index <= 208) || (index >= 214 && index <= 223) || (index >= 264 && index <= 273) || (index >= 330 && index <= 349) ||
                            (index >= 405 && index <= 423) || (index >= 460 && index <= 474) || (index >= 505 && index <= 508) || (index >= 514 && index <= 523) ||
                            (index >= 560 && index <= 564) || (index >= 714 && index <= 723) || (index >= 764 && index <= 773) || (index >= 905 && index <= 908) ||
                            (index >= 955 && index <= 973) || (index >= 1009 && index <= 1023) || (index >= 1205 && index <= 1208) || (index >= 5055 && index <= 5058))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 250 && index <= 254) || (index >= 259 && index <= 263) || (index >= 450 && index <= 454) || (index >= 550 && index <= 554) ||
                            (index >= 574 && index <= 593) || (index >= 705 && index <= 708) || (index >= 749 && index <= 754) || (index >= 924 && index <= 954) ||
                            (index >= 1005 && index <= 1008) || (index >= 1024 && index <= 1054) || (index >= 1059 && index <= 1063) || (index >= 760 && index <= 763) ||
                            (index >= 1214 && index <= 1299) || (index >= 1374 && index <= 1573) || (index >= 1605 && index <= 1608) || (index >= 1650 && index <= 1654) ||
                            (index >= 1660 && index <= 1673) || (index >= 1705 && index <= 1708) || (index >= 1724 && index <= 1754) || (index >= 1759 && index <= 1764) ||
                            (index >= 2555 && index <= 2558) || (index >= 2605 && index <= 2608) || (index >= 2655 && index <= 2658) || (index >= 2705 && index <= 2708))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 75 && index <= 78) || (index >= 84 && index <= 93) || (index >= 2524 && index <= 2554) || (index >= 2560 && index <= 2604) ||
                            (index >= 2609 && index <= 2654) || (index >= 2659 && index <= 2704) || (index >= 2709 && index <= 2723) || (index >= 2850 && index <= 2949) ||
                            (index >= 3805 && index <= 3808) || (index >= 3850 && index <= 3854) || (index >= 3859 && index <= 3873) || (index >= 5300 && index <= 5323) ||
                            (index >= 6052 && index <= 6118))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 4795 && index <= 5023))
                            moveSound = SoundList.WalkCaveL;
                        else if ((index >= 1800 && index <= 2049) || (index >= 3000 && index <= 3149))
                            moveSound = SoundList.WalkWaterL;
                        else if ((index >= 5324 && index <= 6051) || (index >= 6119 && index <= 6296))
                            moveSound = SoundList.WalkRoomL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 102:
                    {
                        //Tiles3
                        if ((index >= 0 && index <= 299) || (index >= 400 && index <= 449) || (index >= 455 && index <= 522) || (index >= 528 && index <= 531) ||
                        (index >= 1553 && index <= 1554) || (index >= 1560 && index <= 1561) || (index >= 1565 && index <= 1566) || (index >= 1569 && index <= 1699) ||
                        (index >= 1805 && index <= 1809) || (index >= 1850 && index <= 1854) || (index >= 1860 && index <= 1864) || (index >= 1950 && index <= 1954) ||
                        (index >= 2000 && index <= 2204) || (index >= 2300 && index <= 2653))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 300 && index <= 399) || (index >= 450 && index <= 454) || (index >= 524 && index <= 527) || (index >= 1705 && index <= 1709) ||
                            (index >= 1715 && index <= 1799) || (index >= 2205 && index <= 2299))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 1700 && index <= 1704) || (index >= 1710 && index <= 1714))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 1800 && index <= 1804) || (index >= 1810 && index <= 1849) || (index >= 1855 && index <= 1859) || (index >= 1865 && index <= 1949) ||
                            (index >= 1955 && index <= 1999))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 1411 && index <= 1550) || (index >= 1555 && index <= 1557))
                            moveSound = SoundList.WalkWoodL;
                        else if ((index >= 532 && index <= 1410) || (index >= 1551 && index <= 1552) || (index >= 1558 && index <= 1559) || (index >= 1562 && index <= 1564) ||
                            (index >= 1567 && index <= 1568))
                            moveSound = SoundList.WalkWaterL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 103:
                    {
                        //Tiles4
                        if ((index >= 0000 && index <= 199) || (index >= 0205 && index <= 209) || (index >= 0250 && index <= 254) || (index >= 260 && index <= 1481) ||
                            (index >= 1495 && index <= 1549) || (index >= 1555 && index <= 1559) || (index >= 2500 && index <= 3349) || (index >= 3355 && index <= 3359) ||
                            (index >= 3365 && index <= 3399) || (index >= 3500 && index <= 3899) || (index >= 4193 && index <= 4254) || (index >= 4650 && index <= 4849) ||
                            (index >= 5000 && index <= 5149) || (index >= 5155 && index <= 5399) || (index >= 5405 && index <= 5409) || (index >= 5415 && index <= 5449) ||
                            (index >= 5550 && index <= 5554) || (index >= 5560 && index <= 5564) || (index >= 5605 && index <= 5609) || (index >= 5615 && index <= 5625) ||
                            (index >= 5680 && index <= 5749) || (index >= 7400 && index <= 7649) || (index >= 7655 && index <= 7749) || (index >= 7900 && index <= 8099) ||
                            (index >= 8166 && index <= 8299) || (index >= 8305 && index <= 8309) || (index >= 8350 && index <= 8354) || (index >= 8360 && index <= 8364) ||
                            (index >= 8400 && index <= 8499) || (index >= 8605 && index <= 8609) || (index >= 8615 && index <= 8619) || (index >= 8623 && index <= 8654) ||
                            (index >= 8660 && index <= 8754) || (index >= 8805 && index <= 8809) || (index >= 9050 && index <= 9299) || (index >= 9450 && index <= 9549))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 200 && index <= 204) || (index >= 210 && index <= 249) || (index >= 255 && index <= 259) || (index >= 1482 && index <= 1494) ||
                            (index >= 1560 && index <= 2499) || (index >= 4255 && index <= 4299) || (index >= 4305 && index <= 4349) || (index >= 5555 && index <= 5559) ||
                            (index >= 5565 && index <= 5604) || (index >= 5610 && index <= 5614) || (index >= 7650 && index <= 7654) || (index >= 7750 && index <= 7899) ||
                            (index >= 8100 && index <= 8165) || (index >= 8300 && index <= 8304) || (index >= 8310 && index <= 8349) || (index >= 8355 && index <= 8359) ||
                            (index >= 8365 && index <= 8399) || (index >= 8505 && index <= 8599) || (index >= 8610 && index <= 8614) || (index >= 8620 && index <= 8622) ||
                            (index >= 8655 && index <= 8659) || (index >= 8755 && index <= 8799) || (index >= 8810 && index <= 8849) || (index >= 8950 && index <= 8999) ||
                            (index >= 9005 && index <= 9049) || (index >= 9550 && index <= 9554))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 4300 && index <= 4304) || (index >= 8600 && index <= 8604) || (index >= 1550 && index <= 1554) || (index >= 4131 && index <= 4193) ||
                            (index >= 4350 && index <= 4549) || (index >= 5150 && index <= 5154) || (index >= 8500 && index <= 8504) || (index >= 8800 && index <= 8804) ||
                            (index >= 8850 && index <= 8949) || (index >= 9000 && index <= 9004))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 3350 && index <= 3354) || (index >= 3360 && index <= 3364) || (index >= 3400 && index <= 3499) || (index >= 5400 && index <= 5404) ||
                            (index >= 5410 && index <= 5414) || (index >= 5450 && index <= 5549) || (index >= 5626 && index <= 5679) || (index >= 9300 && index <= 9449) ||
                            (index >= 9555 && index <= 9749))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 3900 && index <= 4130) || (index >= 4550 && index <= 4649) || (index >= 4850 && index <= 4999) || (index >= 5750 && index <= 7399) ||
                            (index >= 9750 && index <= 11349))
                            moveSound = SoundList.WalkCaveL;
                        else if ((index >= 11350 && index <= 13534))
                            moveSound = SoundList.WalkWoodL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 104:
                    {
                        //Tiles5
                        if ((index >= 10 && index <= 0014) || (index >= 100 && index <= 0104) || (index >= 110 && index <= 0114) || (index >= 156 && index <= 0159) ||
                            (index >= 165 && index <= 0204) || (index >= 210 && index <= 0214) || (index >= 255 && index <= 0259) || (index >= 300 && index <= 0304) ||
                            (index >= 310 && index <= 0314) || (index >= 400 && index <= 0404) || (index >= 410 && index <= 0414) || (index >= 500 && index <= 0504) ||
                            (index >= 510 && index <= 0514) || (index >= 555 && index <= 0559) || (index >= 800 && index <= 0849) || (index >= 855 && index <= 0859) ||
                            (index >= 900 && index <= 0904) || (index >= 910 && index <= 0914) || (index >= 955 && index <= 0959) || (index >= 1000 && index <= 1004) ||
                            (index >= 1010 && index <= 1014) || (index >= 1055 && index <= 1059) || (index >= 1400 && index <= 1404) || (index >= 1410 && index <= 1449) ||
                            (index >= 1455 && index <= 1459) || (index >= 1465 && index <= 1499) || (index >= 1700 && index <= 1749) || (index >= 1755 && index <= 1759) ||
                            (index >= 1765 && index <= 1804) || (index >= 1810 && index <= 1849) || (index >= 1855 && index <= 1859) || (index >= 1900 && index <= 1904) ||
                            (index >= 1910 && index <= 1949) || (index >= 1955 && index <= 1959) || (index >= 2000 && index <= 2099) || (index >= 2150 && index <= 2154) ||
                            (index >= 2160 && index <= 2164) || (index >= 2250 && index <= 2254) || (index >= 2260 && index <= 2263) || (index >= 2300 && index <= 2399) ||
                            (index >= 2500 && index <= 2549) || (index >= 2555 && index <= 2559) || (index >= 2565 && index <= 2699) || (index >= 2750 && index <= 2754) ||
                            (index >= 2760 && index <= 2764) || (index >= 2800 && index <= 2804) || (index >= 2810 && index <= 2814) || (index >= 2855 && index <= 2859) ||
                            (index >= 2865 && index <= 2904) || (index >= 2910 && index <= 2914) || (index >= 2955 && index <= 2959) || (index >= 2965 && index <= 3004) ||
                            (index >= 3010 && index <= 3014) || (index >= 3100 && index <= 3104) || (index >= 3111 && index <= 3114) || (index >= 3200 && index <= 3204) ||
                            (index >= 3300 && index <= 3304) || (index >= 3800 && index <= 3804) || (index >= 3900 && index <= 3904) || (index >= 3910 && index <= 3914) ||
                            (index >= 4000 && index <= 4004) || (index >= 4010 && index <= 4014) || (index >= 4100 && index <= 4104) || (index >= 4110 && index <= 4114) ||
                            (index >= 4200 && index <= 4204) || (index >= 4210 && index <= 4214) || (index >= 4300 && index <= 4304) || (index >= 4310 && index <= 4311) ||
                            (index >= 4500 && index <= 4504) || (index >= 4510 && index <= 4514) || (index >= 4600 && index <= 4604) || (index >= 4610 && index <= 4614) ||
                            (index >= 4620 && index <= 4621) || (index >= 4655 && index <= 4659) || (index >= 4700 && index <= 4904) || (index >= 4910 && index <= 4949) ||
                            (index >= 4955 && index <= 4959) || (index >= 5000 && index <= 5004) || (index >= 5010 && index <= 5014) || (index >= 5100 && index <= 5104) ||
                            (index >= 5110 && index <= 5114) || (index >= 5155 && index <= 5159) || (index >= 5165 && index <= 5204) || (index >= 5210 && index <= 5254) ||
                            (index >= 5260 && index <= 5264) || (index >= 5305 && index <= 5309) || (index >= 5355 && index <= 5359) || (index >= 5400 && index <= 5599) ||
                            (index >= 5650 && index <= 5654) || (index >= 5700 && index <= 5799) || (index >= 5900 && index <= 6049) || (index >= 6100 && index <= 6399) ||
                            (index >= 6850 && index <= 6949) || (index >= 7050 && index <= 7349) || (index >= 7750 && index <= 7754) || (index >= 7760 && index <= 7764) ||
                            (index >= 7805 && index <= 7809) || (index >= 7815 && index <= 7849) || (index >= 7950 && index <= 7999) || (index >= 8150 && index <= 8399))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 710 && index <= 0714) || (index >= 860 && index <= 0899) || (index >= 905 && index <= 0909) || (index >= 915 && index <= 0949) ||
                            (index >= 960 && index <= 0999) || (index >= 1005 && index <= 1009) || (index >= 1015 && index <= 1049) || (index >= 1060 && index <= 1099) ||
                            (index >= 1105 && index <= 1109) || (index >= 1160 && index <= 1199) || (index >= 1260 && index <= 1264) || (index >= 1305 && index <= 1309) ||
                            (index >= 1360 && index <= 1364) || (index >= 1405 && index <= 1409) || (index >= 1460 && index <= 1464) || (index >= 1500 && index <= 1549) ||
                            (index >= 1555 && index <= 1599) || (index >= 1608 && index <= 1609) || (index >= 1660 && index <= 1664) || (index >= 1760 && index <= 1764) ||
                            (index >= 1805 && index <= 1809) || (index >= 1860 && index <= 1899) || (index >= 1905 && index <= 1909) || (index >= 1960 && index <= 1999) ||
                            (index >= 2400 && index <= 2499) || (index >= 2550 && index <= 2554) || (index >= 2560 && index <= 2564) || (index >= 3400 && index <= 3404) ||
                            (index >= 3410 && index <= 3414) || (index >= 3455 && index <= 3459) || (index >= 3465 && index <= 3504) || (index >= 3510 && index <= 3514) ||
                            (index >= 3555 && index <= 3559) || (index >= 3565 && index <= 3604) || (index >= 3610 && index <= 3614) || (index >= 3700 && index <= 3704) ||
                            (index >= 3710 && index <= 3714) || (index >= 4400 && index <= 4404) || (index >= 4410 && index <= 4414) || (index >= 4904 && index <= 4909) ||
                            (index >= 4950 && index <= 4954) || (index >= 4960 && index <= 4999) || (index >= 5005 && index <= 5009) || (index >= 5015 && index <= 5099) ||
                            (index >= 5105 && index <= 5109) || (index >= 5115 && index <= 5154) || (index >= 5160 && index <= 5164) || (index >= 5205 && index <= 5209) ||
                            (index >= 5225 && index <= 5259) || (index >= 5265 && index <= 5304) || (index >= 5310 && index <= 5354) || (index >= 5360 && index <= 5399) ||
                            (index >= 5600 && index <= 5649) || (index >= 5655 && index <= 5699) || (index >= 6050 && index <= 6099) || (index >= 6950 && index <= 7049) ||
                            (index >= 7350 && index <= 7449) || (index >= 7855 && index <= 7859) || (index >= 7865 && index <= 7899))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 600 && index <= 0604) || (index >= 610 && index <= 0614) || (index >= 655 && index <= 0659) || (index >= 700 && index <= 0704) ||
                            (index >= 850 && index <= 0854) || (index >= 950 && index <= 0954) || (index >= 1050 && index <= 1054) || (index >= 1150 && index <= 1154) ||
                            (index >= 1250 && index <= 1254) || (index >= 1350 && index <= 1354) || (index >= 1450 && index <= 1454) || (index >= 1550 && index <= 1554) ||
                            (index >= 1650 && index <= 1654) || (index >= 1750 && index <= 1754) || (index >= 1850 && index <= 1854) || (index >= 1950 && index <= 1954) ||
                            (index >= 5800 && index <= 5899) || (index >= 7450 && index <= 7454) || (index >= 7460 && index <= 7464) || (index >= 7505 && index <= 7509) ||
                            (index >= 7515 && index <= 7554) || (index >= 7560 && index <= 7564) || (index >= 7605 && index <= 7609) || (index >= 7615 && index <= 7649) ||
                            (index >= 7655 && index <= 7659) || (index >= 7665 && index <= 7704) || (index >= 7755 && index <= 7759) || (index >= 7765 && index <= 7804) ||
                            (index >= 7810 && index <= 7814) || (index >= 7850 && index <= 7854) || (index >= 7860 && index <= 7864))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 5 && index <= 0009) || (index >= 15 && index <= 0099) || (index >= 105 && index <= 0109) || (index >= 115 && index <= 0155) ||
                            (index >= 160 && index <= 0164) || (index >= 205 && index <= 0209) || (index >= 215 && index <= 0254) || (index >= 260 && index <= 0299) ||
                            (index >= 305 && index <= 0309) || (index >= 315 && index <= 0399) || (index >= 405 && index <= 0409) || (index >= 415 && index <= 0499) ||
                            (index >= 505 && index <= 0509) || (index >= 515 && index <= 0554) || (index >= 560 && index <= 0599) || (index >= 605 && index <= 0609) ||
                            (index >= 615 && index <= 0654) || (index >= 660 && index <= 0699) || (index >= 705 && index <= 0709) || (index >= 715 && index <= 0799) ||
                            (index >= 1100 && index <= 1104) || (index >= 1110 && index <= 1149) || (index >= 1155 && index <= 1159) || (index >= 1200 && index <= 1249) ||
                            (index >= 1255 && index <= 1259) || (index >= 1265 && index <= 1304) || (index >= 1310 && index <= 1349) || (index >= 1355 && index <= 1359) ||
                            (index >= 1365 && index <= 1399) || (index >= 1600 && index <= 1604) || (index >= 1606 && index <= 1607) || (index >= 1610 && index <= 1649) ||
                            (index >= 1655 && index <= 1659) || (index >= 1665 && index <= 1699) || (index >= 2100 && index <= 2149) || (index >= 2155 && index <= 2159) ||
                            (index >= 2165 && index <= 2249) || (index >= 2255 && index <= 2259) || (index >= 2264 && index <= 2299) || (index >= 2700 && index <= 2749) ||
                            (index >= 2755 && index <= 2759) || (index >= 2765 && index <= 2799) || (index >= 2805 && index <= 2809) || (index >= 2815 && index <= 2854) ||
                            (index >= 2860 && index <= 2864) || (index >= 2905 && index <= 2909) || (index >= 2915 && index <= 2954) || (index >= 2960 && index <= 2964) ||
                            (index >= 3005 && index <= 3009) || (index >= 3015 && index <= 3099) || (index >= 3105 && index <= 3110) || (index >= 3115 && index <= 3199) ||
                            (index >= 3205 && index <= 3299) || (index >= 3305 && index <= 3399) || (index >= 3405 && index <= 3409) || (index >= 3415 && index <= 3454) ||
                            (index >= 3460 && index <= 3464) || (index >= 3505 && index <= 3509) || (index >= 3515 && index <= 3554) || (index >= 3560 && index <= 3564) ||
                            (index >= 3605 && index <= 3609) || (index >= 3615 && index <= 3699) || (index >= 3705 && index <= 3709) || (index >= 3715 && index <= 3799) ||
                            (index >= 3805 && index <= 3899) || (index >= 3905 && index <= 3909) || (index >= 3915 && index <= 3999) || (index >= 4005 && index <= 4009) ||
                            (index >= 4015 && index <= 4099) || (index >= 4105 && index <= 4109) || (index >= 4115 && index <= 4199) || (index >= 4205 && index <= 4209) ||
                            (index >= 4305 && index <= 4309) || (index >= 4312 && index <= 4399) || (index >= 4405 && index <= 4409) || (index >= 4415 && index <= 4499) ||
                            (index >= 4505 && index <= 4509) || (index >= 4515 && index <= 4599) || (index >= 4605 && index <= 4609) || (index >= 4615 && index <= 4619) ||
                            (index >= 4622 && index <= 4654) || (index >= 4660 && index <= 4699) || (index >= 6400 && index <= 6849) || (index >= 7455 && index <= 7459) ||
                            (index >= 7465 && index <= 7504) || (index >= 7510 && index <= 7514) || (index >= 7555 && index <= 7559) || (index >= 7565 && index <= 7604) ||
                            (index >= 7610 && index <= 7614) || (index >= 7650 && index <= 7654) || (index >= 7660 && index <= 7664) ||
                            (index >= 7705 && index <= 7709) || (index >= 7715 && index <= 7749))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 8000 && index <= 8149) || (index >= 8400 && index <= 8481))
                            moveSound = SoundList.WalkCaveL;
                        else if ((index >= 7900 && index <= 7949))
                            moveSound = SoundList.WalkWaterL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 105:
                    {
                        //Tiles6
                        if (index >= 0 && index <= 1539)
                            moveSound = SoundList.WalkLawnL;
                        else if (index >= 1540 && index <= 2368)
                            moveSound = SoundList.WalkRoomL;
                        else
                            moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 106:
                    {
                        //Tiles7
                        if ((index >= 0 && index <= 19) || (index >= 24 && index <= 053) || (index >= 56 && index <= 69) || (index >= 72 && index <= 74) ||
                            (index >= 77 && index <= 93) || (index >= 99 && index <= 115) || (index >= 121 && index <= 124) || (index >= 127 && index <= 132) ||
                            (index >= 135 && index <= 141) || (index >= 185 && index <= 190) || (index >= 191 && index <= 200) || (index >= 927 && index <= 940) ||
                            (index >= 961 && index <= 1160))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 201 && index <= 926) || (index >= 941 && index <= 960) || (index >= 1161 && index <= 3310))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 96 && index <= 98))
                            moveSound = SoundList.WalkWoodL;
                        else if ((index >= 94 && index <= 95) || (index >= 151 && index <= 184))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 20 && index <= 23) || (index >= 54 && index <= 55) || (index >= 70 && index <= 71) || (index >= 75 && index <= 76) ||
                            (index >= 116 && index <= 120))
                            moveSound = SoundList.WalkCaveL;
                        else if ((index >= 125 && index <= 126) || (index >= 133 && index <= 134) || (index >= 142 && index <= 151))
                            moveSound = SoundList.WalkWaterL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                case 107:
                    {
                        //Tiles8
                        if (index >= 0 && index <= 1215)
                            moveSound = SoundList.WalkCaveL;
                        else moveSound = SoundList.WalkWaterL;
                    }
                    break;
                default:
                    moveSound = SoundList.WalkWaterL;
                    break;
            }
            #endregion
            
            index = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleImage & 0x1FFFF) - 1;

            #region Middle Tiles
            switch (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleIndex)
            {
                case 114:
                    {
                        //SMTiles5  iJam Ver
                        if ((index >= 3165 && index <= 21311) || (index >= 23775 && index <= 24650))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 2773 && index <= 3164) || (index >= 21416 && index <= 21462) || (index >= 21510 && index <= 21511) || (index >= 21528 && index <= 21531) ||
                                (index >= 21548 && index <= 21533) || (index >= 21569 && index <= 21575) || (index >= 21591 && index <= 21594) || (index >= 21609 && index <= 21613) ||
                                (index >= 21627 && index <= 21632) || (index >= 21646 && index <= 21650) || (index >= 21634 && index <= 21667) || (index >= 22977 && index <= 23316) ||
                                (index >= 30473 && index <= 31255))
                            moveSound = SoundList.WalkLawnL;
                        else if ((index >= 24651 && index <= 30472))
                            moveSound = SoundList.WalkRoomL;
                        else if ((index >= 0 && index <= 2772) || (index >= 21312 && index <= 21415) || (index >= 21463 && index <= 21509) || (index >= 21512 && index <= 21527) ||
                            (index >= 21532 && index <= 21547) || (index >= 21534 && index <= 21568) || (index >= 21576 && index <= 21590) || (index >= 21595 && index <= 21608) ||
                            (index >= 21614 && index <= 21626) || (index >= 21633 && index <= 21645) || (index >= 21651 && index <= 21633) || (index >= 21668 && index <= 22976) ||
                            (index >= 23317 && index <= 23774) || (index >= 31256 && index <= 31302))
                            moveSound = SoundList.WalkStoneL;
                    }
                    break;
                case 116:
                    {
                        //SMTiles7  iJam Ver
                        if ((index >= 10 && index <= 13) || (index >= 23 && index <= 25) || (index >= 35 && index <= 37) || (index >= 47 && index <= 49) ||
                            (index >= 59 && index <= 61) || (index >= 71 && index <= 73) || (index >= 83 && index <= 85) || (index >= 95 && index <= 97) ||
                            (index >= 107 && index <= 109) || (index >= 119 && index <= 121) || (index >= 131 && index <= 133) || (index >= 143 && index <= 145) ||
                            (index >= 155 && index <= 157) || (index >= 167 && index <= 169) || (index >= 179 && index <= 181) || (index >= 191 && index <= 193) ||
                            (index >= 203 && index <= 205) || (index >= 215 && index <= 217) || (index >= 227 && index <= 229) || (index >= 239 && index <= 241) ||
                            (index >= 251 && index <= 253) || (index >= 263 && index <= 264) || (index >= 269 && index <= 271) || (index >= 281 && index <= 283) ||
                            (index >= 297 && index <= 299) || (index >= 311 && index <= 313) || (index >= 323 && index <= 325) || (index >= 333 && index <= 335) ||
                            (index >= 341 && index <= 343) || (index >= 347 && index <= 349) || (index >= 351 && index <= 353) || (index >= 488 && index <= 489) ||
                            (index >= 494 && index <= 495) || (index >= 502 && index <= 503) || (index >= 512 && index <= 514) || (index >= 524 && index <= 526) ||
                            (index >= 538 && index <= 540) || (index >= 554 && index <= 556) || (index >= 572 && index <= 573) || (index >= 648 && index <= 649) ||
                            (index >= 665 && index <= 667) || (index >= 677 && index <= 679) || (index >= 689 && index <= 690) || (index >= 701 && index <= 702) ||
                            (index >= 713 && index <= 714) || (index >= 1260 && index <= 1567) || (index >= 2772 && index <= 5869) || (index >= 8887 && index <= 10230))
                            moveSound = SoundList.WalkGroundL;
                        else if ((index >= 2013 && index <= 2438) || (index >= 2639 && index <= 2771))
                            moveSound = SoundList.WalkRoughL;
                        else if ((index >= 7810 && index <= 7814) || (index >= 7827 && index <= 7832) || (index >= 7847 && index <= 7853) || (index >= 7866 && index <= 7867) ||
                            (index >= 7872 && index <= 7874) || (index >= 7884 && index <= 7887) || (index >= 7892 && index <= 7910) || (index >= 7916 && index <= 7923) ||
                            (index >= 7929 && index <= 7990))
                            moveSound = SoundList.WalkWoodL;
                        else if ((index >= 5870 && index <= 7753) || (index >= 7868 && index <= 7871) || (index >= 7888 && index <= 7891) || (index >= 7991 && index <= 8886) ||
                            (index >= 10231 && index <= 10430))
                            moveSound = SoundList.WalkRoomL;
                        else if ((index >= 0 && index <= 9) || (index >= 14 && index <= 22) || (index >= 26 && index <= 34) || (index >= 38 && index <= 46) ||
                            (index >= 50 && index <= 58) || (index >= 62 && index <= 70) || (index >= 74 && index <= 82) || (index >= 86 && index <= 94) ||
                            (index >= 98 && index <= 106) || (index >= 110 && index <= 118) || (index >= 122 && index <= 130) || (index >= 134 && index <= 142) ||
                            (index >= 146 && index <= 154) || (index >= 158 && index <= 166) || (index >= 170 && index <= 178) || (index >= 182 && index <= 190) ||
                            (index >= 194 && index <= 202) || (index >= 206 && index <= 214) || (index >= 218 && index <= 226) || (index >= 230 && index <= 238) ||
                            (index >= 242 && index <= 250) || (index >= 254 && index <= 262) || (index >= 265 && index <= 268) || (index >= 272 && index <= 280) ||
                            (index >= 284 && index <= 296) || (index >= 300 && index <= 310) || (index >= 314 && index <= 322) || (index >= 326 && index <= 332) ||
                            (index >= 336 && index <= 340) || (index >= 344 && index <= 346) || (index >= 350 && index <= 350) || (index >= 354 && index <= 487) ||
                            (index >= 490 && index <= 493) || (index >= 496 && index <= 501) || (index >= 504 && index <= 511) || (index >= 515 && index <= 523) ||
                            (index >= 527 && index <= 537) || (index >= 541 && index <= 553) || (index >= 557 && index <= 571) || (index >= 574 && index <= 647) ||
                            (index >= 650 && index <= 664) || (index >= 668 && index <= 676) || (index >= 680 && index <= 688) || (index >= 691 && index <= 700) ||
                            (index >= 703 && index <= 712) || (index >= 715 && index <= 1259) || (index >= 1568 && index <= 2012) || (index >= 7754 && index <= 7809) ||
                            (index >= 7815 && index <= 7826) || (index >= 7833 && index <= 7846) || (index >= 7854 && index <= 7865) || (index >= 7875 && index <= 7883) ||
                            (index >= 7911 && index <= 7915) || (index >= 7924 && index <= 7928))
                            moveSound = SoundList.WalkStoneL;
                        else if ((index >= 2439 && index <= 2638))
                            moveSound = SoundList.WalkCaveL;
                    }
                    break;
            }
            #endregion

            index = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleIndex - 110) * 100000 + index;

            #region Middle Tiles old code method

            if (index >= 0 && index <= 99999)
            {
                //SMTiles
                if (index >= 0 && index <= 7)
                    moveSound = SoundList.WalkGroundL;
                else if (index >= 8 && index <= 1175)
                    moveSound = SoundList.WalkLawnL;
            }

            else if (index >= 100000 && index <= 199999)
            {
                //SMTiles2
                if (index >= 100000 && index <= 106205)
                    moveSound = SoundList.WalkGroundL;
                else if (index >= 106206 && index <= 109914)
                    moveSound = SoundList.WalkStoneL;
            }

            else if (index >= 200000 && index <= 299999)
            {
                //SMTiles3
                if ((index >= 9119 && index <= 9235) || (index >= 9296 && index <= 209355) || (index >= 9416 && index <= 209475) || (index >= 9536 && index <= 209835) ||
                    (index >= 210556 && index <= 210688) || (index >= 210916 && index <= 211035) || (index >= 219200 && index <= 220540) || (index >= 220788 && index <= 221194) ||
                    (index >= 229475 && index <= 230959) || (index >= 231378 && index <= 231436))
                    moveSound = SoundList.WalkGroundL;
                else if ((index >= 209236 && index <= 209295) || (index >= 209356 && index <= 209415) || (index >= 209476 && index <= 209535) || (index >= 209836 && index <= 210435) ||
                    (index >= 210689 && index <= 210915) || (index >= 223242 && index <= 225299) || (index >= 226252 && index <= 227305) || (index >= 228128 && index <= 228132) ||
                    (index >= 228187 && index <= 228192) || (index >= 228245 && index <= 228249) || (index >= 228275 && index <= 228280) || (index >= 228358 && index <= 228454) ||
                    (index >= 228974 && index <= 229474) || (index >= 230960 && index <= 231377))
                    moveSound = SoundList.WalkLawnL;
                else if ((index >= 300000 && index <= 209118) || (index >= 220541 && index <= 220787) || (index >= 221195 && index <= 223241) || (index >= 225300 && index <= 226251) ||
                    (index >= 227306 && index <= 228127) || (index >= 228133 && index <= 228186) || (index >= 228193 && index <= 228244) || (index >= 228250 && index <= 228274) ||
                    (index >= 228281 && index <= 228357) || (index >= 228455 && index <= 228973) || (index >= 231437 && index <= 231703))
                    moveSound = SoundList.WalkStoneL;
                else if ((index >= 211036 && index <= 219199))
                    moveSound = SoundList.WalkRoomL;
                else if ((index >= 210436 && index <= 210555))
                    moveSound = SoundList.WalkRoughL;
            }

            else if (index >= 300000 && index <= 399999)
            {
                //SMTiles4
                if ((index >= 300000 && index <= 300682) || (index >= 300695 && index <= 300699) || (index >= 300714 && index <= 300718) || (index >= 300733 && index <= 300745) ||
                    (index >= 300752 && index <= 300829) || (index >= 300833 && index <= 300849) || (index >= 300852 && index <= 300904) || (index >= 300907 && index <= 300920) ||
                    (index >= 300923 && index <= 300935) || (index >= 300939 && index <= 301088) || (index >= 301105 && index <= 301106) || (index >= 301112 && index <= 301113) ||
                    (index >= 301137 && index <= 301138) || (index >= 301422 && index <= 301423) || (index >= 301441 && index <= 301446) || (index >= 301460 && index <= 301467) ||
                    (index >= 301764 && index <= 301767) || (index >= 301772 && index <= 301786) || (index >= 301790 && index <= 302129) || (index >= 302132 && index <= 302291) ||
                    (index >= 302492 && index <= 302827) || (index >= 304419 && index <= 304646) || (index >= 306951 && index <= 306955) || (index >= 307027 && index <= 307104) ||
                    (index >= 307010 && index <= 307118) || (index >= 307124 && index <= 307133) || (index >= 307138 && index <= 307149) || (index >= 307162 && index <= 307167) ||
                    (index >= 307243 && index <= 308983) || (index >= 310892 && index <= 328054) || (index >= 304935 && index <= 305614) || (index >= 305619 && index <= 305627) ||
                    (index >= 305633 && index <= 305640) || (index >= 305647 && index <= 305651) || (index >= 305661 && index <= 305563) || (index >= 305675 && index <= 305676) ||
                    (index >= 305989 && index <= 306437) || (index >= 306457 && index <= 306474) || (index >= 306484 && index <= 306505) || (index >= 306512 && index <= 306533) ||
                    (index >= 306540 && index <= 306625) || (index >= 306636 && index <= 306761) || (index >= 306788 && index <= 306929) || (index >= 306936 && index <= 306942))
                    moveSound = SoundList.WalkGroundL;
                else if ((index >= 301468 && index <= 301515) || (index >= 302130 && index <= 302131) || (index >= 302292 && index <= 302491) || (index >= 302828 && index <= 303063))
                    moveSound = SoundList.WalkRoughL;
                else if ((index >= 301516 && index <= 301555) || (index >= 304647 && index <= 304934) || (index >= 305615 && index <= 305618) || (index >= 305628 && index <= 303632) ||
                    (index >= 305641 && index <= 305646) || (index >= 305652 && index <= 305660) || (index >= 305664 && index <= 305674) || (index >= 305677 && index <= 305988) ||
                    (index >= 306438 && index <= 306456) || (index >= 306475 && index <= 306483) || (index >= 306506 && index <= 306511) || (index >= 306534 && index <= 306539) ||
                    (index >= 306626 && index <= 306635) || (index >= 306762 && index <= 306787) || (index >= 306930 && index <= 306935) || (index >= 306943 && index <= 306950) ||
                    (index >= 306956 && index <= 307026) || (index >= 307105 && index <= 307109) || (index >= 307119 && index <= 307123) || (index >= 307134 && index <= 307137) ||
                    (index >= 307150 && index <= 307161) || (index >= 307168 && index <= 307242) || (index >= 308983 && index <= 310891))
                    moveSound = SoundList.WalkLawnL;
                else if ((index >= 300691 && index <= 300694) || (index >= 301155 && index <= 301164) || (index >= 301167 && index <= 301173) || (index >= 301181 && index <= 301185) ||
                    (index >= 301194 && index <= 301197) || (index >= 301208 && index <= 301211) || (index >= 301223 && index <= 301225) || (index >= 301233 && index <= 301235) ||
                    (index >= 301238 && index <= 301239) || (index >= 301247 && index <= 301250) || (index >= 301254 && index <= 301256) || (index >= 301262 && index <= 301265) ||
                    (index >= 301271 && index <= 301273) || (index >= 301277 && index <= 301280) || (index >= 301288 && index <= 301295) || (index >= 301305 && index <= 301310) ||
                    (index >= 301322 && index <= 301325) || (index >= 301339 && index <= 301341) || (index >= 301350 && index <= 301352) || (index >= 301356 && index <= 301358) ||
                    (index >= 301368 && index <= 301375) || (index >= 301386 && index <= 301392) || (index >= 301404 && index <= 301409) || (index >= 301424 && index <= 301427) ||
                    (index >= 301587 && index <= 301590) || (index >= 301605 && index <= 301608) || (index >= 301622 && index <= 301625) || (index >= 301638 && index <= 301642) ||
                    (index >= 301655 && index <= 301660) || (index >= 301666 && index <= 301678) || (index >= 301683 && index <= 301685) || (index >= 301688 && index <= 301692) ||
                    (index >= 301700 && index <= 301709) || (index >= 301718 && index <= 301728) || (index >= 301736 && index <= 301744) || (index >= 301754 && index <= 301763))
                    moveSound = SoundList.WalkWoodL;
                else if ((index >= 303064 && index <= 304418))
                    moveSound = SoundList.WalkRoomL;
                else if ((index >= 328055 && index <= 332912))
                    moveSound = SoundList.WalkCaveL;
                else if ((index >= 300683 && index <= 300690) || (index >= 300700 && index <= 300713) || (index >= 300719 && index <= 300732) || (index >= 300746 && index <= 300751) ||
                    (index >= 300830 && index <= 300832) || (index >= 300850 && index <= 300851) || (index >= 300905 && index <= 300906) || (index >= 300921 && index <= 300922) ||
                    (index >= 300936 && index <= 300938) || (index >= 301089 && index <= 301104) || (index >= 301107 && index <= 301111) || (index >= 301114 && index <= 301136) ||
                    (index >= 301139 && index <= 301154) || (index >= 301165 && index <= 301166) || (index >= 301174 && index <= 301180) || (index >= 301186 && index <= 301193) ||
                    (index >= 301198 && index <= 301207) || (index >= 301212 && index <= 301222) || (index >= 301226 && index <= 301232) || (index >= 301236 && index <= 301237) ||
                    (index >= 301240 && index <= 301246) || (index >= 301251 && index <= 301253) || (index >= 301258 && index <= 301261) || (index >= 301266 && index <= 301270) ||
                    (index >= 301274 && index <= 301276) || (index >= 301281 && index <= 301287) || (index >= 301296 && index <= 301304) || (index >= 301311 && index <= 301321) ||
                    (index >= 301326 && index <= 301338) || (index >= 301342 && index <= 301349) || (index >= 301354 && index <= 301355) || (index >= 301359 && index <= 301367) ||
                    (index >= 301376 && index <= 301385) || (index >= 301393 && index <= 301403) || (index >= 301410 && index <= 301421) || (index >= 301428 && index <= 301440) ||
                    (index >= 301447 && index <= 301459) || (index >= 301556 && index <= 301586) || (index >= 301591 && index <= 301604) || (index >= 301609 && index <= 301621) ||
                    (index >= 301626 && index <= 301637) || (index >= 301643 && index <= 301654) || (index >= 301661 && index <= 301665) || (index >= 301679 && index <= 301682) ||
                    (index >= 301686 && index <= 301687) || (index >= 301693 && index <= 301699) || (index >= 301710 && index <= 301717) || (index >= 301729 && index <= 301735) ||
                    (index >= 301768 && index <= 301771) || (index >= 301787 && index <= 301789) || (index >= 301745 && index <= 301753))
                    moveSound = SoundList.WalkWaterL;
            }

            else if (index >= 500000 && index <= 599999)
            {
                //SMTiles6 (115 image)
                if ((index >= 500000 && index <= 501844) || (index >= 503294 && index <= 509086) || (index >= 509520 && index <= 512812) || (index >= 514854 && index <= 515035) ||
                    (index >= 521326 && index <= 522110) || (index >= 526820 && index <= 528871))
                    moveSound = SoundList.WalkGroundL;
                else if ((index >= 501845 && index <= 503293) || (index >= 513557 && index <= 513745))
                    moveSound = SoundList.WalkLawnL;
                else if ((index >= 509087 && index <= 509519) || (index >= 512813 && index <= 512909) || (index >= 516308 && index <= 521325) || (index >= 522111 && index <= 526819) ||
                    (index >= 528872 && index <= 530723))
                    moveSound = SoundList.WalkRoomL;
                else if ((index >= 512910 && index <= 513556) || (index >= 513746 && index <= 514853) || (index >= 515037 && index <= 516307))
                    moveSound = SoundList.WalkStoneL;
            }

            else if (index >= 700000 && index <= 799999)
            {
                //SMTiles8
                if (index >= 700000 && index <= 701702)
                    moveSound = SoundList.WalkCaveL;

            }

            #endregion

        }

        private void PlayWemadeMir3StepSound(int x, int y, out int moveSound)
        {
            moveSound = 0;

            int backIndex = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackIndex);
            int midIndex = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleIndex);
            int midImage = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleImage & 0x7FFF - 1);

            #region Shanda & Wemade Mir 3
            int index = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackImage & 0x1FFFF) - 1; // Back
            #region Back 300 Tilesc*!
            if (backIndex == 300 || backIndex == 200)
            {   // Lawn
                if ((index >= 1605 && index <= 1750) || (index >= 1753 && index <= 1759) || (index >= 1763 && index <= 1769)
                    || (index >= 1772 && index <= 1794) || (index >= 7246 && index <= 7477))
                    moveSound = SoundList.WalkLawnL;
                // Rough
                else if (index >= 6672 && index <= 6918)
                    moveSound = SoundList.WalkRoughL;
                // Stone
                else if ((index >= 500 && index <= 518) || (index >= 523 && index <= 524) || (index >= 527 && index <= 528)
                    || (index >= 907 && index <= 733) || (index >= 1543 && index <= 1564) || (index >= 1561 && index <= 1564)
                    || (index == 1589) || (index >= 1591 && index <= 1592) || (index == 1595) || (index >= 1598 && index <= 1600)
                    || (index >= 1751 && index <= 1752) || (index >= 1760 && index <= 1762) || (index >= 1770 && index <= 1771)
                    || (index >= 1882 && index <= 1883) || (index >= 1891 && index <= 1893) || (index >= 1900 && index <= 1903)
                    || (index >= 1910 && index <= 1911) || (index >= 1920 && index <= 1922) || (index >= 1926 && index <= 1929)
                    || (index >= 1940 && index <= 1941) || (index == 7138) || (index == 7492) || (index == 7552) || (index == 7557)
                    || (index == 7560) || (index == 7932) || (index == 7935) || (index == 7940) || (index == 7943) || (index == 8085)
                    || (index == 8110) || (index == 8111) || (index == 8130) || (index == 8352) || (index == 8356) || (index == 8543)
                    || (index == 8571) || (index == 8579) || (index == 8680) || (index == 9217) || (index == 9248) || (index == 9249)
                    || (index >= 9283 && index <= 9285) || (index >= 9287 && index <= 9288) || (index >= 9300 && index <= 9301)
                    || (index >= 9317 && index <= 9318) || (index >= 9321 && index <= 9322) || (index >= 9333 && index <= 9334)
                    || (index == 9350) || (index == 9353) || (index >= 9377 && index <= 9378) || (index == 9381)
                    || (index >= 9405 && index <= 9406) || (index == 9409) || (index >= 9442 && index <= 9443)
                    || (index >= 9471 && index <= 9472) || (index == 9484) || (index == 9517) || (index >= 9553 && index <= 9554)
                    || (index >= 9630 && index <= 9631) || (index == 9662))
                    moveSound = SoundList.WalkStoneL;
                // Cave
                else if (index >= 980 && index <= 1498)
                    moveSound = SoundList.WalkCaveL;
                // Wood
                else if ((index >= 1502 && index <= 1533) || (index == 1535) || (index == 1539) || (index == 1541) || (index == 1566)
                    || (index == 1569) || (index == 1573) || (index >= 1577 && index <= 1578) || (index >= 1580 && index <= 1587))
                    moveSound = SoundList.WalkWoodL;
                // Room
                else if ((index == 1534) || (index >= 1536 && index <= 1538) || (index == 1540) || (index == 1567)
                    || (index >= 1570 && index <= 1572) || (index >= 1574 && index <= 1576) || (index == 1579) || (index == 1590)
                    || (index >= 1593 && index <= 1594) || (index >= 1596 && index <= 1597))
                    moveSound = SoundList.WalkRoomL;
                // Water
                else if ((index >= 0 && index <= 3) || (index >= 7 && index <= 11) || (index >= 14 && index <= 17)
                    || (index >= 23 && index <= 25) || (index >= 30 && index <= 35) || (index >= 38 && index <= 43)
                    || (index >= 48 && index <= 51) || (index >= 55 && index <= 59) || (index >= 62 && index <= 65)
                    || (index >= 88 && index <= 91) || (index >= 100 && index <= 104) || (index >= 109 && index <= 117)
                    || (index >= 124 && index <= 130) || (index >= 139 && index <= 142) || (index >= 145 && index <= 150)
                    || (index >= 153 && index <= 159) || (index >= 164 && index <= 167) || (index >= 172 && index <= 175)
                    || (index >= 179 && index <= 182) || (index >= 186 && index <= 192) || (index >= 195 && index <= 200)
                    || (index >= 205 && index <= 207) || (index >= 212 && index <= 216) || (index >= 221 && index <= 224)
                    || (index >= 228 && index <= 231) || (index >= 236 && index <= 243) || (index >= 250 && index <= 258)
                    || (index >= 264 && index <= 268) || (index >= 277 && index <= 279) || (index >= 291 && index <= 296)
                    || (index >= 300 && index <= 305) || (index >= 308 && index <= 311) || (index >= 320 && index <= 326)
                    || (index >= 333 && index <= 341) || (index >= 350 && index <= 354) || (index >= 363 && index <= 366)
                    || (index >= 385 && index <= 388) || (index >= 392 && index <= 396) || (index >= 400 && index <= 403)
                    || (index >= 415 && index <= 417) || (index >= 423 && index <= 427) || (index >= 435 && index <= 443)
                    || (index >= 449 && index <= 457) || (index >= 5825 && index <= 5829) || (index >= 5835 && index <= 5844)
                    || (index >= 5849 && index <= 5859) || (index >= 5865 && index <= 5877) || (index >= 5882 && index <= 5894)
                    || (index >= 5900 && index <= 5912) || (index >= 5918 && index <= 5932) || (index >= 5937 && index <= 5951)
                    || (index >= 5958 && index <= 5973) || (index >= 5979 && index <= 5993) || (index >= 5999 && index <= 6011)
                    || (index >= 6017 && index <= 6029) || (index >= 6034 && index <= 6046) || (index >= 6051 && index <= 6061)
                    || (index >= 6067 && index <= 6076) || (index >= 6083 && index <= 6087) || (index >= 6105 && index <= 6110)
                    || (index >= 6116 && index <= 6128) || (index >= 6138 && index <= 6152) || (index >= 6159 && index <= 6283)
                    || (index >= 6290 && index <= 6303) || (index >= 6313 && index <= 6325) || (index >= 6331 && index <= 6336)
                    || (index >= 6340 && index <= 6345) || (index >= 6348 && index <= 6354) || (index >= 6357 && index <= 6363)
                    || (index >= 6366 && index <= 6371) || (index >= 6378 && index <= 6383) || (index >= 6386 && index <= 6392)
                    || (index >= 6395 && index <= 6400) || (index >= 6404 && index <= 6409) || (index >= 6416 && index <= 6421)
                    || (index >= 6424 && index <= 6430) || (index >= 6433 && index <= 6439) || (index >= 6442 && index <= 6448)
                    || (index >= 6454 && index <= 6458) || (index >= 6462 && index <= 6467) || (index >= 6471 && index <= 6477)
                    || (index >= 6480 && index <= 6486) || (index >= 6493 && index <= 6499) || (index >= 6502 && index <= 6508)
                    || (index >= 6511 && index <= 6517) || (index >= 6520 && index <= 6525) || (index >= 6532 && index <= 6537)
                    || (index >= 6541 && index <= 6546) || (index >= 6549 && index <= 6555) || (index >= 6558 && index <= 6563)
                    || (index >= 6569 && index <= 6575) || (index >= 6578 && index <= 6584) || (index >= 6587 && index <= 6593)
                    || (index >= 6597 && index <= 6601) || (index >= 6607 && index <= 6613) || (index >= 6616 && index <= 6622)
                    || (index >= 6627 && index <= 6631) || (index >= 6635 && index <= 6639))
                    moveSound = SoundList.WalkWaterL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 315 Wood/Tilesc*
            if (backIndex == 315 || backIndex == 215)
            {   // Stone
                if ((index == 51) || (index >= 57 && index <= 58) || (index >= 64 && index <= 66) || (index == 74)
                    || (index == 79) || (index == 87) || (index == 99) || (index == 103) || (index == 116) || (index == 123)
                    || (index >= 136 && index <= 140) || (index >= 160 && index <= 166) || (index >= 186 && index <= 196)
                    || (index >= 215 && index <= 221) || (index == 225) || (index >= 245 && index <= 250) || (index >= 268 && index <= 271)
                    || (index == 282) || (index == 286) || (index == 290) || (index >= 302 && index <= 303) || (index == 322)
                    || (index == 345) || (index >= 406 && index <= 409) || (index >= 444 && index <= 448) || (index >= 477 && index <= 481)
                    || (index >= 503 && index <= 504) || (index >= 506 && index <= 507) || (index >= 524 && index <= 526)
                    || (index >= 528 && index <= 529) || (index >= 544 && index <= 548) || (index >= 563 && index <= 564) || (index >= 566 && index <= 567)
                    || (index == 580) || (index >= 586 && index <= 587) || (index >= 589 && index <= 590) || (index == 595) || (index == 602)
                    || (index >= 609 && index <= 613) || (index >= 617 && index <= 618) || (index == 626) || (index >= 633 && index <= 634)
                    || (index >= 636 && index <= 637) || (index == 642) || (index >= 657 && index == 658) || (index >= 660 && index <= 661)
                    || (index >= 666 && index <= 667) || (index >= 681 && index <= 682) || (index == 685) || (index >= 705 && index <= 709)
                    || (index >= 730 && index <= 733) || (index >= 753 && index <= 754) || (index == 905) || (index >= 1166 && index <= 1167)
                    || (index >= 1191 && index <= 1192) || (index >= 1221 && index <= 1222) || (index == 1326) || (index >= 1383 && index <= 1384)
                    || (index == 1427) || (index == 1451) || (index == 1465) || (index == 1488) || (index == 1505) || (index >= 1509 && index <= 1512)
                    || (index == 1525) || (index == 1545) || (index >= 1554 && index <= 1558) || (index == 1570) || (index == 1590)
                    || (index >= 1598 && index <= 1603) || (index >= 1626 && index <= 1627) || (index == 1634) || (index >= 1641 && index <= 1647)
                    || (index == 1673) || (index >= 1687 && index <= 1692) || (index == 1714) || (index >= 1730 && index <= 1733) || (index == 1749)
                    || (index >= 1766 && index <= 1767) || (index >= 1786 && index <= 1787) || (index >= 1834 && index <= 1835) || (index == 1942)
                    || (index >= 2080 && index <= 2081) || (index >= 2091 && index <= 2092) || (index >= 2623 && index <= 2624) || (index == 2850)
                    || (index >= 2862 && index <= 2863) || (index == 2877) || (index >= 2924 && index <= 2925) || (index >= 2939 && index <= 2940)
                    || (index == 3057) || (index >= 3913 && index <= 3914))
                    moveSound = SoundList.WalkStoneL;
                // Wood
                else if ((index >= 3157 && index <= 3159) || (index >= 3168 && index <= 3171) || (index >= 3179 && index <= 3182)
                    || (index >= 3199 && index <= 3202) || (index >= 3220 && index <= 3222) || (index >= 3243 && index <= 3245)
                    || (index >= 3266 && index <= 3268) || (index >= 3286 && index <= 3289) || (index >= 3307 && index <= 3309)
                    || (index >= 3329 && index <= 3331) || (index >= 3352 && index <= 3354) || (index >= 3374 && index <= 3376)
                    || (index >= 3387 && index <= 3389) || (index >= 3502 && index <= 3503) || (index >= 3522 && index <= 3523)
                    || (index >= 3532 && index <= 3538) || (index >= 3545 && index <= 3551) || (index >= 3559 && index <= 3566)
                    || (index >= 3581 && index <= 3582) || (index >= 3591 && index <= 3592) || (index >= 3598 && index <= 3600)
                    || (index >= 3615 && index <= 3617) || (index >= 3634 && index <= 3636) || (index == 3659) || (index == 3715)
                    || (index >= 3727 && index <= 3728) || (index >= 3738 && index <= 3739) || (index == 3745) || (index == 3749)
                    || (index == 3750) || (index >= 3754 && index <= 3757) || (index >= 3774 && index <= 3776)
                    || (index >= 3797 && index <= 3798) || (index >= 3816 && index <= 3818) || (index >= 3831 && index <= 3832)
                    || (index >= 3836 && index <= 3837) || (index >= 3850 && index <= 3853) || (index >= 3866 && index <= 3868))
                    moveSound = SoundList.WalkWoodL;
                // Water
                else if ((index == 3744) || (index >= 3761 && index <= 3769) || (index >= 3782 && index <= 3790)
                    || (index >= 3804 && index <= 3809) || (index >= 3824 && index <= 3830) || (index >= 3842 && index <= 3848)
                    || (index >= 3860 && index <= 3865) || (index == 3870) || (index >= 3875 && index <= 3881) || (index == 3884)
                    || (index >= 3890 && index <= 3894))
                    moveSound = SoundList.WalkWaterL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 330 Sand/Tilesc*
            if (backIndex == 330 || backIndex == 230)
            {   // Stone
                if ((index >= 1029 && index <= 1031) || (index >= 1048 && index <= 1053) || (index >= 1070 && index <= 1075)
                    || (index >= 1094 && index <= 1100) || (index >= 1119 && index <= 1120) || (index >= 1124 && index <= 1127)
                    || (index >= 1145 && index <= 1146) || (index >= 1150 && index <= 1154) || (index >= 1177 && index <= 1183)
                    || (index >= 1185 && index <= 1187) || (index >= 1212 && index <= 1218) || (index >= 1242 && index <= 1246)
                    || (index >= 1269 && index <= 1271) || (index >= 1273 && index <= 1274) || (index >= 1301 && index <= 1305)
                    || (index >= 1308 && index <= 1310) || (index == 1338) || (index >= 1340 && index <= 1342)
                    || (index >= 1345 && index <= 1351) || (index >= 1378 && index <= 1386) || (index >= 1388 && index <= 1393)
                    || (index >= 1414 && index <= 1422) || (index >= 1426 && index <= 1429) || (index >= 1444 && index <= 1451)
                    || (index >= 1458 && index <= 1459) || (index >= 1475 && index <= 1479) || (index >= 1500 && index <= 1503)
                    || (index >= 1520 && index <= 1521) || (index >= 1538 && index <= 1539) || (index >= 1554 && index <= 1557)
                    || (index >= 1576 && index <= 1581) || (index >= 1600 && index <= 1603) || (index >= 1616 && index <= 1618)
                    || (index >= 1629 && index <= 1631) || (index == 2135) || (index >= 2150 && index <= 2151)
                    || (index >= 2163 && index <= 2164) || (index == 2177) || (index >= 2179 && index <= 2180) || (index == 2199)
                    || (index == 2201) || (index >= 2205 && index <= 2208) || (index >= 2219 && index <= 2220)
                    || (index >= 2227 && index <= 2228) || (index == 2230) || (index == 2249) || (index == 2377) || (index == 2493)
                    || (index >= 2600 && index <= 2601) || (index >= 2603 && index <= 2604) || (index >= 2656 && index <= 2659)
                    || (index >= 2677 && index <= 2681) || (index >= 2700 && index <= 2703) || (index >= 2717 && index <= 2718)
                    || (index >= 2723 && index <= 2726) || (index == 2751) || (index == 2793) || (index >= 3020 && index <= 3022)
                    || (index >= 3052 && index <= 3053) || (index >= 3088 && index <= 3089) || (index >= 3115 && index <= 3116)
                    || (index >= 3124 && index <= 3125) || (index >= 3131 && index <= 3133) || (index >= 3159 && index <= 3161)
                    || (index >= 3168 && index <= 3171) || (index >= 3192 && index <= 3193) || (index >= 3197 && index <= 3199)
                    || (index == 3232) || (index >= 4008 && index <= 4009) || (index >= 4030 && index <= 4033)
                    || (index >= 4055 && index <= 4056) || (index >= 4078 && index <= 4079) || (index >= 4101 && index <= 4104)
                    || (index >= 4121 && index <= 4123) || (index >= 4129 && index <= 4130) || (index >= 4140 && index <= 4141)
                    || (index >= 4149 && index <= 4150) || (index >= 4159 && index <= 4160) || (index == 4175) || (index == 4192)
                    || (index >= 4212 && index <= 4215) || (index >= 4299 && index <= 4361) || (index >= 4610 && index <= 4734)
                    || (index >= 4775 && index <= 4810))
                    moveSound = SoundList.WalkStoneL;
                // Cave
                else if ((index >= 4364 && index <= 4607) || (index >= 4737 && index <= 4772) || (index >= 4813 && index <= 4906))
                    moveSound = SoundList.WalkCaveL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 345 Snow/Tilesc*!
            if (backIndex == 345 || backIndex == 245)
            {   // Stone
                if ((index >= 988 && index <= 1116) || (index == 1721) || (index == 1781) || (index == 1801) || (index >= 3051 && index <= 3282)
                    || (index >= 3296 && index <= 3297) || (index >= 3300 && index <= 3301) || (index >= 3994 && index <= 4301)
                    || (index >= 6224 && index <= 6291) || (index >= 6293 && index <= 6308) || (index >= 6313 && index <= 6329)
                    || (index >= 6336 && index <= 6354) || (index >= 6362 && index <= 6382) || (index >= 6392 && index <= 6414)
                    || (index >= 6424 && index <= 6448) || (index >= 6458 && index <= 6484) || (index >= 6494 && index <= 6522)
                    || (index >= 6532 && index <= 6561) || (index >= 6571 && index <= 6600) || (index >= 6610 && index <= 6639)
                    || (index >= 6649 && index <= 6677) || (index >= 6687 && index <= 6712) || (index >= 6720 && index <= 6747)
                    || (index >= 6754 && index <= 6782) || (index >= 6787 && index <= 7941))
                    moveSound = SoundList.WalkStoneL;
                // Snow
                else if ((index >= 2 && index <= 984) || (index >= 2123 && index <= 2453) || (index >= 2456 && index <= 2458)
                    || (index >= 2462 && index <= 2465) || (index >= 2468 && index <= 2736) || (index >= 3287 && index <= 3295)
                    || (index >= 3298 && index <= 3299) || (index >= 3302 && index <= 3306))
                    moveSound = SoundList.WalkSnowL;
                // Wood
                else if ((index >= 2454 && index <= 2455) || (index >= 2459 && index <= 2461) || (index >= 2466 && index <= 2467)
                    || (index >= 4590 && index <= 4593) || (index >= 4611 && index <= 4613) || (index == 4646) || (index == 4655)
                    || (index == 4665) || (index == 4668) || (index == 4675) || (index >= 4687 && index <= 4688) || (index >= 4693 && index <= 4694)
                    || (index >= 4762 && index <= 4713) || (index >= 4729 && index <= 4732) || (index >= 4750 && index <= 4751))
                    moveSound = SoundList.WalkWoodL;
                // Cave
                else if ((index >= 3311 && index <= 3854) || (index >= 4304 && index <= 4519))
                    moveSound = SoundList.WalkCaveL;
                // Room
                if ((index == 6292) || (index >= 6309 && index <= 6312) || (index >= 6330 && index <= 6335)
                    || (index >= 6355 && index <= 6361) || (index >= 6383 && index <= 6391) || (index >= 6415 && index <= 6423)
                    || (index >= 6449 && index <= 6457) || (index >= 6485 && index <= 6493) || (index >= 6523 && index <= 6531)
                    || (index >= 6562 && index <= 6570) || (index >= 6601 && index <= 6609) || (index >= 6640 && index <= 6648)
                    || (index >= 6678 && index <= 6686) || (index >= 6713 && index <= 6719) || (index >= 6748 && index <= 6753)
                    || (index >= 6783 && index <= 6785))
                    moveSound = SoundList.WalkRoomL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 360 Forest/Tilesc*!
            if (backIndex == 360 || backIndex == 260)
            {   // Stone
                if ((index >= 120 && index <= 140) || (index >= 141 && index <= 156) || (index >= 158 && index <= 179)
                    || (index >= 184 && index <= 195) || (index >= 197 && index <= 198) || (index >= 200 && index <= 205)
                    || (index >= 210 && index <= 219) || (index >= 222 && index <= 231) || (index >= 233 && index <= 240)
                    || (index >= 248 && index <= 255) || (index >= 257 && index <= 263) || (index >= 271 && index <= 278)
                    || (index >= 282 && index <= 286) || (index >= 289 && index <= 290) || (index >= 294 && index <= 299)
                    || (index == 306) || (index >= 642 && index <= 646) || (index >= 666 && index <= 670)
                    || (index == 677) || (index >= 687 && index <= 692) || (index >= 698 && index <= 700)
                    || (index >= 709 && index <= 718) || (index >= 720 && index <= 724) || (index >= 732 && index <= 742)
                    || (index >= 744 && index <= 748) || (index >= 755 && index <= 762) || (index >= 768 && index <= 771)
                    || (index >= 775 && index <= 790) || (index >= 792 && index <= 812) || (index >= 817 && index <= 833)
                    || (index >= 836 && index <= 845) || (index >= 847 && index <= 853) || (index >= 855 && index <= 856)
                    || (index >= 1645 && index <= 1786) || (index >= 2041 && index <= 9868))
                    moveSound = SoundList.WalkStoneL;
                // Lawn
                else moveSound = SoundList.WalkLawnL;
            }
            #endregion


            #region Back 301 Tiles30c*
            if (backIndex == 301 || backIndex == 201)
            {   // Lawn
                if ((index == 14) || (index == 19) || (index == 24) || (index == 29) || (index >= 395 && index <= 403)
                    || (index >= 410 && index <= 418) || (index >= 425 && index <= 433) || (index >= 440 && index <= 448)
                    || (index >= 455 && index <= 463) || (index >= 470 && index <= 478) || (index >= 485 && index <= 493)
                    || (index >= 500 && index <= 508) || (index >= 515 && index <= 523) || (index >= 530 && index <= 538)
                    || (index >= 605 && index <= 613) || (index >= 620 && index <= 628) || (index >= 635 && index <= 643)
                    || (index >= 650 && index <= 658) || (index >= 720 && index <= 724) || (index >= 735 && index <= 748)
                    || (index >= 900 && index <= 904) || (index >= 915 && index <= 928) || (index >= 995 && index <= 1003))
                    moveSound = SoundList.WalkLawnL;
                // Stone
                else if ((index >= 30 && index <= 34) || (index >= 45 && index <= 48) || (index >= 120 && index <= 124)
                    || (index >= 135 && index <= 138) || (index >= 185 && index <= 193) || (index >= 200 && index <= 208)
                    || (index >= 330 && index <= 334) || (index >= 345 && index <= 348) || (index >= 450 && index <= 454)
                    || (index >= 465 && index <= 468) || (index >= 1025 && index <= 1033))
                    moveSound = SoundList.WalkStoneL;
                // Rough
                else if ((index >= 600 && index <= 604) || (index >= 615 && index <= 618) || (index >= 780 && index <= 784)
                    || (index >= 795 && index <= 814) || (index >= 825 && index <= 844) || (index >= 855 && index <= 868)
                    || (index >= 930 && index <= 934) || (index >= 945 && index <= 958))
                    moveSound = SoundList.WalkRoughL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            //316 = wood/Tiles30c
            //331 = sand/Tiles30c
            #region Back 346 Snow/Tiles30c*
            if (backIndex == 346 || backIndex == 246)
            {   // Snow
                if ((index >= 0 && index <= 58) || (index >= 75 && index <= 94) || (index >= 105 && index <= 118)
                    || (index >= 125 && index <= 133) || (index >= 140 && index <= 148) || (index >= 165 && index <= 178))
                    moveSound = SoundList.WalkSnowL;
                // Lawn
                else if ((index >= 210 && index <= 214) || (index >= 225 && index <= 238))
                    moveSound = SoundList.WalkLawnL;
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 361 Forest/Tiles30c*
            if (backIndex == 361 || backIndex == 261)
            {   // Lawn
                if ((index >= 0 && index <= 4) || (index >= 14 && index <= 34) || (index >= 45 && index <= 64)
                    || (index >= 75 && index <= 124) || (index >= 135 && index <= 138) || (index >= 150 && index <= 154)
                    || (index >= 165 && index <= 179))
                    moveSound = SoundList.WalkLawnL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion


            #region Back 302 Tiles5c*
            if (backIndex == 302 || backIndex == 202)
            {   // Lawn
                if ((index >= 10330 && index <= 10348) || (index >= 10405 && index <= 10408) || (index >= 10450 && index <= 10454)
                    || (index >= 10460 && index <= 10463) || (index >= 10505 && index <= 10508) || (index >= 10550 && index <= 10554)
                    || (index >= 10560 && index <= 10563) || (index >= 10575 && index <= 10580) || (index >= 10582 && index <= 10585)
                    || (index >= 10588 && index <= 10593) || (index >= 10705 && index <= 10708) || (index >= 10750 && index <= 10754)
                    || (index >= 10760 && index <= 10763) || (index >= 10905 && index <= 10908) || (index >= 10915 && index <= 10954)
                    || (index >= 10960 && index <= 10973) || (index >= 11205 && index <= 11208) || (index >= 11215 && index <= 11254)
                    || (index >= 11260 && index <= 11273) || (index >= 11400 && index <= 11404) || (index >= 11410 && index <= 11423)
                    || (index >= 11455 && index <= 11473) || (index >= 11500 && index <= 11523) || (index >= 11550 && index <= 11573)
                    || (index >= 12555 && index <= 12558) || (index >= 12565 && index <= 12573) || (index >= 12605 && index <= 12608)
                    || (index >= 12615 && index <= 12623) || (index >= 12655 && index <= 12658) || (index >= 12665 && index <= 12673)
                    || (index >= 12705 && index <= 12708) || (index >= 12715 && index <= 12723))
                    moveSound = SoundList.WalkLawnL;
                // Rough
                else if ((index >= 10205 && index <= 10208) || (index >= 10215 && index <= 10223) || (index >= 10250 && index <= 10254)
                    || (index >= 10260 && index <= 10273) || (index >= 11005 && index <= 11008) || (index >= 11015 && index <= 11054)
                    || (index >= 11060 && index <= 11073) || (index >= 11405 && index <= 11408) || (index >= 11450 && index <= 11454))
                    moveSound = SoundList.WalkRoughL;
                // Stone
                else if ((index >= 10 && index <= 14) || (index >= 10085 && index <= 10093) || (index >= 10105 && index <= 10108)
                    || (index >= 10150 && index <= 10154) || (index >= 10160 && index <= 10173) || (index >= 10605 && index <= 10608)
                    || (index >= 10615 && index <= 10623) || (index >= 10650 && index <= 10654) || (index >= 10660 && index <= 10673)
                    || (index >= 11305 && index <= 11308) || (index >= 11315 && index <= 11354) || (index >= 11360 && index <= 11373)
                    || (index >= 11479 && index <= 11481) || (index >= 11486 && index <= 11488) || (index >= 11491 && index <= 11493)
                    || (index >= 12000 && index <= 12023) || (index >= 12400 && index <= 12423) || (index >= 12550 && index <= 12554)
                    || (index >= 12560 && index <= 12563) || (index >= 12600 && index <= 12604) || (index >= 12610 && index <= 12613)
                    || (index >= 12650 && index <= 12654) || (index >= 12660 && index <= 12663) || (index >= 12700 && index <= 12704)
                    || (index >= 12710 && index <= 12713) || (index >= 12850 && index <= 12874) || (index >= 12995 && index <= 13049)
                    || (index >= 14625 && index <= 14678) || (index >= 15045 && index <= 15143) || (index >= 15780 && index <= 15849)
                    || (index >= 16075 && index <= 16077))
                    moveSound = SoundList.WalkStoneL;
                // Cave
                else if ((index >= 10040 && index <= 10078) || (index >= 11375 && index <= 11378) || (index >= 11380 && index <= 11383)
                    || (index >= 11387 && index <= 11390) || (index >= 11394 && index <= 11396) || (index >= 11425 && index <= 11449)
                    || (index >= 11475 && index <= 11478) || (index >= 11482 && index <= 11485) || (index >= 11489 && index <= 11490)
                    || (index >= 11494 && index <= 11499) || (index >= 11525 && index <= 11549) || (index >= 11575 && index <= 11973)
                    || (index >= 12150 && index <= 12173) || (index >= 12450 && index <= 12473) || (index >= 12495 && index <= 12499)
                    || (index >= 12525 && index <= 12549) || (index >= 12575 && index <= 12599) || (index >= 12625 && index <= 12649)
                    || (index >= 12675 && index <= 12699) || (index >= 12725 && index <= 12749) || (index >= 12775 && index <= 12799)
                    || (index >= 12825 && index <= 12849) || (index >= 12875 && index <= 12899) || (index >= 12925 && index <= 12993)
                    || (index >= 13075 && index <= 13098) || (index >= 13125 && index <= 13129) || (index >= 13131 && index <= 13136)
                    || (index >= 13139 && index <= 13176) || (index >= 13180 && index <= 13188) || (index >= 13191 && index <= 13229)
                    || (index >= 13232 && index <= 13239) || (index >= 13242 && index <= 13245) || (index >= 13247 && index <= 13275)
                    || (index >= 13279 && index <= 13281) || (index >= 14825 && index <= 15035) || (index >= 15225 && index <= 15779))
                    moveSound = SoundList.WalkCaveL;
                // Wood
                else if ((index >= 25 && index <= 29) || (index == 11379) || (index >= 11384 && index <= 11386)
                    || (index >= 11391 && index <= 11393) || (index >= 11397 && index <= 11399) || (index == 13130)
                    || (index >= 13137 && index <= 13138) || (index >= 13177 && index <= 13179) || (index >= 13189 && index <= 13190)
                    || (index >= 13230 && index <= 13231) || (index >= 13240 && index <= 13241) || (index == 13246)
                    || (index >= 13276 && index <= 13278) || (index == 13282) || (index >= 13780 && index <= 14434)
                    || (index >= 15178 && index <= 15179) || (index >= 15182 && index <= 15185) || (index >= 15188 && index <= 15191)
                    || (index >= 15194 && index <= 15195) || (index >= 15875 && index <= 16041) || (index >= 16080 && index <= 16083)
                    || (index >= 16145 && index <= 16188))
                    moveSound = SoundList.WalkWoodL;
                // Room
                else if ((index >= 12175 && index <= 12199) || (index >= 12232 && index <= 10249) || (index >= 12275 && index <= 12299)
                    || (index >= 12325 && index <= 12349) || (index >= 12375 && index <= 12399) || (index >= 12425 && index <= 12449)
                    || (index >= 12475 && index <= 12488) || (index >= 12800 && index <= 12824) || (index >= 12900 && index <= 12924)
                    || (index >= 15037 && index <= 15040) || (index >= 16085 && index <= 16088))
                    moveSound = SoundList.WalkRoomL;
                // Water
                else if ((index >= 0 && index <= 4) || (index >= 30 && index <= 34))
                    moveSound = SoundList.WalkWaterL;
                // Ground
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 317 Wood/Tiles5c*
            if (backIndex == 317 || backIndex == 217)
            {   // Cave
                if (index >= 0 && index <= 4)
                    moveSound = SoundList.WalkCaveL;
                // Stone
                else moveSound = SoundList.WalkStoneL;
            }
            #endregion
            #region Back 332 Sand/Tiles5c*
            if (backIndex == 332 || backIndex == 232)
            {   // Stone
                if (index >= 5 && index <= 9)
                    moveSound = SoundList.WalkStoneL;
                // Cave
                else if (index >= 0 && index <= 4)
                    moveSound = SoundList.WalkCaveL;
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 347 Snow/Tiles5c*
            if (backIndex == 347 || backIndex == 247)
            {   // Cave
                if ((index >= 21 && index <= 24) || (index >= 35 && index <= 44))
                    moveSound = SoundList.WalkCaveL;
                // Stone
                else if ((index >= 0 && index <= 4) || (index >= 15 && index <= 19))
                    moveSound = SoundList.WalkStoneL;
                // Water
                else if ((index >= 30 && index <= 34))
                    moveSound = SoundList.WalkWaterL;
                // Snow
                else if ((index >= 30 && index <= 34))
                    moveSound = SoundList.WalkSnowL;
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion
            #region Back 362 Forest/Tiles5c*
            if (backIndex == 362 || backIndex == 262)
            {   // Stone
                if (index >= 0 && index <= 36)
                    moveSound = SoundList.WalkStoneL;
                else moveSound = SoundList.WalkGroundL;
            }
            #endregion

            #region Back 304 Housesc || 219 Wood | 234 Sand | 249 Snow | 264 Forest
            if (backIndex == 304 || backIndex == 204)
            {
                moveSound = SoundList.WalkGroundL;
            }
            #endregion

            index = (GameScene.Scene.MapControl.M2CellInfo[x, y].MiddleImage & 0x7FFF) - 1; // Middle
            #region Middle 310 smObjectsc
            if (midIndex == 310 || backIndex == 210 && midImage != 0)
                // Wood
                if (index >= 7755 && index <= 8300)
                    moveSound = SoundList.WalkWoodL;
                else moveSound = SoundList.WalkGroundL;
            #endregion
            #region Middle 325 Wood/smObjectsc
            if (midIndex == 325 || backIndex == 225 && midImage != 0)
                // Stone
                if ((index >= 480 && index <= 624) || (index >= 2459 && index <= 2519))
                    moveSound = SoundList.WalkStoneL;
                // Room
                else if (index >= 2539 && index <= 2623)
                    moveSound = SoundList.WalkRoomL;
                else moveSound = SoundList.WalkGroundL;
            #endregion
            #region Middle 340 Sand/smObjectsc
            if (midIndex == 340 || backIndex == 240 && midImage != 0)
                // Stone
                if ((index >= 0 && index <= 0) || (index >= 0 && index <= 0))
                    moveSound = SoundList.WalkGroundL;
            #endregion
            #region Middle 355 Snow/smObjectsc
            if (midIndex == 355 || backIndex == 255 && midImage != 0)
                // Wood
                if ((index >= 939 && index <= 942) || (index >= 960 && index <= 966) || (index >= 986 && index <= 994)
                    || (index >= 1013 && index <= 1021) || (index >= 1044 && index <= 1052) || (index >= 1073 && index <= 1081)
                    || (index >= 1104 && index <= 1112) || (index >= 1130 && index <= 1136) || (index >= 1156 && index <= 1161)
                    || (index >= 1178 && index <= 1182) || (index >= 1200 && index <= 1203) || (index >= 1219 && index <= 1220)
                    || (index >= 1302 && index <= 1303) || (index >= 1319 && index <= 1322) || (index >= 1337 && index <= 1341)
                    || (index >= 1358 && index <= 1364) || (index >= 1380 && index <= 1386) || (index >= 1404 && index <= 1412)
                    || (index >= 1429 && index <= 1437) || (index >= 1456 && index <= 1464) || (index >= 1480 && index <= 1489)
                    || (index >= 1509 && index <= 1516) || (index >= 1532 && index <= 1538) || (index >= 1555 && index <= 1558)
                    || (index >= 9681 && index <= 9952))
                    moveSound = SoundList.WalkWoodL;
                // Stone
                else if ((index >= 4596 && index <= 4602) || (index >= 4638 && index <= 4644) || (index >= 4670 && index <= 4676)
                    || (index >= 6383 && index <= 6387))
                    moveSound = SoundList.WalkStoneL;
                else moveSound = SoundList.WalkSnowL;
            #endregion
            #region Middle 370 Forest/smObjectsc
            if (midIndex == 370 || backIndex == 270 && midImage != 0)
                // Stone
                moveSound = SoundList.WalkStoneL;
            #endregion
            #endregion Wemade Mir 3
        }


        public void PlayStruckSound()
        {
            if (RidingMount)
            {
                if (MountType < 7)
                    SoundManager.PlaySound(CMain.Random.Next(10179, 10181));
                else if (MountType < 12)
                    SoundManager.PlaySound(CMain.Random.Next(10193, 10194));

                return;
            }

            int add = 0;
            if (Class != MirClass.Assassin) //Archer to add?
                switch (Armour)
                {
                    case 3:
                    case 6:
                    case 9:
                        add = 10;
                        break;
                }

            switch (StruckWeapon)
            {
                case 0:
                case 23:
                case 1:
                case 12:
                case 28:
                case 40:
                    SoundManager.PlaySound(SoundList.StruckBodySword + add);
                    break;
                case 2:
                case 8:
                case 11:
                case 15:
                case 18:
                case 20:
                case 25:
                case 31:
                case 33:
                case 34:
                case 37:
                case 41:
                    SoundManager.PlaySound(SoundList.StruckBodySword + add);
                    break;
                case 3:
                case 5:
                case 7:
                case 9:
                case 13:
                case 19:
                case 24:
                case 26:
                case 29:
                case 32:
                case 35:
                    SoundManager.PlaySound(SoundList.StruckBodySword + add);
                    break;
                case 4:
                case 14:
                case 16:
                case 38:
                    SoundManager.PlaySound(SoundList.StruckBodyAxe + add);
                    break;
                case 6:
                case 10:
                case 17:
                case 22:
                case 27:
                case 30:
                case 36:
                case 39:
                    SoundManager.PlaySound(SoundList.StruckBodyLongStick + add);
                    break;
                case 21:
                    SoundManager.PlaySound(SoundList.StruckBodyLongStick + add);
                    break;
                case -1:
                    SoundManager.PlaySound(SoundList.StruckBodyFist + add);
                    break;
            }
        }
        public void PlayFlinchSound()
        {
            SoundManager.PlaySound(FlinchSound);
        }
        public void PlayAttackSound()
        {
            if (RidingMount)
            {
                if (MountType < 7)
                    SoundManager.PlaySound(CMain.Random.Next(10181, 10184));
                else if (MountType < 12)
                    SoundManager.PlaySound(CMain.Random.Next(10190, 10193));

                return;
            }

            if (Weapon >= 0 && Class == MirClass.Assassin)
            {
                SoundManager.PlaySound(SoundList.SwingShort);
                return;
            }

            if (Class == MirClass.Archer && HasClassWeapon)
            {
                return;
            }

            switch (Weapon)
            {
                case 0:
                case 23:
                case 28:
                case 40:
                    SoundManager.PlaySound(SoundList.SwingWood);
                    break;
                case 1:
                case 12:
                    SoundManager.PlaySound(SoundList.SwingShort);
                    break;
                case 2:
                case 8:
                case 11:
                case 15:
                case 18:
                case 20:
                case 25:
                case 31:
                case 33:
                case 34:
                case 37:
                case 41:
                    SoundManager.PlaySound(SoundList.SwingSword);
                    break;
                case 3:
                case 5:
                case 7:
                case 9:
                case 13:
                case 19:
                case 24:
                case 26:
                case 29:
                case 32:
                case 35:
                    SoundManager.PlaySound(SoundList.SwingSword2);
                    break;
                case 4:
                case 14:
                case 16:
                case 38:
                    SoundManager.PlaySound(SoundList.SwingAxe);
                    break;
                case 6:
                case 10:
                case 17:
                case 22:
                case 27:
                case 30:
                case 36:
                case 39:
                    SoundManager.PlaySound(SoundList.SwingLong);
                    break;
                case 21:
                    SoundManager.PlaySound(SoundList.SwingClub);
                    break;
                default:
                    SoundManager.PlaySound(SoundList.SwingFist);
                    break;
            }
        }

        public void PlayDieSound()
        {
            if (Gender == 0) { SoundManager.PlaySound(SoundList.MaleDie); }
            else { SoundManager.PlaySound(SoundList.FemaleDie); }
        }

        public void PlayMountSound()
        {
            if (RidingMount)
            {
                if(MountType < 7)
                    SoundManager.PlaySound(10218);
                else if (MountType < 12)
                    SoundManager.PlaySound(10188);
            }
            else
            {
                if (MountType < 7)
                    SoundManager.PlaySound(10219);
                else if (MountType < 12)
                    SoundManager.PlaySound(10189);
            }
        }


        public override void Draw()
        {
            DrawBehindEffects(Settings.Effect);

            float oldOpacity = DXManager.Opacity;
            if (Hidden && !DXManager.Blending) DXManager.SetOpacity(0.5F);

            DrawMount();

            if (!RidingMount)
            {
                if (Direction == MirDirection.Left || Direction == MirDirection.Up || Direction == MirDirection.UpLeft || Direction == MirDirection.DownLeft)
                    DrawWeapon();
                else
                    DrawWeapon2();
            }

            DrawBody();

            if (Direction == MirDirection.Up || Direction == MirDirection.UpLeft || Direction == MirDirection.UpRight || Direction == MirDirection.Right || Direction == MirDirection.Left)
            {
                DrawHead();
                if (this != User)
                {
                    DrawWings();
                }
            }
            else
            {
                if (this != User)
                {
                    DrawWings();
                }
                DrawHead();
            }
            

            if (!RidingMount)
            {
                if (Direction == MirDirection.UpRight || Direction == MirDirection.Right || Direction == MirDirection.DownRight || Direction == MirDirection.Down)
                    DrawWeapon();
                else
                    DrawWeapon2();

                if (Class == MirClass.Archer && HasClassWeapon)
                    DrawWeapon2();
            }

            DXManager.SetOpacity(oldOpacity);
        }

        public override void DrawBehindEffects(bool effectsEnabled)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (!Effects[i].DrawBehind) continue;
                if (!Settings.LevelEffect && (Effects[i] is SpecialEffect) && ((SpecialEffect)Effects[i]).EffectType == 1) continue;
                if ((!effectsEnabled) && (!IsVitalEffect(Effects[i]))) continue;
                Effects[i].Draw();
            }
        }

        public override void DrawEffects(bool effectsEnabled)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].DrawBehind) continue;
                if (!Settings.LevelEffect && (Effects[i] is SpecialEffect) && ((SpecialEffect)Effects[i]).EffectType == 1) continue;
                if ((!effectsEnabled) && (!IsVitalEffect(Effects[i]))) continue;
                Effects[i].Draw();
            }

            if (!effectsEnabled) return;

            switch (CurrentAction)
            {
                case MirAction.Attack1:
                    switch (Spell)
                    {
                        case Spell.Slaying:
                            Libraries.Magic.DrawBlend(1820 + ((int)Direction * 10) + SpellLevel * 90 + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.DoubleSlash:
                            Libraries.Magic2.DrawBlend(1960 + ((int)Direction * 10) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.Thrusting:
                            Libraries.Magic.DrawBlend(2190 + ((int)Direction * 10) + SpellLevel * 90 + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.HalfMoon:
                            Libraries.Magic.DrawBlend(2560 + ((int)Direction * 10) + SpellLevel * 90 + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.TwinDrakeBlade:
                            Libraries.Magic2.DrawBlend(220 + ((int)Direction * 20) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.CrossHalfMoon:
                            Libraries.Magic2.DrawBlend(40 + ((int)Direction * 10) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.FlamingSword:
                            Libraries.Magic.DrawBlend(3480 + ((int)Direction * 10) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                    }
                    break;
                case MirAction.Attack4:

                    switch (Spell)
                    {
                        case Spell.DoubleSlash:
                            Libraries.Magic2.DrawBlend(2050 + ((int)Direction * 10) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.TwinDrakeBlade:
                            Libraries.Magic2.DrawBlend(226 + ((int)Direction * 20) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                        case Spell.FlamingSword:
                            Libraries.Magic.DrawBlend(3480 + ((int)Direction * 10) + FrameIndex, DrawLocation, Color.White, true, 0.7F);
                            break;
                    }
                    break;
            }


        }

        public void DrawCurrentEffects()
        {
            if (CurrentEffect == SpellEffect.MagicShieldUp && !MagicShield)
            {
                MagicShield = true;
                Effects.Add(ShieldEffect = new Effect(Libraries.Magic, 3890, 3, 600, this) { Repeat = true });
                CurrentEffect = SpellEffect.None;
            }

            if (CurrentEffect == SpellEffect.ElementalBarrierUp && !ElementalBarrier)
            {
                ElementalBarrier = true;
                Effects.Add(ElementalBarrierEffect = new Effect(Libraries.Magic3, 1890, 16, 3200, this) { Repeat = true });
                CurrentEffect = SpellEffect.None;
            }

            if (ElementEffect > 0 && !HasElements)
            {
                HasElements = true;
                if (ElementEffect == 4)
                    Effects.Add(new ElementsEffect(Libraries.Magic3, 1660, 10, 10 * 100, this, true, 4, ElementOrbMax));
                else
                {
                    if (ElementEffect >= 1)
                        Effects.Add(new ElementsEffect(Libraries.Magic3, 1630, 10, 10 * 100, this, true, 1, ElementOrbMax));
                    if (ElementEffect >= 2)
                        Effects.Add(new ElementsEffect(Libraries.Magic3, 1640, 10, 10 * 100, this, true, 2, ElementOrbMax));
                    if (ElementEffect == 3)
                        Effects.Add(new ElementsEffect(Libraries.Magic3, 1650, 10, 10 * 100, this, true, 3, ElementOrbMax));
                }
                ElementEffect = 0;
            }
        }

        public override void DrawBlend()
        {
            DXManager.SetBlend(true, 0.3F);
            Draw();
            DXManager.SetBlend(false);
        }
        public void DrawBody()
        {
            bool oldGrayScale = DXManager.GrayScale;
            Color drawColour = ApplyDrawColour();                     

            if (BodyLibrary != null)
                BodyLibrary.Draw(DrawFrame + ArmourOffSet, DrawLocation, drawColour, true);

            DXManager.SetGrayscale(oldGrayScale);

            //BodyLibrary.DrawTinted(DrawFrame + ArmourOffSet, DrawLocation, DrawColour, Color.DarkSeaGreen);
        }
        public void DrawHead()
        {
            if (HairLibrary != null)
                HairLibrary.Draw(DrawFrame + HairOffSet, DrawLocation, DrawColour, true);
        }
		public void DrawWeapon()
		{
			if (Weapon < 0) return;

			if (WeaponLibrary1 != null)
			{
				WeaponLibrary1.Draw(DrawFrame + WeaponOffSet, DrawLocation, DrawColour, true); //original

				if (WeaponEffectLibrary1 != null)
					WeaponEffectLibrary1.DrawBlend(DrawFrame + WeaponOffSet, DrawLocation, DrawColour, true, 0.4F);
			}
		}
		public void DrawWeapon2()
        {
            if (Weapon == -1) return;

            if (WeaponLibrary2 != null)
                WeaponLibrary2.Draw(DrawFrame + WeaponOffSet, DrawLocation, DrawColour, true);
        }
        public void DrawWings()
        {
            if (WingEffect <= 0 || WingEffect >= 100) return;

            if (WingLibrary != null)
                WingLibrary.DrawBlend(DrawWingFrame + WingOffset, DrawLocation, DrawColour, true);
        }


        public void DrawMount()
        {
            if (MountType < 0 || !RidingMount) return;

            if (MountLibrary != null)
                MountLibrary.Draw(DrawFrame - 416 + MountOffset, DrawLocation, DrawColour, true);
        }

        private bool IsVitalEffect(Effect effect)
        {
            if ((effect.Library == Libraries.Magic) && (effect.BaseIndex == 3890))
                return true;
            if ((effect.Library == Libraries.Magic3) && (effect.BaseIndex == 1890))
                return true;
            return false;
        }

        public void GetBackStepDistance(int magicLevel)
        {
            JumpDistance = 0;
            if (InTrapRock) return;

            int travel = 0;
            bool blocked = false;
            int dist = (magicLevel == 0) ? 1 : magicLevel;//3 max
            MirDirection jumpDir = Functions.ReverseDirection(Direction);

            Point location = CurrentLocation;
            for (int i = 0; i < dist; i++)//step 1t/m3
            {
                location = Functions.PointMove(location, jumpDir, 1);
                if (!GameScene.Scene.MapControl.ValidPoint(location)) break;

                CellInfo cInfo = GameScene.Scene.MapControl.M2CellInfo[location.X, location.Y];
                if (cInfo.CellObjects != null)
                    for (int c = 0; c < cInfo.CellObjects.Count; c++)
                    {
                        MapObject ob = cInfo.CellObjects[c];
                        if (!ob.Blocking) continue;
                        blocked = true;
                        if ((cInfo.CellObjects == null) || blocked) break;
                    }
                if (blocked) break;
                travel++;
            }
            JumpDistance = travel;
        }

        public void GetFlashDashDistance(int magicLevel)
        {
            JumpDistance = 0;
            if (InTrapRock) return;

            int travel = 0;
            bool blocked = false;
            int dist = (magicLevel <= 1) ? 0 : 1;
            MirDirection jumpDir = Direction;

            Point location = CurrentLocation;
            for (int i = 0; i < dist; i++)
            {
                location = Functions.PointMove(location, jumpDir, 1);
                if (!GameScene.Scene.MapControl.ValidPoint(location)) break;

                CellInfo cInfo = GameScene.Scene.MapControl.M2CellInfo[location.X, location.Y];
                if (cInfo.CellObjects != null)
                    for (int c = 0; c < cInfo.CellObjects.Count; c++)
                    {
                        MapObject ob = cInfo.CellObjects[c];
                        if (!ob.Blocking) continue;
                        blocked = true;
                        if ((cInfo.CellObjects == null) || blocked) break;
                    }
                if (blocked) break;
                travel++;
            }
            JumpDistance = travel;
        }

        public bool IsDashAttack()
        {
            Point location = CurrentLocation;
            location = Functions.PointMove(location, Direction, 1);

            if (!GameScene.Scene.MapControl.ValidPoint(location)) return false;

            CellInfo cInfo = GameScene.Scene.MapControl.M2CellInfo[location.X, location.Y];

            if (cInfo.CellObjects != null)
            {
                for (int c = 0; c < cInfo.CellObjects.Count; c++)
                {
                    MapObject ob = cInfo.CellObjects[c];
                    if (ob == this) return false;
                    switch (ob.Race)
                    {
                        case ObjectType.Monster:
                        case ObjectType.Player:
                            return true;
                    }
                }
            }

            return false;
        }

        public override bool MouseOver(Point p)
        {
            return MapControl.MapLocation == CurrentLocation || BodyLibrary != null && BodyLibrary.VisiblePixel(DrawFrame + ArmourOffSet, p.Subtract(FinalDrawLocation), false);
        }

        private void CreateNameLabel()
        {
            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].Text != Name || LabelList[i].ForeColour != NameColour) continue;
                NameLabel = LabelList[i];
                break;
            }

            if (NameLabel != null && !NameLabel.IsDisposed) return;

            NameLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = NameColour,
                OutLine = true,
                OutLineColour = Color.Black,
                Text = Name,
            };
            NameLabel.Disposing += (o, e) => LabelList.Remove(NameLabel);
            LabelList.Add(NameLabel);
        }

        private void CreateGuildLabel()
        {
            if (string.IsNullOrEmpty(GuildName))
                return;

            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].Text != GuildName || LabelList[i].ForeColour != NameColour) continue;
                GuildLabel = LabelList[i];
                break;
            }

            if (GuildLabel != null && !GuildLabel.IsDisposed) return;

            GuildLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = NameColour,
                OutLine = true,
                OutLineColour = Color.Black,
                Text = GuildName,
            };
            GuildLabel.Disposing += (o, e) => LabelList.Remove(GuildLabel);
            LabelList.Add(GuildLabel);
        }

        public override void CreateLabel()
        {
            NameLabel = null;
            GuildLabel = null;

            CreateNameLabel();
            CreateGuildLabel();
        }

        public override void DrawName()
        {
            CreateLabel();

            if (GuildLabel != null && !string.IsNullOrEmpty(GuildName))
            {
                GuildLabel.Text = GuildName;
                GuildLabel.Location = new Point(DisplayRectangle.X + (50 - GuildLabel.Size.Width) / 2, DisplayRectangle.Y - (19 - GuildLabel.Size.Height / 2) + (Dead ? 35 : 8)); //was 48 -
                GuildLabel.Draw();
            }

            if (NameLabel != null)
            {
                NameLabel.Text = Name;
                NameLabel.Location = new Point(DisplayRectangle.X + (50 - NameLabel.Size.Width) / 2, DisplayRectangle.Y - (31 - NameLabel.Size.Height / 2) + (Dead ? 35 : 8)); //was 48 -
                NameLabel.Draw();
            }
        }

    }


    public class QueuedAction
    {
        public MirAction Action;
        public Point Location;
        public MirDirection Direction;
        public List<object> Params;
    }

}
