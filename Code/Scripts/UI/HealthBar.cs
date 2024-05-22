using System;
using Components;
using Godot;
using Managers;


namespace UI;

public partial class HealthBar : TextureProgressBar
{
    [Export]
    public float RedBarDelay { get; set; } = 1f;
    public TextureProgressBar greenBar;
    public TextureProgressBar redBar;
    public Timer redHealthTimer;
    public Label label;
    public float currentHP;
    public float maxHP;
    private HealthComponent _playerHealthComponent;

    public override void _Ready()
    {
        // Get references
        greenBar = GetNode<TextureProgressBar>("GreenHealth");
        redBar = this;
        redHealthTimer = GetNode<Timer>("RedHealthTimer");
        label = GetNode<Label>("Label");
        _playerHealthComponent = PlayerManager.Instance.PlayerHealthComponent;

        // Connect signals
        _playerHealthComponent.OnHealthChanged += OnHealthChanged;
        _playerHealthComponent.OnMaxHealthChanged += OnMaxHealthChanged;
        redHealthTimer.Timeout += UpdateRedHealthBar;

        // Get hp values of player
        maxHP = _playerHealthComponent.maxHP;
        currentHP = _playerHealthComponent.currentHP;

        // Set progress bar values
        redBar.MaxValue = maxHP;
        redBar.Value = currentHP;
        greenBar.MaxValue = maxHP;
        greenBar.Value = currentHP;

        UpdateLabelText();
    }

    public override void _ExitTree()
    {
        // Disconnect signals
        _playerHealthComponent.OnHealthChanged -= OnHealthChanged;
        _playerHealthComponent.OnMaxHealthChanged -= OnMaxHealthChanged;
        redHealthTimer.Timeout -= UpdateRedHealthBar;
    }

    public void OnHealthChanged(float newHP)
    {
        //set the health bar's value to the current health
        greenBar.Value = newHP;

        if (newHP > currentHP)
        {
            redBar.Value = newHP;
        }
        currentHP = newHP;
        UpdateLabelText();
        redHealthTimer.Stop();
        redHealthTimer.Start(RedBarDelay);
    }

    public void OnMaxHealthChanged(float newMaxHP)
    {
        maxHP = newMaxHP;
        UpdateLabelText();

        // set max health
        redBar.MaxValue = maxHP;
        greenBar.MaxValue = maxHP;

        // set current health    
        currentHP = PlayerManager.Instance.PlayerHealthComponent.currentHP;
        redBar.Value = currentHP;
        greenBar.Value = currentHP;
    }

    private void UpdateRedHealthBar()
    {
        //set the red health bar's value to the current health
        Tween tween = CreateTween();
        tween.TweenProperty(this, "value", currentHP, 1).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }

    private void UpdateLabelText()
    {
        label.Text = $"{Math.Round(currentHP)} / {Math.Round(maxHP)}";
    }
}