using Components;
using Godot;
using System;

namespace Items;

public partial class Coin : Area2D
{
	public int Coins;
	public bool shouldMove = false;

	[Export] public int CoinValue { get; set; } = 1;
	[Export] public float Speed = 400.0f;
	[Signal] public delegate void OnCoinCollectedEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("CoinSprite").Play();

		OnCoinCollected += GetCoin;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (shouldMove)
		{
			Node2D player = (Node2D)GetTree().GetFirstNodeInGroup("Player");
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
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

	public void GetCoin()
	{
		//TODO: Play coin collection sound  
		//TODO: remove print
		Coins = GameManager.Instance.GetCoins();
		Coins += CoinValue;
		GameManager.Instance.SetCoins(Coins);
		GD.Print("Coins in Manager: " + GameManager.Instance.GetCoins());
	}
	public int GetCoinCount()
	{
		return GameManager.Instance.GetCoins();
	}

	public void SetCoinCount(int value)
	{
		GameManager.Instance.SetCoins(value);
	}

	public void ResetCoinCount()
	{
		GameManager.Instance.ResetCoins();
	}

	public void AddCoins(int value)
	{
		Coins = GameManager.Instance.GetCoins();
		Coins += value;
		GameManager.Instance.SetCoins(Coins);
	}

	public void RemoveCoins(int value)
	{

		Coins = GameManager.Instance.GetCoins();
		Coins -= value;
		GameManager.Instance.SetCoins(Coins);
	}
}
