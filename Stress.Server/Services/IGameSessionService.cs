using Stress.Server.Models;

namespace Stress.Server.Services
{
    public interface IGameSessionService
    {
        string Key { get; }

        GameState AddPlayer(string nickName);
        GameState PlayerCallsStress(int playerNumber);
        GameState PlayerPlaysCardOnStack(int playerNumber, int slotNumber, bool isLeftStack);
        GameState PlayerWantsToDraw(int playerNumber);
        GameState RequestNewGame(int playerNumber);
    }
}