using GdUnit4;
using Managers;

[TestSuite]
public class CoinManagerTests
{
    private CoinManager coinManager;

    [BeforeTest]
    public void SetUp()
    {
        coinManager = new CoinManager { Coins = 5 };
        coinManager._Ready();
    }

    [TestCase]
    public void TestSetAndGetCoins()
    {
        Assertions.AssertThat(coinManager.Coins).IsEqual(5);
    }

    [TestCase]
    public void TestSetCoinsNegativeValue()
    {
        coinManager.SetCoins(-5);
        Assertions.AssertThat(coinManager.Coins).IsEqual(0);
    }

    [AfterTest]
    public void TearDown()
    {
        coinManager.Free();
    }
}