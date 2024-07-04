using Godot;
using Code.Scripts.Managers.Save;
using System;
using System.Collections.Generic;
using Code.Scripts.UI.Shop;
using Code.Scripts.Weapons;

namespace Code.Scripts.Managers;

/// <summary>
/// Manages the shop, including the stats and weapons. It handles the buying, upgrading, unlocking and equipping of stats and weapons.
/// </summary>
public partial class ShopManager : Node2D
{
	[ExportGroup("Weapons")]
	[Export] public WeaponStats DefaultWeaponStats { get; set; }
	[Export] public WeaponStats BouncingWeaponStats { get; set; }
	[Export] public WeaponStats GrenadeWeaponStats { get; set; }
	[Export] public WeaponStats RocketWeaponStats { get; set; }
	[Export] public WeaponStats LaserWeaponStats { get; set; }
	[Export] public WeaponStats MachineGunWeaponStats { get; set; }
	[Signal] public delegate void OnWeaponUnlockedEventHandler();
	[Signal] public delegate void OnWeaponEquippedEventHandler();
	public List<Stat> StatsList = new();
	public List<Weapon> WeaponsList = new();
	public static ShopManager Instance { get; private set; }
	private ShopMenu _shopMenu;
	/*when using, be aware of convention: assignment is dependent on the order and number of the Panels in the scene e.g.
   	_healStat is managed in Panel1, so its addressed by the number 1 (or in an array or list by 0)*/
	//Add new shop Stats here
	private Stat _healStat = new("Heal", 0);
	private Stat _maxHpStat = new("MaxHP", 1);
	private Stat _dmgStat = new("DMG", 2);
	private Stat _speedStat = new("Speed", 3);
	//Add new shop Weapons here
	private Weapon _defaultWeapon;
	private Weapon _bouncingWeapon;
	private Weapon _grenadeWeapon;
	private Weapon _rocketWeapon;
	private Weapon _laserWeapon;
	private Weapon _machineGunWeapon;

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree(); // Ensures there is only one instance of CoinManager
		}

		AddStatsToList(_healStat, _maxHpStat, _dmgStat, _speedStat);

		SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;

		_defaultWeapon = new("Default", 0, DefaultWeaponStats);
		_bouncingWeapon = new("Bouncing", 1, BouncingWeaponStats);
		_grenadeWeapon = new("Grenade", 2, GrenadeWeaponStats);
		_rocketWeapon = new("Rocket", 3, RocketWeaponStats);
		_laserWeapon = new("Laser", 4, LaserWeaponStats);
		_machineGunWeapon = new("MachineGun", 5, MachineGunWeaponStats);
		AddWeaponsToList(_defaultWeapon, _bouncingWeapon, _grenadeWeapon, _rocketWeapon, _laserWeapon, _machineGunWeapon);

	}

	public override void _ExitTree()
	{
		SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
	}

	/// <summary>
	/// Method that will be called when the SaveData is loaded. It will set the Stats and Weapons to the values of the SaveData
	/// </summary>
	/// <param name="saveData">The SaveData that was loaded</param>
	public void OnSaveDataLoaded(SaveData saveData)
	{
		LoadShopStateBySaveData(saveData);
	}

	/// <summary>
	/// Tries to upgrade/buy the stat and returns true if the stat was bought, false if not. 
	/// It increases the price and quantity of the stat and removes the coins if the stat was bought.
	/// </summary>
	/// <param name="stat">The stat to buy</param>
	/// <returns name="bool">True if the stat was bought, false if not</returns>
	public bool BuyStat(Stat stat)
	{
		ShopPrices shopPrices = stat.PriceTag as ShopPrices;
		if (shopPrices == null)
			return false;
		
		int price = stat.Price + shopPrices.BasePrice;

		if (CoinManager.Instance.CheckIfEnoughCoins(price))
		{
			CoinManager.Instance.RemoveCoins(price);

			shopPrices.IncreasePrice(StatsList[stat.ListIndex]);
			IncreaseStatQuantity(StatsList[stat.ListIndex]);
			return true;
		}
		
		_shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		if (_shopMenu == null)
			return false;
			
		_shopMenu.DisplayInsufficientCoinsError(price);
		return false;
	}

	/// <summary>
	/// Increase the quantity of the stat in the list at the listIndex by 1.
	/// </summary>
	/// <param name="stat">The stat to increase the quantity of</param>
	public void IncreaseStatQuantity(Stat stat)
	{
		stat.Quantity++;
		StatsList[stat.ListIndex] = stat;
	}

	/// <summary>
	/// Buy the weapon if the player has enough coins and the weapon is not unlocked yet. Set the pricetag label and weapon.unlocked to true, return true.  
	/// If the player has not enough coins, show an error message and return false.
	/// </summary>
	/// <param name="weapon">The weapon to buy</param>
	/// <returns name="bool">True if the weapon was bought, false if not</returns>
	public bool BuyWeapon(Weapon weapon)
	{
		ShopPrices shopPrices = weapon.PriceTag as ShopPrices;
		if (shopPrices == null)
			return false;
		
		int price = shopPrices.BasePrice;
		
		if (CoinManager.Instance.CheckIfEnoughCoins(price) && !weapon.Unlocked)
		{
			CoinManager.Instance.RemoveCoins(price);

			ShopPrices.WeaponUnlocked(shopPrices);
			UnlockWeapon(weapon);
			return true;
		}
		
		if (!CoinManager.Instance.CheckIfEnoughCoins(price) && !weapon.Unlocked)
		{
			_shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
			if (_shopMenu == null)
				return false;
			_shopMenu.DisplayInsufficientCoinsError(price);
			return false;
		}
		return false;
	}

	/// <summary>
	/// Unlock the weapon by setting the unlocked state to true. Emit the OnWeaponUnlocked signal.
	/// </summary>
	/// <param name="weapon">The weapon to unlock</param>
	public void UnlockWeapon(Weapon weapon)
	{
		weapon.Unlocked = true;
		WeaponsList[weapon.ListIndex] = weapon;
		EmitSignal(SignalName.OnWeaponUnlocked);
	}

	/// <summary>
	/// Equip the weapon by setting the equipped state to true and unequip all other weapons. Emit the OnWeaponEquipped signal.
	/// </summary>
	/// <param name="weapon">The weapon to equip</param>
	public void EquipWeapon(Weapon weapon)
	{
		UnequipAllWeapon(); //make sure only one weapon is equipped
		weapon.Equipped = true;
		WeaponsList[weapon.ListIndex] = weapon;
		EmitSignal(SignalName.OnWeaponEquipped);
	}

	/// <summary>
	/// Returns the equipped weapon. If no weapon is equipped, the default weapon is returned.
	/// </summary>
	/// <returns name="Weapon">The equipped weapon</returns>
	public Weapon GetEquippedWeapon()
	{
		foreach (var weapon in WeaponsList)
		{
			if (weapon.Equipped)
			{
				return weapon;
			}
		}
		return WeaponsList[0]; //default weapon
	}

	/// <summary>
	/// Unequips all weapons by setting the equipped state to false.
	/// </summary>
	private void UnequipAllWeapon()
	{
		bool[] equipped = new bool[WeaponsList.Count];
		for (int i = 0; i < WeaponsList.Count; i++)
		{
			equipped[i] = false;
			Weapon weapon = WeaponsList[i];
			weapon.Equipped = equipped[i];
			WeaponsList[i] = weapon;
		}
	}

	/// <summary>
	/// Tries to get the index of the panel from the node and returns true if successful, false if not.
	/// </summary>
	/// <param name="node">The node to get the panel index from</param>
	/// <param name="index">The index of the panel</param>
	/// <returns name="bool">True if the index was successfully retrieved, false if not</returns>
	public bool TryGetPanelIndex(Node node, out int index)
	{
		string parentName = node.GetParent().Name;
		parentName = parentName.Replace("Panel", "");
		bool parseSuccess = int.TryParse(parentName, out index);

		if (parseSuccess)
		{
			index -= 1; // Subtract 1 from the index
		}

		return parseSuccess;
	}

	/// <summary>
	/// Adds the stats to the list and checks for duplicate indices. When adding a new stat, add it to the list here.
	/// </summary>
	/// <param name="stats">The stats to add to the list</param>
	/// <exception cref="ArgumentException">Thrown when a duplicate index is found</exception>
	private void AddStatsToList(params Stat[] stats)
	{
		HashSet<int> indices = new HashSet<int>();

		foreach (var stat in stats)
		{
			// If the index is already in the HashSet, throw an exception
			if (!indices.Add(stat.ListIndex))
			{
				throw new ArgumentException($"Duplicate index: {stat.ListIndex}");
			}
			// Ensure the list is large enough
			while (StatsList.Count <= stat.ListIndex)
			{
				StatsList.Add(new Stat());
			}
			StatsList[stat.ListIndex] = stat;
		}
	}

	/// <summary>
	/// Adds the weapons to the list and checks for duplicate indices. When adding a new weapon, add it to the list here.
	/// </summary>
	/// <param name="weapons">The weapons to add to the list</param>
	/// <exception cref="ArgumentException">Thrown when a duplicate index is found</exception>
	private void AddWeaponsToList(params Weapon[] weapons)
	{
		HashSet<int> indices = new HashSet<int>();

		foreach (var weapon in weapons)
		{
			// If the index is already in the HashSet, throw an exception
			if (!indices.Add(weapon.ListIndex))
			{
				throw new ArgumentException($"Duplicate index: {weapon.ListIndex}");
			}
			// Ensure the list is large enough
			while (WeaponsList.Count <= weapon.ListIndex)
			{
				WeaponsList.Add(new Weapon());
			}
			WeaponsList[weapon.ListIndex] = weapon;
		}
	}

	/// <summary>
	/// Load the shop state by the save data. This includes the prices and quantities of the stats and the unlocked and equipped state of the weapons.
	/// </summary>
	/// <param name="saveData">The save data to load the shop state from</param>
	private void LoadShopStateBySaveData(SaveData saveData)
	{
		for (int i = 0; i < saveData.Stats.Count; i++)
		{
			Stat stat = StatsList[i];
			stat.Price = (int)saveData.Stats[i][0];
			stat.Quantity = (int)saveData.Stats[i][1];
			StatsList[i] = stat;
		}
		for (int i = 0; i < saveData.Weapons.Count; i++)
		{
			Weapon weapon = WeaponsList[i];
			weapon.Unlocked = (bool)saveData.Weapons[i][0];
			weapon.Equipped = (bool)saveData.Weapons[i][1];
			WeaponsList[i] = weapon;
		}
	}
}
