using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirScenes.Dialogs;
using SlimDX;

namespace Client.MirObjects
{
    public abstract class MapObject
    {
        public static Font ChatFont = new Font(Settings.FontName, 10F);
        public static List<MirLabel> LabelList = new List<MirLabel>();

        public static UserObject User;
        public static MapObject MouseObject, TargetObject, MagicObject;
        public abstract ObjectType Race { get; }
        public abstract bool Blocking { get; }

        public uint ObjectID;
        public string Name = string.Empty;
        public Point CurrentLocation, MapLocation;
        public MirDirection Direction;
        public bool Dead, Hidden, SitDown, Sneaking;
        public PoisonType Poison;
        public long DeadTime;
        public byte AI;
        public bool InTrapRock;
        public int JumpDistance;

        public bool Blend = true;

        public long BlindTime;
        public byte BlindCount;

        public byte PercentHealth;
        public long HealthTime;

        public List<QueuedAction> ActionFeed = new List<QueuedAction>();
        public QueuedAction NextAction
        {
            get { return ActionFeed.Count > 0 ? ActionFeed[0] : null; }
        }

        public List<Effect> Effects = new List<Effect>();
        public List<BuffType> Buffs = new List<BuffType>();

        public MLibrary BodyLibrary;
        public Color DrawColour = Color.White, NameColour = Color.White, LightColour = Color.White, OutLine_Colour = Color.Black;
        public MirLabel NameLabel, ChatLabel, GuildLabel, RebornLabel, InstanceStageLabel, ChallengeStageLabel, BossLabel, BossNameLabel, SubLabel, SubNameLabel;
        public long ChatTime;
        public int DrawFrame, DrawWingFrame;
        public Point DrawLocation, Movement, FinalDrawLocation, OffSetMove;
        public Rectangle DisplayRectangle;
        public int Light, DrawY;
        public long NextMotion, NextMotion2;
        public MirAction CurrentAction;
        public byte CurrentActionLevel;
        public bool SkipFrames;
        public FrameLoop FrameLoop = null;

        //Sound
        public int StruckWeapon;

        public MirLabel TempLabel;

        public static List<MirLabel> DamageLabelList = new List<MirLabel>();
        public List<Damage> Damages = new List<Damage>();

        protected Point GlobalDisplayLocationOffset
        {
            get { return new Point(0, 0); }
        }

        protected MapObject(uint objectID)
        {
            ObjectID = objectID;

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != ObjectID) continue;
                ob.Remove();
            }

            MapControl.Objects.Add(this);
        }
        public void Remove()
        {
            if (MouseObject == this) MouseObject = null;
            if (TargetObject == this) TargetObject = null;
            if (MagicObject == this) MagicObject = null;

            if (this == User.NextMagicObject)
                User.ClearMagic();

            MapControl.Objects.Remove(this);
            GameScene.Scene.MapControl.RemoveObject(this);

            if (ObjectID != GameScene.NPCID) return;

            GameScene.NPCID = 0;
            GameScene.Scene.NPCDialog.Hide();
        }

        public abstract void Process();
        public abstract void Draw();
        public abstract bool MouseOver(Point p);

        public void AddBuffEffect(BuffType type)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (!(Effects[i] is BuffEffect)) continue;
                if (((BuffEffect)(Effects[i])).BuffType == type) return;
            }

            PlayerObject ob = null;

            if (Race == ObjectType.Player)
            {
                ob = (PlayerObject)this;
            }

            switch (type)
            {
                case BuffType.Fury:
                    Effects.Add(new BuffEffect(Libraries.Magic3, 190, 7, 1400, this, true, type) { Repeat = true });
                    break;
                case BuffType.ImmortalSkin:
                    Effects.Add(new BuffEffect(Libraries.Magic3, 570, 5, 1400, this, true, type) { Repeat = true });
                    break;
                case BuffType.SwiftFeet:
                    if (ob != null) ob.Sprint = true;
                    break;
                case BuffType.MoonLight:
                case BuffType.DarkBody:
                    if (ob != null) ob.Sneaking = true;
                    break;
                case BuffType.VampireShot:
                    Effects.Add(new BuffEffect(Libraries.Magic3, 2110, 6, 1400, this, true, type) { Repeat = false });
                    break;
                case BuffType.PoisonShot:
                    Effects.Add(new BuffEffect(Libraries.Magic3, 2310, 7, 1400, this, true, type) { Repeat = false });
                    break;
                case BuffType.EnergyShield:
                    BuffEffect effect;

                    Effects.Add(effect = new BuffEffect(Libraries.Magic2, 1880, 9, 900, this, true, type) { Repeat = false });
                    SoundManager.PlaySound(20000 + (ushort)Spell.EnergyShield * 10 + 0);

                    effect.Complete += (o, e) =>
                    {
                        Effects.Add(new BuffEffect(Libraries.Magic2, 1900, 2, 800, this, true, type) { Repeat = true });
                    };
                    break;
                case BuffType.MagicBooster:
					Effects.Add(new BuffEffect(Libraries.Magic3, 90, 6, 1200, this, true, type) { Repeat = true });
                    break;
                case BuffType.PetEnhancer:
                    Effects.Add(new BuffEffect(Libraries.Magic3, 230, 6, 1200, this, true, type) { Repeat = true });
                    break;
				case BuffType.GameMaster:
					Effects.Add(new BuffEffect(Libraries.CHumEffect[5], 0, 1, 1200, this, true, type) { Repeat = true });
					break;
                case BuffType.GeneralMeowMeowShield:
                    Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.GeneralMeowMeow], 529, 7, 700, this, true, type) { Repeat = true, Light = 1 });
                    MirSounds.SoundManager.PlaySound(8322);
                    break;
                case BuffType.PowerBeadBuff:
                    Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.PowerUpBead], 64, 6, 600, this, true, type) { Blend = true, Repeat = true });
                    break;
                case BuffType.HornedArcherBuff:
                    Effects.Add(effect = new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedArcher], 468, 6, 600, this, true, type) { Repeat = false });
                    effect.Complete += (o, e) =>
                    {
                        Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedArcher], 474, 3, 1000, this, true, type) { Blend = true, Repeat = true });
                    };
                    break;
                case BuffType.ColdArcherBuff:
                    Effects.Add(effect = new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedArcher], 477, 7, 700, this, true, type) { Repeat = false });
                    effect.Complete += (o, e) =>
                    {
                        Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedArcher], 484, 3, 1000, this, true, type) { Blend = true, Repeat = true });
                    };
                    break;
                case BuffType.HornedWarriorShield:
                    Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedWarrior], 912, 18, 1800, this, true, type) { Repeat = true });
                    break;
                case BuffType.HornedCommanderShield:
                    Effects.Add(effect = new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedCommander], 1173, 1, 100, this, true, type) { Repeat = false, Light = 1 });
                    effect.Complete += (o, e) =>
                    {
                        Effects.Add(new BuffEffect(Libraries.Monsters[(ushort)Monster.HornedCommander], 1174, 16, 1600, this, true, type) { Repeat = true, Light = 1 });
                    };
                    break;
            }
        }
        public void RemoveBuffEffect(BuffType type)
        {
            PlayerObject ob = null;

            if (Race == ObjectType.Player)
            {
                ob = (PlayerObject)this;
            }

            for (int i = 0; i < Effects.Count; i++)
            {
                if (!(Effects[i] is BuffEffect)) continue;
                if (((BuffEffect)(Effects[i])).BuffType != type) continue;
                Effects[i].Repeat = false;
            }

            switch (type)
            {
                case BuffType.SwiftFeet:
                    if (ob != null) ob.Sprint = false;
                    break;
                case BuffType.MoonLight:
                case BuffType.DarkBody:
                    if (ob != null) ob.Sneaking = false;
                    break;
            }
        }

        public virtual Missile CreateProjectile(int baseIndex, MLibrary library, bool blend, int count, int interval, int skip, int lightDistance = 6, bool direction16 = true, Color? lightColour = null, uint targetID = 0)
        {
            return null;
        }

        public void Chat(string text)
        {
            if (ChatLabel != null && !ChatLabel.IsDisposed)
            {
                ChatLabel.Dispose();
                ChatLabel = null;
            }

            const int chatWidth = 200;
            List<string> chat = new List<string>();

            int index = 0;
            for (int i = 1; i < text.Length; i++)
                if (TextRenderer.MeasureText(CMain.Graphics, text.Substring(index, i - index), ChatFont).Width > chatWidth)
                {
                    chat.Add(text.Substring(index, i - index - 1));
                    index = i - 1;
                }
            chat.Add(text.Substring(index, text.Length - index));

            text = chat[0];
            for (int i = 1; i < chat.Count; i++)
                text += string.Format("\n{0}", chat[i]);

            ChatLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = Color.White,
                OutLine = true,
                OutLineColour = Color.Black,
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Text = text,
            };
            ChatTime = CMain.Time + 5000;
        }
        public virtual void DrawChat()
        {
            if (ChatLabel == null || ChatLabel.IsDisposed) return;

            if (CMain.Time > ChatTime)
            {
                ChatLabel.Dispose();
                ChatLabel = null;
                return;
            }

            ChatLabel.ForeColour = Dead ? Color.Gray : Color.White;
            ChatLabel.Location = new Point(DisplayRectangle.X + (48 - ChatLabel.Size.Width) / 2, DisplayRectangle.Y - (60 + ChatLabel.Size.Height) - (Dead ? 35 : 0));
            ChatLabel.Draw();
        }

        public virtual void CreateLabel()
        {
            NameLabel = null;
            MonsterObject tmp = null;
            if (Race == ObjectType.Monster)
            {
                tmp = (MonsterObject)this;
                if (!tmp.IsBoss)
                    tmp = null;

            }
            bool created = false;
            for (int i = 0; i < LabelList.Count; i++)
            {
                if (tmp != null)
                {
                    //  Elite Boss
                    if (tmp.IsElite)
                    {
                        //  Not the same name
                        if (LabelList[i].Text != Name ||
                            //  Text color is not white
                            LabelList[i].ForeColour != Color.White ||
                            //  Outline color is not the same as Name color
                            LabelList[i].OutLineColour != NameColour)
                            //  Discontinue
                            continue;
                        //  It's the same (I.E Created)
                        NameLabel = LabelList[i];
                    }
                    //  None Elite Boss
                    else
                    {
                        //  Not the same name
                        if (LabelList[i].Text != Name ||
                            //  Text color is not white
                            LabelList[i].ForeColour != Color.White ||
                            //  Outline color is not Red
                            LabelList[i].OutLineColour != Color.Red)
                            //  Discontinue
                            continue;
                        //  It's the same (I.E Created)
                        NameLabel = LabelList[i];
                    }
                }
                //  It wont be a boss label
                else
                {
                    if (LabelList[i].Text != Name ||
                        LabelList[i].ForeColour != NameColour) continue;
                    //  It's the same (I.E Created)
                    NameLabel = LabelList[i];
                }

                if (LabelList[i].Text == Name)
                    created = true;
            }


            if (created)
                return;
            //  Label isn't null
            if (NameLabel != null &&
                //  Label isn't disposed
                !NameLabel.IsDisposed)
                //  Already created, don't need to create any more!
                return;

            //  The label is valid now check if it by object type and their values
            if (tmp != null)
            {
                //  It is a boss
                if (tmp.IsBoss &&
                    !tmp.IsSub &&
                    !tmp.IsPet)
                {
                    //  Elite Boss
                    if (tmp.IsElite)
                    {
                        NameLabel = new MirLabel
                        {
                            AutoSize = true,
                            BackColour = Color.Transparent,
                            ForeColour = Color.White,
                            OutLine = true,
                            OutLineColour = tmp.NameColour,
                            Text = Name,
                        };
                    }
                    //  Boss Non Elite
                    else
                    {
                        NameLabel = new MirLabel
                        {
                            AutoSize = true,
                            BackColour = Color.Transparent,
                            ForeColour = Color.White,
                            OutLine = true,
                            OutLineColour = Color.Red,
                            Text = Name,
                        };
                    }
                }
                //  Mob
                else
                {
                    NameLabel = new MirLabel
                    {
                        AutoSize = true,
                        BackColour = Color.Transparent,
                        ForeColour = NameColour,
                        OutLine = true,
                        OutLineColour = Color.Black,
                        Text = Name,
                    };
                }
            }
            //  Other
            else
            {
                NameLabel = new MirLabel
                {
                    AutoSize = true,
                    BackColour = Color.Transparent,
                    ForeColour = NameColour,
                    OutLine = true,
                    OutLineColour = OutLine_Colour,
                    Text = Name,
                };
            }
            NameLabel.Disposing += (o, e) => LabelList.Remove(NameLabel);
            LabelList.Add(NameLabel);
        }
        public virtual void DrawName()
        {
            CreateLabel();

            if (NameLabel == null) return;
            
            NameLabel.Text = Name;
            NameLabel.Location = new Point(DisplayRectangle.X + (50 - NameLabel.Size.Width) / 2, DisplayRectangle.Y - (32 - NameLabel.Size.Height / 2) + (Dead ? 35 : 8)); //was 48 -
            NameLabel.Draw();
        }
        public virtual void DrawBlend()
        {
            DXManager.SetBlend(true, 0.3F); //0.8
            Draw();
            DXManager.SetBlend(false);
        }
        public void DrawDamages()
        {
            for (int i = Damages.Count - 1; i >= 0; i--)
            {
                Damage info = Damages[i];
                if (CMain.Time > info.ExpireTime)
                {
                    if (info.DamageLabel != null)
                    {
                        info.DamageLabel.Dispose();
                    }

                    Damages.RemoveAt(i);
                }
                else
                {
                    info.Draw(DisplayRectangle.Location);
                }
            }
        }

        public void DrawBossName()
        {
            //  Create the Label
            CreateBossLabel();
            //  If the label wasn't created we won't draw it
            if (BossLabel == null)
                return;

            //  Apply Text to the Label
            BossLabel.Text = string.Format("{0} - Boss -{1}%", Name, PercentHealth);
            //  Set the location of the Label
            BossLabel.Location = new Point(Settings.ScreenWidth / 2 - BossLabel.Size.Width / 2, 82); //was 53 -
                                                                                                     //  Draw the Label
            BossLabel.Draw();
        }

        public void DrawSubName()
        {
            //  Create the Label
            CreateSubLabel();
            //  If the label wasn't created we won't draw it
            if (SubLabel == null)
                return;

            //  Apply Text to the Label
            SubLabel.Text = string.Format("{0} - Sub - {1}%", Name, PercentHealth);
            //  Set the location of the Label
            SubLabel.Location = new Point(Settings.ScreenWidth / 2 - SubLabel.Size.Width / 2, 82); //was 53 -
                                                                                                   //  Draw the Label
            SubLabel.Draw();
        }

        public void CreateBossLabel()
        {
            BossLabel = null;

            for (int i = 0; i < LabelList.Count; i++)
            {
                //  Not the same name
                if (LabelList[i].Text.Contains(string.Format("{0}", Name)) ||
                    //  Not the same name color
                    LabelList[i].ForeColour != NameColour)
                    continue;
                BossLabel = LabelList[i];
                break;
            }


            if (BossLabel != null && !BossLabel.IsDisposed)
                return;

            BossLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = NameColour,
                OutLine = true,
                OutLineColour = Color.Black,
                Text = string.Format("{0} - {1}%", Name, PercentHealth),
            };
            BossLabel.Disposing += (o, e) => LabelList.Remove(BossLabel);
            LabelList.Add(BossLabel);
        }

        public void CreateSubLabel()
        {
            SubLabel = null;

            for (int i = 0; i < LabelList.Count; i++)
            {
                //  Not the same name
                if (LabelList[i].Text.Contains(string.Format("{0}", Name)) ||
                    //  Not the same name color
                    LabelList[i].ForeColour != NameColour)
                    continue;
                SubLabel = LabelList[i];
                break;
            }


            if (SubLabel != null && !SubLabel.IsDisposed)
                return;

            SubLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = NameColour,
                OutLine = true,
                OutLineColour = Color.Black,
                Text = string.Format("{0} - {1}%", Name, PercentHealth),
            };
            SubLabel.Disposing += (o, e) => LabelList.Remove(SubLabel);
            LabelList.Add(SubLabel);
        }

        public void DrawBossHealthBar()
        {

            string name = Name;
            if (Name.Contains("("))
                name = Name.Substring(Name.IndexOf("(") + 1, Name.Length - Name.IndexOf("(") - 2);

            //  Object is dead, don't draw
            if (Dead)
                return;
            //  Object isn't a monster!
            if (Race != ObjectType.Monster)
                return;

            //  Check the Rev time (may not even need this)
            if (CMain.Time >= HealthTime)
            {
                if (Race == ObjectType.Monster)
                {
                    MonsterObject temp = (MonsterObject)this;
                    if (!temp.IsBoss && !temp.IsSub)
                        return;

                }
            }
            //  Draw the base of the Image (I.E Empty health bar)
            Libraries.Prguse.Draw(2552, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 158, 33 + 10), Color.White, false);
            //  Draw the Health ontop of the base and shirnk it based on the Health of the object
            Libraries.Prguse.Draw(2553, new Rectangle(0, 0, (int)(231 * PercentHealth / 100F), 12), new Point(Settings.ScreenWidth / 2 - 83, 74 + 10), Color.White, false);
            //  Draw the Player Heath ontop of the base and shirnk it based on the Health of the object
            Libraries.Prguse.Draw(2554, new Rectangle(0, 0, (int)(231 * User.PercentHealth / 100F), 12), new Point(Settings.ScreenWidth / 2 - 83, 92 + 10), Color.White, false);
            //  Now Draw the bosses name ontop of the Health Bar
            DrawBossName();
        }

        public void DrawSubHealthBar()
        {

            string name = Name;
            if (Name.Contains("("))
                name = Name.Substring(Name.IndexOf("(") + 1, Name.Length - Name.IndexOf("(") - 2);

            //  Object is dead, don't draw
            if (Dead)
                return;
            //  Object isn't a monster!
            if (Race != ObjectType.Monster)
                return;

            //  Check the Rev time (may not even need this)
            if (CMain.Time >= HealthTime)
            {
                if (Race == ObjectType.Monster)
                {
                    MonsterObject temp = (MonsterObject)this;
                    if (!temp.IsBoss && !temp.IsSub)
                        return;
                }
            }

            //  Draw the base of the Image (I.E Empty health bar)
            Libraries.Prguse.Draw(2564, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 158, 33 + 10), Color.White, false);
            //  Draw the Health ontop of the base and shirnk it based on the Health of the object
            Libraries.Prguse.Draw(2553, new Rectangle(0, 0, (int)(231 * PercentHealth / 100F), 12), new Point(Settings.ScreenWidth / 2 - 83, 74 + 10), Color.White, false);
            //  Draw the Player Heath ontop of the base and shirnk it based on the Health of the object
            Libraries.Prguse.Draw(2554, new Rectangle(0, 0, (int)(231 * User.PercentHealth / 100F), 12), new Point(Settings.ScreenWidth / 2 - 83, 92 + 10), Color.White, false);
            //  Now Draw the bosses name ontop of the Health Bar
            DrawSubName();
        }

        public void DrawBossPoison()
        {
            //  Object is dead, don't draw
            if (Dead)
                return;
            //  Object isn't a monster!
            if (Race != ObjectType.Monster)
                return;

            if (Poison != PoisonType.None)
            {
                if (Poison.HasFlag(PoisonType.Red))
                {
                    Libraries.Prguse.Draw(2555, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 84, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Green))
                {
                    Libraries.Prguse.Draw(2556, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 66, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Stun))
                {
                    Libraries.Prguse.Draw(2557, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 48, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Slow))
                {
                    Libraries.Prguse.Draw(2558, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 30, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Frozen))
                {
                    Libraries.Prguse.Draw(2559, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - 12, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.DelayedExplosion))
                {
                    Libraries.Prguse.Draw(2560, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - -6, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Paralysis) || Poison.HasFlag(PoisonType.LRParalysis))
                {
                    Libraries.Prguse.Draw(2561, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - -24, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Bleeding))
                {
                    Libraries.Prguse.Draw(2562, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - -42, 102 + 10), Color.White, false);
                }
                if (Poison.HasFlag(PoisonType.Blindness))
                {
                    Libraries.Prguse.Draw(2560, new Rectangle(0, 0, 324, 97), new Point(Settings.ScreenWidth / 2 - -60, 102 + 10), Color.White, false);
                }
            }
        }

        public void DrawBossImage()
        {
            //  Object is dead, don't draw
            if (Dead)
                return;
            //  Object isn't a monster!
            if (Race != ObjectType.Monster)
                return;

            switch (AI) // if you use any of these mob ingame check isboss in db
            {
                case 11: //Wt
                    Libraries.MobImage.Draw(187, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 13: //RME
                    Libraries.MobImage.Draw(230, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 14: //EC
                    Libraries.MobImage.Draw(202, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 16: //RTZ
                    Libraries.MobImage.Draw(221, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 17: //Zt
                case 22: //IncarnatedZT
                    Libraries.MobImage.Draw(219, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 19: //KingScorpion
                    Libraries.MobImage.Draw(290, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 20: //DarkDevil
                    Libraries.MobImage.Draw(150, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 21: //IncarnatedGhoul
                    Libraries.MobImage.Draw(192, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 27: //Khazard
                    Libraries.MobImage.Draw(208, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 30: //BoneLord
                    Libraries.MobImage.Draw(250, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 33: //MinotaurKing
                    Libraries.MobImage.Draw(156, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(31, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 34: //FrostTiger - FlameTiger
                    Libraries.MobImage.Draw(298, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 36: //Yimoogi
                    Libraries.MobImage.Draw(284, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 37: //CrystalSpider
                    Libraries.MobImage.Draw(238, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 43: //OmaKing - Oks
                    Libraries.MobImage.Draw(282, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 50: //GreatFoxSpirit
                    Libraries.MobImage.Draw(271, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 73: //TurtleKing
                    Libraries.MobImage.Draw(318, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 79: //HellKeeper
                    Libraries.MobImage.Draw(307, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 106: //OrcMutant
                    Libraries.MobImage.Draw(344, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 107: //OrcGeneral
                    Libraries.MobImage.Draw(345, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 102: //WOLFKING
                    Libraries.MobImage.Draw(157, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 114: //AncDarkDevil
                    Libraries.MobImage.Draw(150, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 115: //AncKingScorpion
                    Libraries.MobImage.Draw(290, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 127: //Wg
                    Libraries.MobImage.Draw(184, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 237: //IDarkDevil
                    Libraries.MobImage.Draw(150, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 238: //IMinotaurKing
                    Libraries.MobImage.Draw(156, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(31, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;

            }
        }

        public void DrawSubImage()
        {
            //  Object is dead, don't draw
            if (Dead)
                return;
            //  Object isn't a monster!
            if (Race != ObjectType.Monster)
                return;

            switch (AI)
            {
                case 16: //RTZ
                    Libraries.MobImage.Draw(221, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 22: //IncarnatedZT
                    Libraries.MobImage.Draw(219, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 19: //KingScorpion
                    Libraries.MobImage.Draw(290, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 21: //IncarnatedGhoul
                    Libraries.MobImage.Draw(192, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 106: //OrcMutant
                    Libraries.MobImage.Draw(344, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 115: //AncKingScorpion
                    Libraries.MobImage.Draw(290, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 127: //Wg
                    Libraries.MobImage.Draw(184, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
                case 240: //Wb
                    Libraries.MobImage.Draw(213, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    Libraries.Prguse.Draw(2551, new Rectangle(0, 0, 550, 78), new Point(Settings.ScreenWidth / 2 - 132, 68 + 10), Color.White, false);
                    break;
            }
        }
        public void DrawHealth()
        {
            string name = Name;

            if (Name.Contains("(")) name = Name.Substring(Name.IndexOf("(") + 1, Name.Length - Name.IndexOf("(") - 2);

            if (Dead) return;
            if (Race != ObjectType.Player && Race != ObjectType.Monster) return;

            if (CMain.Time >= HealthTime)
            {
                if (Race == ObjectType.Monster && !Name.EndsWith(string.Format("({0})", User.Name)) && !GroupDialog.GroupList.Contains(name)) return;
                if (Race == ObjectType.Player && this != User && !GroupDialog.GroupList.Contains(Name)) return;
                if (this == User && GroupDialog.GroupList.Count == 0) return;
            }


            Libraries.Prguse2.Draw(0, DisplayRectangle.X + 8, DisplayRectangle.Y - 64);
            int index = 1;

            switch (Race)
            {
                case ObjectType.Player:
                    if (GroupDialog.GroupList.Contains(name)) index = 10;
                    break;
                case ObjectType.Monster:
                    if (GroupDialog.GroupList.Contains(name) || name == User.Name) index = 11;
                    break;
            }

            Libraries.Prguse2.Draw(index, new Rectangle(0, 0, (int)(32 * PercentHealth / 100F), 4), new Point(DisplayRectangle.X + 8, DisplayRectangle.Y - 64), Color.White, false);
        }

        public void DrawPoison()
        {
            byte poisoncount = 0;
            if (Poison != PoisonType.None)
            {
                if (Poison.HasFlag(PoisonType.Green))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Green);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Red))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Red);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Bleeding))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.DarkRed);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Slow))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Purple);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Stun) || Poison.HasFlag(PoisonType.Dazed))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Yellow);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Blindness))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.MediumVioletRed);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Frozen))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Blue);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.Paralysis) || Poison.HasFlag(PoisonType.LRParalysis))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Gray);
                    poisoncount++;
                }
                if (Poison.HasFlag(PoisonType.DelayedExplosion))
                {
                    DXManager.Sprite.Draw(DXManager.PoisonDotBackground, new Rectangle(0, 0, 6, 6), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 7 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 21), 0.0F), Color.Black);
                    DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 4, 4), Vector3.Zero, new Vector3((float)(DisplayRectangle.X + 8 + (poisoncount * 5)), (float)(DisplayRectangle.Y - 20), 0.0F), Color.Orange);
                    poisoncount++;
                }
            }
        }

        public abstract void DrawBehindEffects(bool effectsEnabled);

        public abstract void DrawEffects(bool effectsEnabled);

        protected void LoopFrame(int start, int frameCount, int frameInterval, int duration)
        {
            if (FrameLoop == null)
            {
                FrameLoop = new FrameLoop
                {
                    Start = start,
                    End = start + frameCount - 1,
                    Loops = (duration / (frameInterval * frameCount)) - 1 //Remove 1 count as we've already done a loop before this is checked
                };
            }
        }
    }

    public class FrameLoop
    {
        public MirAction Action { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Loops { get; set; }

        public int CurrentCount { get; set; }
    }

}
