using Godot;
using Managers.Save;
using System;
using System.Collections.Generic;
using Shop;

namespace Managers;

public partial class ShopManager : Node2D
{
	public Dictionary<string, int> panelIndexMap = new()
	{
		{ "Panel1", 0 },
		{ "Panel2", 1 },
		{ "Panel3", 2 },
		{ "Panel4", 3 },
		{ "Panel5", 4 }
	};
	public static ShopManager Instance { get; private set; }
	private ShopMenu shopMenu;
	[Signal] public delegate void OnItemBoughtUpdatePricesEventHandler();
	[Signal] public delegate void OnWeaponUnlockedEventHandler();
	[Signal] public delegate void OnWeaponEquippedEventHandler();
	public List<Stat> statsList = new();
	public List<Weapon> weaponsList = new();
	/*when using, be aware of convention: assignment is dependent on the order and number of the Panels in the scene e.g.
   	healStat is managed in Panel1, so its addressed by the number 1 (or in an array or list by 0)*/
	//Add new shop Stats here
	private Stat healStat = new("Heal", 0);
	private Stat maxHPStat = new("MaxHP", 1);
	private Stat DMGStat = new("DMG", 2);
	private Stat speedStat = new("Speed", 3);
	//Add new shop Weapons here
	private Weapon defaultWeapon = new("Default", 0, Bullet.DefaultBulllet);
	private Weapon bouncingWeapon = new("Bouncing", 1, Bullet.BouncingBullet);
	private Weapon grenadeWeapon = new("Grenade", 2, Bullet.GrenadeBullet); //TODO: change to grenade bullet
	private Weapon rocketWeapon = new("Rocket", 3, Bullet.RocketBullet); 
	private Weapon laserWeapon = new("Laser", 4, Bullet.LaserBullet); //TODO: change to laser bullet

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

		AddStatsToList(healStat, maxHPStat, DMGStat, speedStat);
		AddWeaponsToList(defaultWeapon, bouncingWeapon, grenadeWeapon, rocketWeapon, laserWeapon);

		SaveManager.Instance.OnSaveDataLoaded += OnSaveDataLoaded;
	}

	public override void _ExitTree()
	{
		SaveManager.Instance.OnSaveDataLoaded -= OnSaveDataLoaded;
	}

	public void OnSaveDataLoaded(SaveData saveData) //TODO: savedata for weapons
	{
		LoadShopStateBySaveData(saveData);
	}
	
	///<summary>
	///Tries to upgrade/buy the stat and returns true if the stat was bought, false if not. 
	///It increases the price and quantity of the stat and removes the coins if the stat was bought.
	///</summary>
	public bool BuyStat(Stat stat)
	{
		ShopPrices shopPrices = stat.priceTag as ShopPrices;

		if (CoinManager.Instance.CheckIfEnoughCoins(stat.price + shopPrices.basePrice))
		{
			CoinManager.Instance.RemoveCoins(stat.price + shopPrices.basePrice);

			shopPrices.IncreasePrice(stat);
			IncreaseStatQuantity(stat);
			EmitSignal(SignalName.OnItemBoughtUpdatePrices);
			return true;
		}
		else
		{
			shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
			shopMenu.DisplayInsufficientCoinsError(stat.price + shopPrices.basePrice);
			return false;
		}
	}

	///<summary>
	///Increase the quantity of the stat in the list at the listIndex by 1.
	///</summary>
	public void IncreaseStatQuantity(Stat stat)
	{
		stat.quantity++;
		statsList[stat.listIndex] = stat;
	}

	/// <summary>
	/// Buy the weapon if the player has enough coins and the weapon is not unlocked yet. Set the pricetag label and weapon.unlocked to true, return true.  
	/// If the player has not enough coins, show an error message and return false.
	/// </summary>
	public bool BuyWeapon(Weapon weapon)
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) && weapon.unlocked == false)
		{
			ShopPrices shopPrices = weapon.priceTag as ShopPrices;
			CoinManager.Instance.RemoveCoins(weapon.price);

			ShopPrices.WeaponUnlocked(shopPrices);
			UnlockWeapon(weapon);
			return true;
		}
		else if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) == false && weapon.unlocked == false)
		{
			shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
			shopMenu.DisplayInsufficientCoinsError(weapon.price);
			return false;
		}
		return false;
	}

	public void UnlockWeapon(Weapon weapon)
	{
		weapon.unlocked = true;
		weaponsList[weapon.listIndex] = weapon;
		EmitSignal(SignalName.OnWeaponUnlocked);
	}

	public void EquipWeapon(Weapon weapon)
	{
		UnequipAllWeapon(); //make sure only one weapon is equipped
		weapon.equipped = true;
		weaponsList[weapon.listIndex] = weapon;
		EmitSignal(SignalName.OnWeaponEquipped);
	}

	public Weapon GetEquippedWeapon()
	{
		foreach (var weapon in weaponsList)
		{
			if (weapon.equipped)
			{
				return weapon;
			}
		}
		return weaponsList[0]; //default weapon
	}

	private void UnequipAllWeapon()
	{
		bool[] equipped = new bool[weaponsList.Count];
		for (int i = 0; i < weaponsList.Count; i++)
		{
			equipped[i] = weaponsList[i].equipped;
			equipped[i] = false;
			Weapon weapon = weaponsList[i];
			weapon.equipped = equipped[i];
			weaponsList[i] = weapon;
		}
	}

	private void AddStatsToList(params Stat[] stats)
	{
		HashSet<int> indices = new HashSet<int>();

		foreach (var stat in stats)
		{
			// If the index is already in the HashSet, throw an exception
			if (!indices.Add(stat.listIndex))
			{
				throw new ArgumentException($"Duplicate index: {stat.listIndex}");
			}
			// Ensure the list is large enough
			while (statsList.Count <= stat.listIndex)
			{
				statsList.Add(new Stat());
			}
			statsList[stat.listIndex] = stat;
		}
	}

	private void AddWeaponsToList(params Weapon[] weapons)
	{
		HashSet<int> indices = new HashSet<int>();

		foreach (var weapon in weapons)
		{
			// If the index is already in the HashSet, throw an exception
			if (!indices.Add(weapon.listIndex))
			{
				throw new ArgumentException($"Duplicate index: {weapon.listIndex}");
			}
			// Ensure the list is large enough
			while (weaponsList.Count <= weapon.listIndex)
			{
				weaponsList.Add(new Weapon());
			}
			weaponsList[weapon.listIndex] = weapon;
		}
	}

	private void LoadShopStateBySaveData(SaveData saveData)
	{
		for (int i = 0; i < saveData.Stats.Count; i++)
		{
			Stat stat = statsList[i];
			stat.price = (int)saveData.Stats[i][0];
			stat.quantity = (int)saveData.Stats[i][1];
			statsList[i] = stat;
		}
		for (int i = 0; i < saveData.Weapons.Count; i++)
		{
			Weapon weapon = weaponsList[i];
			weapon.unlocked = (bool)saveData.Weapons[i][0];
			weapon.equipped = (bool)saveData.Weapons[i][1];
			weaponsList[i] = weapon;
		}
	}
}
