namespace Stress.Server.Models
{
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
    }

    public class PlayerState
    {
        public string NickName { get; set; }
        public string CardSlot1 { get; set; }
        public string CardSlot2 { get; set; }
        public string CardSlot3 { get; set; }
        public string CardSlot4 { get; set; }
        public int CardsLeft { get; set; }
        public int PlayerNumber { get; set; }
    }
}