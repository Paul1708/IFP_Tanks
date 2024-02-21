using Godot;
using System;
using Managers.Level;
using Managers;
using Items;

public partial class ShopEquipWeaponsTab : TabBar
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	public int price1, price2, price3, price4;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
	
		hScrollBar = GetNode<HScrollBar>("HScrollBar");
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		levelManager.OnLevelChanged += ResetScrollBar;
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
		shopMenu.Buy(price1);
	}

	private void OnBuy2Pressed()
	{
		shopMenu.Buy(price2);
	}

	private void OnBuy3Pressed()
	{
		shopMenu.Buy(price3);
	}

	private void OnBuy4Pressed()
	{
		shopMenu.Buy(price4);
	}

}

/*
var price = GetNode<Label>("RichTextLabel/Control/Panel4/PriceTag").Text;
var priceInt = int.Parse(price);
if (priceInt <= 0)
{
	return;
}
var coins = GetTree().GetFirstNodeInGroup("Level") as Level;
if (coins.Coins >= priceInt)
{
	coins.Coins -= priceInt;
	GetNode<Label>("RichTextLabel/Control/Panel4/PriceTag").Text = "0";
}
*/