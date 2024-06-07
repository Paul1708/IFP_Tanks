using System;
using Code.Scripts.Audio;
using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI;

public partial class VictoryScreen : Control
{
	private LevelManager _levelManager;
	private AnimationPlayer _blurAnimation;
	private ColorRect _blur;
	private MusicController _musicController;
	private ParticleController _particleController;

	[Export]
	public float FireworkCooldown = 0.2f;

	[Signal]
	public delegate void OnBacktoMainMenuPressedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blur = GetNode<ColorRect>("Blur");
		_blurAnimation = GetNode<AnimationPlayer>("BlurAnimation");
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		_musicController = GetNode<MusicController>("/root/MusicController");
		_particleController = GetNode<ParticleController>("/root/ParticleController");


		_levelManager.OnGameWonShowVictoryScreen += ShowVictoryScreen;

		Hide();
	}

	public override void _ExitTree()
	{
		_levelManager.OnGameWonShowVictoryScreen -= ShowVictoryScreen;
	}

	public async void ShowVictoryScreen()
	{
		_blurAnimation.Play("LevelCooldown");
		_musicController.Play(Sound.Victory);
		GetTree().Paused = true;
	
		Show();

		while (IsVisibleInTree())
		{
			GenerateFireworks();
			await ToSignal(GetTree().CreateTimer(FireworkCooldown), "timeout");
		}
	}

	private void OnReturnPressed()
	{
		GetTree().Paused = false;
		_musicController.Play(Sound.ButtonClick);
		EmitSignal(SignalName.OnBacktoMainMenuPressed);
		Hide();
	}

	private void GenerateFireworks()
	{
		// Get the size of the window
		Vector2 windowSize = GetViewportRect().Size;

		// Generate a random position within the window
		Random random = new();
		Vector2 randomPosition = new(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));

		// Emit particles at the random position
		_particleController.EmitParticles(randomPosition, Scene.Fireworks);
	}
}
