using S = ServerPackets;
using Client.MirControls;
using Client.MirGraphics;

namespace Client.MirScenes.Dialogs
{
    public sealed class TimerDialog : MirControl
    {
        private bool _timerStarted = false;
        private int _timerCounter = 0;
        private long _timerTime = 0;

        private readonly MirAnimatedControl _eggTimer = null;
        private readonly MirImageControl _1000 = null;
        private readonly MirImageControl _100 = null;
        private readonly MirImageControl _10 = null;
        private readonly MirImageControl _1 = null;
        private readonly MirImageControl _colon = null;
        private readonly int _libraryOffset = 900;

        private readonly List<ClientTimer> ActiveTimers = new List<ClientTimer>();
        private ClientTimer CurrentTimer = null;

        public TimerDialog()
        {
            Location = new Point(Settings.ScreenWidth - 120, Settings.ScreenHeight - 230);
            NotControl = true;
            Size = new Size(120, 100);
            Movable = false;
            Sort = true;

            _eggTimer = new MirAnimatedControl
            {
                Index = 960,
                AnimationCount = 6,
                AnimationDelay = 333,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(23, 0),
                NotControl = true,
                UseOffSet = true,
                Animated = true,
                Loop = false,
                Opacity = 1F
            };

            _1000 = new MirImageControl
            {
                Parent = this,
                Index = _libraryOffset + 0,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(0, 70),
                Visible = false
            };

            _100 = new MirImageControl
            {
                Parent = this,
                Index = _libraryOffset + 0,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(22, 70),
                Visible = false
            };

            _colon = new MirImageControl
            {
                Parent = this,
                Index = _libraryOffset + 10,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(44, 70),
                Visible = false
            };

            _10 = new MirImageControl
            {
                Parent = this,
                Index = _libraryOffset + 0,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(58, 70),
                Visible = false
            };

            _1 = new MirImageControl
            {
                Parent = this,
                Index = _libraryOffset + 0,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(80, 70),
                Visible = false
            };
        }

        public void Process()
        {
            var timer = GetBestTimer();

            if (timer != null)
            {
                if (timer != CurrentTimer || timer.Refresh)
                {
                    CurrentTimer = timer;
                    CurrentTimer.Refresh = false;

                    _timerStarted = true;
                    _timerTime = CMain.Time + 1000;
                    _timerCounter = (int)(CurrentTimer.RelativeTime - (CMain.Time / 1000));

                    UpdateTimeGraphic();
                }
            }

            if (CurrentTimer == null || _timerStarted == false || CMain.Time < _timerTime) return;

            _timerCounter--;
            _timerTime = CMain.Time + 1000;

            if (_timerCounter < 0 && _eggTimer != null)
            {
                Visible = false;
                _1000.Visible = _100.Visible = _10.Visible = _1.Visible = _colon.Visible = false;
                _eggTimer.Loop = false;
                _timerStarted = false;

                ActiveTimers.Remove(CurrentTimer);
                return;
            }

            UpdateTimeGraphic();
        }

        private ClientTimer GetBestTimer()
        {
            return ActiveTimers.OrderBy(x => x.RelativeTime).FirstOrDefault();
        }

        public ClientTimer GetTimer(string key)
        {
            return ActiveTimers.FirstOrDefault(x => x.Key == key);
        }

        public void AddTimer(S.SetTimer p)
        {
            var currentTimer = GetTimer(p.Key);

            if (currentTimer != null)
            {
                currentTimer.Update(p.Seconds, p.Type);
                return;
            }

            ActiveTimers.Add(new ClientTimer(p.Key, p.Seconds, p.Type));
        }

        public void ExpireTimer(string key)
        {
            var timer = ActiveTimers.FirstOrDefault(x => x.Key == key);

            if (timer != null)
            {
                timer.RelativeTime = 0;

                if (timer == CurrentTimer)
                {
                    _timerCounter = 0;
                }
            }
        }

        private void UpdateTimeGraphic()
        {
            TimeSpan ts = new TimeSpan(0, 0, _timerCounter);

            if (ts.Hours > 0)
            {
                _1000.Index = _libraryOffset + (ts.Hours / 10);
                _100.Index = _libraryOffset + (ts.Hours % 10);
                _10.Index = _libraryOffset + (ts.Minutes / 10);
                _1.Index = _libraryOffset + (ts.Minutes % 10);
            }
            else
            {
                _1000.Index = _libraryOffset + (ts.Minutes / 10);
                _100.Index = _libraryOffset + (ts.Minutes % 10);
                _10.Index = _libraryOffset + (ts.Seconds / 10);
                _1.Index = _libraryOffset + (ts.Seconds % 10);
            }

            Visible = true;
            _1000.Visible = _100.Visible = _10.Visible = _1.Visible = _colon.Visible = true;

            switch (CurrentTimer.Type)
            {
                default:
                case 0:
                    _eggTimer.Visible = false;
                    _eggTimer.Index = 960;
                    break;
                case 1:
                    _eggTimer.Visible = true;
                    _eggTimer.Index = 960;
                    break;
                case 2:
                    _eggTimer.Visible = true;
                    _eggTimer.Index = 440;
                    break;
            }

            _eggTimer.Loop = true;
        }
    }

    public class ClientTimer
    {
        public string Key;
        public byte Type;
        public int Seconds;

        public long RelativeTime;
        public bool Refresh;

        public ClientTimer(string key, int time, byte type)
        {
            Key = key;
            Update(time, type);
        }

        public void Update(int time, byte type)
        {
            Seconds = time;
            Type = type;

            RelativeTime = Seconds + (CMain.Time / 1000);
            Refresh = true;
        }
    }
}
