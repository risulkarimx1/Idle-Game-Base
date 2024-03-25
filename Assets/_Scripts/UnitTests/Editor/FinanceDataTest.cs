using GameCode.Persistence;
using NUnit.Framework;
using Services.DataFramework;

namespace _Scripts.UnitTests.Editor
{
    [TestFixture]
    public class FinanceDataTest : DataManagerTestFixture
    {
        [Test]
        public void FinanceData_UpdatesMoneyCorrectly()
        {
            var financeData = DataManagerInstance.Get<FinanceData>();
            financeData.Money = 100;

            var newFinanceData = DataManagerInstance.Get<FinanceData>();
            Assert.AreEqual(100, newFinanceData.Money);
        }

        [Test]
        public void FinanceData_UpdatesDepositRateCorrectly()
        {
            var dataManager = Container.Resolve<IDataManager>();
            dataManager.Initialize();
            var financeData = dataManager.Get<FinanceData>();
            financeData.SetDepositRate("mine_test", 10);

            var newFinanceData = dataManager.Get<FinanceData>();
            var newDepositRate = newFinanceData.GetDepositRate("mine_test");
            Assert.AreEqual(10, newDepositRate);
        }
    }
}