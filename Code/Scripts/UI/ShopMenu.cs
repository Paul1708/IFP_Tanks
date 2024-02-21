using Godot;
using Managers;
using Movement;
using System;
using Managers.Level;


public partial class ShopMenu : Control
{
	AnimationPlayer animationPlayer;
	Level level;
	LevelManager levelManager;
	MusicController musicController;
	Label errorLabel;
	Panel errorPanel;
	ShopUpgradeStatsTab statsTab;
	ShopEquipWeaponsTab weaponsTab;

	[Signal]
	public delegate void OnShopMenuClosedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		
		statsTab = GetNode<ShopUpgradeStatsTab>("TabContainer/Stats");
		weaponsTab = GetNode<ShopEquipWeaponsTab>("TabContainer/Weapons");

		errorLabel = GetNode<Label>("ErrorScreen/Label");
		errorPanel = GetNode<Panel>("ErrorScreen");

		musicController = GetNode<MusicController>("/root/MusicController");
		animationPlayer = GetNode<AnimationPlayer>("BlurAnimation");
		level = GetTree().GetFirstNodeInGroup("Level") as Level;
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;		

		level.OnCoinsMoved += ShowShopMenu;
		levelManager.OnLevelChanged += UpdateSetup;

		GetPrices();
	}

	//Called when a new level is loaded to update the level reference
	public void UpdateSetup()
	{
		level = GetTree().GetFirstNodeInGroup("Level") as Level;
		level.OnCoinsMoved += ShowShopMenu;
	}

	//Show the shop menu and pause the game
	public void ShowShopMenu()
	{	
		GetTree().Paused = true;
		Show();
		animationPlayer.Play("StartPause");
	}

	//Hide the shop menu, unpause the game and tell the level to continue by emitting the OnLevelComplete signal
	public void OnContinueButtonPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().Paused = false;
		Hide();
		level.SendOnLevelComplete();
	}

	//Return to the main menu
	private void OnMainMenuPressed()
	{
		musicController.Play(Sound.ButtonClick);

		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}

	private void OnErrorAcknowledgedPressed() 
	{
		errorPanel.Hide();
	}

	public void Buy(int price) 
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(price)) 
		{
			CoinManager.Instance.RemoveCoins(price);
		}
		else 
		{
			var neededCoins = price - CoinManager.Instance.Coins;
			errorLabel.Text = "You need " + neededCoins + " more coins to buy this item!";
			errorPanel.Show();
		}
	}

	public void GetPrices() 
	{
		string stringToBeReplaced = "Price: ";
		statsTab.price1 =  int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel1/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price2 =  int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel2/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price3 =  int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel3/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price4 =  int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel4/PriceTag").Text.Replace(stringToBeReplaced, ""));

		weaponsTab.price1 =  int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel1/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price2 =  int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel2/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price3 =  int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel3/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price4 =  int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel4/PriceTag").Text.Replace(stringToBeReplaced, ""));
	}
}
