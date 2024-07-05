using Godot;

namespace Code.Scripts.Managers.Save;

/// <summary>
/// Manages saveing and loading of user preferences/settings
/// </summary>
[GlobalClass]
public partial class UserPreferences : Resource
{
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float MasterVolume { get; set; } = 1.0f;
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float MusicVolume { get; set; } = 1.0f;
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float SfxVolume { get; set; } = 1.0f;
    [Export]
    public bool IsFullscreen { get; set; }

    /// <summary>
    /// Saves the UserPreferences to the user://UserPreferences.tres file
    /// </summary>
    public void Save()
    {
        ResourceSaver.Save(this, "user://UserPreferences.tres");
    }
    
    /// <summary>
    /// Loads the UserPreferences from the user://UserPreferences.tres file or creates a new one if it does not exist
    /// </summary>
    /// <returns>The UserPreferences object</returns>
    public static UserPreferences LoadOrCreate()
    {
        UserPreferences userPreferences;
        if (!ResourceLoader.Exists("user://UserPreferences.tres"))
        {
            userPreferences = new UserPreferences();
            userPreferences.Save();
        }
        else
        {
            userPreferences = ResourceLoader.Load<UserPreferences>("user://UserPreferences.tres");
        }
        return userPreferences;
    }
}
