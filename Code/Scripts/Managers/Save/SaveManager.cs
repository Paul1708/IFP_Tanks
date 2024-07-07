using System.ComponentModel.DataAnnotations;
using Code.Scripts.Movement;
using Code.Scripts.UI.Shop;
using Godot;

namespace Code.Scripts.Managers.Save;

/// <summary>
/// Manages the saving and loading of the games progress, player stats, and shop state.
/// </summary>
public partial class SaveManager : Node
{
	[Export]
	public PlayerStats InitialPlayerStats { get; set; }
	public static SaveManager Instance { get; private set; }
	public SaveData SaveData { get; private set; }
	public LoadingType LoadingType { get; set; } = LoadingType.None;

	[Signal]
	public delegate void OnSaveDataLoadedEventHandler(SaveData saveData);

	public override void _Ready()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree(); // Ensures there is only one instance of SaveManager
		}

		SaveData = new SaveData();
	}

	/// <summary>
	/// Saves the game. Saves the player's current health, coin count, player stats, and shop state in the SaveData object.
	/// </summary>
	public void SaveGame()
	{
		SaveData.PlayerCurrentHp = PlayerManager.Instance.PlayerHealthComponent.CurrentHp;
		SaveData.CoinCount = CoinManager.Instance.Coins;
		SaveData.PlayerStats = PlayerManager.Instance.PlayerStats;
		SaveShopState();

		ResourceSaver.Save(SaveData, "user://savegame.tres");
		GD.Print("Game Saved");
	}
	
	public void LoadGame(LoadingType loadingType)
	{
		LoadingType = loadingType;
		LoadGame();
	}

	/// <summary>
	/// Loads the game. If LoadingType is set to NewGame, a new SaveData object is created and saved. If LoadingType is set to LoadGame, the SaveData object is loaded from the save file.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when LoadingType is not set.</exception>
	public void LoadGame()
	{

		if (LoadingType == LoadingType.NewGame)
		{
			SaveData = new SaveData();
			ResourceSaver.Save(SaveData, "user://savegame.tres");
			GD.Print("New Game loaded");
		}
		else if (LoadingType == LoadingType.LoadGame)
		{
			SaveData = ResourceLoader.Load<SaveData>("user://savegame.tres");
			GD.Print("Game loaded from save file");
		}
		else
		{
			throw new ValidationException("LoadingType is not set. Please set the LoadingType before calling LoadGame()");
		}
		EmitSignal(SignalName.OnSaveDataLoaded, SaveData);
		LoadingType = LoadingType.None;
	}

	/// <summary>
	/// Checks if the save file is available.
	/// </summary>
	/// <returns>True if the save file is available, false if not.</returns>
	public bool IsSaveFileAvailable()
	{
		return ResourceLoader.Exists("user://savegame.tres");
	}

	/// <summary>
	/// Saves the shop state of Stats and Weapons in the SaveData object.
	/// </summary>
	public void SaveShopState()
	{
		//saves shop state by iterating through the statsList and saving the price and quantity of each stat
		for (int i = 0; i < ShopManager.Instance.StatsList.Count; i++)
		{
			Stat stat = ShopManager.Instance.StatsList[i];
			SaveData.Stats[i][0] = stat.Price;
			SaveData.Stats[i][1] = stat.Quantity;
		}
		for (int i = 0; i < ShopManager.Instance.WeaponsList.Count; i++)
		{
			Weapon weapon = ShopManager.Instance.WeaponsList[i];
			SaveData.Weapons[i][0] = weapon.Unlocked;
			SaveData.Weapons[i][1] = weapon.Equipped;
		}
	}
}

public enum LoadingType
{
	NewGame,
	LoadGame,
	None
}
