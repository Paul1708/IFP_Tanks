using Godot;
using System;

public partial class ShopPrices : Label
{

	ShopMenu shopMenu;

	//price is the default price of the item 
	[Export] public int price;
	[Export] public float priceMultiplier = 1.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		shopMenu.OnItemBought += UpdatePrice;
		//TODO: make it to load the price from the save file and not to set default after going to main menu
		this.Text = "Price: " + price.ToString();
	}

	private void UpdatePrice (Label label, int value, int itemQuantity) 
	{
		value = (int)Math.Round(value * priceMultiplier * itemQuantity);
		label.Text = "Price: " + value.ToString();
	}



}
