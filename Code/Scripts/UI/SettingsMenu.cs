using Code.Scripts.Audio;
using Code.Scripts.Managers.Save;
using Godot;

namespace Code.Scripts.UI;

/// <summary>
/// Controls the settings menu
/// </summary>
public partial class SettingsMenu : Control
{
	protected MusicController MusicController;

	private string _masterBusName = "Master";
	private string _musicBusName = "Music";
	private string _sfxBusName = "SFX";

	private int _masterBusIndex = AudioServer.GetBusIndex("Master");
	private int _musicBusIndex = AudioServer.GetBusIndex("Music");
	private int _sfxBusIndex = AudioServer.GetBusIndex("SFX");
 
	private Slider _masterSlider;
	private Slider _musicSlider;
	private Slider _sfxSlider;
	private CheckButton _fullscreenButton;
	private UserPreferences _userPreferences;
	private ColorRect _blur;

	public override void _Ready()
	{
		MusicController = GetNode<MusicController>("/root/MusicController");

		_masterSlider = GetNode<Slider>("%MasterSlider");
		_musicSlider = GetNode<Slider>("%MusicSlider");
		_sfxSlider = GetNode<Slider>("%SFXSlider");
		_fullscreenButton = GetNode<CheckButton>("%Fullscreen");

		_blur = GetNode<ColorRect>("Blur");
		if (GetParent().Name == "PauseMenu")
		{
			EnableBlur();
		}

		_userPreferences = UserPreferences.LoadOrCreate();
		LoadUserSettings();
	}

	/// <summary>
	/// Sets the blur to visible
	/// </summary>
	private void EnableBlur()
	{
		_blur.Visible = true;
	}

	/// <summary>
	/// Returns to the main menu camera position if current scene is the main menu, otherwise hides the settings menu
	/// </summary>
	private void OnBackPressed()
	{
		MusicController.Play(Sound.ButtonClick);
		if (GetTree().CurrentScene.IsInGroup("MainGame")) 
		{
			Hide();
		}
		else 
		{
			MainMenu mainMenu = GetTree().GetFirstNodeInGroup("MainMenu") as MainMenu;
			mainMenu.Camera.Position = mainMenu.MainMenuCameraPosition;
		}
	}

	/// <summary>
	/// Toggles fullscreen and windowed mode
	/// </summary>
	/// <param name="toggledOn">The state of the button</param>
	private void OnFullscreenToggled(bool toggledOn)
	{
		MusicController.Play(Sound.ButtonClick);
		DisplayServer.WindowSetMode(toggledOn ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
		_userPreferences.IsFullscreen = toggledOn;
		_userPreferences.Save();
	}

	/// <summary>
	/// Changes MasterBus volume when slider is moved
	/// </summary>
	/// <param name="value">The value of the slider</param>
	private void OnMasterSliderValueChanged(float value)
	{
		SetVolume(_masterBusIndex, value);
		_userPreferences.MasterVolume = value;
		_userPreferences.Save();
	}

	/// <summary>
	/// Changes MusicBus volume when slider is moved
	/// </summary>
	/// <param name="value">The value of the slider</param>
	private void OnMusicSliderValueChanged(float value)
	{
		SetVolume(_musicBusIndex, value);
		_userPreferences.MusicVolume = value;
		_userPreferences.Save();
	}

	/// <summary>
	/// Changes SFXBus volume when slider is moved
	/// </summary>
	/// <param name="value">The value of the slider</param>
	private void OnSFXSliderValueChanged(float value)
	{
		SetVolume(_sfxBusIndex, value);
		_userPreferences.SfxVolume = value;
		_userPreferences.Save();
	}

	/// <summary>
	/// Sets the volume of the bus to the given value
	/// </summary>
	/// <param name="busIndex">The index of the bus</param>
	/// <param name="value">The value to set the volume to</param>
	public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

	/// <summary>
	/// Loads the user settings from the UserPreferences file
	/// </summary>
	public void LoadUserSettings()
	{
		_masterSlider.Value = _userPreferences.MasterVolume;
		_musicSlider.Value = _userPreferences.MusicVolume;
		_sfxSlider.Value = _userPreferences.SfxVolume;
		_fullscreenButton.SetPressedNoSignal(_userPreferences.IsFullscreen);
	}

}
