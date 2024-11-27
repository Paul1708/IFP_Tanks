using Godot;
using System;
using Code.Scripts.Audio;


namespace Code.Scripts.UI;

public partial class AdminMenu : Control
{
	[Export]
	public Resource BasicEnemy;
	protected MusicController _musicController;
	private LineEdit _basicEnemyDMG;
	private LineEdit _basicEnemyBPS;
	private LineEdit _basicBSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _basicEnemyDMG = GetNode <LineEdit>("BasicEnemyContainer/VBoxContainer/HSplitContainer/LineEdit");
		_basicEnemyBPS = GetNode <LineEdit>("BasicEnemyContainer/VBoxContainer/HSplitContainer2/LineEdit");
		_basicBSpeed = GetNode <LineEdit>("BasicEnemyContainer/VBoxContainer/HSplitContainer3/LineEdit");

		_musicController = GetNode<MusicController>("/root/MusicController");


		_basicEnemyDMG.Text = BasicEnemy.Get("Damage").ToString();
		_basicEnemyBPS.Text = BasicEnemy.Get("BulletsPerSecond").ToString();
		_basicBSpeed.Text = BasicEnemy.Get("BulletSpeed").ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBackPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		MainMenu mainMenu = GetTree().GetFirstNodeInGroup("MainMenu") as MainMenu;
		mainMenu.Camera.Position = mainMenu.SettingsMenuCameraPosition;
	}

	private void OnBasicDMGChanged(string newText)
	{
		if (int.TryParse(newText, out int result))
		{
			BasicEnemy.Set("Damage", result);
		}
	}

	private void OnBasicBPSChanged(string newText)
	{
		if (int.TryParse(newText, out int result))
		{
			BasicEnemy.Set("BulletsPerSecond", result);
		}
	}

	private void OnBasicBSpeedChanged(string newText)
	{
		if (int.TryParse(newText, out int result))
		{
			BasicEnemy.Set("BulletSpeed", result);
		}
	}

}
