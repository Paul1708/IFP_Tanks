using Godot;

namespace UI;
public partial class SettingsMenu : Control
{
	private void _on_back_pressed()
	{
		var MusicController = GetNode<MusicController>("/root/MusicController");
		MusicController.ClickButton();
		GetTree().ChangeSceneToFile("res://Scenes/UI/main_menu.tscn");
	}

}
