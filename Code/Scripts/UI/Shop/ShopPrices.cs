using Godot;
using System;
using System.Collections.Generic;

//manages the text of the price tags in the shop and calculates the price of the items
public partial class ShopPrices : Label
{
	ShopMenu shopMenu;

	//price is the default price of the item 
	[Export] public int basePrice;
	public float priceMultiplier = 1.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

		Dictionary<string, int> panelIndexMap = new()
        {
			{ "Panel1", 0 },
			{ "Panel2", 1 },
			{ "Panel3", 2 },
			{ "Panel4", 3 }
		};

		if (this.IsInGroup("Stats"))
		{
			if (panelIndexMap.TryGetValue(GetParent().Name, out int index))
			{
				this.Text = "Price: " + (basePrice + ShopManager.Instance.statsList[index].price).ToString();
			}
		}
		else if (this.IsInGroup("Weapons"))
		{
			if (panelIndexMap.TryGetValue(GetParent().Name, out int index) && ShopManager.Instance.weaponsList[index].unlocked)
			{
				WeaponUnlocked(this);
			}
			else
			{
				this.Text = "Price: " + basePrice.ToString();
			}
		}
	}

	//calculate the price of the item based on the quantity of the item
	public void IncreasePrice(Stat stat)
	{
		int value = (int)(basePrice * Mathf.Pow(priceMultiplier, stat.quantity));
		stat.priceTag.Text = "Price: " + value.ToString();
	}

	public void WeaponUnlocked(Label label)
	{
		label.Text = "Unlocked";
	}
}

