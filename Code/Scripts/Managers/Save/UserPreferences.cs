using Godot;

namespace Code.Scripts.Managers.Save;

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

    public void Save()
    {
        ResourceSaver.Save(this, "user://UserPreferences.tres");
    }
    
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
