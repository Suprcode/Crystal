namespace Server.VisualMapInfo.Control {
    public class MapContainer : Panel {
        protected override Point ScrollToControl(System.Windows.Forms.Control activeControl) {
            return DisplayRectangle.Location;
        }
    }
}
