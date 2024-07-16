using Code.Scripts.Audio;
using Code.Scripts.UI.Shop;
using Godot;

namespace Code.Scripts.UI;

/// <summary>
/// Controls the pause menu
/// </summary>
public partial class PauseMenu : Control
{
	protected MusicController MusicController;
	protected SettingsMenu SettingsMenu;
	protected ShopMenu ShopMenu;
	protected LevelCountdown LevelCountdown;
	private bool _leavePauseMenu;
	private LevelFailed _levelFailed;
	private VictoryScreen _victoryScreen;

	public override void _Ready()
	{
		MusicController = GetNode<MusicController>("/root/MusicController");
		SettingsMenu = GetNode<SettingsMenu>("SettingsMenu");
		ShopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		LevelCountdown = GetTree().GetFirstNodeInGroup("LevelCountdown") as LevelCountdown;
		_levelFailed = GetTree().GetFirstNodeInGroup("LevelFailed") as LevelFailed;
		_victoryScreen = GetTree().GetFirstNodeInGroup("VictoryScreen") as VictoryScreen;

		SettingsMenu.Hide(); // Hide the settings menu when the game starts
		Hide(); // Hide the pause menu when the game starts
	}

	/// <summary>
	/// Fires when the pause button is pressed and shows/hides the pause menu
	/// </summary>
	/// <param name="event">The input event</param>
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

	/// <summary>
	/// Pause the game and show the pause menu
	/// </summary>
	private void Pause()
	{
		GetTree().Paused = true;

		if (SettingsMenu.Visible)
		{
			SettingsMenu.Hide();
		}

		Show();

		GetNode<AnimationPlayer>("BlurAnimation").Play("StartPause");
	}

	/// <summary>
	/// Unpause the game and hide the pause menu
	/// </summary>
	private void Unpause()
	{
		GetTree().Paused = false;
		Hide();
	}

	/// <summary>
	/// Check if the shop menu is open and if the next step should be to show the shop and hide the pauseMenu (toShop true), 
	/// or to just pause the game (toShop false) and show the pauseMenu. Else just unpause the game.
	/// </summary>
	private void CheckBeforeUnpause()
	{
		switch (ShopMenu.Visible || LevelCountdown.Visible || _levelFailed.Visible || _victoryScreen.Visible) //another menu open?
		{
			case true when _leavePauseMenu && LevelCountdown.Visible:
				Hide(); //just hide the pause Menu without unpausing the game because another menu is open and paused the game
				LevelCountdown.CountdownTimer.Paused = false;
				MusicController.GetNode<AudioStreamPlayer>("CountDown").StreamPaused = false;
				LevelCountdown.BlurAnimation.Play();
				_leavePauseMenu = false;
				break;
			case true when _leavePauseMenu && !LevelCountdown.Visible:
				Hide();
				LevelCountdown.CountdownTimer.Paused = false;
				_leavePauseMenu = false;
				break;
			case true:
				LevelCountdown.CountdownTimer.Paused = true;
				MusicController.GetNode<AudioStreamPlayer>("CountDown").StreamPaused = true;
				LevelCountdown.BlurAnimation.Pause();
				_leavePauseMenu = true;
				Pause();
				break;
			default:
				Unpause();
				break;
		}
	}

	/// <summary>
	/// Continue the game. Calls the CheckBeforeUnpause method to check if the game should be unpaused or not.
	/// </summary>
	private void OnResumePressed()
	{
		MusicController.Play(Sound.ButtonClick);
		CheckBeforeUnpause();
	}

	/// <summary>
	/// Show the settings menu scene.
	/// </summary>
	private void OnSettingsPressed()
	{
		MusicController.Play(Sound.ButtonClick);

		//show Settingsmenu
		SettingsMenu.Show();
	}

	/// <summary>
	/// Return to the main menu
	/// </summary>
	private void OnMainMenuPressed()
	{
		MusicController.Play(Sound.ButtonClick);
		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
