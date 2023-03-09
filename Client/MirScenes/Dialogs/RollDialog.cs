using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class RollDialog : MirControl
    {
        private readonly MirAnimatedControl _animation;
        private readonly MirImageControl _image;

        private int _currentLoop;
        private int _type;
        private string _npcPage;
        private int _result;

        private bool _rolled;
        private bool _rolling;

        public RollDialog()
        {
            Movable = false;
            Sort = true;

            _animation = new MirAnimatedControl
            {
                Parent = this,
                Index = 0,
                Library = Libraries.Prguse2,
                UseOffSet = true,
                Location = new Point(0, 0),
                Visible = true
            };
            _animation.AfterAnimation += (o, e) =>
            {
                switch (_type)
                {
                    case 0: //Die
                        {
                            if (_currentLoop < 5)
                            {
                                _currentLoop++;
                                return;
                            }

                            _image.Visible = true;
                            _animation.Visible = false;
                            _animation.Animated = false;
                            ReturnResult();
                        }
                        break;
                    case 1: //Yut
                        {
                            _image.Visible = true;
                            _animation.Visible = false;
                            _animation.Animated = false;
                            ReturnResult();
                        }
                        break;
                }
                
            };

            _image = new MirImageControl
            {
                Parent = this,
                Index = 0,
                Library = Libraries.Prguse2,
                UseOffSet = true,
                Location = new Point(0, 0),
                Visible = false
            };
            _image.Click += _image_Click;
        }

        public void Setup(int type, string page, int result, bool autoRoll)
        {
            _type = type;
            _npcPage = page;
            _result = result;
            _rolled = false;

            _currentLoop = 0;
            Visible = true;

            switch (type)
            {
                case 0: //Die
                    {
                        Size = new Size(65, 65);
                        Location = new Point((Settings.ScreenWidth / 2) - 38, (Settings.ScreenHeight / 2) - 40);

                        _image.Index = 282;
                        _image.Library = Libraries.Prguse;
                        _image.Visible = true;

                        _animation.Loop = true;
                        _animation.Visible = false;
                        _animation.Animated = false;
                        _animation.OffSet = 0;
                    }
                    break;
                case 1: //Yut
                    {
                        Size = new Size(180, 130);
                        Location = new Point((Settings.ScreenWidth / 2) - 90, (Settings.ScreenHeight / 2) - 65);

                        _image.Index = 2581;
                        _image.Library = Libraries.Items;
                        _image.Visible = true;

                        _animation.Loop = false;
                        _animation.Visible = false;
                        _animation.Animated = false;
                        _animation.OffSet = 0;
                    }
                    break;
            }

            if (autoRoll)
            {
                Roll();
            }
        }

        private void Roll()
        {
            Visible = true;

            _rolling = true;

            switch (_type)
            {
                case 0: //Die
                    {
                        _image.Index = 281 + _result;
                        _image.Library = Libraries.Prguse;
                        _image.Visible = false;

                        _animation.Index = 290;
                        _animation.Library = Libraries.Prguse;
                        _animation.AnimationCount = 4;
                        _animation.AnimationDelay = 100;
                        _animation.Loop = true;
                        _animation.Visible = true;
                        _animation.Animated = true;
                        _animation.OffSet = 0;

                        SoundManager.PlaySound(10600);
                    }
                    break;
                case 1: //Yut
                    {
                        _image.Index = 2587 + _result;
                        _image.Library = Libraries.Items;
                        _image.Visible = false;

                        _animation.Index = 2581;
                        _animation.Library = Libraries.Items;
                        _animation.AnimationCount = 6;
                        _animation.AnimationDelay = 100;
                        _animation.Loop = false;
                        _animation.Visible = true;
                        _animation.Animated = true;
                        _animation.OffSet = 0;

                        SoundManager.PlaySound(10601);
                    }
                    break;
            }
        }

        private void _image_Click(object sender, EventArgs e)
        {
            if (_rolling) return;

            if (_rolled)
            {
                Hide();
                return;
            }

            Roll();
        }

        private void ReturnResult()
        {
            _rolling = false;
            _rolled = true;

            if (CMain.Time <= GameScene.NPCTime) return;

            GameScene.NPCTime = CMain.Time + 5000;
            Network.Enqueue(new C.CallNPC { ObjectID = GameScene.NPCID, Key = $"[{_npcPage}]" });
        }
    }
}
