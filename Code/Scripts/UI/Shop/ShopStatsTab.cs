using Godot;
using Managers.Level;
using Managers;
using Movement;

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
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	ShopMenu shopMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

		ShopManager.Instance.OnItemBoughtUpdatePrices += UpdatePrices;

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

	public override void _ExitTree()
	{
		ShopManager.Instance.OnItemBoughtUpdatePrices -= UpdatePrices;
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
	///Get all price tags from the scene by their path that only differs in the Panel number and connect them to the stats.
	/// </summary>
	protected override void GetPriceTags()
	{
		Label[] statsPriceTags = new Label[ShopManager.Instance.statsList.Count];

		for (int i = 0; i < ShopManager.Instance.statsList.Count; i++)
		{
			statsPriceTags[i] = GetPriceTagByPanel(i + 1);
			Stat stat = ShopManager.Instance.statsList[i];
			stat.priceTag = statsPriceTags[i];
			ShopManager.Instance.statsList[i] = stat;
		}
	}

	///<summary>
	///Update the prices of the items in the shop by parsing the price from the price tags and updating the price in the list.
	///The price stored in the list is the difference between the parsed price and the base price of the item.
	/// </summary>
	protected override void UpdatePrices()
	{
		int[] statsPrices = new int[ShopManager.Instance.statsList.Count];

		for (int i = 0; i < ShopManager.Instance.statsList.Count; i++)
		{
			statsPrices[i] = ParsePrice(i + 1);
			Stat stat = ShopManager.Instance.statsList[i];
			ShopPrices shopPrices = stat.priceTag as ShopPrices;
			int basePrice = shopPrices.basePrice;
			stat.price = statsPrices[i] - basePrice;
			ShopManager.Instance.statsList[i] = stat;
		}
	}

	private void OnBuy1Pressed()
	{
		if (PlayerManager.Instance.PlayerHealthComponent.currentHP == PlayerManager.Instance.PlayerHealthComponent.maxHP)
		{
			shopMenu.DisplayAlreadyMaxHealthError();
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
		if (ShopManager.Instance.BuyStat(maxHPStat)) PlayerManager.Instance.PlayerHealthComponent.IncreaseMaxHealth(10);

	}

	private void OnBuy3Pressed()
	{
		Stat DMGStat = ShopManager.Instance.statsList[2];
		ShopManager.Instance.BuyStat(DMGStat);
	}

	private void OnBuy4Pressed()
	{
		Stat speedStat = ShopManager.Instance.statsList[3];
		ShopManager.Instance.BuyStat(speedStat);
	}
}
