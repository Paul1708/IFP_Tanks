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
	private MusicController _musicController;
	private AudioStreamPlayer _menuMusic;
	private SettingsMenu _settingsMenu;
	
	
	public override void _Ready()
	{
		_musicController = GetNode<MusicController>("/root/MusicController");
		camera = GetNode<Camera2D>("Camera2D");
		_menuMusic = GetNode<AudioStreamPlayer>("MenuMusic");
		//Get the settings menu node to make sure its loaded to apply userSettings before playing any music
		_settingsMenu = GetNode<SettingsMenu>("SettingsMenu"); 

		GetNode<Button>("UI/Buttons/VBoxContainer/Continue").Disabled = !SaveManager.Instance.IsSaveFileAvailable();
		_musicController.StopCurrentMusic();
		_menuMusic.Play();
	}

	private void OnNewGamePressed()
	{
		_musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.NEW_GAME;
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");
	}

	private void OnContinueGamePressed()
	{
		_musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.LOAD_GAME;
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");
	}

	private void OnSettingsPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		camera.Position = settingsMenuCameraPosition;
	}

	private void OnQuitPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		GetTree().Quit();
	}

}
