using Code.Scripts.Managers;
using Godot;

namespace Code.Scripts.UI;

/// <summary>
/// Updates the coin display label when the coin count changes
/// </summary>
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

	/// <summary>
	/// Changes the label to the new coin count
	/// </summary>
	/// <param name="coins">The new coin count</param>
	public void ChangeLabel(int coins)
	{
		_coinLabel.Text = coins.ToString();
	}
}
