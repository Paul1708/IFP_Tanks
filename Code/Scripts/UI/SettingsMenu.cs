using System;
using Godot;
using Managers.Save;


namespace UI;
public partial class SettingsMenu : Control
{
	string masterBusName = "Master";
	string musicBusName = "Music";
	string sfxBusName = "SFX";

	int masterBusIndex = AudioServer.GetBusIndex("Master");
	int musicBusIndex = AudioServer.GetBusIndex("Music");
	int sfxBusIndex = AudioServer.GetBusIndex("SFX");

	protected MusicController musicController;
	Slider masterSlider;
	Slider musicSlider;
	Slider sfxSlider;
	CheckButton fullscreenButton;
	UserPreferences userPreferences;
	ColorRect blur;

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");

		masterSlider = GetNode<Slider>("%MasterSlider");
		musicSlider = GetNode<Slider>("%MusicSlider");
		sfxSlider = GetNode<Slider>("%SFXSlider");
		fullscreenButton = GetNode<CheckButton>("%Fullscreen");

		blur = GetNode<ColorRect>("Blur");
		if (GetParent().Name == "PauseMenu")
		{
			EnableBlur();
		}

		userPreferences = UserPreferences.LoadOrCreate();
		LoadUserSettings();
	}

	private void EnableBlur()
	{
		blur.Visible = true;
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
		userPreferences.IsFullscreen = ToggledOn;
		userPreferences.Save();
	}

	//changes MasterBus volume when slider is moved
	private void OnMasterSliderValueChanged(float value)
	{
		SetVolume(masterBusIndex, value);
		userPreferences.MasterVolume = value;
		userPreferences.Save();
	}

	//changes MusicBus volume when slider is moved
	private void OnMusicSliderValueChanged(float value)
	{
		SetVolume(musicBusIndex, value);
		userPreferences.MusicVolume = value;
		userPreferences.Save();
	}

	//changes SFXBus volume when slider is moved
	private void OnSFXSliderValueChanged(float value)
	{
		SetVolume(sfxBusIndex, value);
		userPreferences.SFXVolume = value;
		userPreferences.Save();
	}

	public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

	public void LoadUserSettings()
	{
		masterSlider.Value = userPreferences.MasterVolume;
		musicSlider.Value = userPreferences.MusicVolume;
		sfxSlider.Value = userPreferences.SFXVolume;
		fullscreenButton.SetPressedNoSignal(userPreferences.IsFullscreen);
	}

}
