using Godot;
public partial class CoinDisplay : TextureRect
{
	Label coinLabel;
	public override void _Ready()
	{
		coinLabel = GetNode<Label>("CoinLabel");
		Manager.Instance.CoinManager.OnCoinChanged += ChangeLabel;
	}

	public override void _ExitTree()
	{
		Manager.Instance.CoinManager.OnCoinChanged -= ChangeLabel;
	}

	public void ChangeLabel(int coins)
	{
		coinLabel.Text = coins.ToString();
	}
}
