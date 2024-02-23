using Godot;
using Managers.Level;
using System;
using Managers;
using System.Linq;
using System.Collections.Generic;

public partial class ShopUpgradeStatsTab : ShopBaseTab
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	/*when using, be aware that assignment is dependent on the order of the Panels in the scene:
	healStat is managed in Panel1, so its addressed by the number 1 (or in an array or list by 0)*/
	private Stat healStat = new("Heal");
	private Stat maxHPStat = new("MaxHP");
	private Stat DMGStat = new("DMG");
	private Stat speedStat = new("Speed");
	public List<Stat> statsList = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		OnItemBoughtUpdatePrices += UpdateStatPrices;

		hScrollBar = GetNode<HScrollBar>("HScrollBar");
		control = GetNode<Node2D>("RichTextLabel/Control");

		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		levelManager.OnLevelChanged += ResetScrollBar;

		AddStatsToList(healStat, maxHPStat, DMGStat, speedStat); //example: healStat is managed in Panel1 and adressed by number 0 in the list
		GetPriceTags();
		UpdateStatPrices();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Scroll();
	}

	private void AddStatsToList(params Stat[] stats)
	{
		foreach (var stat in stats)
		{
			statsList.Add(stat);
		}
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
	///tries to upgrade the stat and returns true if the stat was bought, false if not. It increases the price of the stat and removes the coins if the stat was bought
	///</summary>
	public bool UpgradeStat(Stat stat)
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(stat.price))
		{
			ShopPrices shopPrices = stat.priceTag as ShopPrices;
			CoinManager.Instance.RemoveCoins(stat.price);

			shopPrices.IncreasePrice(stat.priceTag, stat.quantity + 1);
			EmitSignal(SignalName.OnItemBoughtUpdatePrices);
			return true;
		}
		else
		{
			shopMenu.ShowError(stat.price);
			return false;
		}
	}

	///<summary>
	///Increase the quantity of the stat in the list at the listIndex by 1
	///</summary>
	private void IncreaseStatQuantity(Stat stat, int listIndex)
	{
		stat.quantity++;
		statsList[listIndex] = stat;
	}

	///<summary>
	///get all price tags from the scene by their path that only differs in the Panel number and connect them to the stats
	/// </summary>
	private void GetPriceTags()
	{
		Label[] statsPriceTags = new Label[statsList.Count];

		for (int i = 0; i < statsList.Count; i++)
		{
			statsPriceTags[i] = GetPriceTagByPanel(i + 1);
			Stat stat = statsList[i];
			stat.priceTag = statsPriceTags[i];
			statsList[i] = stat;
		}
	}

	///<summary>
	///Update the prices of the items in the shop by parsing the price from the price tags and updating the price in the list
	/// </summary>
	private void UpdateStatPrices()
	{
		int[] statsPrices = new int[statsList.Count];

		for (int i = 0; i < statsList.Count; i++)
		{
			statsPrices[i] = ParsePrice(i+1);
			Stat stat = statsList[i];
			stat.price = statsPrices[i];
			statsList[i] = stat;
		}
	}

		private void OnBuy1Pressed()
	{
		if (UpgradeStat(statsList[0]))
		{
			IncreaseStatQuantity(statsList[0], 0);
		}
	}

	private void OnBuy2Pressed()
	{
		if (UpgradeStat(statsList[1]))
		{
			IncreaseStatQuantity(statsList[1], 1);
		}
	}

	private void OnBuy3Pressed()
	{
		if (UpgradeStat(statsList[2]))
		{
			IncreaseStatQuantity(statsList[2], 2);
		}
	}

	private void OnBuy4Pressed()
	{
		if (UpgradeStat(statsList[3]))
		{
			IncreaseStatQuantity(statsList[3], 3);
		}
	}
}
