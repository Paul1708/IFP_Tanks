using Godot;
using System;
using Managers.Level;
using Managers;
using Items;
using System.Linq;
using System.Collections.Generic;

public struct Weapon
{
	public string name;
	public int price;
	public Label priceTag;
	public bool unlocked = false;
	public bool equipped = false;
	public Weapon(string name)
	{
		this.name = name;
	}
}

public partial class ShopWeaponsTab : ShopBaseTab
{
	ShopMenu shopMenu;
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		hScrollBar = GetNode<HScrollBar>("HScrollBar");
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		levelManager.OnLevelChanged += ResetScrollBar;
		
		GetPriceTags();
		GetWeaponPrices();
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

	private void UnlockWeapon(int listIndex)
	{
		Weapon weapon = ShopManager.Instance.weaponsList[listIndex];
		weapon.unlocked = true;
		ShopManager.Instance.weaponsList[listIndex] = weapon;
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
		else if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) == false && weapon.unlocked == false)
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
		Label[] weaponPriceTags = new Label[ShopManager.Instance.weaponsList.Count];

		for (int i = 0; i < ShopManager.Instance.weaponsList.Count; i++)
		{
			weaponPriceTags[i] = GetPriceTagByPanel(i + 1);
			Weapon weapon = ShopManager.Instance.weaponsList[i];
			weapon.priceTag = weaponPriceTags[i];
			ShopManager.Instance.weaponsList[i] = weapon;
		}
	}

	///<summary>
	///Update the prices of the items in the shop by parsing the price from the price tags.
	/// </summary>
	private void GetWeaponPrices()
	{
		int[] weaponPrices = new int[ShopManager.Instance.weaponsList.Count];

		for (int i = 0; i < ShopManager.Instance.weaponsList.Count; i++)
		{
			weaponPrices[i] = ParsePrice(i+1);
			Weapon weapon = ShopManager.Instance.weaponsList[i];
			weapon.price = weaponPrices[i];
			ShopManager.Instance.weaponsList[i] = weapon;
		}
	}
 
	private void OnBuy1Pressed()
	{
		if (BuyWeapon(ShopManager.Instance.weaponsList[0]))
		{
			UnlockWeapon(0);
		}
	}

	private void OnBuy2Pressed()
	{
		if (BuyWeapon(ShopManager.Instance.weaponsList[1]))
		{
			UnlockWeapon(1);
		}
	}

	private void OnBuy3Pressed()
	{
		if (BuyWeapon(ShopManager.Instance.weaponsList[2]))
		{
			UnlockWeapon(2);
		}
	}

	private void OnBuy4Pressed()
	{
		if (BuyWeapon(ShopManager.Instance.weaponsList[3]))
		{
			UnlockWeapon(3);
		}
	}
}