using System.Threading.Tasks;
using Godot;
using Managers.Level;

public partial class LevelFailed : Control
{
	private LevelManager _levelManager;
	private AnimationPlayer _blurAnimation;
	private ColorRect _blur;
	private MusicController _musicController;

	[Export]
	double slowDownTime; // The time over which to slow down the game

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _ExitTree()
	{
		_levelManager.OnLevelFailedShowDeathScreen -= ShowDeathScreen;
	}

	public void ShowDeathScreen()
	{
		_blurAnimation.Play("LevelCooldown");
		Show();

		// Start the slow down process
		Tween tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(SetEngingeTimeScale), 1.0f, 0.1f, 0.4);
	}

	private void OnRetryButtonPressed()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(SetEngingeTimeScale), 0.1f, 1f, 0.01);
		
		_musicController.Play(Sound.ButtonClick);
		EmitSignal(SignalName.OnRetryPressed);
		Hide();
	}

	private void SetEngingeTimeScale(float timeScale)
	{
		Engine.TimeScale = timeScale;
	}

}
