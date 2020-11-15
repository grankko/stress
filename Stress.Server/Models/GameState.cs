namespace Stress.Server.Models
{

    /// <summary>
    /// The only type of message returned to the clients. Any event affecting the game state should
    /// signal this message back to all clients in the game.
    /// </summary>
    public class GameState
    {
        public PlayerState PlayerOneState { get; set; }
        public PlayerState PlayerTwoState { get; set; }
        public string LeftStackTopCard { get; set; }
        public string RightStackTopCard { get; set; }
        public bool DrawExecuted { get; set; }
        public int DrawRequestedByPlayer { get; set; }
        public string WinnerName { get; set; }
        public int NewGameRequestedByPlayer { get; set; }
        public bool RematchStarted { get; set; }
        public int LeftStackSize { get; set; }
        public int RightStackSize { get; set; }
        public bool IsReady { get; set; }
    }
}