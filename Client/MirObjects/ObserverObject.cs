using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirNetwork;
using Client.MirScenes;
using S = ServerPackets;
using C = ClientPackets;
using System.IO;
using System.Diagnostics;

namespace Client.MirObjects
{

    public class ObserverObject : MapObject, ICamera
    {
        public string Name { get; set; }
        public bool LockedOn { get { return LockedID == 0 ? false : true; }}
        public QueuedAction QueuedAction;
        public uint LockedID;

        public ObserverObject(uint objectID) : base(objectID)
        {
            Frames = FrameSet.Players;
        }

        public void RequestLock(uint objID)
        {
            Network.Enqueue(new C.ObserveLock { ObjectID = objID });
        }

        public void SetCamera(uint objID)
        {
            bool NewLock = false;

            if (LockedID != objID)
            {
                LockedID = objID;
                NewLock = true;
            }

            if (LockedOn && !NewLock)
            {
                MapObject ob;

                for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
                {
                    ob = MapControl.Objects[i];
                    if (ob.ObjectID != LockedID) continue;

                    GameScene.Scene.InspectObserve.Visible = (ob.Race == ObjectType.Player); //Show Inspect button if Locked is player
                    GameScene.Scene.StatusObserve.Index = 814; //Set button to show as Locked
                    Light = 0; //Set light to zero so we use the observees light

                    GameScene.Camera = ob as ICamera;
                    return;
                }

                RequestLock(0);
            }
            else if (!LockedOn)
            {
                CurrentLocation = GameScene.Camera.CurrentLocation;
                Name = "Observer";
                Light = 100; //Set light so we can see ingame when in freemode

                GameScene.Scene.InspectObserve.Visible = false; //Hide Inspect button
                GameScene.Scene.StatusObserve.Index = 815; //Set button to show as Unlocked

                GameScene.Camera = this;
                SetLibraries();
            }
        }

        public virtual void SetLibraries()
        { 
            bool altAnim = false;

            bool showMount = true;
            bool showFishing = true;

                switch (CurrentAction)
                {
                    case MirAction.Standing:
                        Frames.Frames.TryGetValue(MirAction.Standing, out Frame);
                        break;
                    case MirAction.ObserveMove:
                        Frames.Frames.TryGetValue(MirAction.ObserveMove, out Frame);
                        break;
                }
        }

        public override ObjectType Race => ObjectType.Observer;
        public override bool Blocking => false;
        public override void Draw()
        {
            
        }
        public override void DrawBehindEffects(bool effectsEnabled)
        {
            
        }
        public override void DrawEffects(bool effectsEnabled)
        {
           
        }
        public override bool MouseOver(Point p)
        {
            return false;
        }

        public FrameSet Frames;
        public Frame Frame, WingFrame;
        public int FrameIndex = 0, FrameInterval = 0, EffectFrameIndex = 0, EffectFrameInterval = 0, SlowFrameIndex = 0;
        public byte SkipFrameUpdate = 0;

        public override void Process()
        {
            bool update = CMain.Time >= NextMotion || GameScene.CanMove;

            ProcessFrames();

            switch (CurrentAction)
            {
                case MirAction.ObserveMove:
                    if (Frame == null)
                    {
                        OffSetMove = Point.Empty;
                        Movement = CurrentLocation;
                        break;
                    }

                    var i = 3;

                    Movement = Functions.PointMove(CurrentLocation, Direction, -i);

                    int count = Frame.Count;
                    int index = FrameIndex;

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


            DrawY = Movement.Y > CurrentLocation.Y ? Movement.Y : CurrentLocation.Y;


           
            DrawLocation = new Point((Movement.X - (Camera.Movement.X) + MapControl.OffSetX) * MapControl.CellWidth, (Movement.Y - (Camera.Movement.Y) + MapControl.OffSetY) * MapControl.CellHeight);


            UpdateDrawLocationOffset(GlobalDisplayLocationOffset);

            if (BodyLibrary != null && update)
            {
                //FinalDrawLocation = DrawLocation.Add(BodyLibrary.GetOffSet(DrawFrame));
                //DisplayRectangle = new Rectangle(DrawLocation, BodyLibrary.GetTrueSize(DrawFrame));
            }

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

        public virtual void ProcessFrames()
        {
            bool clear = CMain.Time >= NextMotion;

            ProcFrames();

            if (clear) QueuedAction = null;
            if ((CurrentAction == MirAction.Standing) && (QueuedAction != null || NextAction != null))
                SetAction();
        }

        public virtual void ProcFrames()
        {

            if (Frame == null) return;

            switch (CurrentAction)
            {
                case MirAction.ObserveMove:
                    if (!GameScene.CanMove) return;

                    GameScene.Scene.MapControl.TextureValid = false;

                    GameScene.Scene.MapControl.FloorValid = false;

                    //if (CMain.Time < NextMotion) return;

                    if (SkipFrames) UpdateFrame();

                    if (UpdateFrame(false) >= Frame.Count)
                    {
                        FrameIndex = Frame.Count - 1;
                        SetAction();
                    }
                    else
                    {
                    }

                    break;
                case MirAction.Standing:
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
                    break;
            }

            if ((CurrentAction == MirAction.Standing || CurrentAction == MirAction.MountStanding || CurrentAction == MirAction.Stance || CurrentAction == MirAction.Stance2 || CurrentAction == MirAction.DashFail) && NextAction != null)
                SetAction();
            //if Revive and dead set action

        }

        public virtual void SetAction()
        {
            if (QueuedAction != null)
            {
                if ((ActionFeed.Count == 0) || (ActionFeed.Count == 1 && NextAction.Action == MirAction.Stance))
                {
                    ActionFeed.Clear();
                    ActionFeed.Add(QueuedAction);
                    QueuedAction = null;
                }
            }

            if (Observer == this && CMain.Time < MapControl.NextAction)// && CanSetAction)
            {
                //NextMagic = null;
                return;
            }

            if (ActionFeed.Count == 0)
            {
                CurrentAction = MirAction.Standing;

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

                CurrentLocation = action.Location;
                MirDirection olddirection = Direction;
                Direction = action.Direction;

                Point temp;
                switch (CurrentAction)
                {
                    case MirAction.ObserveMove:
                        var steps = 3;

                        temp = Functions.PointMove(CurrentLocation, Direction, -steps);

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

                Frames.Frames.TryGetValue(CurrentAction, out Frame);

                SetLibraries();

                FrameIndex = 0;
                EffectFrameIndex = 0;

                if (Frame == null) return;

                FrameInterval = Frame.Interval;
                EffectFrameInterval = Frame.EffectInterval;

                if (this == Observer)
                {
                    switch (CurrentAction)
                    {
                        case MirAction.ObserveMove:
                            Network.Enqueue(new C.ObserveMove { Direction = Direction });
                            GameScene.Scene.MapControl.FloorValid = false;
                            MapControl.NextAction = CMain.Time + 1000;
                            break;
                    }
                }


                switch (CurrentAction)
                {
                    case MirAction.ObserveMove:
                        GameScene.Scene.Redraw();
                        break;
                }

            }

            GameScene.Scene.MapControl.TextureValid = false;

            NextMotion = CMain.Time + FrameInterval;
            NextMotion2 = CMain.Time + EffectFrameInterval;

        }

    }
}