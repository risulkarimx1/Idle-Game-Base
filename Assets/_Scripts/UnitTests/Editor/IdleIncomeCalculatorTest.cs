using System;
using System.Collections.Generic;
using GameCode.Finance;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class PassiveIncomeCalculatorTests : ZenjectUnitTestFixture
{
    private PassiveIncomeCalculator _passiveIncomeCalculator;

    [SetUp]
    public void SetUp()
    {
        _passiveIncomeCalculator = new PassiveIncomeCalculator();
    }

    [Test]
    public void CalculateIncomeRate_ReturnsCorrectIncomeRate()
    {
        _passiveIncomeCalculator._deposits = new List<(DateTime Time, double Amount)>
        {
            (DateTime.UtcNow.AddSeconds(-10), 100),
            (DateTime.UtcNow.AddSeconds(-5), 200),
            (DateTime.UtcNow, 300)
        };

        var incomeRate = _passiveIncomeCalculator.CalculateIncomeRate();

        var isCloseEnough = Math.Abs(60 - incomeRate) < 0.01;
        Assert.IsTrue(isCloseEnough);
    }
}