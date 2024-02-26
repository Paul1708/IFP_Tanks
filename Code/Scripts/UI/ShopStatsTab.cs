using Godot;
using Managers.Level;
using System;
using Managers;
using System.Linq;
using System.Collections.Generic;
using Managers.Save;

public struct Stat
{
	public string name;
	public int listIndex;
	public int price;
	public Label priceTag;
	public int quantity;
	public Stat () { }
	public Stat(string name, int listIndex)
	{
		this.name = name;
		this.listIndex = listIndex;
	}
}

public partial class ShopStatsTab : ShopBaseTab
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		OnItemBoughtUpdatePrices += UpdatePrices;

		hScrollBar = GetNode<HScrollBar>("HScrollBar");
		control = GetNode<Node2D>("RichTextLabel/Control");

		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		levelManager.OnLevelChanged += ResetScrollBar;

		GetPriceTags();
		UpdatePrices();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Scroll();
	}

	private void ResetScrollBar()
	{
		hScrollBar.Value = 0;
	}

	private void Scroll()
	{
		Vector2 position = control.Position;
		position.X = (float)-hScrollBar.Value;
		control.Position = position;
	}

	///<summary>
	///Tries to upgrade/buy the stat and returns true if the stat was bought, false if not. 
	///It increases the price and quantity of the stat and removes the coins if the stat was bought.
	///</summary>
	public bool Buy(Stat stat)
	{
		ShopPrices shopPrices = stat.priceTag as ShopPrices;

		if (CoinManager.Instance.CheckIfEnoughCoins(stat.price + shopPrices.basePrice))
		{
			CoinManager.Instance.RemoveCoins(stat.price + shopPrices.basePrice);

			shopPrices.IncreasePrice(stat);
			IncreaseQuantity(stat);
			EmitSignal(SignalName.OnItemBoughtUpdatePrices);
			return true;
		}
		else
		{
			shopMenu.ShowError(stat.price + shopPrices.basePrice);
			return false;
		}
	}

	///<summary>
	///Increase the quantity of the stat in the list at the listIndex by 1.
	///</summary>
	protected void IncreaseQuantity(Stat stat)
	{
		stat.quantity++;
		ShopManager.Instance.statsList[stat.listIndex] = stat;
	}

	///<summary>
	///Get all price tags from the scene by their path that only differs in the Panel number and connect them to the stats.
	/// </summary>
	protected override void GetPriceTags()
	{
		Label[] statsPriceTags = new Label[ShopManager.Instance.statsList.Count];

		for (int i = 0; i < ShopManager.Instance.statsList.Count; i++)
		{
			statsPriceTags[i] = GetPriceTagByPanel(i + 1);
			Stat stat = ShopManager.Instance.statsList[i];
			stat.priceTag = statsPriceTags[i];
			ShopManager.Instance.statsList[i] = stat;
		}
	}

	///<summary>
	///Update the prices of the items in the shop by parsing the price from the price tags and updating the price in the list.
	///The price stored in the list is the difference between the parsed price and the base price of the item.
	/// </summary>
	protected override void UpdatePrices()
	{
		int[] statsPrices = new int[ShopManager.Instance.statsList.Count];

		for (int i = 0; i < ShopManager.Instance.statsList.Count; i++)
		{
			statsPrices[i] = ParsePrice(i + 1);
			Stat stat = ShopManager.Instance.statsList[i];
			ShopPrices shopPrices = stat.priceTag as ShopPrices;
			int basePrice = shopPrices.basePrice;
			stat.price = statsPrices[i]-basePrice;
			ShopManager.Instance.statsList[i] = stat;
		}
	}

	private void OnBuy1Pressed()
	{
		Stat healStat = ShopManager.Instance.statsList[0];
		Buy(healStat);
	}

	private void OnBuy2Pressed()
	{
		Stat maxHPStat = ShopManager.Instance.statsList[1];
		Buy(maxHPStat);

	}

	private void OnBuy3Pressed()
	{
		Stat DMGStat = ShopManager.Instance.statsList[2];
		Buy(DMGStat);
	}

	private void OnBuy4Pressed()
	{
		Stat speedStat = ShopManager.Instance.statsList[3];
		Buy(speedStat);
	}
}
