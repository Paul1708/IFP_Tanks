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


	private void OnBackPressed()
	{
		var musicController = GetNode<MusicController>("/root/MusicController");
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
		var musicController = GetNode<MusicController>("/root/MusicController");
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
		AudioServer.SetBusVolumeDb(masterBusIndex, Mathf.LinearToDb(value));
	}
	//sets the slider state when the scene is loaded
	private void SetMasterSliderState()
	{
		var masterSlider = GetNode<Slider>("SliderContainer/VBoxContainer/MasterSlider");
		masterSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(masterBusIndex));
	}

	//changes MusicBus volume when slider is moved
	private void OnMusicSliderValueChanged(float value)
	{
		AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(value));
	}
	//sets the slider state when the scene is loaded
	private void SetMusicSliderState()
	{
		var musicSlider = GetNode<Slider>("SliderContainer/VBoxContainer/MusicSlider");
		musicSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(musicBusIndex));
	}

	//changes SFXBus volume when slider is moved
	private void OnSFXSliderValueChanged(float value)
	{
		AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(value));
	}
	//sets the slider state when the scene is loaded
	private void SetSFXSliderState()
	{
		var sfxSlider = GetNode<Slider>("SliderContainer/VBoxContainer/SFXSlider");
		sfxSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(sfxBusIndex));
	}
}
