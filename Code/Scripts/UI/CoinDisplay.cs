using Godot;
using Managers;

public partial class CoinDisplay : TextureRect
{
	private Label _coinLabel;
	
	public override void _Ready()
	{
		_coinLabel = GetNode<Label>("CoinLabel");
		CoinManager.Instance.OnCoinChanged += ChangeLabel;
		ChangeLabel(CoinManager.Instance.Coins);
	}

	public override void _ExitTree()
	{
		CoinManager.Instance.OnCoinChanged -= ChangeLabel;
	}

	public void ChangeLabel(int coins)
	{
		_coinLabel.Text = coins.ToString();
	}
}
