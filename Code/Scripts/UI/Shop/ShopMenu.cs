using Godot;
using Managers;
using Managers.Level;

namespace Shop;

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

	//Show the shop menu and pause the game
	public void ShowShopMenu()
	{
		GetTree().Paused = true;
		Show();
		_animationPlayer.Play("StartPause");
		_musicController.Play(Sound.OpenShop);
	}

	//Hide the shop menu, unpause the game and tell the level to continue by emitting the OnLevelComplete signal
	public void OnContinueButtonPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		GetTree().Paused = false;
		Hide();
		EmitSignal(SignalName.OnShopMenuClosed);
	}

	//Return to the main menu
	private void OnMainMenuPressed()
	{
		_musicController.Play(Sound.ButtonClick);

		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}

	private void OnErrorAcknowledgedPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		_errorPanel.Hide();
	}

	public void DisplayInsufficientCoinsError(int price)
	{
		//TODO: play error sound
		var neededCoins = price - CoinManager.Instance.Coins;
		_errorLabel.Text = "You need " + neededCoins + " more coins to buy this item!";
		_errorPanel.Show();
	}

	public void DisplayAlreadyMaxHealthError()
	{
		//TODO: play error sound
		_errorLabel.Text = "You already have the maximum health!";
		_errorPanel.Show();
	}
}
