using Godot;
using Managers;

namespace Shop;

//manages the text of the price tags in the shop and calculates the price of the items
public partial class ShopPrices : Label
{
	//price is the default price of the item 
	[Export] public int basePrice;
	[Export] public float priceMultiplier = 1.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		var greatGreatGrandParent = this.GetParent().GetParent().GetParent().GetParent();

		if (TryGetPanelIndex(out int index))
		{
			if (greatGreatGrandParent.IsInGroup("Stats"))
			{
				this.Text = $"Price: {basePrice + ShopManager.Instance.statsList[index].price}";
			}
			else if (greatGreatGrandParent.IsInGroup("Weapons") && ShopManager.Instance.weaponsList[index].unlocked)
			{
				WeaponUnlocked(this);
			}
			else
			{
				this.Text = $"Price: {basePrice}";
			}
		}
	}
	private bool TryGetPanelIndex(out int index)
	{
		return ShopManager.Instance.panelIndexMap.TryGetValue(GetParent().Name, out index);
	}

	//calculate the price of the item based on the quantity of the item
	public void IncreasePrice(Stat stat)
	{
		int value = (int)(basePrice * Mathf.Pow(priceMultiplier, stat.quantity));
		stat.priceTag.Text = "Price: " + value.ToString();
	}

	public static void WeaponUnlocked(Label label)
	{
		label.Text = "Unlocked";
	}
}

