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
		OnItemBoughtUpdatePrices += UpdatePrices;

		//TODO: Save file values
		CreateItemQuantityDictionary();
		SetItemQuantity(new int[] { 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1 });
		UpdatePrices();
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
			shopPrices.UpdatePrice(priceTag, price, itemQuantity + 1);
			EmitSignal(SignalName.OnItemBoughtUpdatePrices);
		}
		else
		{
			var neededCoins = price - CoinManager.Instance.Coins;
			errorLabel.Text = "You need " + neededCoins + " more coins to buy this item!";
			errorPanel.Show();
		}
	}

	private int ParsePrice(string tabName, int index)
	{
		string stringToBeReplaced = "Price: ";
		string nodePath = $"TabContainer/{tabName}/RichTextLabel/Control/Panel{index + 1}/PriceTag";
		string priceText = GetNode<Label>(nodePath).Text.Replace(stringToBeReplaced, "");
		return int.Parse(priceText);
	}

	public void UpdatePrices()
	{
		int[] statsPrices = new int[statsTab.itemCount];
		int[] weaponsPrices = new int[weaponsTab.itemCount];

		for (int i = 0; i < statsTab.itemCount; i++)
		{
			statsPrices[i] = ParsePrice("Stats", i);
			statsTab.prices[i] = statsPrices[i];
		}
		for (int i = 0; i < weaponsTab.itemCount; i++)
		{
			weaponsPrices[i] = ParsePrice("Weapons", i);
			weaponsTab.prices[i] = weaponsPrices[i];
		}
	}

	private void CreateItemQuantityDictionary()
	{
		itemActions = new Dictionary<string, Action<int>>
		{
			{ "Item1", quantity => statsTab.itemQuantities[0]++ },
			{ "Item2", quantity => statsTab.itemQuantities[1]++ },
			{ "Item3", quantity => statsTab.itemQuantities[2]++ },
			{ "Item4", quantity => statsTab.itemQuantities[3]++ },
			{ "tab2Item1", quantity => weaponsTab.itemQuantities[0]++ },
			{ "tab2Item2", quantity => weaponsTab.itemQuantities[1]++ },
			{ "tab2Item3", quantity => weaponsTab.itemQuantities[2]++ },
			{ "tab2Item4", quantity => weaponsTab.itemQuantities[3]++ },
		};
	}

	public void SetItemQuantity(int[] statsTabQuantities, int[] weaponsTabQuantities)
	{
		for (int i = 0; i < statsTab.itemCount; i++)
		{
			statsTab.itemQuantities[i] = statsTabQuantities[i];
		}
		for (int i = 0; i < weaponsTab.itemCount; i++)
		{
			weaponsTab.itemQuantities[i] = weaponsTabQuantities[i];
		}
	}
}
