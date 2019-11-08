namespace Server.MirForms.Control
{
    public class MapContainer : System.Windows.Forms.Panel
    {
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            return this.DisplayRectangle.Location;
        }
    }
}
