using Code.Scripts.Audio;
using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI.Shop;

/// <summary>
/// Custom control class for the shop menu, which handles opening and closing the shop menu
/// </summary>
public partial class ShopMenu : Control
{
	private AnimationPlayer _animationPlayer;
	private LevelManager _levelManager;
	private MusicController _musicController;
	private Label _errorLabel;
	private Panel _errorPanel;
	[Signal] public delegate void OnShopMenuClosedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();

		_errorLabel = GetNode<Label>("ErrorScreen/Label");
		_errorPanel = GetNode<Panel>("ErrorScreen");

		_musicController = GetNode<MusicController>("/root/MusicController");
		_animationPlayer = GetNode<AnimationPlayer>("BlurAnimation");
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		_levelManager.OnLevelChangedShowShop += ShowShopMenu;
	}

	public override void _ExitTree()
	{
		_levelManager.OnLevelChangedShowShop -= ShowShopMenu;
	}

	/// <summary>
	/// Show the shop menu and pause the game. Plays OpenShop sound.
	/// </summary>
	public void ShowShopMenu()
	{
		GetTree().Paused = true;
		Show();
		_animationPlayer.Play("StartPause");
		_musicController.Play(Sound.OpenShop);
	}

	/// <summary>
	/// Hide the shop menu, unpause the game and tell the level to continue by emitting the OnLevelComplete signal
	/// </summary>
	public void OnContinueButtonPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		GetTree().Paused = false;
		Hide();
		EmitSignal(SignalName.OnShopMenuClosed);
	}

	/// <summary>
	/// Return to the main menu
	/// </summary>
	private void OnMainMenuPressed()
	{
		_musicController.Play(Sound.ButtonClick);

		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}

	/// <summary>
	/// Hide the error panel
	/// </summary>
	private void OnErrorAcknowledgedPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		_errorPanel.Hide();
	}

	/// <summary>
	/// Display an error message when the player tries to buy an item with insufficient coins
	/// </summary>
	/// <param name="price">The price of the item</param>
	public void DisplayInsufficientCoinsError(int price)
	{
		var neededCoins = price - CoinManager.Instance.Coins;
		_errorLabel.Text = "You need " + neededCoins + " more coins to buy this item!";
		_errorPanel.Show();
	}

	/// <summary>
	/// Display an error message when the player tries to buy "heal" when they already have max health
	/// </summary>
	public void DisplayAlreadyMaxHealthError()
	{
		_errorLabel.Text = "You already have the maximum health!";
		_errorPanel.Show();
	}
}
