using Godot;
using Managers;

public partial class CoinDisplay : TextureRect
{
	Label coinLabel;
	public override void _Ready()
	{
		coinLabel = GetNode<Label>("CoinLabel");
		CoinManager.Instance.OnCoinChanged += ChangeLabel;
	}

	public override void _ExitTree()
	{
		CoinManager.Instance.OnCoinChanged -= ChangeLabel;
	}

	public void ChangeLabel(int coins)
	{
		coinLabel.Text = coins.ToString();
	}
}
