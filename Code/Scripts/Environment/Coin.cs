using Godot;
using Code.Scripts.Movement;
using Code.Scripts.Managers;
using Code.Scripts.Audio;

namespace Code.Scripts.Environment;

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


	private void OnCoinBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			EmitSignal(SignalName.OnCoinCollected);
			QueueFree();
		}
	}

	public void SetCoinValue(int value)
	{
		CoinValue = value;
	}

	public void CollectCoin()
	{
		CoinManager.Instance.AddCoins(CoinValue);
		MusicController.Play(Sound.CoinPickup);
	}
}
