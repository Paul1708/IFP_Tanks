
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

	public void ShowDeathScreen()
	{
		_blurAnimation.Play("LevelCooldown");
		_musicController.Play(Sound.Death);
		GetTree().Paused = true;
		Show();

	}

	private void OnRetryButtonPressed()
	{	
		_musicController.Play(Sound.ButtonClick);
		EmitSignal(SignalName.OnRetryPressed);
		Hide();
	}
}
