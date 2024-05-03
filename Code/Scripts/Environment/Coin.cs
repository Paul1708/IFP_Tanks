using Godot;
using Player;
using Managers;

namespace Items;

public partial class Coin : Area2D
{
	[Export] public int CoinValue { get; set; } = 1;
	[Export] public float Speed = 400.0f;
	[Signal] public delegate void OnCoinCollectedEventHandler();
	public bool shouldMove = false;
	private PlayerMovement _player;
	protected MusicController musicController;

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("CoinSprite").Play();
		_player = (PlayerMovement)GetTree().GetFirstNodeInGroup("Player");
		musicController = GetNode<MusicController>("/root/MusicController");

		OnCoinCollected += CollectCoin;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (shouldMove)
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
		//TODO: Play coin collection sound  
		CoinManager.Instance.AddCoins(CoinValue);
		musicController.Play(Sound.CoinPickup);
	}
}
