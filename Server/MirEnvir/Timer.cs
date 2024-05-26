namespace Server.Library.MirEnvir {
    public class Timer {
        private static Envir Envir => Envir.Main;

        public string Key;
        public byte Type;
        public int Seconds;

        public long RelativeTime;

        public Timer(string key, int seconds, byte type) {
            Key = key;
            Seconds = seconds;
            Type = type;

            RelativeTime = Envir.Time + (seconds * Settings.Second);
        }
    }
}
