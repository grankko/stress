namespace Stress.Server.Models
{
    public class GameState
    {
        // todo: introduce model of Card
        public PlayerState PlayerOneState { get; set; }
        public PlayerState PlayerTwoState { get; set; }
        public string LeftStackTopCard { get; set; }
        public string RightStackTopCard { get; set; }
    }

    public class PlayerState
    {
        // todo: introduce model of Card
        public string NickName { get; set; }
        public string CardSlot1 { get; set; }
        public string CardSlot2 { get; set; }
        public string CardSlot3 { get; set; }
        public string CardSlot4 { get; set; }
        public int CardsLeft { get; set; }
    }
}