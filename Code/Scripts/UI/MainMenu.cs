using Godot;
using Managers.Save;

namespace UI;
public partial class MainMenu : Control
{
	[Export]
	public Vector2 mainMenuCameraPosition = new Vector2(960, 540);
	[Export]
	public Vector2 settingsMenuCameraPosition = new Vector2(2880, 540);
	public Camera2D camera;
	protected MusicController musicController;
	
	
	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
		camera = GetNode<Camera2D>("Camera2D");
		GetNode<Button>("UI/Buttons/VBoxContainer/Continue").Disabled = !SaveManager.Instance.IsSaveFileAvailable();
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
		camera.Position = settingsMenuCameraPosition;
	}

	private void OnQuitPressed()
	{
		musicController.Play(Sound.ButtonClick);
		GetTree().Quit();
	}

}
