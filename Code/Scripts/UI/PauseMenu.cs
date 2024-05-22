using Godot;
using UI;
using Shop;

public partial class PauseMenu : Control
{
	protected MusicController musicController;
	protected SettingsMenu settingsMenu;
	protected ShopMenu shopMenu;
	protected LevelCountdown levelCountdown;
	private bool _leavePauseMenu = false;
	private LevelFailed _levelFailed;
	private VictoryScreen _victoryScreen;

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
		settingsMenu = GetNode<SettingsMenu>("SettingsMenu");
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		levelCountdown = GetTree().GetFirstNodeInGroup("LevelCountdown") as LevelCountdown;
		_levelFailed = GetTree().GetFirstNodeInGroup("LevelFailed") as LevelFailed;
		_victoryScreen = GetTree().GetFirstNodeInGroup("VictoryScreen") as VictoryScreen;

		settingsMenu.Hide(); // Hide the settings menu when the game starts
		Hide(); // Hide the pause menu when the game starts
	}

	//check if the pause button is pressed and pause or unpause the game
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("pause") && GetTree().Paused)
		{
			CheckBeforeUnpause();
		}

		else if (Input.IsActionJustPressed("pause") && !GetTree().Paused)
		{
			Pause();
		}
	}

	//pause the game
	private void Pause()
	{
		GetTree().Paused = true;

		if (settingsMenu.Visible)
		{
			settingsMenu.Hide();
		}

		Show();

		GetNode<AnimationPlayer>("BlurAnimation").Play("StartPause");
	}

	//unpause the game
	private void Unpause()
	{
		GetTree().Paused = false;
		Hide();
	}
	/*
	Check if the shop menu is open and if the next step should be to show the shop and hide the pauseMenu (toShop true), 
	or to just pause the game (toShop false) and show the pauseMenu. Else just unpause the game.
	*/
	private void CheckBeforeUnpause()
	{
		switch (shopMenu.Visible || levelCountdown.Visible || _levelFailed.Visible || _victoryScreen.Visible) //another menu open?
		{
			case true when _leavePauseMenu && levelCountdown.Visible:
				Hide(); //just hide the pause Menu without unpausing the game because another menu is open and paused the game
				levelCountdown.countdownTimer.Paused = false;
				musicController.GetNode<AudioStreamPlayer>("CountDown").StreamPaused = false;
				levelCountdown.blurAnimation.Play();
				_leavePauseMenu = false;
				break;
			case true when _leavePauseMenu && !levelCountdown.Visible:
				Hide();
				levelCountdown.countdownTimer.Paused = false;
				_leavePauseMenu = false;
				break;
			case true:
				levelCountdown.countdownTimer.Paused = true;
				musicController.GetNode<AudioStreamPlayer>("CountDown").StreamPaused = true;
				levelCountdown.blurAnimation.Pause();
				_leavePauseMenu = true;
				Pause();
				break;
			default:
				Unpause();
				break;
		}
	}

	//button functions for the pause menu
	private void OnResumePressed()
	{
		musicController.Play(Sound.ButtonClick);
		CheckBeforeUnpause();
	}

	private void OnSettingsPressed()
	{
		musicController.Play(Sound.ButtonClick);

		//show Settingsmenu
		settingsMenu.Show();
	}

	//Return to the main menu
	private void OnMainMenuPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
