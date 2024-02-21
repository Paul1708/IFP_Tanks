using Godot;
using Managers.Level;
using System;
using Managers;

public partial class ShopUpgradeStatsTab : TabBar
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	public int price1, price2, price3, price4;
	public Label priceTag1, priceTag2, priceTag3, priceTag4;
	public int itemQuantity1, itemQuantity2, itemQuantity3, itemQuantity4; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		hScrollBar = GetNode<HScrollBar>("HScrollBar");	
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		
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
		shopMenu.Buy(price1, priceTag1, itemQuantity1, "Item1");
	}

	private void OnBuy2Pressed()
	{
		shopMenu.Buy(price2, priceTag2, itemQuantity2, "Item2");
	}

	private void OnBuy3Pressed()
	{
		shopMenu.Buy(price3, priceTag3, itemQuantity3, "Item3");
	}

	private void OnBuy4Pressed()
	{
		shopMenu.Buy(price4, priceTag4, itemQuantity4, "Item4");
	}

	private void GetPriceTags()
	{
		//Stats tab
		priceTag1 = GetNode<Label>("RichTextLabel/Control/Panel1/PriceTag");
		priceTag2 = GetNode<Label>("RichTextLabel/Control/Panel2/PriceTag");
		priceTag3 = GetNode<Label>("RichTextLabel/Control/Panel3/PriceTag");
		priceTag4 = GetNode<Label>("RichTextLabel/Control/Panel4/PriceTag");
	}
}
