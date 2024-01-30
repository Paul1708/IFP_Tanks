using Components;
using Godot;
using System;

public partial class Coin : Area2D
{
	Node coinComponent; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	 	coinComponent = GetTree().GetFirstNodeInGroup("Coin");
		
		GetNode<AnimatedSprite2D>("CoinSprite").Play();

	}

	private void OnCoinBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
            coinComponent.EmitSignal(nameof(CoinComponent.OnCoinCollected));
			QueueFree();
		}
	}
}
