using Godot;
using Managers.Level;
using Managers;

namespace Shop;

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
	public static readonly PackedScene RocketBullet = GD.Load<PackedScene>("res://Scenes/Weapons/HomingBullet.tscn");
	public static readonly PackedScene GrenadeBullet = GD.Load<PackedScene>("res://Scenes/Weapons/GrenadeBullet.tscn");
	public static readonly PackedScene LaserBullet = GD.Load<PackedScene>("res://Scenes/Weapons/LaserBullet.tscn");
}

public partial class ShopWeaponsTab : ShopBaseTab
{
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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

	protected override void ResetScrollBar()
	{
		hScrollBar.Value = 0;
	}

	protected override void Scroll()
	{
		Vector2 position = control.Position;
		position.X = (float)-hScrollBar.Value;
		control.Position = position;
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
		else ShopManager.Instance.BuyWeapon(defaultWeapon);

	}

	private void OnBuy2Pressed()
	{
		Weapon bouncingWeapon = ShopManager.Instance.weaponsList[1];
		if (bouncingWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(bouncingWeapon);
		else ShopManager.Instance.BuyWeapon(bouncingWeapon);

	}

	private void OnBuy3Pressed()
	{
		Weapon grenadeWeapon = ShopManager.Instance.weaponsList[2];
		if (grenadeWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(grenadeWeapon);
		else ShopManager.Instance.BuyWeapon(grenadeWeapon);

	}

	private void OnBuy4Pressed()
	{
		Weapon rocketWeapon = ShopManager.Instance.weaponsList[3];
		if (rocketWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(rocketWeapon);
		else ShopManager.Instance.BuyWeapon(rocketWeapon);
	}

	private void OnBuy5Pressed()
	{
		Weapon laserWeapon = ShopManager.Instance.weaponsList[4];
		if (laserWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(laserWeapon);
		else ShopManager.Instance.BuyWeapon(laserWeapon);
	}
}