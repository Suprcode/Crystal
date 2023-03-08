using Client.MirControls;

namespace Client.MirObjects
{
    public class Damage
    {
        public string Text;
        public Color Colour;
        public int Distance;
        public long ExpireTime;
        public double Factor;
        public int Offset;

        public MirLabel DamageLabel;

        public Damage(string text, int duration, Color colour, int distance = 50)
        {
            ExpireTime = (long)(CMain.Time + duration);
            Text = text;
            Distance = distance;
            Factor = duration / this.Distance;
            Colour = colour;
        }

        public void Draw(Point displayLocation)
        {
            long timeRemaining = ExpireTime - CMain.Time;

            if (DamageLabel == null)
            {
                DamageLabel = new MirLabel
                {
                    AutoSize = true,
                    BackColour = Color.Transparent,
                    ForeColour = Colour,
                    OutLine = true,
                    OutLineColour = Color.Black,
                    Text = Text,
                    Font = new Font(Settings.FontName, 8F, FontStyle.Bold)
                };
            }

            displayLocation.Offset((int)(15 - (Text.Length * 3)), (int)(((int)((double)timeRemaining / Factor)) - Distance) - 75 - Offset);

            DamageLabel.Location = displayLocation;
            DamageLabel.Draw();
        }
    }

}
