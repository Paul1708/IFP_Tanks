using GdUnit4;
using Components;

[TestSuite]
public class TestHealthComponent
{
    HealthComponent hc;

    [BeforeTest]
    public void Setup()
    {
        hc = new HealthComponent { maxHP = 100 };
        hc._Ready();
    }

    [TestCase]
    public void HealthGetsCorrectlyInitialized()
    {
        float expected = 100;

        float actual = hc.currentHP;
        Assertions.AssertThat(actual).IsEqual(expected);

        actual = hc.maxHP;
        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void TakeDamageWorks()
    {
        hc.TakeDamage(10);

        float expected = 90;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void HealWorks()
    {
        hc.SetCurrentHP(90);
        hc.Heal(10);

        float expected = 100;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DoesNotHealPastMaxHP()
    {
        hc.Heal(10);

        float expected = 100;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeHealDoesNotDamage()
    {
        hc.SetCurrentHP(90);
        hc.Heal(-10);

        float expected = 90;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DamageWorks()
    {
        hc.TakeDamage(10);

        float expected = 90;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeDamageDoesNotHeal()
    {
        hc.SetCurrentHP(90);
        hc.TakeDamage(-10);

        float expected = 90;
        float actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void IncreaseMaxHealthWorks()
    {
        hc.IncreaseMaxHealth(10);

        float expected = 110;
        float actual = hc.maxHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeIncreaseMaxHealthDoesNotDecrease()
    {
        hc.IncreaseMaxHealth(-10);

        float expected = 100;
        float actual = hc.maxHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DyingCallsOnDeath()
    {
        bool called = false;
        hc.OnDeath += () => called = true;

        hc.TakeDamage(100);

        Assertions.AssertThat(called).IsTrue();
    }


    [AfterTest]
    public void Clear()
    {
        hc.QueueFree();
    }
}