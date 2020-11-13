namespace Stress.Server.Models
{
    public class PlayerState
    {
        public string NickName { get; set; }
        public string CardSlot1 { get; set; }
        public string CardSlot2 { get; set; }
        public string CardSlot3 { get; set; }
        public string CardSlot4 { get; set; }
        public int CardsLeft { get; set; }
        public int PlayerNumber { get; set; }
        public bool LostStressEvent { get; set; }
    }
}