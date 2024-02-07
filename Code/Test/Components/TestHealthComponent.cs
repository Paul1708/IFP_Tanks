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
        int expected = 100;

        int actual = hc.currentHP;
        Assertions.AssertThat(actual).IsEqual(expected);

        actual = hc.maxHP;
        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void TakeDamageWorks()
    {
        hc.TakeDamage(10);

        int expected = 90;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void HealWorks()
    {
        hc.SetCurrentHP(90);
        hc.Heal(10);

        int expected = 100;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DoesNotHealPastMaxHP()
    {
        hc.Heal(10);

        int expected = 100;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeHealDoesNotDamage()
    {
        hc.SetCurrentHP(90);
        hc.Heal(-10);

        int expected = 90;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void DamageWorks()
    {
        hc.TakeDamage(10);

        int expected = 90;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeDamageDoesNotHeal()
    {
        hc.SetCurrentHP(90);
        hc.TakeDamage(-10);

        int expected = 90;
        int actual = hc.currentHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void IncreaseMaxHealthWorks()
    {
        hc.IncreaseMaxHealth(10);

        int expected = 110;
        int actual = hc.maxHP;

        Assertions.AssertThat(actual).IsEqual(expected);
    }

    [TestCase]
    public void NegativeIncreaseMaxHealthDoesNotDecrease()
    {
        hc.IncreaseMaxHealth(-10);

        int expected = 100;
        int actual = hc.maxHP;

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