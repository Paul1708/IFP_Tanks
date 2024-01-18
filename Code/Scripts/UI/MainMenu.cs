using Godot;

namespace UI;
public partial class MainMenu : Control
{
	private void OnPlayPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/Misc/main_game.tscn");
	}

	private void OnSettingsPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/UI/SettingsMenu.tscn");
	}

	private void OnQuitPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();
		GetTree().Quit();
	}

}
