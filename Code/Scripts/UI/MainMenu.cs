using Godot;

namespace UI;
public partial class MainMenu : Control
{
	private void _on_play_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/Misc/main_game.tscn");
	}

	private void _on_settings_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/UI/settings_menu.tscn");
	}

	private void _on_quit_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().Quit();
	}

}
