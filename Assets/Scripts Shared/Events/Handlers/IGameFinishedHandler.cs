namespace Events.Handlers
{
    public interface IGameFinishedHandler
    {
        public void OnGameFinished(bool isWin);
    }
}