using System;
using System.Security.Authentication.ExtendedProtection;
using System.Transactions;
using Godot;

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

	public override void _Ready()
	{
		musicController = GetNode<MusicController>("/root/MusicController");
	}

	//gets called when the back button is pressed and hides the settings menu if the current scene is the main game, otherwise it changes the scene to the main menu
	private void OnBackPressed()
	{
		musicController.Play(Sound.ButtonClick);

		if (GetTree().CurrentScene.IsInGroup("MainGame"))
		{
			Hide();
		}
		else
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
		}

	}

	//toggles fullscreen and windowed mode
	private void OnFullscreenToggled(bool ToggledOn)
	{
		musicController.Play(Sound.ButtonClick);

		if (ToggledOn == true)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	//gets called when the scene is loaded and sets the toggle state to true when in fullscreen
	private void SetToggleState()
	{
		var fullscreenButton = GetNode<CheckButton>("SliderContainer/VBoxContainer/Fullscreen");
		if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
		{
			fullscreenButton.SetPressedNoSignal(true);
		}
	}
	//changes MasterBus volume when slider is moved
	private void OnMasterSliderValueChanged(float value)
	{
		SetVolume(masterBusIndex, value);
	}

	//changes MusicBus volume when slider is moved
	private void OnMusicSliderValueChanged(float value)
	{
		SetVolume(musicBusIndex, value);
	}

	//changes SFXBus volume when slider is moved
	private void OnSFXSliderValueChanged(float value)
	{
		SetVolume(sfxBusIndex, value);
	}

	public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

	//TODO: Save audio settings in save file and load them, instead of using AudioServer.GetBusVolumeDb
	public void LoadAudioSettings()
	{
		masterSlider = GetNode<Slider>("SliderContainer/VBoxContainer/MasterSlider");
		musicSlider = GetNode<Slider>("SliderContainer/VBoxContainer/MusicSlider");
		sfxSlider = GetNode<Slider>("SliderContainer/VBoxContainer/SFXSlider");

		masterSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(masterBusIndex));
		musicSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(musicBusIndex));
		sfxSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(sfxBusIndex));
	}
}
