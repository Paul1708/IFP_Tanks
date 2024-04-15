using Godot;
using System;

namespace Managers.Save;

[GlobalClass]
public partial class UserPreferences : Resource
{
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float MasterVolume { get; set; } = 1.0f;
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float MusicVolume { get; set; } = 1.0f;
    [Export(PropertyHint.Range, "0, 1, 0.05")]
    public float SFXVolume { get; set; } = 1.0f;
    [Export]
    public bool IsFullscreen { get; set; } = false;

    public void Save()
    {
        ResourceSaver.Save(this, "user://UserPreferences.tres");
    }
    
    public static UserPreferences LoadOrCreate()
    {
        var userPreferences = new UserPreferences();
        if (userPreferences == null || ResourceLoader.Exists("user://UserPreferences.tres") == false)
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
