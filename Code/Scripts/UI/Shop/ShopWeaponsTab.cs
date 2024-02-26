using Godot;
using Managers.Level;
using Managers;

public struct Weapon
{
	public string name;
	public int listIndex;
	public int price;
	public Label priceTag;
	public bool unlocked = false;
	public bool equipped = false;
	public PackedScene bulletScene;
	public Weapon() { }
	public Weapon(string name, int listIndex, PackedScene bulletScene)
	{
		this.name = name;
		this.listIndex = listIndex;
		this.bulletScene = bulletScene;
	}
}

readonly struct Bullet
{
	public static readonly PackedScene DefaultBulllet = GD.Load<PackedScene>("res://Scenes/Weapons/50cal.tscn");
	public static readonly PackedScene BouncingBullet = GD.Load<PackedScene>("res://Scenes/Weapons/BouncingBullet.tscn");
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
		UpdatePrices();
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

	private void UnlockWeapon(Weapon weapon)
	{
		weapon.unlocked = true;
		ShopManager.Instance.weaponsList[weapon.listIndex] = weapon;
	}

	/// <summary>
	/// Buy the weapon if the player has enough coins and the weapon is not unlocked yet. Set the pricetag label and weapon.unlocked to true, return true.  
	/// If the player has not enough coins, show an error message and return false.
	/// </summary>
	private bool Buy(Weapon weapon)
	{
		if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) && weapon.unlocked == false)
		{
			ShopPrices shopPrices = weapon.priceTag as ShopPrices;
			CoinManager.Instance.RemoveCoins(weapon.price);

			shopPrices.WeaponUnlocked(shopPrices);
			UnlockWeapon(weapon);
			return true;
		}
		else if (CoinManager.Instance.CheckIfEnoughCoins(weapon.price) == false && weapon.unlocked == false)
		{
			shopMenu.DisplayInsufficientCoinsError(weapon.price);
			return false;
		}
		return false;
	}

	///<summary>
	///get all price tags from the scene by their path that only differs in the Panel number and connect them to the weapons
	/// </summary>
	protected override void GetPriceTags()
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
	protected override void UpdatePrices()
	{
		int[] weaponPrices = new int[ShopManager.Instance.weaponsList.Count];

		for (int i = 0; i < ShopManager.Instance.weaponsList.Count; i++)
		{
			weaponPrices[i] = ParsePrice(i + 1);
			Weapon weapon = ShopManager.Instance.weaponsList[i];
			weapon.price = weaponPrices[i];
			ShopManager.Instance.weaponsList[i] = weapon;
		}
	}

	private void OnBuy1Pressed()
	{
		Weapon defaultWeapon = ShopManager.Instance.weaponsList[0];
		if (defaultWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(defaultWeapon);
		else Buy(defaultWeapon);

	}

	private void OnBuy2Pressed()
	{
		Weapon bouncingWeapon = ShopManager.Instance.weaponsList[1];
		if (bouncingWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(bouncingWeapon);
		else Buy(bouncingWeapon);

	}

	private void OnBuy3Pressed()
	{
		Weapon grenadeWeapon = ShopManager.Instance.weaponsList[2];
		if (grenadeWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(grenadeWeapon);
		else Buy(grenadeWeapon);

	}

	private void OnBuy4Pressed()
	{
		Weapon laserWeapon = ShopManager.Instance.weaponsList[3];
		if (laserWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(laserWeapon);
		else Buy(laserWeapon);
	}
}