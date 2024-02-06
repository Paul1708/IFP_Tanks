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

    [TestCase]
    public void TestOnCheckpointSaveCoins()
    {
        coinManager.SetCoins(10);
        coinManager.OnCheckpointSaveCoins();
        Assertions.AssertThat(coinManager.checkpointCoins).IsEqual(10);
    }

    [TestCase]
    public void TestResetCoins()
    {
        coinManager.SetCoins(10);
        coinManager.OnCheckpointSaveCoins();
        coinManager.ResetCoins();
        Assertions.AssertThat(coinManager.Coins).IsEqual(0);
        Assertions.AssertThat(coinManager.GetCheckpointCoins()).IsEqual(0);

    }

    [AfterTest]
    public void TearDown()
    {
        coinManager.Free();
    }
}