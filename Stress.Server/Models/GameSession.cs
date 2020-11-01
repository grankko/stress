using Stress.Game;

namespace Stress.Server.Models
{
    public class GameSession
    {
        public string Key { get; private set; }
        private Gameplay _gameplay;

        public GameSession(string key)
        {
            Key = key;
            _gameplay = new Gameplay();
        }

        public void AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
        }
    }
}