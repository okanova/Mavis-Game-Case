public interface IMiniGame
{
    string GameId { get; }
    void Initialize();
    void StartGame();
    void EndGame();
}

