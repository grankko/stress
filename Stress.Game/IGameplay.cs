using Stress.Game.Cards;

namespace Stress.Game
{
    public interface IGameplay
    {
        StackOfCards Deck { get; }
        OpenStackOfCards LeftStack { get; }
        Player PlayerOne { get; }
        Player PlayerTwo { get; }
        OpenStackOfCards RightStack { get; }

        void AddPlayer(string nickName);
        bool CanStart();
        void Deal();
        void Draw();
        Player GetPlayerByNumber(int number);
        void PlayCardOnStack(Player player, Card card, OpenStackOfCards stack);
        Player PlayerCallsStressEvent(int playerNumber);
        void RestartGame();
    }
}