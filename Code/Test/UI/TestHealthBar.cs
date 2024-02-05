using UI;
using GdUnit4;


[TestSuite]
public class TestHealthBar
{
    ISceneRunner runner;
    HealthBar hb;

    [BeforeTest]
    public void Setup()
    {
        runner = ISceneRunner.Load("res://Test/UI/TestHealthBar.tscn");
        hb = runner.Scene().GetTree().GetNodesInGroup("HealthBar")[0] as HealthBar;
    }


    [TestCase]
    public void CorrectlyInitializeHealthValues()
    {
        float maxHealth = hb.maxHP;
        float currentHealth = hb.currentHP;

        Assertions.AssertFloat(maxHealth).Equals(hb.maxHP);
        Assertions.AssertFloat(maxHealth).Equals(hb.MaxValue);
        Assertions.AssertFloat(maxHealth).Equals(hb.greenBar.MaxValue);

        Assertions.AssertFloat(currentHealth).Equals(hb.currentHP);
        Assertions.AssertFloat(currentHealth).Equals(hb.Value);
        Assertions.AssertFloat(currentHealth).Equals(hb.greenBar.Value);

        string expectedLabelText = $"{currentHealth} / {maxHealth}";
        Assertions.AssertString(expectedLabelText).Equals(hb.label.Text);
    }

    [TestCase]

    public void OnHealthChangedUpdatesHealthValues()
    {
        float newHealth = 10f;
        hb.OnHealthChanged(newHealth);

        Assertions.AssertFloat(newHealth).Equals(hb.currentHP);
        Assertions.AssertFloat(newHealth).Equals(hb.Value);
        Assertions.AssertFloat(newHealth).Equals(hb.greenBar.Value);

        string expectedLabelText = $"{hb.currentHP} / {hb.maxHP}";
        Assertions.AssertString(expectedLabelText).Equals(hb.label.Text);
    }

    [TestCase]
    public void OnHealthChangedStartsRedHealthTimer()
    {
        float newHealth = 10f;
        hb.OnHealthChanged(newHealth);

        Assertions.AssertBool(hb.redHealthTimer.IsStopped()).IsFalse();
    }


    public void OnMaxHealthChangedUpdatesMaxHealthValues()
    {
        float newMaxHealth = 100f;
        hb.OnMaxHealthChanged(newMaxHealth);

        Assertions.AssertFloat(newMaxHealth).Equals(hb.maxHP);
        Assertions.AssertFloat(newMaxHealth).Equals(hb.MaxValue);
        Assertions.AssertFloat(newMaxHealth).Equals(hb.greenBar.MaxValue);

        string expectedLabelText = $"{hb.currentHP} / {hb.maxHP}";
        Assertions.AssertString(expectedLabelText).Equals(hb.label.Text);
    }
}