using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI.Shop;

public struct Stat
{
	public string Name;
	public int ListIndex;
	public int Price;
	public Label PriceTag;
	public int Quantity;
	public Stat() { }
	public Stat(string name, int listIndex)
	{
		this.Name = name;
		this.ListIndex = listIndex;
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
		if (PlayerManager.Instance.PlayerHealthComponent.CurrentHp == PlayerManager.Instance.PlayerHealthComponent.MaxHp)
		{
			_shopMenu.DisplayAlreadyMaxHealthError();
		}
		else
		{
			Stat healStat = ShopManager.Instance.StatsList[0];
			if (ShopManager.Instance.BuyStat(healStat)) PlayerManager.Instance.PlayerHealthComponent.HealPercentage(0.25f);
		}
	}

	private void OnBuy2Pressed()
	{
		Stat maxHpStat = ShopManager.Instance.StatsList[1];
		if (ShopManager.Instance.BuyStat(maxHpStat))
		{
			PlayerManager.Instance.AddMaxHealth(10);
		}

	}

	private void OnBuy3Pressed()
	{
		Stat dmgStat = ShopManager.Instance.StatsList[2];
		if (ShopManager.Instance.BuyStat(dmgStat))
		{
			PlayerManager.Instance.AddDamageModifier(0.1f);
		}
	}

	private void OnBuy4Pressed()
	{
		Stat speedStat = ShopManager.Instance.StatsList[3];
		if(ShopManager.Instance.BuyStat(speedStat))
			PlayerManager.Instance.AddMovementSpeed(10f);
	}

	private void OnBuy5Pressed()
	{
		Stat speedStat = ShopManager.Instance.StatsList[3];
		if(ShopManager.Instance.BuyStat(speedStat))
			PlayerManager.Instance.AddRotationSpeed(0.25f);
	}
}

