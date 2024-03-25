using NUnit.Framework;
using Moq;
using GameCode.TimeProvider;
using System;
using GameCode.Persistence;
using Services.DataFramework;
using Zenject;

namespace _Scripts.UnitTests.Editor
{
    [TestFixture]
    public class DataManagerTestFixture : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Container.BindInterfacesAndSelfTo<MockEncryptionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MockDataHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataManager>().AsSingle();
        }

        protected DataManager DataManagerInstance
        {
            get
            {
                var instance = Container.Resolve<DataManager>();
                instance.Initialize();
                return instance;
            }
        }
    }

    public class GameSessionDataTests : DataManagerTestFixture
    {
        private Mock<ITimeProvider> _mockTimeProvider;
        private DateTime _fixedDateTime;

        [SetUp]
        public new void SetUp()
        {
            base.Setup();
            Container.BindInterfacesAndSelfTo<MockEncryptionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MockDataHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataManager>().AsSingle();

            _fixedDateTime = new DateTime(2020, 1, 1, 12, 0, 0); // Fixed time for testing
            _mockTimeProvider = new Mock<ITimeProvider>();
            _mockTimeProvider.Setup(provider => provider.UtcNow).Returns(_fixedDateTime);
        }

        [Test]
        public void GameSessionData_UpdatesMineIdCorrectly()
        {
            var mineId = "TestMine";


            var gameSessionData = DataManagerInstance.Get<GameSessionData>();

            gameSessionData.MineId = mineId;

            Assert.AreEqual(mineId, gameSessionData.MineId);
            Assert.IsTrue(gameSessionData.IsDirty, "GameSessionData should be marked as dirty after updating MineId.");
        }

        [Test]
        public void GameSessionData_UpdatesAndGetsLogOffTimeCorrectly()
        {
            var mineId = "TestMine";

            var gameSessionData = DataManagerInstance.Get<GameSessionData>();
            gameSessionData.UpdateLoggOffTime(mineId, _mockTimeProvider.Object);
            var logOffTime = gameSessionData.GetLogOffTime(mineId, _mockTimeProvider.Object);

            Assert.AreEqual(_fixedDateTime, logOffTime);
            Assert.IsTrue(gameSessionData.IsDirty,
                "GameSessionData should be marked as dirty after updating logoff time.");
        }

        [Test]
        public void GameSessionData_ReturnsCurrentTimeForUnknownMineId()
        {
            var unknownMineId = "UnknownMine";

            var gameSessionData = DataManagerInstance.Get<GameSessionData>();
            var logOffTime = gameSessionData.GetLogOffTime(unknownMineId, _mockTimeProvider.Object);

            Assert.AreEqual(_fixedDateTime, logOffTime,
                "GameSessionData should return the current time for unknown mine IDs.");
        }
    }
}