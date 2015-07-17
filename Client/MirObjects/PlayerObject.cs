using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirControls;
using S = ServerPackets;
using C = ClientPackets;

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
        public byte Level;

        public MLibrary WeaponLibrary1, WeaponLibrary2, HairLibrary, WingLibrary, MountLibrary;
        public int Armour, Weapon, ArmourOffSet, HairOffSet, WeaponOffSet, WingOffset, MountOffset;

        public int DieSound, FlinchSound, AttackSound;


        public FrameSet Frames;
        public Frame Frame, WingFrame;
        public int FrameIndex, FrameInterval, EffectFrameIndex, EffectFrameInterval, SlowFrameIndex;

        public bool HasClassWeapon
        {
            get
            {
                switch (Weapon / 100)
                {
                    default:
                        return Class == MirClass.Wizard || Class == MirClass.Warrior || Class == MirClass.Taoist;
                    case 1:
                        return Class == MirClass.Assassin;
                    case 2:
                        return Class == MirClass.Archer;
                    case 3://stupple
                        return Class == MirClass.HighWarrior;
                    case 4:
                        return Class == MirClass.HighWizard;
                    case 5:
                        return Class == MirClass.HighTaoist;
                    case 6:
                        return Class == MirClass.HighAssassin;
                    case 7:
                        return Class == MirClass.HighArcher;
                }
            }
        }

        public bool HasFishingRod
        {
            get
            {
                return Weapon == 49 || Weapon == 50;
            }
        }

        public Spell Spell;
        public byte SpellLevel;
        public int JumpDistance;
        public bool Cast;
        public uint TargetID;
        public Point TargetPoint;

        public bool MagicShield;
        public Effect ShieldEffect;

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
        public bool ElementalBarrier;
        public Effect ElementalBarrierEffect;
        //Elemental system END

        public SpellEffect CurrentEffect; //Stephenking effect sync test

        public long StanceTime, BlizzardFreezeTime, ReincarnationStopTime;

        public string GuildName;
        public string GuildRankName;

        public short MountType;
        public long MountTime;
        public bool RidingMount;

        public bool Sprint;
        public bool FastRun;

        public long FishingTime;
        public bool Fishing;
        public Point FishingPoint;
        public bool FoundFish;

        public LevelEffects LevelEffects;

        public List<BuffType> Buffs = new List<BuffType>();

        public PlayerObject(uint objectID, MirClass Class)//stupple
            : base(objectID)
        {
            if ((byte)Class < 5)
                Frames = FrameSet.Players;
            else
                Frames = FrameSet.HighPlayers[0];
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
            Armour = info.Armour;
            Light = info.Light;
            WingEffect = info.WingEffect;
            SetLibraries();
            SetEffects();
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

                    if (p.FoundFish)
                        GameScene.Scene.ChatDialog.ReceiveChat("Found fish!!", ChatType.Hint);
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
        private void ResetFramePos(bool AltAnim, int idx) //stupple
        {
            FrameSet tempFrames = Frames;
            Frames = AltAnim ? FrameSet.HighPlayers[idx] : FrameSet.HighPlayers[0];

            if (tempFrames != Frames)
            {
                DrawFrame = 0;
                DrawWingFrame = 0;
                FrameIndex = 0;
                EffectFrameIndex = 0;
                Frame = Frames.Frames[CurrentAction];
            }
        }


        public virtual void SetLibraries()
        {
            bool AltAnim = false;

            switch(Class)
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
                                AltAnim = true;
                                break;
                        }
                    }

                    if (CurrentAction == MirAction.Jump) AltAnim = true;

                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        switch (Armour)
                        {
                            case 9: //heaven
                            case 10: //mir
                            case 11: //oma
                            case 12: //spirit
                                BodyLibrary = Armour + 1 < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour + 1] : Libraries.ARArmours[0];
                                break;

                            case 19:
                                BodyLibrary = Armour - 5 < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour - 5] : Libraries.ARArmours[0];
                                break;

                            case 29:
                            case 30:
                                BodyLibrary = Armour - 14 < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour - 14] : Libraries.ARArmours[0];
                                break;

                            case 35:
                            case 36:
                            case 37:
                            case 38:
                            case 39:
                            case 40:
                            case 41:
                                BodyLibrary = Armour - 32 < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour - 32] : Libraries.ARArmours[0];
                                break;

                            default:
                                BodyLibrary = Armour < Libraries.ARArmours.Length ? Libraries.ARArmours[Armour] : Libraries.ARArmours[0];
                                break;
                        }

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

                        if (AltAnim)
                            WeaponLibrary2 = Index < Libraries.ARWeaponsS.Length ? Libraries.ARWeaponsS[Index] : null;
                        else
                            WeaponLibrary2 = Index < Libraries.ARWeapons.Length ? Libraries.ARWeapons[Index] : null;

                        WeaponLibrary1 = null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        if (AltAnim)
                            WingLibrary = (WingEffect - 1) < Libraries.ARHumEffect.Length ? Libraries.ARHumEffect[WingEffect - 1] : null;
                        else
                            WingLibrary = (WingEffect - 1) < Libraries.CHumEffect.Length ? Libraries.CHumEffect[WingEffect - 1] : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 352 : 808;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 352 : 808;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 352 : 416;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 352 : 840;
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
                                AltAnim = true;
                                break;
                        }
                    }
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        switch (Armour)
                        {
                            case 9: //heaven
                            case 10: //mir
                            case 11: //oma
                            case 12: //spirit
                                BodyLibrary = Armour + 3 < Libraries.AArmours.Length ? Libraries.AArmours[Armour + 3] : Libraries.AArmours[0];
                                break;

                            case 19:
                                BodyLibrary = Armour - 3 < Libraries.AArmours.Length ? Libraries.AArmours[Armour - 3] : Libraries.AArmours[0];
                                break;

                            case 20:
                            case 21:
                            case 22:
                            case 23: //red bone
                            case 24:
                                BodyLibrary = Armour - 17 < Libraries.AArmours.Length ? Libraries.AArmours[Armour - 17] : Libraries.AArmours[0];
                                break;

                            case 28:
                            case 29:
                            case 30:
                                BodyLibrary = Armour - 20 < Libraries.AArmours.Length ? Libraries.AArmours[Armour - 20] : Libraries.AArmours[0];
                                break;

                            case 34:
                                BodyLibrary = Armour - 23 < Libraries.AArmours.Length ? Libraries.AArmours[Armour - 23] : Libraries.AArmours[0];
                                break;

                            default:
                                BodyLibrary = Armour < Libraries.AArmours.Length ? Libraries.AArmours[Armour] : Libraries.AArmours[0];
                                break;
                        }

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
                            WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        if(AltAnim)
                            WingLibrary = (WingEffect - 1) < Libraries.AHumEffect.Length ? Libraries.AHumEffect[WingEffect - 1] : null;
                        else
                            WingLibrary = (WingEffect - 1) < Libraries.CHumEffect.Length ? Libraries.CHumEffect[WingEffect - 1] : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 512 : 808;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 512 : 808;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 512 : 416;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 544 : 840;
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
                        WeaponLibrary1 = Weapon < Libraries.CWeapons.Length ? Libraries.CWeapons[Weapon] : null;
                    else
                        WeaponLibrary1 = null;
                    WeaponLibrary2 = null;

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
                  #region High War //stupple
                case MirClass.HighWarrior:

                    #region WeaponType
                    if (HasClassWeapon)
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                            case MirAction.Walking:
                            case MirAction.Running:
                            case MirAction.Stance:
                            case MirAction.Stance2:
                            case MirAction.Attack1:
                            case MirAction.Attack2:
                            case MirAction.Attack3:
                            case MirAction.Attack4:
                            case MirAction.Spell:
                            case MirAction.Harvest:
                            case MirAction.Struck:
                            case MirAction.Die:
                            case MirAction.Dead:
                            case MirAction.Revive:
                            case MirAction.DashL:
                            case MirAction.DashR:
                                AltAnim = true;
                                break;
                        }
                    }

                    ResetFramePos(AltAnim, 1);
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        BodyLibrary = Armour < Libraries.UpWarArmours.Length ? Libraries.UpWarArmours[Armour] : Libraries.UpWarArmours[0];
                        HairLibrary = Hair < Libraries.UpWarHair.Length ? Libraries.UpWarHair[Hair] : null;
                    }
                    else
                    {
                        int fixArmour = Armour;
                        switch (Armour)
                        {
                            case 0:
                                fixArmour = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                fixArmour = (fixArmour - 1) * 5 + 1;
                                break;
                            default:
                                fixArmour = fixArmour + 16;
                                break;
                        }

                        BodyLibrary = fixArmour < Libraries.UpCArmours.Length ? Libraries.UpCArmours[fixArmour] : Libraries.UpCArmours[0];
                        HairLibrary = Hair < Libraries.UpCHair.Length ? Libraries.UpCHair[Hair] : null;
                    }
                    #endregion

                    #region Weapons
                    if (HasClassWeapon)
                    {
                        int Index = Weapon - 300;

                        WeaponLibrary1 = Index < Libraries.UpWarWeapons.Length ? Libraries.UpWarWeapons[Index] : null;
                        WeaponLibrary2 = null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.UpCWeapons.Length ? Libraries.UpCWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        WingLibrary = (WingEffect - 1) < (AltAnim ? Libraries.UpWarHumEffect.Length : Libraries.UpCHumEffect.Length) ? (AltAnim ? Libraries.UpWarHumEffect[WingEffect - 1] : Libraries.UpCHumEffect[WingEffect - 1]) : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 784 : 1112;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 784 : 1112;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 784 : 600;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 784 : 1112;
                    MountOffset = 0;
                    #endregion

                    break;
                #endregion

                #region High Wiz //stupple
                case MirClass.HighWizard:

                    #region WeaponType
                    if (HasClassWeapon)
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                            case MirAction.Walking:
                            case MirAction.Running:
                            case MirAction.Stance:
                            case MirAction.Stance2:
                            case MirAction.Attack1:
                            case MirAction.Attack2:
                            case MirAction.Attack3:
                            case MirAction.Attack4:
                            case MirAction.Spell:
                            case MirAction.Harvest:
                            case MirAction.Struck:
                            case MirAction.Die:
                            case MirAction.Dead:
                            case MirAction.Revive:
                            case MirAction.DashL:
                            case MirAction.DashR:
                                AltAnim = true;
                                break;
                        }
                    }

                    ResetFramePos(AltAnim, 2);
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        BodyLibrary = Armour < Libraries.UpWizArmours.Length ? Libraries.UpWizArmours[Armour] : Libraries.UpWizArmours[0];
                        HairLibrary = Hair < Libraries.UpWizHair.Length ? Libraries.UpWizHair[Hair] : null;
                    }
                    else
                    {
                        int fixArmour = Armour;
                        switch (Armour)
                        {
                            case 0:
                                fixArmour = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                fixArmour = (fixArmour - 1) * 5 + 2;
                                break;
                            default:
                                fixArmour = fixArmour + 16;
                                break;
                        }

                        BodyLibrary = fixArmour < Libraries.UpCArmours.Length ? Libraries.UpCArmours[fixArmour] : Libraries.UpCArmours[0];
                        HairLibrary = Hair < Libraries.UpCHair.Length ? Libraries.UpCHair[Hair] : null;
                    }
                    #endregion

                    #region Weapons
                    if (HasClassWeapon)
                    {
                        int Index = Weapon - 400;

                        WeaponLibrary1 = Index < Libraries.UpWizWeapons.Length ? Libraries.UpWizWeapons[Index] : null;
                        WeaponLibrary2 = null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.UpCWeapons.Length ? Libraries.UpCWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        WingLibrary = (WingEffect - 1) < (AltAnim ? Libraries.UpWizHumEffect.Length : Libraries.UpCHumEffect.Length) ? (AltAnim ? Libraries.UpWizHumEffect[WingEffect - 1] : Libraries.UpCHumEffect[WingEffect - 1]) : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 600;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    MountOffset = 0;
                    #endregion

                    break;
                #endregion

                #region High Tao //stupple
                case MirClass.HighTaoist:

                    #region WeaponType
                    if (HasClassWeapon)
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                            case MirAction.Walking:
                            case MirAction.Running:
                            case MirAction.Stance:
                            case MirAction.Stance2:
                            case MirAction.Attack1:
                            case MirAction.Attack2:
                            case MirAction.Attack3:
                            case MirAction.Attack4:
                            case MirAction.Spell:
                            case MirAction.Harvest:
                            case MirAction.Struck:
                            case MirAction.Die:
                            case MirAction.Dead:
                            case MirAction.Revive:
                            case MirAction.DashL:
                            case MirAction.DashR:
                                AltAnim = true;
                                break;
                        }
                    }

                    ResetFramePos(AltAnim, 3);
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        BodyLibrary = Armour < Libraries.UpTaoArmours.Length ? Libraries.UpTaoArmours[Armour] : Libraries.UpTaoArmours[0];
                        HairLibrary = Hair < Libraries.UpTaoHair.Length ? Libraries.UpTaoHair[Hair] : null;
                    }
                    else
                    {
                        int fixArmour = Armour;
                        switch (Armour)
                        {
                            case 0:
                                fixArmour = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                fixArmour = (fixArmour - 1) * 5 + 3;
                                break;
                            default:
                                fixArmour = fixArmour + 16;
                                break;
                        }

                        BodyLibrary = fixArmour < Libraries.UpCArmours.Length ? Libraries.UpCArmours[fixArmour] : Libraries.UpCArmours[0];
                        HairLibrary = Hair < Libraries.UpCHair.Length ? Libraries.UpCHair[Hair] : null;
                    }
                    #endregion

                    #region Weapons
                    if (HasClassWeapon)
                    {
                        int Index = Weapon - 500;

                        WeaponLibrary1 = Index < Libraries.UpTaoWeapons.Length ? Libraries.UpTaoWeapons[Index] : null;
                        WeaponLibrary2 = null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.UpCWeapons.Length ? Libraries.UpCWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        WingLibrary = (WingEffect - 1) < (AltAnim ? Libraries.UpTaoHumEffect.Length : Libraries.UpCHumEffect.Length) ? (AltAnim ? Libraries.UpTaoHumEffect[WingEffect - 1] : Libraries.UpCHumEffect[WingEffect - 1]) : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 600;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 720 : 1112;
                    MountOffset = 0;
                    #endregion

                    break;
                #endregion

                #region High Ass //stupple
                case MirClass.HighAssassin:

                    #region WeaponType
                    if (HasClassWeapon)
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                            case MirAction.Walking:
                            case MirAction.Running:
                            case MirAction.Stance:
                            case MirAction.Stance2:
                            case MirAction.Attack1:
                            case MirAction.Attack2:
                            case MirAction.Attack3:
                            case MirAction.Attack4:
                            case MirAction.Spell:
                            case MirAction.Harvest:
                            case MirAction.Struck:
                            case MirAction.Die:
                            case MirAction.Dead:
                            case MirAction.Revive:
                            case MirAction.DashL:
                            case MirAction.DashR:
                            case MirAction.DashAttack:
                            case MirAction.Sneek:
                                AltAnim = true;
                                break;
                        }
                    }

                    ResetFramePos(AltAnim, 4);
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        BodyLibrary = Armour < Libraries.UpAssArmours.Length ? Libraries.UpAssArmours[Armour] : Libraries.UpAssArmours[0];
                        HairLibrary = Hair < Libraries.UpAssHair.Length ? Libraries.UpAssHair[Hair] : null;
                    }
                    else
                    {
                        int fixArmour = Armour;
                        switch (Armour)
                        {
                            case 0:
                                fixArmour = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                fixArmour = (fixArmour - 1) * 5 + 4;
                                break;
                            default:
                                fixArmour = fixArmour + 16;
                                break;
                        }

                        BodyLibrary = fixArmour < Libraries.UpCArmours.Length ? Libraries.UpCArmours[fixArmour] : Libraries.UpCArmours[0];
                        HairLibrary = Hair < Libraries.UpCHair.Length ? Libraries.UpCHair[Hair] : null;
                    }
                    #endregion

                    #region Weapons
                    if (HasClassWeapon)
                    {
                        int Index = Weapon - 600;

                        WeaponLibrary1 = Index < Libraries.UpAssWeaponsR.Length ? Libraries.UpAssWeaponsR[Index] : null;
                        WeaponLibrary2 = Index < Libraries.UpAssWeaponsL.Length ? Libraries.UpAssWeaponsL[Index] : null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.UpCWeapons.Length ? Libraries.UpCWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        WingLibrary = (WingEffect - 1) < (AltAnim ? Libraries.UpAssHumEffect.Length : Libraries.UpCHumEffect.Length) ? (AltAnim ? Libraries.UpAssHumEffect[WingEffect - 1] : Libraries.UpCHumEffect[WingEffect - 1]) : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 912 : 1112;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 912 : 1112;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 912 : 600;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 912 : 1112;
                    MountOffset = 0;
                    #endregion

                    break;
                #endregion

                #region High Arc //stupple
                case MirClass.HighArcher:

                    #region WeaponType
                    if (HasClassWeapon)
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Walking:
                            case MirAction.Running:
                            case MirAction.AttackRange1:
                            case MirAction.AttackRange2:
                            case MirAction.AttackRange3:
                            case MirAction.Jump:
                                AltAnim = true;
                                break;
                        }
                    }

                    ResetFramePos(AltAnim, 5);
                    #endregion

                    #region Armours
                    if (AltAnim)
                    {
                        BodyLibrary = Armour < Libraries.UpArcArmours.Length ? Libraries.UpArcArmours[Armour] : Libraries.UpArcArmours[0];
                        HairLibrary = Hair < Libraries.UpArcHair.Length ? Libraries.UpArcHair[Hair] : null;
                    }
                    else
                    {
                        int fixArmour = Armour;
                        switch (Armour)
                        {
                            case 0:
                                fixArmour = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                fixArmour = (fixArmour - 1) * 5 + 5;
                                break;
                            default:
                                fixArmour = fixArmour + 16;
                                break;
                        }

                        BodyLibrary = fixArmour < Libraries.UpCArmours.Length ? Libraries.UpCArmours[fixArmour] : Libraries.UpCArmours[0];
                        HairLibrary = Hair < Libraries.UpCHair.Length ? Libraries.UpCHair[Hair] : null;
                    }
                    #endregion

                    #region Weapons
                    if (HasClassWeapon)
                    {
                        int Index = Weapon - 700;

                        if (AltAnim)
                            WeaponLibrary2 = Index < Libraries.UpArcWeapons.Length ? Libraries.UpArcWeapons[Index] : null;
                        else
                            WeaponLibrary2 = Index < Libraries.UpArcWeaponsS.Length ? Libraries.UpArcWeaponsS[Index] : null;

                        WeaponLibrary1 = null;
                    }
                    else
                    {
                        if (Weapon >= 0)
                            WeaponLibrary1 = Weapon < Libraries.UpCWeapons.Length ? Libraries.UpCWeapons[Weapon] : null;
                        else
                            WeaponLibrary1 = null;

                        WeaponLibrary2 = null;
                    }
                    #endregion

                    #region WingEffects
                    if (WingEffect > 0 && WingEffect < 100)
                    {
                        WingLibrary = (WingEffect - 1) < (AltAnim ? Libraries.UpArcHumEffect.Length : Libraries.UpCHumEffect.Length) ? (AltAnim ? Libraries.UpArcHumEffect[WingEffect - 1] : Libraries.UpCHumEffect[WingEffect - 1]) : null;
                    }
                    #endregion

                    #region Offsets
                    ArmourOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 384 : 1112;
                    HairOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 384 : 1112;
                    WeaponOffSet = Gender == MirGender.Male ? 0 : AltAnim ? 384 : 600;
                    WingOffset = Gender == MirGender.Male ? 0 : AltAnim ? 384 : 1112;
                    MountOffset = 0;
                    #endregion

                    break;
                #endregion
                
             
            }

            #region Common
            //Harvest
            if (CurrentAction == MirAction.Harvest)
                WeaponLibrary1 = 1 < Libraries.CWeapons.Length ? Libraries.CWeapons[1] : null;

            //Mounts
            if (MountType > -1 && RidingMount)
            {
                MountLibrary = MountType < Libraries.Mounts.Length ? Libraries.Mounts[MountType] : null;
            }
            else
            {
                MountLibrary = null;
            }

            //Fishing
           if (HasFishingRod)
            {
                if (CurrentAction == MirAction.FishingCast || CurrentAction == MirAction.FishingWait || CurrentAction == MirAction.FishingReel)
                {
                    if (Class < MirClass.HighWarrior)//stupple
                    {
                        WeaponLibrary1 = 0 < Libraries.Fishing.Length ? Libraries.Fishing[Weapon - 49] : null;
                        WeaponLibrary2 = null;
                        WeaponOffSet = -632;
                    }
                    else
                    {
                        WeaponLibrary1 = 0 < (Gender == MirGender.Male ? Libraries.UpFishingM.Length : Libraries.UpFishingF.Length) ? (Gender == MirGender.Male ? Libraries.UpFishingM[Weapon - 49] : Libraries.UpFishingF[Weapon - 49]) : null;
                        WeaponLibrary2 = null;
                        WeaponOffSet = -600;
                    }
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
                SpecialEffect effect = new SpecialEffect(Libraries.Effect, 1240, 32, 4200, this, true, false, 1) { Repeat = true, Delay = delay };
                effect.SetStart(CMain.Time + delay);
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
        }

        public override void Process()
        {
            bool update = CMain.Time >= NextMotion || GameScene.CanMove;

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

            switch (Poison)
            {
                case PoisonType.None:
                    DrawColour = Color.White;
                    break;
                case PoisonType.Green:
                    DrawColour = Color.Green;
                    break;
                case PoisonType.Red:
                    DrawColour = Color.Red;
                    break;
                case PoisonType.Bleeding:
                    DrawColour = Color.DarkRed;
                    break;
                case PoisonType.Slow:
                    DrawColour = Color.Purple;
                    break;
                case PoisonType.Stun:
                    DrawColour = Color.Yellow;
                    break;
                case PoisonType.Frozen:
                    DrawColour = Color.Blue;
                    break;
                case PoisonType.Paralysis:
                    DrawColour = Color.Gray;
                    break;
                case PoisonType.DelayedExplosion:
                    DrawColour = Color.Orange;
                    break;
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

            if (ActionFeed.Any())
                if (User.RidingMount)
                    switch (ActionFeed.First().Action)
                    {
                        case MirAction.Spell:
                        //case MirAction.Attack1:
                        case MirAction.Attack2:
                        case MirAction.Attack3:
                        case MirAction.Attack4:
                        case MirAction.AttackRange1:
                        case MirAction.AttackRange2:
                        case MirAction.Mine:
                        case MirAction.Harvest:
                            ActionFeed.RemoveAt(0);
                            return;
                    }


            if (ActionFeed.Count == 0)
            {
                CurrentAction = CMain.Time > BlizzardFreezeTime ? MirAction.Standing : MirAction.Stance2; //ArcherTest

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
                    if ((Class == MirClass.Archer || Class == MirClass.HighArcher) && HasClassWeapon)
                        CurrentAction = MirAction.Standing;
                    else
                        CurrentAction = CMain.Time > StanceTime ? MirAction.Standing : MirAction.Stance;

                    if (Concentrating && ConcentrateInterrupted)
                        Network.Enqueue(new C.SetConcentration { ObjectID = User.ObjectID, Enabled = Concentrating, Interrupted = false });
                }

                if (Fishing) CurrentAction = MirAction.FishingWait;

                Frames.Frames.TryGetValue(CurrentAction, out Frame);
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
                        Frames.Frames.TryGetValue(MirAction.Walking, out Frame);
                        break;
                    case MirAction.DashL:
                    case MirAction.DashR:
                        Frames.Frames.TryGetValue(MirAction.Running, out Frame);
                        break;
                    case MirAction.DashAttack:
                        Frames.Frames.TryGetValue(MirAction.DashAttack, out Frame);
                        break;
                    case MirAction.DashFail:
                        Frames.Frames.TryGetValue(RidingMount ? MirAction.MountStanding : MirAction.Standing, out Frame);
                        //Frames.Frames.TryGetValue(MirAction.Standing, out Frame);
                        //CanSetAction = false;
                        break;
                    case MirAction.Jump:
                        Frames.Frames.TryGetValue(MirAction.Jump, out Frame);
                        break;
                    case MirAction.Attack1:
                        switch (Class)
                        {
                           case MirClass.Archer:
                            case MirClass.HighArcher:
                                Frames.Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                            case MirClass.Assassin:
                            case MirClass.HighAssassin:
                                if(GameScene.DoubleSlash)
                                    Frames.Frames.TryGetValue(MirAction.Attack1, out Frame);
                                else if (CMain.Shift)
                                    Frames.Frames.TryGetValue(CMain.Random.Next(100) >= 20 ? (CMain.Random.Next(100) > 40 ? MirAction.Attack1 : MirAction.Attack4) : (CMain.Random.Next(100) > 10 ? MirAction.Attack2 : MirAction.Attack3), out Frame);
                                else
                                    Frames.Frames.TryGetValue(CMain.Random.Next(100) >= 40 ? MirAction.Attack1 : MirAction.Attack4, out Frame);
                                break;
                            default:
                                if (CMain.Shift && GameScene.Thrusting != true)
                                    Frames.Frames.TryGetValue(CMain.Random.Next(100) >= 20 ? MirAction.Attack1 : MirAction.Attack3, out Frame);
                                else
                                    Frames.Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        break;
                    case MirAction.Attack4:
                        Spell = (Spell)action.Params[0];
                        Frames.Frames.TryGetValue(Spell == Spell.TwinDrakeBlade || Spell == Spell.FlamingSword ? MirAction.Attack1 : CurrentAction, out Frame);
                        break;
                    case MirAction.Spell:
                        Spell = (Spell)action.Params[0];
                        switch (Spell)
                        {
                            case Spell.ShoulderDash:
                                Frames.Frames.TryGetValue(MirAction.Running, out Frame);
                                CurrentAction = MirAction.DashL;
                                Direction = olddirection;
                                CurrentLocation = Functions.PointMove(CurrentLocation, Direction, 1);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2500;
                                    GameScene.SpellTime = CMain.Time + 2500; //Spell Delay

                                    Network.Enqueue(new C.Magic { Spell = Spell, Direction = Direction, });
                                }
                                break;
                            case Spell.BladeAvalanche:
                                Frames.Frames.TryGetValue(MirAction.Attack3, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2500;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.SlashingBurst:
                                 Frames.Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2000; // 80%
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.CounterAttack:
                                Frames.Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    GameScene.AttackTime = CMain.Time + User.AttackSpeed;
                                    MapControl.NextAction = CMain.Time + 100; // 80%
                                    GameScene.SpellTime = CMain.Time + 100; //Spell Delay
                                }
                                break;
                            case Spell.PoisonSword:
                                Frames.Frames.TryGetValue(MirAction.Attack1, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 2000; // 80%
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.HeavenlySword:
                                Frames.Frames.TryGetValue(MirAction.Attack2, out Frame);
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1200;
                                    GameScene.SpellTime = CMain.Time + 1200; //Spell Delay
                                }
                                break;
                            case Spell.CrescentSlash:
                                Frames.Frames.TryGetValue(MirAction.Attack3, out Frame);
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
                                        Frames.Frames.TryGetValue(MirAction.DashAttack, out Frame);
                                        CurrentAction = MirAction.DashAttack;
                                        CurrentLocation = Functions.PointMove(CurrentLocation, Direction, JumpDistance);
                                    }
                                    else
                                    {
                                        Frames.Frames.TryGetValue(CMain.Random.Next(100) >= 40 ? MirAction.Attack1 : MirAction.Attack4, out Frame);
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
                                Frames.Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.DoubleShot:                          
                                Frames.Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 500; //Spell Delay
                                }
                                break;
                            case Spell.ExplosiveTrap:
                                Frames.Frames.TryGetValue(MirAction.Harvest, out Frame);
                                CurrentAction = MirAction.Harvest;
                                ArcherLayTrap = true;
                                if (this == User)
                                {
                                    uint targetID = (uint)action.Params[1];
                                    Point location = (Point)action.Params[2];
                                    Network.Enqueue(new C.Magic { Spell = Spell, Direction = Direction, TargetID = targetID, Location = location });
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                }
                                break;
                            case Spell.DelayedExplosion:
                                Frames.Frames.TryGetValue(MirAction.AttackRange2, out Frame);
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
                                    Frames.Frames.TryGetValue(MirAction.Jump, out Frame);
                                    CurrentAction = MirAction.Jump;
                                    CurrentLocation = Functions.PointMove(CurrentLocation, Functions.ReverseDirection(Direction), JumpDistance);
                                    if (this == User)
                                    {
                                        MapControl.NextAction = CMain.Time + 800;
                                        GameScene.SpellTime = CMain.Time + 2500; //Spell Delay
                                        Network.Enqueue(new C.Magic { Spell = Spell, Direction = Direction });
                                    }
                                    break;
                                }
                            case Spell.ElementalShot:
                                if (HasElements && !ElementCasted)
                                {
                                    Frames.Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                    CurrentAction = MirAction.AttackRange2;
                                    if (this == User)
                                    {
                                        MapControl.NextAction = CMain.Time + 1000;
                                        GameScene.SpellTime = CMain.Time + 1500; //Spell Delay
                                    }
                                }
                                else Frames.Frames.TryGetValue(CurrentAction, out Frame);
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
                                Frames.Frames.TryGetValue(MirAction.AttackRange2, out Frame);
                                CurrentAction = MirAction.AttackRange2;
                                if (this == User)
                                {
                                    MapControl.NextAction = CMain.Time + 1000;
                                    GameScene.SpellTime = CMain.Time + 1000; //Spell Delay
                                }
                                break;
                            default:
                                Frames.Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        
                        break;
                    default:
                        Frames.Frames.TryGetValue(CurrentAction, out Frame);
                        break;

                }

                //ArcherTest - Need to check for bow weapon only
                 if ((Class == MirClass.Archer || Class == MirClass.HighArcher) && HasClassWeapon) //stupple
                {
                    switch (CurrentAction)
                    {
                        case MirAction.Walking:
                            Frames.Frames.TryGetValue(MirAction.WalkingBow, out Frame);
                            break;
                        case MirAction.Running:
                            Frames.Frames.TryGetValue(MirAction.RunningBow, out Frame);
                            break;
                    }
                }

                //Assassin sneekyness
                if ((Class == MirClass.Assassin || Class == MirClass.HighAssassin) && Sneaking && (CurrentAction == MirAction.Walking || CurrentAction == MirAction.Running))

                {
                    Frames.Frames.TryGetValue(MirAction.Sneek, out Frame);
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
                            Network.Enqueue(new C.Walk { Direction = Direction });
                            GameScene.Scene.MapControl.FloorValid = false;
                            GameScene.CanRun = true;
                            MapControl.NextAction = CMain.Time + 2500;
                            break;
                        case MirAction.Running:
                        case MirAction.MountRunning:
                            Network.Enqueue(new C.Run { Direction = Direction });
                            GameScene.Scene.MapControl.FloorValid = false;
                            MapControl.NextAction = CMain.Time + (Sprint ? 1000 : 2500);
                            break;
                        case MirAction.Pushed:
                            GameScene.Scene.MapControl.FloorValid = false;
                            MapControl.InputDelay = CMain.Time + 500;
                            break;
                        case MirAction.DashL:
                        case MirAction.DashR:
                        case MirAction.Jump:
                        case MirAction.DashAttack:
                            GameScene.Scene.MapControl.FloorValid = false;
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
                                if (GameScene.Slaying && TargetObject != null)
                                    Spell = Spell.Slaying;

                                if (GameScene.Thrusting && GameScene.Scene.MapControl.HasTarget(Functions.PointMove(CurrentLocation, Direction, 2)))
                                    Spell = Spell.Thrusting;

                                if (GameScene.HalfMoon)
                                {
                                    if (TargetObject != null || GameScene.Scene.MapControl.CanHalfMoon(CurrentLocation, Direction))
                                    {
                                        magic = User.GetMagic(Spell.HalfMoon);
                                        if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                            Spell = Spell.HalfMoon;
                                    }
                                }

                                if (GameScene.CrossHalfMoon)
                                {
                                    if (TargetObject != null || GameScene.Scene.MapControl.CanCrossHalfMoon(CurrentLocation))
                                    {
                                        magic = User.GetMagic(Spell.CrossHalfMoon);
                                        if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                            Spell = Spell.CrossHalfMoon;
                                    }
                                }

                                if (GameScene.DoubleSlash)
                                {
                                    magic = User.GetMagic(Spell.DoubleSlash);
                                    if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                        Spell = Spell.DoubleSlash;
                                }


                                if (GameScene.TwinDrakeBlade)
                                {
                                    magic = User.GetMagic(Spell.TwinDrakeBlade);
                                    if (magic != null && magic.BaseCost + magic.LevelCost * magic.Level <= User.MP)
                                        Spell = Spell.TwinDrakeBlade;
                                }

                                if (GameScene.FlamingSword)
                                {
                                    if (TargetObject != null)
                                    {
                                        magic = User.GetMagic(Spell.FlamingSword);
                                        if (magic != null)
                                            Spell = Spell.FlamingSword;
                                    }
                                }
                            }

                            Network.Enqueue(new C.Attack { Direction = Direction, Spell = Spell });

                            if (Spell == Spell.Slaying)
                                GameScene.Slaying = false;
                            if (Spell == Spell.TwinDrakeBlade)
                                GameScene.TwinDrakeBlade = false;
                            if (Spell == Spell.FlamingSword)
                                GameScene.FlamingSword = false;

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

                        case MirAction.AttackRange1: //ArcherTest
                            GameScene.AttackTime = CMain.Time + User.AttackSpeed + 200;

                            uint targetID = (uint)action.Params[0];
                            Point location = (Point)action.Params[1];

                            Network.Enqueue(new C.RangeAttack { Direction = Direction, Location = CurrentLocation, TargetID = targetID, TargetLocation = location });
                            break;
                        case MirAction.AttackRange2:
                        case MirAction.Spell:
                            Spell = (Spell)action.Params[0];
                            targetID = (uint)action.Params[1];
                            location = (Point)action.Params[2];

                            //magic = User.GetMagic(Spell);
                            //magic.LastCast = CMain.Time;

                            Network.Enqueue(new C.Magic { Spell = Spell, Direction = Direction, TargetID = targetID, Location = location });

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
                                FrameInterval = (FrameInterval * 7 / 10); //50% Faster Animation
                                EffectFrameInterval = (EffectFrameInterval * 7 / 10);
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
                                FrameInterval = FrameInterval * 7 / 10; //50% Animation Speed
                                EffectFrameInterval = EffectFrameInterval * 7 / 10;
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
                            if (player.Class != MirClass.Assassin || player.Class != MirClass.HighAssassin || StruckWeapon == -1) break;
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
                                //SoundManager.PlaySound(20000 + (ushort)Spell * 10);
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

                            #region LionRoar

                            case Spell.LionRoar:
                                Effects.Add(new Effect(Libraries.Magic2, 710, 20, 1200, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10 + (Gender == MirGender.Male ? 0 : 1));
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
                                Effects.Add(new Effect(Libraries.Magic2, 1700 + (int)Direction * 10, 9, 9 * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
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
                                break;

                            #endregion

                            #region MeteorStrike

                            case Spell.MeteorStrike:
                                Effects.Add(new Effect(Libraries.Magic2, 1590, 10, Frame.Count * FrameInterval, this));
                                SoundManager.PlaySound(20000 + (ushort)Spell * 10);
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

                            #region ElementalBarrier ArcherSpells - Elemental system

                            case Spell.ElementalBarrier:
                                if (HasElements && !ElementalBarrier)
                                {
                                    Effects.Add(new Effect(Libraries.Magic3, 1880, 8, Frame.Count * FrameInterval, this));
                                    SoundManager.PlaySound(20000 + (ushort)Spell * 10);
                                }
                                break;

                            #endregion

                            #region PoisonShot      ArcherSpells - PoisonShot
                            case Spell.PoisonShot:
                                Effects.Add(new Effect(Libraries.Magic3, 2300, 8, 1000, this));
                                break;
                            #endregion

                            #region OneWithNature       ArcherSpells - OneWithNature
                            case Spell.OneWithNature:
                                MapControl.Effects.Add(new Effect(Libraries.Magic3, 2710, 8, 1200, CurrentLocation));
                                SoundManager.PlaySound(20000 + 139 * 10);
                                break;
                            #endregion

                        }


                        break;
                    case MirAction.Dead:
                        GameScene.Scene.Redraw();
                        GameScene.Scene.MapControl.SortObject(this);
                        if (MouseObject == this) MouseObject = null;
                        if (TargetObject == this) TargetObject = null;
                        if (MagicObject == this) MagicObject = null;
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

            if (!MagicShield) return;

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

                    //slow frame speed
                    if (Poison == PoisonType.Slow)
                    {
                        if ((CurrentAction == MirAction.Walking || CurrentAction == MirAction.Running))
                        {
                            if (SlowFrameIndex >= 3)
                            {
                                SlowFrameIndex = 0;
                            }
                            else
                            {
                                SlowFrameIndex++;
                                return;
                            }
                        }
                    }
                    else
                    {
                        SlowFrameIndex = 0;
                    }
                    
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
                        if (this == User)
                        {
                            if (FrameIndex == 1 || FrameIndex == 4)
                                PlayStepSound();
                        }
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
                                                Effects.Add(new Effect(Libraries.Prguse, 1350, 2, 720, this) { Light = 0 });
                                                ((MirAnimatedButton)GameScene.Scene.FishingStatusDialog.FishButton).Visible = true;
                                            }
                                            else
                                            {
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 650, 6, 720, FishingPoint) { Light = 0 });
                                                MapControl.Effects.Add(new Effect(Libraries.Effect, 640, 6, 720, FishingPoint) { Light = 0 });
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
                                                                //
                                                                if (GameScene.Scene.Buffs.Where(x => x.Type == BuffType.VampireShot).Any()) exIdx = 20;
                                                                if (GameScene.Scene.Buffs.Where(x => x.Type == BuffType.PoisonShot).Any()) exIdx = 10;
                                                            }
                                                            else
                                                            {
                                                                if (Buffs.Where(x => x == BuffType.VampireShot).Any()) exIdx = 20;
                                                                if (Buffs.Where(x => x == BuffType.PoisonShot).Any()) exIdx = 10;
                                                            }

                                                            //GameScene.Scene.ChatDialog.ReceiveChat("Debug: "+exIdx.ToString(),ChatType.System);

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
                                                SoundManager.PlaySound(20000 + (ushort)Spell.FireBall * 10 + 2);
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
                                        BlizzardFreezeTime = CMain.Time + 3000;
                                        break;

                                    #endregion

                                    #region MeteorStrike

                                    case Spell.MeteorStrike:
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 1);
                                        SoundManager.PlaySound(20000 + (ushort)Spell * 10 + 2);
                                        BlizzardFreezeTime = CMain.Time + 3000;
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
        public int UpdateFrame()
        {
            if (Frame == null) return 0;

            if (Frame.Reverse) return Math.Abs(--FrameIndex);

            return ++FrameIndex;
        }

        public int UpdateFrame2()
        {
            if (Frame == null) return 0;

            if (Frame.Reverse) return Math.Abs(--EffectFrameIndex);

            return ++EffectFrameIndex;
        }


        private Missile CreateProjectile(int baseIndex, MLibrary library, bool blend, int count, int interval, int skip)
        {
            MapObject ob = MapControl.GetObject(TargetID);

            if (ob != null) TargetPoint = ob.CurrentLocation;

            int duration = Functions.MaxDistance(CurrentLocation, TargetPoint) * 50;


            Missile missile = new Missile(library, baseIndex, duration / interval, duration, this, TargetPoint)
            {
                Target = ob,
                Interval = interval,
                FrameCount = count,
                Blend = blend,
                Skip = skip
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
                moveSound = SoundList.WalkGroundL;
            }
            else //wemade tiles
            {
                int index = (GameScene.Scene.MapControl.M2CellInfo[x, y].BackImage & 0x1FFFF) - 1;
                index = (GameScene.Scene.MapControl.M2CellInfo[x, y].FrontIndex - 2) * 10000 + index;

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

            if (RidingMount) moveSound = SoundList.MountWalkL;

            if (CurrentAction == MirAction.Running) moveSound += 2;
            if (FrameIndex == 4) moveSound++;

            SoundManager.PlaySound(moveSound);
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
            if (Class != MirClass.Assassin || Class != MirClass.HighAssassin)//stupple //Archer to add?

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

           if (Weapon >= 0 && (Class == MirClass.Assassin || Class == MirClass.HighAssassin))
            {
                SoundManager.PlaySound(SoundList.SwingShort);
                return;
            }
            
            if (Weapon >= 0 && Class == MirClass.HighWarrior)//stupple
            {
                switch (Weapon)
                {
                    default:
                        SoundManager.PlaySound(SoundList.SwingSword);
                        break;
                }
                return;
            }

           
            if ((Class == MirClass.Archer || Class == MirClass.HighArcher) && HasClassWeapon)
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
            float oldOpacity = DXManager.Opacity;
            if (Hidden && !DXManager.Blending) DXManager.SetOpacity(0.5F);

            if (Settings.Effect)
            {
                DrawBehindEffects();
            }

            DrawMount();

            if (!RidingMount)
            {
                if (Direction == MirDirection.Left || Direction == MirDirection.Up || Direction == MirDirection.UpLeft || Direction == MirDirection.DownLeft)
                    DrawWeapon();
                else
                    DrawWeapon2();
                   if ((Class == MirClass.Archer || Class == MirClass.HighArcher) && HasClassWeapon)

            }

            DrawBody();

            DrawHead();

            if (this != User)
            {
                DrawWings();
                DrawCurrentEffects();
            }

            if (!RidingMount)
            {
                if (Direction == MirDirection.UpRight || Direction == MirDirection.Right || Direction == MirDirection.DownRight || Direction == MirDirection.Down)
                    DrawWeapon();
                else
                    DrawWeapon2();

                 if (Class == MirClass.Archer || Class == MirClass.HighArcher && HasClassWeapon)///stupple
                    DrawWeapon2();
            }

            DXManager.SetOpacity(oldOpacity);
        }

        public override void DrawBehindEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Hidden || !Effects[i].DrawBehind) continue;
                if (!Settings.LevelEffect && (Effects[i] is SpecialEffect) && ((SpecialEffect)Effects[i]).EffectType == 1) continue;

                Effects[i].Draw();
            }
        }

        public override void DrawEffects()
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Hidden || Effects[i].DrawBehind) continue;
                if (!Settings.LevelEffect && (Effects[i] is SpecialEffect) && ((SpecialEffect)Effects[i]).EffectType == 1) continue;

                Effects[i].Draw();
            }

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
            if (BodyLibrary != null)
                BodyLibrary.Draw(DrawFrame + ArmourOffSet, DrawLocation, DrawColour, true);

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
                WeaponLibrary1.Draw(DrawFrame + WeaponOffSet, DrawLocation, DrawColour, true);
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

        public override void CreateLabel()
        {
            NameLabel = null;
            GuildLabel = null;

            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].Text != Name || LabelList[i].ForeColour != NameColour) continue;
                NameLabel = LabelList[i];
                break;
            }

            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].Text != GuildName || LabelList[i].ForeColour != NameColour) continue;
                GuildLabel = LabelList[i];
                break;
            }

            if (NameLabel != null && !NameLabel.IsDisposed && GuildLabel != null && !GuildLabel.IsDisposed) return;

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

        public override void DrawName()
        {
            CreateLabel();

            if (NameLabel == null || GuildLabel == null) return;

            if (GuildName != "")
            {
                GuildLabel.Text = GuildName;
                GuildLabel.Location = new Point(DisplayRectangle.X + (50 - GuildLabel.Size.Width) / 2, DisplayRectangle.Y - (42 - GuildLabel.Size.Height / 2) + (Dead ? 35 : 8)); //was 48 -
                GuildLabel.Draw();
            }

            NameLabel.Text = Name;
            NameLabel.Location = new Point(DisplayRectangle.X + (50 - NameLabel.Size.Width) / 2, DisplayRectangle.Y - (32 - NameLabel.Size.Height / 2) + (Dead ? 35 : 8)); //was 48 -
            NameLabel.Draw();
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
