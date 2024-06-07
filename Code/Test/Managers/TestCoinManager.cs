using Code.Scripts.Managers;
using GdUnit4;

namespace Code.Test.Managers;

[TestSuite]
public class CoinManagerTests
{
    private CoinManager _coinManager;

    [BeforeTest]
    public void SetUp()
    {
        _coinManager = new CoinManager { Coins = 5 };
        _coinManager._Ready();
    }

    [TestCase]
    public void TestSetAndGetCoins()
    {
        Assertions.AssertThat(_coinManager.Coins).IsEqual(5);
    }

    [TestCase]
    public void TestSetCoinsNegativeValue()
    {
        _coinManager.SetCoins(-5);
        Assertions.AssertThat(_coinManager.Coins).IsEqual(0);
    }

    [AfterTest]
    public void TearDown()
    {
        _coinManager.Free();
    }
}