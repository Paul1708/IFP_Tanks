using Godot;
using Code.Scripts.Managers.Save;

namespace Code.Scripts.Managers;
public partial class LoadUserPreferences : Node
{

    private UserPreferences _userPreferences;
    private string _masterBusName = "Master";
	private string _musicBusName = "Music";
	private string _sfxBusName = "SFX";

	private int _masterBusIndex = AudioServer.GetBusIndex("Master");
	private int _musicBusIndex = AudioServer.GetBusIndex("Music");
	private int _sfxBusIndex = AudioServer.GetBusIndex("SFX");

    public override void _Ready()
    {
        // Load user preferences and set the window mode accordingly
        _userPreferences = UserPreferences.LoadOrCreate();
        DisplayServer.WindowSetMode(_userPreferences.IsFullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

        // Set the volume of the master, music and sfx buses
        SetVolume(_masterBusIndex, _userPreferences.MasterVolume);
        SetVolume(_musicBusIndex, _userPreferences.MusicVolume);
        SetVolume(_sfxBusIndex, _userPreferences.SfxVolume);
    }

    public void SetVolume(int busIndex, float value)
	{
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}

}