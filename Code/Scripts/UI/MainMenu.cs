using Godot;
using Managers.Save;

namespace UI;
public partial class MainMenu : Control
{
	protected MusicController musicController;
	UserPreferences userPreferences;

	public override void _Ready()
	{
		// Load user preferences and set the window mode accordingly
		userPreferences = UserPreferences.LoadOrCreate();
		DisplayServer.WindowSetMode(userPreferences.IsFullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

		musicController = GetNode<MusicController>("/root/MusicController");
		GetNode<Button>("Buttons/VBoxContainer/Continue").Disabled = !SaveManager.Instance.IsSaveFileAvailable();
	}

	private void OnNewGamePressed()
	{
		musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.NEW_GAME;
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");
	}

	private void OnContinueGamePressed()
	{
		musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.LOAD_GAME;
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
