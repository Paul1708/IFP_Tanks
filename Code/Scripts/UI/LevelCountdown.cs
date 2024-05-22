using Godot;
using Managers.Level;
using Shop;
using System;

public partial class LevelCountdown : Control
{
	public Timer countdownTimer;
	public AnimationPlayer blurAnimation;
	private Label _cooldownLabel;
	private ShopMenu _shopMenu;
	private ColorRect _blur;
	private LevelManager _levelManager;
	private MusicController _musicController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		countdownTimer = GetNode<Timer>("Countdown");
		blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
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
		int timeLeft = (int)countdownTimer.TimeLeft + 1;
		_cooldownLabel.Text = timeLeft.ToString();
	}

	public void StartLevelCooldown()
	{
		_blur.Color = new Color(1, 1, 1);
		Show();
		blurAnimation.Play("LevelCooldown");
		countdownTimer.Start();
		_musicController.PlayMusic(Sound.CountDown);
		GetTree().Paused = true;
	}

	public void OnCooldownTimeout()
	{
		GetTree().Paused = false;
		Hide();
	}
}
