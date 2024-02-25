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
	private Stat healStat = new("Heal");
	private Stat maxHPStat = new("MaxHP");
	private Stat DMGStat = new("DMG");
	private Stat speedStat = new("Speed");
	//Weapons
	private Weapon defaultWeapon = new("Default");
	private Weapon bouncingWeapon = new("Bouncing");
	private Weapon grenadeWeapon = new("Grenade");
	private Weapon laserWeapon = new("Laser");

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
		for (int i = 0; i < saveData.Stats.Count; i++)
		{
			Stat stat = statsList[i];
			stat.price = (int)saveData.Stats[i][0];
			stat.quantity = (int)saveData.Stats[i][1];
			statsList[i] = stat;
		}
	}

	private void AddStatsToList(params Stat[] stats)
	{
		foreach (var stat in stats)
		{
			statsList.Add(stat);
		}
	}

	private void AddWeaponsToList(params Weapon[] weapons)
	{
		foreach (var weapon in weapons)
		{
			weaponsList.Add(weapon);
		}
	}
}
