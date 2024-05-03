using System;
using Godot;
using Managers.Save;


namespace UI;
public partial class SettingsMenu : Control
{
	protected MusicController musicController;

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
		musicController = GetNode<MusicController>("/root/MusicController");

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

	private void EnableBlur()
	{
		_blur.Visible = true;
	}

	private void OnBackPressed()
	{
		musicController.Play(Sound.ButtonClick);
		if (GetTree().CurrentScene.IsInGroup("MainGame")) 
		{
			Hide();
		}
		else 
		{
			MainMenu mainMenu = GetTree().GetFirstNodeInGroup("MainMenu") as MainMenu;
			mainMenu.camera.Position = mainMenu.mainMenuCameraPosition;
		}
	}

	//toggles fullscreen and windowed mode
	private void OnFullscreenToggled(bool ToggledOn)
	{
		musicController.Play(Sound.ButtonClick);
		DisplayServer.WindowSetMode(ToggledOn ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
		_userPreferences.IsFullscreen = ToggledOn;
		_userPreferences.Save();
	}

	//changes MasterBus volume when slider is moved
	private void OnMasterSliderValueChanged(float value)
	{
		SetVolume(_masterBusIndex, value);
		_userPreferences.MasterVolume = value;
		_userPreferences.Save();
	}

	//changes MusicBus volume when slider is moved
	private void OnMusicSliderValueChanged(float value)
	{
		SetVolume(_musicBusIndex, value);
		_userPreferences.MusicVolume = value;
		_userPreferences.Save();
	}

	//changes SFXBus volume when slider is moved
	private void OnSFXSliderValueChanged(float value)
	{
		SetVolume(_sfxBusIndex, value);
		_userPreferences.SFXVolume = value;
		_userPreferences.Save();
	}

	public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

	public void LoadUserSettings()
	{
		_masterSlider.Value = _userPreferences.MasterVolume;
		_musicSlider.Value = _userPreferences.MusicVolume;
		_sfxSlider.Value = _userPreferences.SFXVolume;
		_fullscreenButton.SetPressedNoSignal(_userPreferences.IsFullscreen);
	}

}
