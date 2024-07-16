using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI;

/// <summary>
/// Dynamically shows the levels of the current world anr their states.
/// </summary>
public partial class LevelDisplay : HBoxContainer
{
    // Textures for the different level states
    public Texture2D BasicCompleted = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/BasicCompleted.png");
    public Texture2D BasicCurrent = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/BasicCurrent.png");
    public Texture2D BasicLocked = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/BasicLocked.png");
    public Texture2D CheckpointCompleted = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/CheckpointCompleted.png");
    public Texture2D CheckpointCurrent = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/CheckpointCurrent.png");
    public Texture2D CheckpointLocked = GD.Load<Texture2D>("res://Sprites/UI/LevelIndicators/CheckpointLocked.png");

    /// <summary>
    /// Renders the level display based on the levels and their states.
    /// </summary>
    /// <param name="levels">The levels to render</param>
    public void RenderLevelDisplay(LevelData[] levels)
    {
        // Clear the current level display
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        // Iterate over lvels and create a texture for each level based on its state
        foreach (var level in levels)
        {
            var texture = new TextureRect();
            if (level.IsCheckpoint)
            {
                switch (level.LevelState)
                {
                    case LevelState.Completed:
                        texture.Texture = CheckpointCompleted;
                        break;
                    case LevelState.Current:
                        texture.Texture = CheckpointCurrent;
                        break;
                    case LevelState.Locked:
                        texture.Texture = CheckpointLocked;
                        break;
                }
            }
            else
            {
                switch (level.LevelState)
                {
                    case LevelState.Completed:
                        texture.Texture = BasicCompleted;
                        break;
                    case LevelState.Current:
                        texture.Texture = BasicCurrent;
                        break;
                    case LevelState.Locked:
                        texture.Texture = BasicLocked;
                        break;
                }
            }
            texture.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            AddChild(texture);
        }
    }
}

