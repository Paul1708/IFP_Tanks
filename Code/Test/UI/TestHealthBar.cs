
using Code.Scripts.UI;
using GdUnit4;

namespace Code.Test.UI;

[TestSuite]
public class TestHealthBar
{
    ISceneRunner _runner;
    HealthBar _hb;

    [BeforeTest]
    public void Setup()
    {
        _runner = ISceneRunner.Load("res://Test/UI/TestHealthBar.tscn");
        _hb = _runner.Scene().GetTree().GetNodesInGroup("HealthBar")[0] as HealthBar;
    }


    [TestCase]
    public void CorrectlyInitializeHealthValues()
    {
        float maxHealth = _hb.MaxHp;
        float currentHealth = _hb.CurrentHp;

        Assertions.AssertFloat(maxHealth).Equals(_hb.MaxHp);
        Assertions.AssertFloat(maxHealth).Equals(_hb.MaxValue);
        Assertions.AssertFloat(maxHealth).Equals(_hb.GreenBar.MaxValue);

        Assertions.AssertFloat(currentHealth).Equals(_hb.CurrentHp);
        Assertions.AssertFloat(currentHealth).Equals(_hb.Value);
        Assertions.AssertFloat(currentHealth).Equals(_hb.GreenBar.Value);

        string expectedLabelText = $"{currentHealth} / {maxHealth}";
        Assertions.AssertString(expectedLabelText).Equals(_hb.Label.Text);
    }

    [TestCase]

    public void OnHealthChangedUpdatesHealthValues()
    {
        float newHealth = 10;
        _hb.OnHealthChanged(newHealth);

        Assertions.AssertFloat(newHealth).Equals(_hb.CurrentHp);
        Assertions.AssertFloat(newHealth).Equals(_hb.Value);
        Assertions.AssertFloat(newHealth).Equals(_hb.GreenBar.Value);

        string expectedLabelText = $"{_hb.CurrentHp} / {_hb.MaxHp}";
        Assertions.AssertString(expectedLabelText).Equals(_hb.Label.Text);
    }

    [TestCase]
    public void OnHealthChangedStartsRedHealthTimer()
    {
        float newHealth = 10;
        _hb.OnHealthChanged(newHealth);

        Assertions.AssertBool(_hb.RedHealthTimer.IsStopped()).IsFalse();
    }


    public void OnMaxHealthChangedUpdatesMaxHealthValues()
    {
        float newMaxHealth = 100;
        _hb.OnMaxHealthChanged(newMaxHealth);

        Assertions.AssertFloat(newMaxHealth).Equals(_hb.MaxHp);
        Assertions.AssertFloat(newMaxHealth).Equals(_hb.MaxValue);
        Assertions.AssertFloat(newMaxHealth).Equals(_hb.GreenBar.MaxValue);

        string expectedLabelText = $"{_hb.CurrentHp} / {_hb.MaxHp}";
        Assertions.AssertString(expectedLabelText).Equals(_hb.Label.Text);
    }
}