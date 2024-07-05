using Godot;
using Code.Scripts.Movement;
using Code.Scripts.Managers;
using Code.Scripts.Audio;

namespace Code.Scripts.Environment;

/// <summary>
/// Coin class which handles the coin collection and movement
/// </summary>
public partial class Coin : Area2D
{
	[Export] public int CoinValue { get; set; } = 1;
	[Export] public float Speed = 1500f;
	[Signal] public delegate void OnCoinCollectedEventHandler();
	public bool ShouldMove = false;
	private PlayerMovement _player;
	protected MusicController MusicController;

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("CoinSprite").Play();
		_player = (PlayerMovement)GetTree().GetFirstNodeInGroup("Player");
		MusicController = GetNode<MusicController>("/root/MusicController");

		OnCoinCollected += CollectCoin;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (ShouldMove)
		{
			Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
			Position += direction * Speed * (float)delta;
		}
	}

	/// <summary>
	/// Called when a body enters the coin area, checks if the body is the player if so it emits the OnCoinCollected signal
	/// </summary>
	/// <param name="body">The node that entered the coin area</param>
	private void OnCoinBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			EmitSignal(SignalName.OnCoinCollected);
			QueueFree();
		}
	}

	/// <summary>
	/// Sets the coin value to the given value
	/// </summary>
	/// <param name="value">The value to set the coin value to</param>
	public void SetCoinValue(int value)
	{
		CoinValue = value;
	}

	/// <summary>
	/// Collects the coin, adds the coin value to the CoinManager and plays the coin pickup sound
	/// </summary>
	public void CollectCoin()
	{
		CoinManager.Instance.AddCoins(CoinValue);
		MusicController.Play(Sound.CoinPickup);
	}
}
