using Godot;

namespace UI;
public partial class MainMenu : Control
{	
	protected MusicController musicController;

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
	}

	private void OnPlayPressed()
	{
		musicController.Play(Sound.ButtonClick);
		Manager.Instance.LoadCheckpointStats();
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");

	}

	private void OnSettingsPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().ChangeSceneToFile("res://Scenes/UI/SettingsMenu.tscn");
	}

	private void OnQuitPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().Quit();
	}

}
