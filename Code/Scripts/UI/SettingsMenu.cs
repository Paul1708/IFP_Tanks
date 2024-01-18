using Godot;

namespace UI;
public partial class SettingsMenu : Control
{
	private void OnBackPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
		musicController.ClickButton();

		if (GetTree().CurrentScene.IsInGroup("level"))
		{
			Hide();
		}
		else
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
		}

	}

}
