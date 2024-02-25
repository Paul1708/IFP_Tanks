using Godot;
using Managers.Save;
using System;
using System.Collections.Generic;


public partial class ShopManager : Node2D
{
	public static ShopManager Instance { get; private set; }
	public List<Stat> statsList = new();
	public List<Weapon> weaponsList = new();

	/*when using, be aware of convention: assignment is dependent on the order and number of the Panels in the scene e.g.
   	healStat is managed in Panel1, so its addressed by the number 1 (or in an array or list by 0)*/
	//Stats
	private Stat healStat = new("Heal", 0);
	private Stat maxHPStat = new("MaxHP", 1);
	private Stat DMGStat = new("DMG", 2);
	private Stat speedStat = new("Speed", 3);
	//Weapons
	private Weapon defaultWeapon = new("Default", 0);
	private Weapon bouncingWeapon = new("Bouncing", 1);
	private Weapon grenadeWeapon = new("Grenade", 2);
	private Weapon laserWeapon = new("Laser", 3);

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
		AddWeaponsToList(defaultWeapon, bouncingWeapon, grenadeWeapon, laserWeapon);
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

	private void AddStatsToList(params Stat[] stats)
	{
		foreach (var stat in stats)
		{
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
		foreach (var weapon in weapons)
		{
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
