using System;
using System.Collections.Generic;
using GameCode.Finance;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class IdleIncomeCalculatorTests : ZenjectUnitTestFixture
{
    private IdleIncomeCalculator _idleIncomeCalculator;

    [SetUp]
    public void SetUp()
    {
        _idleIncomeCalculator = new IdleIncomeCalculator();
    }

    [Test]
    public void CalculateIncomeRate_ReturnsCorrectIncomeRate()
    {
        _idleIncomeCalculator._deposits = new List<(DateTime Time, double Amount)>
        {
            (DateTime.UtcNow.AddSeconds(-10), 100),
            (DateTime.UtcNow.AddSeconds(-5), 200),
            (DateTime.UtcNow, 300)
        };

        var incomeRate = _idleIncomeCalculator.CalculateIncomeRate();

        var isCloseEnough = Math.Abs(60 - incomeRate) < 0.01;
        Assert.IsTrue(isCloseEnough);
    }
}