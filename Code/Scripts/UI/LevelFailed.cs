
using Code.Scripts.Audio;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI;

public partial class LevelFailed : Control
{
	private LevelManager _levelManager;
	private AnimationPlayer _blurAnimation;
	private ColorRect _blur;
	private MusicController _musicController;

	[Signal]
	public delegate void OnRetryPressedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blur = GetNode<ColorRect>("Blur");
		_blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		_musicController = GetNode<MusicController>("/root/MusicController");


		_levelManager.OnLevelFailedShowDeathScreen += ShowDeathScreen;

		Hide();
	}

	public override void _ExitTree()
	{
		_levelManager.OnLevelFailedShowDeathScreen -= ShowDeathScreen;
	}

	/// <summary>
	/// Show the death screen and pause the game. Plays the Death sound.
	/// </summary>
	public void ShowDeathScreen()
	{
		_blurAnimation.Play("LevelCooldown");
		_musicController.Play(Sound.Death);
		GetTree().Paused = true;
		Show();

	}

	/// <summary>
	/// Hide the death screen, unpause the game and tell the level to retry by emitting the OnRetryPressed signal
	/// </summary>
	private void OnRetryButtonPressed()
	{	
		_musicController.Play(Sound.ButtonClick);
		EmitSignal(SignalName.OnRetryPressed);
		Hide();
	}
}
