using Godot;
using System;
using UI;

public partial class PauseMenu : Control
{
	protected MusicController musicController;
	protected SettingsMenu settingsMenu;
	protected ShopMenu shopMenu;
	private bool toShop = false;

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
		settingsMenu = GetNode<SettingsMenu>("SettingsMenu");
		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;

		settingsMenu.Hide(); // Hide the settings menu when the game starts
		Hide(); // Hide the pause menu when the game starts
	}

	//check if the pause button is pressed
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

		var blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		blurAnimation.Play("StartPause");
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
		switch (shopMenu.Visible)
		{
			case true when toShop:
				Hide();
				toShop = false;
				break;
			case true:
				toShop = true;
				Pause();
				break;
			default:
				Unpause();
				break;
		}
	}

	//button functions
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
