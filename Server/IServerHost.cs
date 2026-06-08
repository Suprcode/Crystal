namespace Server
{
    public interface IServerHost
    {
        void Log(string msg);
        void UpdatePlayerCount(int count);
        bool IsRunning { get; }
    }
}
