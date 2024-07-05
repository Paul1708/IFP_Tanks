using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI.Shop;

/// <summary>
/// Manages the text of the price tags in the shop and calculates the price of the items.
/// </summary>
public partial class ShopPrices : Label
{
	//price is the default price of the item 
	[Export] public int BasePrice;
	[Export] public float PriceMultiplier = 1.1f;
	private LevelManager _levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		
		_levelManager.OnLevelReset += SetShopPriceState;
		
		LinkPriceTagToItem();
		SetShopPriceState();
	}

	public override void _ExitTree()
	{
		_levelManager.OnLevelReset -= SetShopPriceState;
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
				var stat = ShopManager.Instance.StatsList[index];
				stat.PriceTag = this;
				ShopManager.Instance.StatsList[index] = stat;
			}
			else if (greatGreatGrandParent.IsInGroup("Weapons"))
			{
				var weapon = ShopManager.Instance.WeaponsList[index];
				weapon.PriceTag = this;
				ShopManager.Instance.WeaponsList[index] = weapon;
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
				Text = $"Price: {BasePrice + ShopManager.Instance.StatsList[index].Price}";
			}
			else if (greatGreatGrandParent.IsInGroup("Weapons") && ShopManager.Instance.WeaponsList[index].Unlocked)
			{
				WeaponUnlocked(this);
			}
			else
			{
				Text = $"Price: {BasePrice}";
			}
		}
	}

	/// <summary>
	/// Increase the price of the item by the price multiplier to the power of the quantity of the item. 
	/// Set the price of the item in the list to the new value and set the price tag text to the new value.
	/// </summary>
	/// <param name="stat">The stat to increase the price of</param>
	public void IncreasePrice(Stat stat)
	{
		int value = (int)(BasePrice * Mathf.Pow(PriceMultiplier, stat.Quantity));
		stat.Price = value - BasePrice;
		ShopManager.Instance.StatsList[stat.ListIndex] = stat;
		stat.PriceTag.Text = "Price: " + value.ToString();
	}

	/// <summary>
	/// Sets the price tag text to "Unlocked"
	/// </summary>
	/// <param name="label">The label to set the text to</param>
	public static void WeaponUnlocked(Label label)
	{
		label.Text = "Unlocked";
	}
}

