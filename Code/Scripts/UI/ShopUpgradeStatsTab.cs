using Godot;
using Managers.Level;
using System;
using Managers;
using System.Linq;

public partial class ShopUpgradeStatsTab : ShopBaseTab
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		hScrollBar = GetNode<HScrollBar>("HScrollBar");	
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		tabItems = GetTree().GetNodesInGroup("StatsTab").ToList();
		itemCount = tabItems.Count;
		
		levelManager.OnLevelChanged += ResetScrollBar;

		GetPriceTags();
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

	private void OnBuy1Pressed()
	{
		shopMenu.Buy(prices[0], priceTag1, itemQuantities[0], "Item1");
	}

	private void OnBuy2Pressed()
	{
		shopMenu.Buy(prices[1], priceTag2, itemQuantities[1], "Item2");
	}

	private void OnBuy3Pressed()
	{
		shopMenu.Buy(prices[2], priceTag3, itemQuantities[2], "Item3");
	}

	private void OnBuy4Pressed()
	{
		shopMenu.Buy(prices[3], priceTag4, itemQuantities[3], "Item4");
	}

	private void GetPriceTags()
	{
		//Stats tab
		priceTag1 = GetPriceTag(1);
		priceTag2 = GetPriceTag(2);
		priceTag3 = GetPriceTag(3);
		priceTag4 = GetPriceTag(4);
	}
}
