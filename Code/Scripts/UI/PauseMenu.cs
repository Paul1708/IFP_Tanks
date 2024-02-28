using Godot;
using System;
using UI;

public partial class PauseMenu : Control
{
	protected MusicController musicController;
	protected SettingsMenu settingsMenu;

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
		settingsMenu = GetNode<SettingsMenu>("SettingsMenu");
		
		settingsMenu.Hide(); // Hide the settings menu when the game starts
		Hide(); // Hide the pause menu when the game starts
	}

	//check if the pause button is pressed and pause or unpause the game
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("pause") && GetTree().Paused)
		{
			Unpause();
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

	//button functions for the pause menu
	private void OnResumePressed()
	{
		musicController.Play(Sound.ButtonClick);

		Unpause();
	}

	private void OnSettingsPressed()
	{
		musicController.Play(Sound.ButtonClick);

		//show Settingsmenu
		settingsMenu.Show();
	}

	private void OnMainMenuPressed()
	{
		musicController.Play(Sound.ButtonClick);

		GetTree().Paused = false; //make sure the game is unpaused

		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
