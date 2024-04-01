using Godot;
using System;
using Managers.Save;



public partial class LoadUserPreferences : Node
{

    UserPreferences userPreferences;
    string masterBusName = "Master";
	string musicBusName = "Music";
	string sfxBusName = "SFX";

	int masterBusIndex = AudioServer.GetBusIndex("Master");
	int musicBusIndex = AudioServer.GetBusIndex("Music");
	int sfxBusIndex = AudioServer.GetBusIndex("SFX");

    public override void _Ready()
    {
        // Load user preferences and set the window mode accordingly
        userPreferences = UserPreferences.LoadOrCreate();
        DisplayServer.WindowSetMode(userPreferences.IsFullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

        // Set the volume of the master, music and sfx buses
        SetVolume(masterBusIndex, userPreferences.MasterVolume);
        SetVolume(musicBusIndex, userPreferences.MusicVolume);
        SetVolume(sfxBusIndex, userPreferences.SFXVolume);
    }

    public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

}