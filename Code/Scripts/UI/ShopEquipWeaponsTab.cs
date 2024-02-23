using Godot;
using System;
using Managers.Level;
using Managers;
using Items;
using System.Linq;
using System.Collections.Generic;

public partial class ShopEquipWeaponsTab : ShopBaseTab
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	/*when using, be aware that assignment is dependent on the order of the Panels in the scene:
	 default is managed in Panel1, so its addressed by the number 1 (or in an array or list by 0)*/
	private Weapon defaultWeapon = new("Default");
	private Weapon bouncingWeapon = new("Bouncing");
	private Weapon grenadeWeapon = new("Grenade");
	private Weapon laserWeapon = new("Laser");
	public List<Weapon> weaponsList = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		hScrollBar = GetNode<HScrollBar>("HScrollBar");
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		levelManager.OnLevelChanged += ResetScrollBar;
		
		//example: defaultWeapon is managed in Panel1 and adressed by number 0 in the list
		AddStatsToList(defaultWeapon, bouncingWeapon, grenadeWeapon, laserWeapon);
		GetPriceTags();
		GetWeaponPrices();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Scroll();
	}

	private void AddStatsToList(params Weapon[] weapons)
	{
		foreach (var weapon in weapons)
		{
			weaponsList.Add(weapon);
		}
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

	private void UnlockWeapon(Weapon weapon, int listIndex)
	{
		weapon.unlocked = true;
		weaponsList[listIndex] = weapon;
	}

	/// <summary>
	/// Buy the weapon if the player has enough coins and the weapon is not unlocked yet. Set the pricetag label to Unlocked, return true.  
	/// If the player has not enough coins, show an error message and return false.
	/// </summary>
	private bool BuyWeapon(Weapon weapon)
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) && weapon.unlocked == false)
		{
			ShopPrices shopPrices = weapon.priceTag as ShopPrices;
			CoinManager.Instance.RemoveCoins(weapon.price);
			
			shopPrices.WeaponUnlocked(shopPrices);
			return true;
		}
		else if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) == false)
		{
			shopMenu.ShowError(weapon.price);
			return false;
		}
		return false;
	}

	///<summary>
	///get all price tags from the scene by their path that only differs in the Panel number and connect them to the weapons
	/// </summary>
	private void GetPriceTags()
	{
		Label[] weaponPriceTags = new Label[weaponsList.Count];

		for (int i = 0; i < weaponsList.Count; i++)
		{
			weaponPriceTags[i] = GetPriceTagByPanel(i + 1);
			Weapon weapon = weaponsList[i];
			weapon.priceTag = weaponPriceTags[i];
			weaponsList[i] = weapon;
		}
	}

	///<summary>
	///Update the prices of the items in the shop by parsing the price from the price tags and updating the price in the list
	/// </summary>
	private void GetWeaponPrices()
	{
		int[] weaponPrices = new int[weaponsList.Count];

		for (int i = 0; i < weaponsList.Count; i++)
		{
			weaponPrices[i] = ParsePrice(i+1);
			Weapon weapon = weaponsList[i];
			weapon.price = weaponPrices[i];
			weaponsList[i] = weapon;
		}
	}
 
	private void OnBuy1Pressed()
	{
		if (BuyWeapon(weaponsList[0]))
		{
			UnlockWeapon(weaponsList[0], 0);
		}
	}

	private void OnBuy2Pressed()
	{
		if (BuyWeapon(weaponsList[1]))
		{
			UnlockWeapon(weaponsList[1], 1);
		}
	}

	private void OnBuy3Pressed()
	{
		if (BuyWeapon(weaponsList[2]))
		{
			UnlockWeapon(weaponsList[2], 2);
		}
	}

	private void OnBuy4Pressed()
	{
		if (BuyWeapon(weaponsList[3]))
		{
			UnlockWeapon(weaponsList[3], 3);
		}
	}
}