using System.Threading.Tasks;
using Godot;
using Managers.Level;

public partial class VictoryScreen : Control
{
	private LevelManager _levelManager;
	private AnimationPlayer _blurAnimation;
	private ColorRect _blur;
	private MusicController _musicController;

	[Signal]
	public delegate void OnBacktoMainMenuPressedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blur = GetNode<ColorRect>("Blur");
		_blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		_musicController = GetNode<MusicController>("/root/MusicController");


		_levelManager.OnGameWonShowVictoryScreen += ShowVictoryScreen;

		Hide();
	}

	public override void _ExitTree()
	{
		_levelManager.OnGameWonShowVictoryScreen -= ShowVictoryScreen;
	}

	public void ShowVictoryScreen()
	{
		_blurAnimation.Play("LevelCooldown");
		GetTree().Paused = true;
		Show();

	}

	private void OnReturnPressed()
	{	
		GetTree().Paused = false;
		_musicController.Play(Sound.ButtonClick);
		EmitSignal(SignalName.OnBacktoMainMenuPressed);
		Hide();
	}
}
