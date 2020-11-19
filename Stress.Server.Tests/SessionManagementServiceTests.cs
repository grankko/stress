using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stress.Game;
using Stress.Server.Services;

namespace Stress.Server.Tests
{
    [TestClass]
    public class SessionManagementServiceTests
    {

        [TestInitialize]
        public void InitializeTest()
        {
            var gameSessionMock = new Mock<IGameSessionService>();
            var gameplayMock = new Mock<IGameplay>();
            var sut = new SessionManagementService();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ISessionManagementService>(sut);
            serviceCollection.AddTransient<IGameplay>(p => new Mock<IGameplay>().Object);
            serviceCollection.AddTransient<IGameSessionService>(p =>
                new GameSessionService(p.GetService<ISessionManagementService>().GenerateNewSessionKey(), p.GetRequiredService<IGameplay>()));

            Program.ServiceProvider = serviceCollection.BuildServiceProvider();

        }

        [TestMethod]
        public void CanCreateNewGameSession()
        {
            var sut = Program.ServiceProvider.GetRequiredService<ISessionManagementService>();
            sut.CreateNewGameSession();
            Assert.AreEqual(1, sut.GameSessions.Count);
        }

    }
}
