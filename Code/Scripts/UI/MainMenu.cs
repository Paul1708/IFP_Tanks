using Godot;

namespace UI;
public partial class MainMenu : Control
{
	private void _on_play_pressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/Misc/main_game.tscn");
	}

	private void _on_settings_pressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/UI/SettingsMenu.tscn");
	}

	private void _on_quit_pressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().Quit();
	}

}
