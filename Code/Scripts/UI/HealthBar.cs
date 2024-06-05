using System;
using Code.Scripts.Components;
using Code.Scripts.Managers;
using Godot;


namespace Code.Scripts.UI;

public partial class HealthBar : TextureProgressBar
{
    [Export]
    public float RedBarDelay { get; set; } = 1f;
    public TextureProgressBar GreenBar;
    public TextureProgressBar RedBar;
    public Timer RedHealthTimer;
    public Label Label;
    public float CurrentHp;
    public float MaxHp;
    private HealthComponent _playerHealthComponent;

    public override void _Ready()
    {
        // Get references
        GreenBar = GetNode<TextureProgressBar>("GreenHealth");
        RedBar = this;
        RedHealthTimer = GetNode<Timer>("RedHealthTimer");
        Label = GetNode<Label>("Label");
        _playerHealthComponent = PlayerManager.Instance.PlayerHealthComponent;

        // Connect signals
        _playerHealthComponent.OnHealthChanged += OnHealthChanged;
        _playerHealthComponent.OnMaxHealthChanged += OnMaxHealthChanged;
        RedHealthTimer.Timeout += UpdateRedHealthBar;

        // Get hp values of player
        MaxHp = _playerHealthComponent.MaxHp;
        CurrentHp = _playerHealthComponent.CurrentHp;

        // Set progress bar values
        RedBar.MaxValue = MaxHp;
        RedBar.Value = CurrentHp;
        GreenBar.MaxValue = MaxHp;
        GreenBar.Value = CurrentHp;

        UpdateLabelText();
    }

    public override void _ExitTree()
    {
        // Disconnect signals
        _playerHealthComponent.OnHealthChanged -= OnHealthChanged;
        _playerHealthComponent.OnMaxHealthChanged -= OnMaxHealthChanged;
        RedHealthTimer.Timeout -= UpdateRedHealthBar;
    }

    public void OnHealthChanged(float newHp)
    {
        //set the health bar's value to the current health
        GreenBar.Value = newHp;

        if (newHp > CurrentHp)
        {
            RedBar.Value = newHp;
        }
        CurrentHp = newHp;
        UpdateLabelText();
        RedHealthTimer.Stop();
        RedHealthTimer.Start(RedBarDelay);
    }

    public void OnMaxHealthChanged(float newMaxHp)
    {
        MaxHp = newMaxHp;
        UpdateLabelText();

        // set max health
        RedBar.MaxValue = MaxHp;
        GreenBar.MaxValue = MaxHp;

        // set current health    
        CurrentHp = PlayerManager.Instance.PlayerHealthComponent.CurrentHp;
        RedBar.Value = CurrentHp;
        GreenBar.Value = CurrentHp;
    }

    private void UpdateRedHealthBar()
    {
        //set the red health bar's value to the current health
        Tween tween = CreateTween();
        tween.TweenProperty(this, "value", CurrentHp, 1).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }

    private void UpdateLabelText()
    {
        Label.Text = $"{Math.Round(CurrentHp)} / {Math.Round(MaxHp)}";
    }
}