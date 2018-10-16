using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirNetwork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S = ServerPackets;
using C = ClientPackets;

/*
NOTES
Removing & Adding Objects when Locked on are loading for 3 x space movement.
Need to have access to chat / chat commands from Observer Scene - started
*/


namespace Server.MirObjects
{
    public sealed class ObserverObject : MapObject
    {
        public MirConnection Connection;
        public bool IsGM, GMLogin;
        public string GMPassword = Settings.GMPassword;
        public int CharIndex;

        public MapObject LockedTarget;
        public const long MoveDelay = 600, MovementDelay = 2000;
        public long ActionTime, MovementTime;

        public bool LockedOn {
            get { return LockedTarget == null ? false : true; }
        }

        public void Enqueue(Packet p)
        {
            if (Connection == null) return;
            Connection.Enqueue(p);
        }

        public ObserverObject(Point CurLoc, int CurMapIndex, Map CurMap, MirConnection Con, bool GM = false, uint ObjID = 0, int CharInd = 0)
        {
            Connection = Con;
            Connection.Stage = GameStage.Observing;

            PlayerObject player = null;

            if (ObjID != 0)
                player = (PlayerObject)Envir.GetObject(ObjID);

            if (player != null)
            {
                LockedTarget = player;

                CurrentLocation = player.CurrentLocation;
                CurrentMapIndex = player.CurrentMapIndex;
                CurrentMap = player.CurrentMap;
            }
            else
            {
                LockedTarget = null;

                CurrentLocation = CurLoc;
                CurrentMapIndex = CurMapIndex;
                CurrentMap = CurMap;
            }


            IsGM = GM;
            CharIndex = CharInd;

            Enqueue(new S.Observe { ObserveObjectID = ObjID });

            CurrentMap.GetCell(CurrentLocation).Add(this);

            Envir.Observers.Add(this);

            LocationChanged();

            if (player != null)
                player.CurrentObservers.Add(this);

        }

        public void LocationChanged()
        {
            Enqueue(new S.MapChanged
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                BigMap = CurrentMap.Info.BigMap,
                Lights = CurrentMap.Info.Light,
                Location = CurrentLocation,
                Direction = Direction,
                MapDarkLight = CurrentMap.Info.MapDarkLight,
                Music = CurrentMap.Info.Music
            });

            GetObjectsPassive();
        }

        public void ObserveUnlock()
        {
            LockedTarget.CurrentObservers.Remove(this);
            LockedTarget = null;
        }

        public void StopGame(byte reason)
        {
            if (LockedOn)
                LockedTarget.CurrentObservers.Remove(this);

            CurrentMap.RemoveObject(this);
            Envir.Observers.Remove(this);
            LockedTarget = null;
            Connection.Observer = null;
        }

        public void ObserveLock(uint ObjectID)
        {
            if (ObjectID == 0 && IsGM)
            {
                if (LockedOn)
                {
                    ObserveUnlock();
                }
            }
            else
            {
                PlayerObject player = (PlayerObject)Envir.GetObject(ObjectID);

                if (player == null)
                {
                    ReceiveChat(string.Format("Player {0} was not found.", ObjectID.ToString()), ChatType.System);
                    return;
                }
                else
                {
                    if (LockedTarget != null)
                        LockedTarget.CurrentObservers.Remove(this);

                    player.CurrentObservers.Add(this);
                    LockedTarget = player;

                    LocationChanged();
                }
            }

        }

        private void GetObjectsPassive()
        {
            MapObject player = this;
            PlayerObject mainPlayer;

            if (LockedOn && LockedTarget is PlayerObject)
                mainPlayer = (PlayerObject)LockedTarget;
            else
                mainPlayer = null;

            for (int y = player.CurrentLocation.Y - Globals.DataRange; y <= player.CurrentLocation.Y + Globals.DataRange; y++)
            {
                if (y < 0) continue;
                if (y >= player.CurrentMap.Height) break;

                for (int x = player.CurrentLocation.X - Globals.DataRange; x <= player.CurrentLocation.X + Globals.DataRange; x++)
                {
                    if (x < 0) continue;
                    if (x >= player.CurrentMap.Width) break;
                    if (x < 0 || x >= player.CurrentMap.Width) continue;

                    Cell cell = player.CurrentMap.GetCell(x, y);

                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (ob == player | ob.Race == ObjectType.Observer) continue;

                        if (ob.Race == ObjectType.Deco)
                        {
                            var tt = 0;

                            tt++;
                        }
                        if (ob.Race == ObjectType.Player)
                        {
                            PlayerObject Player = (PlayerObject)ob;
                            Enqueue(Player.GetInfoEx(mainPlayer));
                        }
                        else if (ob.Race == ObjectType.Spell)
                        {
                            SpellObject obSpell = (SpellObject)ob;
                            if ((obSpell.Spell != Spell.ExplosiveTrap) || (IsFriendlyTarget(obSpell.Caster)))
                                Enqueue(ob.GetInfo());
                        }
                        else if (ob.Race == ObjectType.Merchant)
                        {
                            NPCObject NPC = (NPCObject)ob;

                            if (mainPlayer != null)
                            {
                                NPC.CheckVisible(mainPlayer);

                                if (NPC.VisibleLog[mainPlayer.Info.Index] && NPC.Visible) Enqueue(ob.GetInfo());
                            }
                            else
                                if (NPC.Visible) Enqueue(ob.GetInfo());
                        }
                        else
                        {
                            Enqueue(ob.GetInfo());
                        }

                        //if (ob.Race == ObjectType.Player || ob.Race == ObjectType.Monster || ob == mainPlayer)
                            //ob.SendHealth(mainPlayer);
                    }
                }
            }
        }

        public void ObserveMove(MirDirection dir)
        {
            if (LockedOn)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }
           
            Point location = Functions.PointMove(CurrentLocation, dir, 3);

            if (!(location.X >= 0 && location.X < CurrentMap.Width && location.Y >= 0 && location.Y < CurrentMap.Height))
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            if (!CurrentMap.CheckDoorOpen(location))
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            Cell cell = CurrentMap.GetCell(location);

            Direction = dir;
            if (CheckMovement(location)) return;

            CurrentMap.GetCell(CurrentLocation).Remove(this);
            RemoveObjects(dir, 3);

            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, 3);

            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + MoveDelay;

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
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

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
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
                                ob.Add(this);
                            }
                        }
                    }
                    break;
            }
        }

        public bool CheckMovement(Point location)
        {
            if (Envir.Time < MovementTime) return false;


            //Map movements
            for (int i = 0; i < CurrentMap.Info.Movements.Count; i++)
            {
                MovementInfo info = CurrentMap.Info.Movements[i];

                if (info.Source != location) continue;

                if (info.NeedHole)
                {
                    Cell cell = CurrentMap.GetCell(location);

                    if (cell.Objects == null ||
                        cell.Objects.Where(ob => ob.Race == ObjectType.Spell).All(ob => ((SpellObject)ob).Spell != Spell.DigOutZombie))
                        continue;
                }

                Map temp = Envir.GetMap(info.MapIndex);

                if (temp == null || !temp.ValidPoint(info.Destination)) continue;

                CurrentMap.RemoveObject(this);
                Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

                CompleteMapMovement(temp, info.Destination, CurrentMap, CurrentLocation);

                return true;
            }
            return false;
        }

        private void CompleteMapMovement(params object[] data)
        {
            if (this == null) return;
            Map temp = (Map)data[0];
            Point destination = (Point)data[1];
            Map checkmap = (Map)data[2];
            Point checklocation = (Point)data[3];

            if (CurrentMap != checkmap || CurrentLocation != checklocation) return;

            bool mapChanged = temp != CurrentMap;

            CurrentMap = temp;
            CurrentLocation = destination;

            CurrentMap.AddObject(this);

            MovementTime = Envir.Time + MovementDelay;

            Enqueue(new S.MapChanged
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                BigMap = CurrentMap.Info.BigMap,
                Lights = CurrentMap.Info.Light,
                Location = CurrentLocation,
                Direction = Direction,
                MapDarkLight = CurrentMap.Info.MapDarkLight,
                Music = CurrentMap.Info.Music
            });
        }

        public bool LockedProcess()
        {
            if (LockedTarget != null)
            {
                if (CurrentLocation != LockedTarget.CurrentLocation)
                {
                    if (LockedTarget.CurrentMap != CurrentMap || !Functions.InRange(LockedTarget.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        CurrentMap.GetCell(CurrentLocation).Remove(this);

                        CurrentMapIndex = LockedTarget.CurrentMapIndex;
                        CurrentMap = LockedTarget.CurrentMap;
                        CurrentLocation = LockedTarget.CurrentLocation;

                        CurrentMap.GetCell(CurrentLocation).Add(this);

                        LocationChanged();
                    }
                    else
                    {
                        CurrentMap.GetCell(CurrentLocation).Remove(this);
                        RemoveObjects(LockedTarget.Direction, 3);

                        CurrentLocation = LockedTarget.CurrentLocation;
                        CurrentMap.GetCell(CurrentLocation).Add(this);
                        AddObjects(LockedTarget.Direction, 3);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ObserverEnd()
        {
            if (CharIndex == 0)
            {
                Enqueue(new S.EndObserving { });
                StopGame(24);
            }
            else
            {
                if (IsGM & LockedOn)
                {
                    ObserveUnlock();
                }
                else
                {
                    C.StartGame packet = new C.StartGame { CharacterIndex = CharIndex };
                    Connection.StartGame(packet);
                    StopGame(24);
                }
            }
        }

        public override void ReceiveChat(string text, ChatType type)
        {
            Enqueue(new S.Chat { Message = text, Type = type });
        }

        public void Chat(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            uint count = 1;

            SMain.EnqueueChat(string.Format("{0}: {1}", Name, message));

            if (GMLogin)
            {
                if (message == GMPassword)
                {
                    IsGM = true;
                    SMain.Enqueue(string.Format("{0} is now a GM", Name));
                    ReceiveChat("You have been made a GM", ChatType.System);
                }
                else
                {
                    SMain.Enqueue(string.Format("{0} attempted a GM login", Name));
                    ReceiveChat("Incorrect login password", ChatType.System);
                }
                GMLogin = false;
                return;
            }

            //if (Info.ChatBanned)
            //{
            //    if (Info.ChatBanExpiryDate > DateTime.Now)
            //    {
            //        ReceiveChat("You are currently banned from chatting.", ChatType.System);
            //        return;
            //    }
            //
            //    Info.ChatBanned = false;
            //}
            //else
            //{
            //    if (ChatTime > Envir.Time)
            //    {
            //        if (ChatTick >= 5 & !IsGM)
            //        {
            //            Info.ChatBanned = true;
            //            Info.ChatBanExpiryDate = DateTime.Now.AddMinutes(5);
            //            ReceiveChat("You have been banned from chatting for 5 minutes.", ChatType.System);
            //            return;
            //        }
            //
            //        ChatTick++;
            //    }
            //    else
            //        ChatTick = 0;
            //
            //    ChatTime = Envir.Time + 2000;
            //}

            string[] parts;

            message = message.Replace("$pos", Functions.PointToString(CurrentLocation));


            Packet p;
            if (message.StartsWith("/"))
            {
                ////Private Message
                //message = message.Remove(0, 1);
                //parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //
                //if (parts.Length == 0) return;
                //
                //PlayerObject player = Envir.GetPlayer(parts[0]);
                //
                //if (player == null)
                //{
                //    IntelligentCreatureObject creature = GetCreatureByName(parts[0]);
                //    if (creature != null)
                //    {
                //        creature.ReceiveChat(message.Remove(0, parts[0].Length), ChatType.WhisperIn);
                //        return;
                //    }
                //    ReceiveChat(string.Format("Could not find {0}.", parts[0]), ChatType.System);
                //    return;
                //}
                //
                //if (player.Info.Friends.Any(e => e.Info == Info && e.Blocked))
                //{
                //    ReceiveChat("Player is not accepting your messages.", ChatType.System);
                //    return;
                //}
                //
                //if (Info.Friends.Any(e => e.Info == player.Info && e.Blocked))
                //{
                //    ReceiveChat("Cannot message player whilst they are on your blacklist.", ChatType.System);
                //    return;
                //}
                //
                //ReceiveChat(string.Format("/{0}", message), ChatType.WhisperOut);
                //player.ReceiveChat(string.Format("{0}=>{1}", Name, message.Remove(0, parts[0].Length)), ChatType.WhisperIn);
            }
            else if (message.StartsWith("!!"))
            {
                if (GroupMembers == null) return;
                //Group
                message = String.Format("{0}:{1}", Name, message.Remove(0, 2));

                p = new S.ObjectChat { ObjectID = ObjectID, Text = message, Type = ChatType.Group };

                for (int i = 0; i < GroupMembers.Count; i++)
                    GroupMembers[i].Enqueue(p);
            }
            else if (message.StartsWith("!~"))
            {
                //if (MyGuild == null) return;
                //
                ////Guild
                //message = message.Remove(0, 2);
                //MyGuild.SendMessage(String.Format("{0}: {1}", Name, message));

            }
            else if (message.StartsWith("!#"))
            {
                ////Mentor Message
                //message = message.Remove(0, 2);
                //parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //
                //if (parts.Length == 0) return;
                //
                //if (Info.Mentor == 0) return;
                //
                //CharacterInfo Mentor = Envir.GetCharacterInfo(Info.Mentor);
                //PlayerObject player = Envir.GetPlayer(Mentor.Name);
                //
                //if (player == null)
                //{
                //    ReceiveChat(string.Format("{0} isn't online.", Mentor.Name), ChatType.System);
                //    return;
                //}
                //
                //ReceiveChat(string.Format("{0}: {1}", Name, message), ChatType.Mentor);
                //player.ReceiveChat(string.Format("{0}: {1}", Name, message), ChatType.Mentor);
            }
            else if (message.StartsWith("!"))
            {
                return;

            }
            else if (message.StartsWith(":)"))
            {
                // //Relationship Message
                // message = message.Remove(0, 2);
                // parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //
                // if (parts.Length == 0) return;
                //
                // if (Info.Married == 0) return;
                //
                // CharacterInfo Lover = Envir.GetCharacterInfo(Info.Married);
                // PlayerObject player = Envir.GetPlayer(Lover.Name);
                //
                // if (player == null)
                // {
                //     ReceiveChat(string.Format("{0} isn't online.", Lover.Name), ChatType.System);
                //     return;
                // }
                //
                // ReceiveChat(string.Format("{0}: {1}", Name, message), ChatType.Relationship);
                // player.ReceiveChat(string.Format("{0}: {1}", Name, message), ChatType.Relationship);
            }
            else if (message.StartsWith("@!"))
            {
                if (!IsGM) return;

                message = String.Format("(*){0}:{1}", Name, message.Remove(0, 2));

                p = new S.Chat { Message = message, Type = ChatType.Announcement };

                Envir.Broadcast(p);
            }
            else if (message.StartsWith("@"))
            {

                //Command
                message = message.Remove(0, 1);
                parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) return;

                PlayerObject player;
                CharacterInfo data;
                String hintstring;
                UserItem item;

                switch (parts[0].ToUpper())
                {
                    case "LOGIN":
                        GMLogin = true;
                        ReceiveChat("Please type the GM Password", ChatType.Hint);
                        return;

                    case "KILL":
                        if (!IsGM) return;

                        if (parts.Length >= 2)
                        {
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Could not find {0}", parts[0]), ChatType.System);
                                return;
                            }
                            if (!player.GMNeverDie) player.Die();
                        }
                        else
                        {
                            if (!CurrentMap.ValidPoint(Front)) return;

                            Cell cell = CurrentMap.GetCell(Front);

                            if (cell == null || cell.Objects == null) return;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];

                                switch (ob.Race)
                                {
                                    case ObjectType.Player:
                                    case ObjectType.Monster:
                                        if (ob.Dead) continue;
                                        ob.EXPOwner = this;
                                        ob.ExpireTime = Envir.Time + MonsterObject.EXPOwnerDelay;
                                        ob.Die();
                                        break;
                                    default:
                                        continue;
                                }
                            }
                        }
                        return;

                    case "RESTORE":
                        if (!IsGM || parts.Length < 2) return;

                        data = Envir.GetCharacterInfo(parts[1]);

                        if (data == null)
                        {
                            ReceiveChat(string.Format("Player {0} was not found", parts[1]), ChatType.System);
                            return;
                        }

                        if (!data.Deleted) return;
                        data.Deleted = false;

                        ReceiveChat(string.Format("Player {0} has been restored by", data.Name), ChatType.System);
                        SMain.Enqueue(string.Format("Player {0} has been restored by {1}", data.Name, Name));

                        break;

                    case "CHANGEGENDER":
                        //if (!IsGM && !Settings.TestServer) return;
                        //
                        //data = parts.Length < 2 ? Info : Envir.GetCharacterInfo(parts[1]);
                        //
                        //if (data == null) return;
                        //
                        //switch (data.Gender)
                        //{
                        //    case MirGender.Male:
                        //        data.Gender = MirGender.Female;
                        //        break;
                        //    case MirGender.Female:
                        //        data.Gender = MirGender.Male;
                        //        break;
                        //}
                        //
                        //ReceiveChat(string.Format("Player {0} has been changed to {1}", data.Name, data.Gender), ChatType.System);
                        //SMain.Enqueue(string.Format("Player {0} has been changed to {1} by {2}", data.Name, data.Gender, Name));
                        //
                        //if (data.Player != null)
                        //    data.Player.Connection.LogOut();
                        //
                        break;

                    case "LEVEL":
                        //if ((!IsGM && !Settings.TestServer) || parts.Length < 2) return;
                        //
                        //ushort level;
                        //ushort old;
                        //if (parts.Length >= 3)
                        //{
                        //    if (!IsGM) return;
                        //
                        //    if (ushort.TryParse(parts[2], out level))
                        //    {
                        //        if (level == 0) return;
                        //        player = Envir.GetPlayer(parts[1]);
                        //        if (player == null) return;
                        //        old = player.Level;
                        //        player.Level = level;
                        //        player.LevelUp();
                        //
                        //        ReceiveChat(string.Format("Player {0} has been Leveled {1} -> {2}.", player.Name, old, player.Level), ChatType.System);
                        //        SMain.Enqueue(string.Format("Player {0} has been Leveled {1} -> {2} by {3}", player.Name, old, player.Level, Name));
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    if (parts[1] == "-1")
                        //    {
                        //        parts[1] = ushort.MaxValue.ToString();
                        //    }
                        //
                        //    if (ushort.TryParse(parts[1], out level))
                        //    {
                        //        if (level == 0) return;
                        //        old = Level;
                        //        Level = level;
                        //        LevelUp();
                        //
                        //        ReceiveChat(string.Format("Leveled {0} -> {1}.", old, Level), ChatType.System);
                        //        SMain.Enqueue(string.Format("Player {0} has been Leveled {1} -> {2} by {3}", Name, old, Level, Name));
                        //        return;
                        //    }
                        //}
                        //ReceiveChat("Could not level player", ChatType.System);
                        break;
                    case "TIME":
                        ReceiveChat(string.Format("The time is : {0}", DateTime.Now.ToString("hh:mm tt")), ChatType.System);
                        break;

                    case "ROLL":
                        int diceNum = Envir.Random.Next(5) + 1;

                        if (GroupMembers == null) { return; }

                        for (int i = 0; i < GroupMembers.Count; i++)
                        {
                            PlayerObject playerSend = GroupMembers[i];
                            playerSend.ReceiveChat(string.Format("{0} has rolled a {1}", Name, diceNum), ChatType.Group);
                        }
                        break;

                    case "MAP":
                        var mapName = CurrentMap.Info.FileName;
                        var mapTitle = CurrentMap.Info.Title;
                        ReceiveChat((string.Format("You are currently in {0}. Map ID: {1}", mapTitle, mapName)), ChatType.System);
                        break;

                    case "SAVEPLAYER":
                        if (!IsGM) return;

                        if (parts.Length < 2) return;

                        CharacterInfo tempInfo = null;

                        System.IO.Directory.CreateDirectory("Character Backups");

                        for (int i = 0; i < Envir.AccountList.Count; i++)
                        {
                            for (int j = 0; j < Envir.AccountList[i].Characters.Count; j++)
                            {
                                if (String.Compare(Envir.AccountList[i].Characters[j].Name, parts[1], StringComparison.OrdinalIgnoreCase) != 0) continue;

                                tempInfo = Envir.AccountList[i].Characters[j];
                                break;
                            }
                        }

                        using (System.IO.FileStream stream = System.IO.File.Create(string.Format("Character Backups/{0}", tempInfo.Name)))
                        {
                            using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream))
                            {
                                tempInfo.Save(writer);
                            }
                        }

                        break;

                    case "LOADPLAYER":
                        if (!IsGM) return;

                        if (parts.Length < 2) return;

                        tempInfo = null;

                        System.IO.Directory.CreateDirectory("Character Backups");

                        for (int i = 0; i < Envir.AccountList.Count; i++)
                        {
                            for (int j = 0; j < Envir.AccountList[i].Characters.Count; j++)
                            {
                                if (String.Compare(Envir.AccountList[i].Characters[j].Name, parts[1], StringComparison.OrdinalIgnoreCase) != 0) continue;

                                tempInfo = Envir.AccountList[i].Characters[j];

                                using (System.IO.FileStream stream = System.IO.File.OpenRead(string.Format("Character Backups/{0}", tempInfo.Name)))
                                {
                                    using (System.IO.BinaryReader reader = new System.IO.BinaryReader(stream))
                                    {
                                        CharacterInfo tt = new CharacterInfo(reader);

                                        if (Envir.AccountList[i].Characters[j].Index != tt.Index)
                                        {
                                            ReceiveChat("Player name was matched however IDs did not. Likely due to player being recreated. Player not restored", ChatType.System);
                                            return;
                                        }

                                        Envir.AccountList[i].Characters[j] = tt;
                                    }
                                }
                            }
                        }

                        Envir.BeginSaveAccounts();
                        break;
                    case "MOB":
                        if (!IsGM && !Settings.TestServer) return;
                        if (parts.Length < 2)
                        {
                            ReceiveChat("Not enough parameters to spawn monster", ChatType.System);
                            return;
                        }

                        MonsterInfo mInfo = Envir.GetMonsterInfo(parts[1]);
                        if (mInfo == null)
                        {
                            ReceiveChat((string.Format("Monster {0} does not exist", parts[1])), ChatType.System);
                            return;
                        }

                        count = 1;
                        if (parts.Length >= 3 && IsGM)
                            if (!uint.TryParse(parts[2], out count)) count = 1;

                        for (int i = 0; i < count; i++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(mInfo);
                            if (monster == null) return;
                            monster.Spawn(CurrentMap, Front);
                        }

                        ReceiveChat((string.Format("Monster {0} x{1} has been spawned.", mInfo.Name, count)), ChatType.System);
                        break;
                    case "RELOADDROPS":
                        if (!IsGM) return;
                        foreach (var t in Envir.MonsterInfoList)
                            t.LoadDrops();
                        ReceiveChat("Drops Reloaded.", ChatType.Hint);
                        break;

                    case "RELOADNPCS":
                        if (!IsGM) return;

                        for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                        {
                            CurrentMap.NPCs[i].LoadInfo(true);
                        }

                        DefaultNPC.LoadInfo(true);

                        ReceiveChat("NPCs Reloaded.", ChatType.Hint);
                        break;

                    case "GIVEGOLD":
                        if ((!IsGM && !Settings.TestServer) || parts.Length < 2) return;

                        player = null;

                        if (parts.Length > 2)
                        {
                            if (!IsGM) return;

                            if (!uint.TryParse(parts[2], out count)) return;
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[1]), ChatType.System);
                                return;
                            }
                        }

                        else if (!uint.TryParse(parts[1], out count)) return;

                        if (player != null)
                        {
                            if (count + player.Account.Gold >= uint.MaxValue)
                                count = uint.MaxValue - player.Account.Gold;

                            player.GainGold(count);
                            SMain.Enqueue(string.Format("Player {0} has been given {1} gold", player.Name, count));
                        }

                        break;
                    case "TRIGGER":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        if (parts.Length >= 3)
                        {
                            player = Envir.GetPlayer(parts[2]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[2]), ChatType.System);
                                return;
                            }

                            player.CallDefaultNPC(DefaultNPCType.Trigger, parts[1]);
                            return;
                        }

                        foreach (var pl in Envir.Players)
                        {
                            pl.CallDefaultNPC(DefaultNPCType.Trigger, parts[1]);
                        }

                        break;
                    case "CLEARFLAGS":
                        //if (!IsGM && !Settings.TestServer) return;
                        //
                        //player = parts.Length > 1 && IsGM ? Envir.GetPlayer(parts[1]) : this;
                        //
                        //if (player == null)
                        //{
                        //    ReceiveChat(parts[1] + " is not online", ChatType.System);
                        //    return;
                        //}
                        //
                        //for (int i = 0; i < player.Info.Flags.Length; i++)
                        //{
                        //    player.Info.Flags[i] = false;
                        //}
                        break;
                    case "CLEARMOB":
                        //if (!IsGM) return;
                        //
                        //if (parts.Length > 1)
                        //{
                        //    map = Envir.GetMapByNameAndInstance(parts[1]);
                        //
                        //    if (map == null) return;
                        //
                        //}
                        //else
                        //{
                        //    map = CurrentMap;
                        //}
                        //
                        //foreach (var cell in map.Cells)
                        //{
                        //    if (cell == null || cell.Objects == null) continue;
                        //
                        //    int obCount = cell.Objects.Count();
                        //
                        //    for (int m = 0; m < obCount; m++)
                        //    {
                        //        MapObject ob = cell.Objects[m];
                        //
                        //        if (ob.Race != ObjectType.Monster) continue;
                        //        if (ob.Dead) continue;
                        //        ob.Die();
                        //    }
                        //}
                        //
                        break;

                    case "INFO":
                        {
                            if (!IsGM && !Settings.TestServer) return;

                            MapObject ob = null;

                            if (parts.Length < 2)
                            {
                                Point target = Functions.PointMove(CurrentLocation, Direction, 1);
                                Cell cell = CurrentMap.GetCell(target);

                                if (cell.Objects == null || cell.Objects.Count < 1) return;

                                ob = cell.Objects[0];
                            }
                            else
                            {
                                ob = Envir.GetPlayer(parts[1]);
                            }

                            if (ob == null) return;

                            switch (ob.Race)
                            {
                                case ObjectType.Player:
                                    PlayerObject plOb = (PlayerObject)ob;
                                    ReceiveChat("--Player Info--", ChatType.System2);
                                    ReceiveChat(string.Format("Name : {0}, Level : {1}, X : {2}, Y : {3}", plOb.Name, plOb.Level, plOb.CurrentLocation.X, plOb.CurrentLocation.Y), ChatType.System2);
                                    break;
                                case ObjectType.Monster:
                                    MonsterObject monOb = (MonsterObject)ob;
                                    ReceiveChat("--Monster Info--", ChatType.System2);
                                    ReceiveChat(string.Format("ID : {0}, Name : {1}", monOb.Info.Index, monOb.Name), ChatType.System2);
                                    ReceiveChat(string.Format("Level : {0}, X : {1}, Y : {2}", monOb.Level, monOb.CurrentLocation.X, monOb.CurrentLocation.Y), ChatType.System2);
                                    ReceiveChat(string.Format("HP : {0}, MinDC : {1}, MaxDC : {1}", monOb.Info.HP, monOb.MinDC, monOb.MaxDC), ChatType.System2);
                                    break;
                                case ObjectType.Merchant:
                                    NPCObject npcOb = (NPCObject)ob;
                                    ReceiveChat("--NPC Info--", ChatType.System2);
                                    ReceiveChat(string.Format("ID : {0}, Name : {1}", npcOb.Info.Index, npcOb.Name), ChatType.System2);
                                    ReceiveChat(string.Format("X : {0}, Y : {1}", ob.CurrentLocation.X, ob.CurrentLocation.Y), ChatType.System2);
                                    ReceiveChat(string.Format("File : {0}", npcOb.Info.FileName), ChatType.System2);
                                    break;
                            }
                        }
                        break;

                    case "CLEARQUESTS":
                        // if (!IsGM && !Settings.TestServer) return;
                        //
                        // player = parts.Length > 1 && IsGM ? Envir.GetPlayer(parts[1]) : this;
                        //
                        // if (player == null)
                        // {
                        //     ReceiveChat(parts[1] + " is not online", ChatType.System);
                        //     return;
                        // }
                        //
                        // foreach (var quest in player.CurrentQuests)
                        // {
                        //     SendUpdateQuest(quest, QuestState.Remove);
                        // }
                        //
                        // player.CurrentQuests.Clear();
                        //
                        // player.CompletedQuests.Clear();
                        // player.GetCompletedQuests();
                        //
                        break;

                    case "SETQUEST":
                        //if ((!IsGM && !Settings.TestServer) || parts.Length < 3) return;
                        //
                        //player = parts.Length > 3 && IsGM ? Envir.GetPlayer(parts[3]) : this;
                        //
                        //if (player == null)
                        //{
                        //    ReceiveChat(parts[3] + " is not online", ChatType.System);
                        //    return;
                        //}
                        //
                        //int questid = 0;
                        //int questState = 0;
                        //
                        //int.TryParse(parts[1], out questid);
                        //int.TryParse(parts[2], out questState);
                        //
                        //if (questid < 1) return;
                        //
                        //var activeQuest = player.CurrentQuests.FirstOrDefault(e => e.Index == questid);
                        //
                        ////remove from active list
                        //if (activeQuest != null)
                        //{
                        //    player.SendUpdateQuest(activeQuest, QuestState.Remove);
                        //    player.CurrentQuests.Remove(activeQuest);
                        //}
                        //
                        //switch (questState)
                        //{
                        //    case 0: //cancel
                        //        if (player.CompletedQuests.Contains(questid))
                        //            player.CompletedQuests.Remove(questid);
                        //        break;
                        //    case 1: //complete
                        //        if (!player.CompletedQuests.Contains(questid))
                        //            player.CompletedQuests.Add(questid);
                        //        break;
                        //}
                        //
                        //player.GetCompletedQuests();
                        break;
                    case "OBS":
                        //OBS
                        if ((!IsGM) || parts.Length < 1) return;


                        ObserverEnd();

                        break;
                    default:
                        break;
                }
            }
            else
            {
                message = String.Format("{0}:{1}", CurrentMap.Info.NoNames ? "?????" : Name, message);

                p = new S.ObjectChat { ObjectID = ObjectID, Text = message, Type = ChatType.Normal };

                Enqueue(p);
                Broadcast(p);
            }
        }

        public override ObjectType Race
        {
            get { return ObjectType.Observer; }
        }
        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            throw new NotSupportedException();
        }
        public override void SetOperateTime()
        {
            OperateTime = Envir.Time;
        }
        public override string Name
        {
            get { return ""; }
            set { throw new NotSupportedException(); }
        }
        public override MirDirection Direction { get; set; }
        public override uint Health
        {
            get { throw new NotSupportedException(); }
        }
        public override uint MaxHealth
        {
            get { throw new NotSupportedException(); }
        }
        public override Packet GetInfo()
        {
            return null;
        }
        public override void Process(DelayedAction action)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return false;
        }
        public override bool IsFriendlyTarget(PlayerObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            //   throw new NotSupportedException();
            return false;
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
        public override void SendHealth(PlayerObject player)
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
        public override ushort Level
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        public override Point CurrentLocation { get; set; }
        public override int CurrentMapIndex { get; set; }
        public override bool Blocking
        {
            get { return false; }
        }
        public NPCObject DefaultNPC
        {
            get
            {
                return SMain.Envir.DefaultNPC;
            }
        }
    }
}
