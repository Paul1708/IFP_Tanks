using Godot;
using Managers.Level;
using Managers;

namespace Shop;

public struct Stat
{
	public string name;
	public int listIndex;
	public int price;
	public Label priceTag;
	public int quantity;
	public Stat() { }
	public Stat(string name, int listIndex)
	{
		this.name = name;
		this.listIndex = listIndex;
	}
}

public partial class ShopStatsTab : ShopBaseTab
{
	private HScrollBar _hScrollBar;
	private Node2D _control;
	private LevelManager _levelManager;
	private ShopMenu _shopMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

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
		if (PlayerManager.Instance.PlayerHealthComponent.currentHP == PlayerManager.Instance.PlayerHealthComponent.maxHP)
		{
			_shopMenu.DisplayAlreadyMaxHealthError();
		}
		else
		{
			Stat healStat = ShopManager.Instance.statsList[0];
			if (ShopManager.Instance.BuyStat(healStat)) PlayerManager.Instance.PlayerHealthComponent.HealToMax();
		}
	}

	private void OnBuy2Pressed()
	{
		Stat maxHPStat = ShopManager.Instance.statsList[1];
		if (ShopManager.Instance.BuyStat(maxHPStat))
		{
			PlayerManager.Instance.AddMaxHealth(10);
		}

	}

	private void OnBuy3Pressed()
	{
		Stat DMGStat = ShopManager.Instance.statsList[2];
		if (ShopManager.Instance.BuyStat(DMGStat))
		{
			PlayerManager.Instance.AddDamageModifier(0.1f);
		}
	}

	private void OnBuy4Pressed()
	{
		Stat speedStat = ShopManager.Instance.statsList[3];
		ShopManager.Instance.BuyStat(speedStat);
		{
			PlayerManager.Instance.AddMovementSpeed(10f);
		}
	}
}
