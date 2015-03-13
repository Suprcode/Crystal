using System;
using System.Drawing;
using System.Windows.Forms;

namespace Server.MirForms.VisualMapInfo.Class
{
    public static class VisualizerGlobal
    {
        public static event EventHandler FocusModeActivated;

        public static Cursor
            Cursor = Cursors.Arrow;

        public static void ActivateFocusMode(object o, EventArgs s)
        {
            if (FocusModeActivated != null)
                FocusModeActivated(null, null);
        }

        public static Server.MirDatabase.MapInfo
            MapInfo;

        public static int
            ZoomLevel = 1; // 1 - 6

        public static bool FocusMode = false;
        public static bool FocusModeActive
        {
            get { return FocusMode; }
            set
            {
                FocusMode = value;
                if (FocusMode == true)
                    ActivateFocusMode(null, null);
            }
        }

        public static MapInfo
            ActiveMap;

        public static Bitmap
            ClippingMap = null;

        public static Tool
            SelectedTool = Tool.Select;

        public static FocusType
            SelectedFocusType;

        public static Server.MirForms.VisualMapInfo.Control.MineEntry
            FocusMineEntry;

        public static Server.MirForms.VisualMapInfo.Control.RespawnEntry
            FocusRespawnEntry;

        public enum Tool
        {
            Select,
            Add,
            Move,
            Resize,
            Focus
        }

        public enum FocusType
        {
            None,
            Mining,
            Respawn,
            NPC,
            SafeZone,
            Guard
        }
    }
}
