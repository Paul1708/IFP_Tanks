using Components;
using Godot;

namespace UI;

public partial class HealthBar : TextureProgressBar
{
    [Export]
    public float RedBarDelay { get; set; } = 1f;
    public TextureProgressBar greenBar;
    public Timer redHealthTimer;
    public Label label;

    HealthComponent playerHealthComponent;

    public float currentHP;
    public float maxHP;
    public override void _Ready()
    {
        // Get references
        greenBar = GetNode<TextureProgressBar>("GreenHealth");
        redHealthTimer = GetNode<Timer>("RedHealthTimer");
        label = GetNode<Label>("Label");
        playerHealthComponent = Manager.Instance.PlayerManager.PlayerHealthComponent;

        // Connect signals
        playerHealthComponent.OnHealthChanged += OnHealthChanged;
        playerHealthComponent.OnMaxHealthChanged += OnMaxHealthChanged;
        redHealthTimer.Timeout += UpdateRedHealthBar;

        // Get hp values of player
        maxHP = playerHealthComponent.maxHP;
        currentHP = playerHealthComponent.currentHP;

        // Set progress bar values
        MaxValue = maxHP;
        Value = currentHP;
        greenBar.MaxValue = maxHP;
        greenBar.Value = currentHP;

        UpdateLabelText();
    }

    public override void _ExitTree()
    {
        // Disconnect signals
        playerHealthComponent.OnHealthChanged -= OnHealthChanged;
        playerHealthComponent.OnMaxHealthChanged -= OnMaxHealthChanged;
        redHealthTimer.Timeout -= UpdateRedHealthBar;
    }

    public void OnHealthChanged(float newHP)
    {
        //set the health bar's value to the current health
        currentHP = newHP;
        greenBar.Value = currentHP;

        UpdateLabelText();
        redHealthTimer.Stop();
        redHealthTimer.Start(RedBarDelay);
    }

    public void OnMaxHealthChanged(float newMaxHP)
    {
        UpdateLabelText();
        maxHP = newMaxHP;
        MaxValue = maxHP;
        greenBar.MaxValue = maxHP;
    }

    private void UpdateRedHealthBar()
    {
        //set the red health bar's value to the current health
        Tween tween = CreateTween();
        tween.TweenProperty(this, "value", currentHP, 1).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }

    private void UpdateLabelText()
    {
        label.Text = $"{currentHP} / {maxHP}";
    }


}