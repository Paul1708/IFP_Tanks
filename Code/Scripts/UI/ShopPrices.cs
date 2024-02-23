using Godot;
using System;

//manages the text of the price tags in the shop and calculates the price of the items
public partial class ShopPrices : Label
{

	ShopMenu shopMenu;

	//price is the default price of the item 
	[Export] public int price;
	public float priceMultiplier = 1.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		
		//TODO: make it to load the price from the save file and not to set default after going to main menu
		this.Text = "Price: " + price.ToString();
	}

	//calculate the price of the item based on the quantity of the item
	public void IncreasePrice (Label label, int itemQuantity) 
	{
		int value = (int)(price * Mathf.Pow(priceMultiplier, itemQuantity));
		label.Text = "Price: " + value.ToString();
	}

	public void WeaponUnlocked (Label label)
	{
		label.Text = "Unlocked";
	}
}

