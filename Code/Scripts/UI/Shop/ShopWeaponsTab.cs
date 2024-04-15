using Godot;
using Managers.Level;
using Managers;

namespace Shop;

public struct Weapon
{
	public string name;
	public int listIndex;
	public Label priceTag;
	public bool unlocked = false;
	public bool equipped = false;
	public WeaponStats weaponStats;
	public Weapon() { }
	public Weapon(string name, int listIndex, WeaponStats weaponStats)
	{
		this.name = name;
		this.listIndex = listIndex;
		this.weaponStats = weaponStats;
	}
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
	private void OnBuy6Pressed()
	{
		Weapon machineGunWeapon = ShopManager.Instance.weaponsList[5];
		if (machineGunWeapon.unlocked == true) ShopManager.Instance.EquipWeapon(machineGunWeapon);
		else ShopManager.Instance.BuyWeapon(machineGunWeapon);
	}
}