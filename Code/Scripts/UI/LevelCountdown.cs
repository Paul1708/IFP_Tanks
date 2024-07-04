using Godot;
using Code.Scripts.Audio;
using Code.Scripts.Managers.Level;
using Code.Scripts.UI.Shop;

namespace Code.Scripts.UI;

/// <summary>
/// Custom control class for the level countdown, which handles the countdown before the level starts
/// </summary>
public partial class LevelCountdown : Control
{
	public Timer CountdownTimer;
	public AnimationPlayer BlurAnimation;
	private Label _cooldownLabel;
	private ShopMenu _shopMenu;
	private ColorRect _blur;
	private LevelManager _levelManager;
	private MusicController _musicController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CountdownTimer = GetNode<Timer>("Countdown");
		BlurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		_cooldownLabel = GetNode<Label>("CountdownContainer/Countdown");
		_blur = GetNode<ColorRect>("Blur");

		_shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		_musicController = GetNode<MusicController>("/root/MusicController");
		
		_shopMenu.OnShopMenuClosed += StartLevelCooldown;
		_levelManager.OnFirstLevelLoaded += StartLevelCooldown;
	}

	public override void _ExitTree()
	{
		_shopMenu.OnShopMenuClosed -= StartLevelCooldown;
		_levelManager.OnFirstLevelLoaded -= StartLevelCooldown;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int timeLeft = (int)CountdownTimer.TimeLeft;
		_cooldownLabel.Text = timeLeft.ToString();
	}

	/// <summary>
	/// Start the level Countdown. Shows the countdown label, pauses the game and plays the countdown animation with Countdown sound.
	/// The countdown timer is started.
	/// </summary>
	public void StartLevelCooldown()
	{
		_blur.Color = new Color(1, 1, 1);
		Show();
		BlurAnimation.Play("LevelCooldown");
		CountdownTimer.Start();
		_musicController.PlayMusic(Sound.CountDown);
		GetTree().Paused = true;
	}

	/// <summary>
	/// Called when the countdown timer has finished. Unpauses the game and hides the countdown label.
	/// </summary>
	public void OnCooldownTimeout ()
	{
		GetTree().Paused = false;
		Hide();
	}
}
