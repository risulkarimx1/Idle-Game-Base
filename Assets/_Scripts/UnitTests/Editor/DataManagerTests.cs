using Cysharp.Threading.Tasks;
using GameCode.Persistence;
using NUnit.Framework;
using Services.DataFramework;
using Zenject;

namespace _Scripts.UnitTests.Editor
{
    [TestFixture]
    public class DataManagerTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Container.BindInterfacesAndSelfTo<MockEncryptionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MockDataHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataManager>().AsSingle();
        }

        [Test]
        public void DataManager_LoadsMineDataCorrectly()
        {
            var dataManager = Container.Resolve<IDataManager>();
            dataManager.Initialize();

            var minesData = dataManager.Get<MinesData>();
            Assert.IsNotNull(minesData);
        }

        [Test]
        public void DataManager_SavesMineDataCorrectly()
        {
            var dataManager = Container.Resolve<IDataManager>();
            dataManager.Initialize();
            var testMineId = "mine_test";

            var minesData = dataManager.Get<MinesData>();
            minesData.GetMineData(testMineId).SetMineShaftLevel(1, 10);
            dataManager.SaveAsync<MinesData>().Forget();

            var newMinesData = dataManager.Get<MinesData>();
            Assert.AreEqual(10, newMinesData.GetMineData(testMineId).GetMineShaftsLevel()[1]);
        }
    }
}