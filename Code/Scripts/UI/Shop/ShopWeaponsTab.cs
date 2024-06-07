using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Code.Scripts.Weapons;
using Godot;

namespace Code.Scripts.UI.Shop;

public struct Weapon
{
	public string Name;
	public int ListIndex;
	public Label PriceTag;
	public bool Unlocked = false;
	public bool Equipped = false;
	public WeaponStats WeaponStats;
	public Weapon() { }
	public Weapon(string name, int listIndex, WeaponStats weaponStats)
	{
		this.Name = name;
		this.ListIndex = listIndex;
		this.WeaponStats = weaponStats;
	}
}

public partial class ShopWeaponsTab : ShopBaseTab
{
	private HScrollBar _hScrollBar;
	private Node2D _control;
	private LevelManager _levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hScrollBar = GetNode<HScrollBar>("HScrollBar");
		_control = GetNode<Node2D>("RichTextLabel/Control");
		
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		_levelManager.OnLevelChanged += ResetScrollBar;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Scroll();
	}

	protected override void ResetScrollBar()
	{
		_hScrollBar.Value = 0;
	}

	protected override void Scroll()
	{
		Vector2 position = _control.Position;
		position.X = (float)-_hScrollBar.Value;
		_control.Position = position;
	}

	private void OnBuy1Pressed()
	{
		Weapon defaultWeapon = ShopManager.Instance.WeaponsList[0];
		if (defaultWeapon.Unlocked) ShopManager.Instance.EquipWeapon(defaultWeapon);
		else ShopManager.Instance.BuyWeapon(defaultWeapon);

	}

	private void OnBuy2Pressed()
	{
		Weapon bouncingWeapon = ShopManager.Instance.WeaponsList[1];
		if (bouncingWeapon.Unlocked) ShopManager.Instance.EquipWeapon(bouncingWeapon);
		else ShopManager.Instance.BuyWeapon(bouncingWeapon);

	}

	private void OnBuy3Pressed()
	{
		Weapon grenadeWeapon = ShopManager.Instance.WeaponsList[2];
		if (grenadeWeapon.Unlocked) ShopManager.Instance.EquipWeapon(grenadeWeapon);
		else ShopManager.Instance.BuyWeapon(grenadeWeapon);

	}

	private void OnBuy4Pressed()
	{
		Weapon rocketWeapon = ShopManager.Instance.WeaponsList[3];
		if (rocketWeapon.Unlocked) ShopManager.Instance.EquipWeapon(rocketWeapon);
		else ShopManager.Instance.BuyWeapon(rocketWeapon);
	}

	private void OnBuy5Pressed()
	{
		Weapon laserWeapon = ShopManager.Instance.WeaponsList[4];
		if (laserWeapon.Unlocked) ShopManager.Instance.EquipWeapon(laserWeapon);
		else ShopManager.Instance.BuyWeapon(laserWeapon);
	}
	private void OnBuy6Pressed()
	{
		Weapon machineGunWeapon = ShopManager.Instance.WeaponsList[5];
		if (machineGunWeapon.Unlocked) ShopManager.Instance.EquipWeapon(machineGunWeapon);
		else ShopManager.Instance.BuyWeapon(machineGunWeapon);
	}
}