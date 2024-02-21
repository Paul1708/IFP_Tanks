using Godot;
using Managers;
using Movement;
using System;
using Managers.Level;
using System.Collections.Generic;

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

	private Dictionary<string, Action<int>> itemActions;

	[Signal] public delegate void OnShopMenuClosedEventHandler();
	[Signal] public delegate void OnItemBoughtUpdatePricesEventHandler();

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
		OnItemBoughtUpdatePrices += GetPrices;
		
		//TODO: Save file values
		CreateItemQuantityDictionary();
		SetItemQuantity(1, 1, 1, 1, 1, 1, 1, 1);
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

	private void IncreaseItemQuantity(int itemQuantity, string itemName)
	{
		if (itemActions.ContainsKey(itemName))
		{
			itemActions[itemName](itemQuantity);
		}
		else
		{
			GD.Print("Unknown item name: " + itemName);
		}
	}

	public void Buy(int price, Label priceTag, int itemQuantity, string itemName)
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(price))
		{
			ShopPrices shopPrices = priceTag as ShopPrices;
			CoinManager.Instance.RemoveCoins(price);
			IncreaseItemQuantity(itemQuantity, itemName);
			shopPrices.UpdatePrice(priceTag, price, itemQuantity+1);
			EmitSignal(SignalName.OnItemBoughtUpdatePrices);
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
		statsTab.price1 = int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel1/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price2 = int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel2/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price3 = int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel3/PriceTag").Text.Replace(stringToBeReplaced, ""));
		statsTab.price4 = int.Parse(GetNode<Label>("TabContainer/Stats/RichTextLabel/Control/Panel4/PriceTag").Text.Replace(stringToBeReplaced, ""));

		weaponsTab.price1 = int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel1/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price2 = int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel2/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price3 = int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel3/PriceTag").Text.Replace(stringToBeReplaced, ""));
		weaponsTab.price4 = int.Parse(GetNode<Label>("TabContainer/Weapons/RichTextLabel/Control/Panel4/PriceTag").Text.Replace(stringToBeReplaced, ""));
	}

	private void CreateItemQuantityDictionary()
	{
		itemActions = new Dictionary<string, Action<int>>
		{
			{ "Item1", quantity => statsTab.itemQuantity1++ },
			{ "Item2", quantity => statsTab.itemQuantity2++ },
			{ "Item3", quantity => statsTab.itemQuantity3++ },
			{ "Item4", quantity => statsTab.itemQuantity4++ },
			{ "2Item1", quantity => weaponsTab.itemQuantity1++ },
			{ "2Item2", quantity => weaponsTab.itemQuantity2++ },
			{ "2Item3", quantity => weaponsTab.itemQuantity3++ },
			{ "2Item4", quantity => weaponsTab.itemQuantity4++ },
		};
	}

	public void SetItemQuantity(int itemQuantity1, int itemQuantity2, int itemQuantity3, int itemQuantity4, int tab2ItemQuantity1, int tab2ItemQuantity2, int tab2ItemQuantity3, int tab2ItemQuantity4)
	{
		statsTab.itemQuantity1 = itemQuantity1;
		statsTab.itemQuantity2 = itemQuantity2;
		statsTab.itemQuantity3 = itemQuantity3;
		statsTab.itemQuantity4 = itemQuantity4;

		weaponsTab.itemQuantity1 = tab2ItemQuantity1;
		weaponsTab.itemQuantity2 = tab2ItemQuantity2;
		weaponsTab.itemQuantity3 = tab2ItemQuantity3;
		weaponsTab.itemQuantity4 = tab2ItemQuantity4;
	}
}
