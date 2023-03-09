using Client.MirControls;
using Client.MirGraphics;

namespace Client.MirScenes.Dialogs
{
    public class CompassDialog : MirControl
    {
        public Point Destination = Point.Empty;

        private readonly MirImageControl _image;

        public CompassDialog()
        {
            Location = new Point((Settings.ScreenWidth / 2) - 25, (Settings.ScreenHeight / 2) - 120);
            NotControl = true;
            Size = new Size(10, 10);
            Movable = false;
            Sort = true;

            _image = new MirImageControl
            {
                Parent = this,
                Index = 0,
                Library = Libraries.Prguse2,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(0, 0),
                Visible = true
            };
        }

        public void ClearPoint()
        {
            Destination = Point.Empty;
        }

        public void SetPoint(Point point)
        {
            Destination = point;
        }

        public void Process()
        {
            if (Destination == Point.Empty || (Destination.X == GameScene.User.CurrentLocation.X && Destination.Y == GameScene.User.CurrentLocation.Y))
            {
                Visible = false;
                return;
            }

            Visible = true;

            float xDiff = GameScene.User.CurrentLocation.X - Destination.X;
            float yDiff = GameScene.User.CurrentLocation.Y - Destination.Y;

            var angle = Math.Atan2(xDiff * -1, yDiff) * 180 / Math.PI;

            var degree = (angle + 360) % 360;
 
            var offset = (double)40 / 360 * degree;

            _image.Index = (int)(1470 + Math.Floor(offset));
        }
    }
}
