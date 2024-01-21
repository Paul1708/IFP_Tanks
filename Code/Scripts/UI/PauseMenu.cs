using Godot;
using System;

public partial class PauseMenu : Control
{

	public override void _Ready()
	{
		var settingsMenu = GetNode<Control>("SettingsMenu");
		settingsMenu.Hide(); // Hide the settings menu when the game starts
		Hide(); // Hide the pause menu when the game starts
	}

	//check if the pause button is pressed
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

	private void DeleteBullets()
	{
		foreach (Node node in GetTree().GetNodesInGroup("bullets"))
		{
			node.QueueFree(); //delete all bullets
		}
	}

	//button functions
	private void OnResumePressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.Play(Sound.ButtonClick);

		Unpause();
	}

	private void OnSettingsPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.Play(Sound.ButtonClick);
		DeleteBullets();

		//show Settingsmenu
		var settingsMenu = GetNode<Control>("SettingsMenu");
		settingsMenu.Show();
	}

	private void OnMainMenuPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.Play(Sound.ButtonClick);

		DeleteBullets();

		GetTree().Paused = false; //make sure the game is unpaused

		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
