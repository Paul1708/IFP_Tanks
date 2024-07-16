using Code.Scripts.Audio;
using Code.Scripts.Managers.Save;
using Godot;

namespace Code.Scripts.UI;

/// <summary>
/// Controls the main menu
/// </summary>
public partial class MainMenu : Control
{
	[Export]
	public Vector2 MainMenuCameraPosition = new (960, 540);
	[Export]
	public Vector2 SettingsMenuCameraPosition = new (2880, 540);
	public Camera2D Camera;
	private MusicController _musicController;
	private AudioStreamPlayer _menuMusic;
	private SettingsMenu _settingsMenu;
	
	
	public override void _Ready()
	{
		_musicController = GetNode<MusicController>("/root/MusicController");
		Camera = GetNode<Camera2D>("Camera2D");
		_menuMusic = GetNode<AudioStreamPlayer>("MenuMusic");
		//Get the settings menu node to make sure its loaded to apply userSettings before playing any music
		_settingsMenu = GetNode<SettingsMenu>("SettingsMenu"); 

		GetNode<Button>("UI/Buttons/VBoxContainer/Continue").Disabled = !SaveManager.Instance.IsSaveFileAvailable();
		_musicController.StopCurrentMusic();
		_menuMusic.Play();
	}

	/// <summary>
	/// Start a new game. Set the LoadingType to NewGame and change the scene to MainGame
	/// </summary>
	private void OnNewGamePressed()
	{
		_musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.NewGame;
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");
	}

	/// <summary>
	/// Continue the game. Set the LoadingType to LoadGame and change the scene to MainGame
	/// </summary>
	private void OnContinueGamePressed()
	{
		_musicController.Play(Sound.ButtonClick);
		SaveManager.Instance.LoadingType = LoadingType.LoadGame;
		GetTree().ChangeSceneToFile("res://Scenes/Misc/MainGame.tscn");
	}

	/// <summary>
	/// Show the settings menu by changing the camera position
	/// </summary>
	private void OnSettingsPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		Camera.Position = SettingsMenuCameraPosition;
	}

	/// <summary>
	/// Closes the game.
	/// </summary>
	private void OnQuitPressed()
	{
		_musicController.Play(Sound.ButtonClick);
		GetTree().Quit();
	}

}
