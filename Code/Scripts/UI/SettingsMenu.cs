using Godot;

namespace UI;
public partial class SettingsMenu : Control
{
	private void _on_back_pressed()
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
