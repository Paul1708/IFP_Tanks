using Godot;
using System;

public partial class ShopPrices : Label
{

	[Export]
	public int price;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Text = "Price: " + price.ToString();
	}

}
