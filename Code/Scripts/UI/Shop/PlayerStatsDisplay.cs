using System;
using Code.Scripts.Managers;
using Code.Scripts.Movement;
using Godot;

namespace Code.Scripts.UI.Shop;

public partial class PlayerStatsDisplay : Control
{
    private Label _coinsLabel;
    private Label _hpLabel;
    private Label _damageLabel;
    private Label _movementSpeedLabel;

    public override void _Ready()
    {
        // Get the labels from the scene
        _coinsLabel = GetNode<Label>("LeftSide/CoinsLabel");
        _hpLabel = GetNode<Label>("LeftSide/HPLabel");
        _damageLabel = GetNode<Label>("RightSide/DamageLabel");
        _movementSpeedLabel = GetNode<Label>("RightSide/SpeedLabel");

        // Set the labels to the current values
        _damageLabel.Text = $"x {PlayerManager.Instance.PlayerStats.CurrentDamageModifier}";
        _movementSpeedLabel.Text = PlayerManager.Instance.PlayerStats.CurrentMovementSpeed.ToString();
        _coinsLabel.Text = CoinManager.Instance.Coins.ToString();
        _hpLabel.Text = $"{PlayerManager.Instance.PlayerHealthComponent.CurrentHp}/{PlayerManager.Instance.PlayerHealthComponent.MaxHp}";

        // Connect signals
        CoinManager.Instance.OnCoinChanged += UpdateCoinLabel;
        PlayerManager.Instance.PlayerHealthComponent.OnHealthChanged += UpdateHealthLabel;
        PlayerManager.Instance.PlayerHealthComponent.OnMaxHealthChanged += UpdateHealthLabel;
        PlayerManager.Instance.OnPlayerStatsChanged += UpdateStatLabel;
    }

    public override void _ExitTree()
    {
        // Disconnect signals
        CoinManager.Instance.OnCoinChanged -= UpdateCoinLabel;
        PlayerManager.Instance.PlayerHealthComponent.OnHealthChanged -= UpdateHealthLabel;
        PlayerManager.Instance.PlayerHealthComponent.OnMaxHealthChanged -= UpdateHealthLabel;
        PlayerManager.Instance.OnPlayerStatsChanged -= UpdateStatLabel;
    }

    /// <summary>
    /// Updates the damage and movement speed labels
    /// </summary>
    /// <param name="stats">New stat values.</param>
    public void UpdateStatLabel(PlayerStats stats)
    {
        _damageLabel.Text = $"x {Mathf.Round(stats.CurrentDamageModifier * 10) / 10}";
        _movementSpeedLabel.Text = (Mathf.Round(stats.CurrentMovementSpeed * 10) / 10).ToString();
    }

    /// <summary>
    /// Updates the health label.
    /// </summary>
    /// <param name="maxHealth">Is ignored, only needed beacuse of Event.</param>
    public void UpdateHealthLabel(float maxHealth)
    {
        _hpLabel.Text = $"{Math.Round(PlayerManager.Instance.PlayerHealthComponent.CurrentHp)}/{Math.Round(PlayerManager.Instance.PlayerHealthComponent.MaxHp)}";
    }

    /// <summary>
    /// Updates the coin label.
    /// </summary>
    /// <param name="coinCount">New coin value.</param>
    public void UpdateCoinLabel(int coinCount)
    {
        _coinsLabel.Text = coinCount.ToString();
    }
}
