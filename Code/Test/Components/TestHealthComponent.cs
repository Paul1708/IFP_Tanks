using Code.Scripts.Components;
using GdUnit4;

namespace Code.Test.Components;

[TestSuite]
public class TestHealthComponent
{
    HealthComponent _hc;

    [BeforeTest]
    public void Setup()
    {
        _hc = new HealthComponent { MaxHp = 100 };
        _hc._Ready();
    }

    [TestCase]
    public void HealthGetsCorrectlyInitialized()
    {
        float expected = 100;

        float actual = _hc.CurrentHp;
        Assertions.AssertThat(actual).IsEqual(expected);

        actual = _hc.MaxHp;
        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void TakeDamageWorks()
    {
        _hc.TakeDamage(10);

        float expected = 90;
        float actual = _hc.CurrentHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void HealWorks()
    {
        _hc.SetCurrentHp(90);
        _hc.Heal(10);

        float expected = 100;
        float actual = _hc.CurrentHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DoesNotHealPastMaxHP()
    {
        _hc.Heal(10);

        float expected = 100;
        float actual = _hc.CurrentHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeHealDoesNotDamage()
    {
        _hc.SetCurrentHp(90);
        _hc.Heal(-10);

        float expected = 90;
        float actual = _hc.CurrentHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }
    

    [TestCase]
    public void NegativeDamageDoesNotHeal()
    {
        _hc.SetCurrentHp(90);
        _hc.TakeDamage(-10);

        float expected = 90;
        float actual = _hc.CurrentHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void IncreaseMaxHealthWorks()
    {
        _hc.IncreaseMaxHealth(10);

        float expected = 110;
        float actual = _hc.MaxHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeIncreaseMaxHealthDoesNotDecrease()
    {
        _hc.IncreaseMaxHealth(-10);

        float expected = 100;
        float actual = _hc.MaxHp;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DyingCallsOnDeath()
    {
        bool called = false;
        _hc.OnDeath += () => called = true;

        _hc.TakeDamage(100);

        Assertions.AssertThat(called).IsTrue();
    }


    [AfterTest]
    public void Clear()
    {
        _hc.QueueFree();
    }
}