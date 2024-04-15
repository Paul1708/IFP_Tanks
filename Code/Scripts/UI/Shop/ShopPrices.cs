using Godot;
using Managers;
using Managers.Level;

namespace Shop;

//manages the text of the price tags in the shop and calculates the price of the items
public partial class ShopPrices : Label
{
	//price is the default price of the item 
	[Export] public int basePrice;
	[Export] public float priceMultiplier = 1.1f;
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		
		levelManager.OnLevelReset += SetShopPriceState;
		
		LinkPriceTagToItem();
		SetShopPriceState();
	}

	public override void _ExitTree()
	{
		levelManager.OnLevelReset -= SetShopPriceState;
	}

	/// <summary>
	/// Links the price tag to the item in the shop.
	/// </summary>
	private void LinkPriceTagToItem()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();
		if (ShopManager.Instance.TryGetPanelIndex(this, out int index)) 
		{
			if (greatGreatGrandParent.IsInGroup("Stats"))
			{
				var stat = ShopManager.Instance.statsList[index];
				stat.priceTag = this;
				ShopManager.Instance.statsList[index] = stat;
			}
			else if (greatGreatGrandParent.IsInGroup("Weapons"))
			{
				var weapon = ShopManager.Instance.weaponsList[index];
				weapon.priceTag = this;
				ShopManager.Instance.weaponsList[index] = weapon;
			}
		}
	}

	/// <summary>
	/// Sets the priceTags of the items in the shop to the prices of the items. 
	/// If the item is a weapon and is unlocked, the price tag will be set to "Unlocked".
	/// </summary>
	private void SetShopPriceState()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();

		if (ShopManager.Instance.TryGetPanelIndex(this, out int index))
		{
			if (greatGreatGrandParent.IsInGroup("Stats"))
			{
				Text = $"Price: {basePrice + ShopManager.Instance.statsList[index].price}";
			}
			else if (greatGreatGrandParent.IsInGroup("Weapons") && ShopManager.Instance.weaponsList[index].unlocked)
			{
				WeaponUnlocked(this);
			}
			else
			{
				Text = $"Price: {basePrice}";
			}
		}
	}

	/// <summary>
	/// Increase the price of the item by the price multiplier to the power of the quantity of the item. 
	/// Set the price of the item in the list to the new value and set the price tag text to the new value.
	/// </summary>
	public void IncreasePrice(Stat stat)
	{
		int value = (int)(basePrice * Mathf.Pow(priceMultiplier, stat.quantity));
		stat.price = value - basePrice;
		ShopManager.Instance.statsList[stat.listIndex] = stat;
		stat.priceTag.Text = "Price: " + value.ToString();
	}

	public static void WeaponUnlocked(Label label)
	{
		label.Text = "Unlocked";
	}
}

