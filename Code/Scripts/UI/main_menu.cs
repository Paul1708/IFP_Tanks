using Godot;
using Microsoft.CodeAnalysis;
using System;

public partial class main_menu : Control
{	
	private void _on_play_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/main_game.tscn");
	}

	private void _on_settings_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/settings_menu.tscn");
	}

	private void _on_quit_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().Quit();
	}

}
