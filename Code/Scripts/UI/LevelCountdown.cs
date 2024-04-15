using Godot;
using Managers.Level;
using Shop;
using System;

public partial class LevelCountdown : Control
{
	Label cooldownLabel;
	ShopMenu shopMenu;
	ColorRect blur;
	LevelManager levelManager;
	public Timer countdownTimer;
	public AnimationPlayer blurAnimation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		countdownTimer = GetNode<Timer>("Countdown");
		blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		cooldownLabel = GetNode<Label>("CountdownContainer/Countdown");
		blur = GetNode<ColorRect>("Blur");

		shopMenu = GetTree().GetFirstNodeInGroup("Shop") as ShopMenu;
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		
		shopMenu.OnShopMenuClosed += StartLevelCooldown;
		levelManager.OnFirstLevelLoaded += StartLevelCooldown;
	}

	public override void _ExitTree()
	{
		shopMenu.OnShopMenuClosed -= StartLevelCooldown;
		levelManager.OnFirstLevelLoaded -= StartLevelCooldown;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int timeLeft = (int)countdownTimer.TimeLeft;
		cooldownLabel.Text = timeLeft.ToString();
	}

	public void StartLevelCooldown()
	{
		blur.Color = new Color(1, 1, 1);
		Show();
		blurAnimation.Play("LevelCooldown");
		countdownTimer.Start();
		GetTree().Paused = true;
	}

	public void OnCooldownTimeout ()
	{
		GetTree().Paused = false;
		Hide();
	}
}
