using Godot;
using System;
using Managers.Level;
using Managers;
using Items;
using System.Linq;

public partial class ShopEquipWeaponsTab : ShopBaseTab
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
		shopMenu.Buy(prices[0], priceTag1, itemQuantities[0], "tab2Item1");
	}

	private void OnBuy2Pressed()
	{
		shopMenu.Buy(prices[1], priceTag2, itemQuantities[1], "tab2Item2");
	}

	private void OnBuy3Pressed()
	{
		shopMenu.Buy(prices[2], priceTag3, itemQuantities[2], "tab2Item3");
	}

	private void OnBuy4Pressed()
	{
		shopMenu.Buy(prices[3], priceTag4, itemQuantities[3], "tab2Item4");
	}

	private void GetPriceTags()
	{
		//Weapons tab
		priceTag1 = GetPriceTag(1);
		priceTag2 = GetPriceTag(2);
		priceTag3 = GetPriceTag(3);
		priceTag4 = GetPriceTag(4);
	}	
}