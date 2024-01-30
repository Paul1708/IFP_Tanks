using Godot;
using System;

namespace Components;

public partial class CoinComponent : Node2D
{
	[Export] int Coins { get; set; } = 0;

	[Signal] public delegate void OnCoinCollectedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OnCoinCollected += GetCoin;
	}

	public void GetCoin()
	{
		//TODO: Play coin collection sound  
		//TODO: remove print
		Coins += 1;
		GD.Print("Coin collected: " + GetCoinCount());
	}

	public int GetCoinCount()
	{
		return Coins;
	}

	public void SetCoinCount(int value)
	{
		Coins = value;
	}

	public void ResetCoinCount()
	{
		Coins = 0;
	}

	public void AddCoins(int value)
	{
		Coins += value;
	}

	public void RemoveCoins(int value)
	{
		Coins -= value;
	}

}
