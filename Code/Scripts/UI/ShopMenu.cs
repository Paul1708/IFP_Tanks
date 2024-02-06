using Godot;
using Managers;
using Movement;
using System;


public partial class ShopMenu : Control
{
	AnimationPlayer animationPlayer;
	Level level;
	LevelManager levelManager;
	MusicController musicController;

	[Signal]
	public delegate void OnShopMenuClosedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();

		musicController = GetNode<MusicController>("/root/MusicController");
		animationPlayer = GetNode<AnimationPlayer>("BlurAnimation");
		level = GetTree().GetFirstNodeInGroup("Level") as Level;
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;		

		level.OnCoinsMoved += ShowShopMenu;
		levelManager.OnLevelChanged += UpdateSetup;
	}

	//Called when a new level is loaded to update the level reference
	public void UpdateSetup()
	{
		level = GetTree().GetFirstNodeInGroup("Level") as Level;
		level.OnCoinsMoved += ShowShopMenu;
	}

	//Show the shop menu and pause the game
	public void ShowShopMenu()
	{	
		GetTree().Paused = true;
		Show();
		animationPlayer.Play("StartPause");
	}

	//Hide the shop menu, unpause the game and tell the level to continue by emitting the OnLevelComplete signal
	public void OnContinueButtonPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().Paused = false;
		Hide();
		level.SendOnLevelComplete();
	}

	//Return to the main menu
	private void OnMainMenuPressed()
	{
		musicController.Play(Sound.ButtonClick);

		GetTree().Paused = false; //make sure the game is unpaused
		GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
	}
}
