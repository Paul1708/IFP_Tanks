using Godot;
using System;

namespace Code.Scripts.Managers;

/// <summary>
/// Particle scenes that can be used to emit particles
/// </summary>
struct Scene
{
	public static readonly PackedScene BulletCrack = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/BulletCrack.tscn");
	public static readonly PackedScene Explosion = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/Explosion.tscn");
	public static readonly PackedScene DrivingMud = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/DrivingMud.tscn");
	public static readonly PackedScene DrivingGrass = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/DrivingGrass.tscn");
	public static readonly PackedScene KamikazeExplosion = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/KamikazeExplosion.tscn");
	public static readonly PackedScene Fireworks = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/Fireworks.tscn");
}

/// <summary>
/// Controller for emitting particles
/// </summary>
public partial class ParticleController : Node2D
{
	/// <summary>
	/// Sets the position of the particle system to the position of the given opject
	/// </summary>
	/// <param name="particles">The particle scene</param>
	/// <param name="position">The position to set the particle scene to</param>
	private static void SetPostion(GpuParticles2D particles, Node2D position)
	{
		particles.GlobalPosition = position.GlobalPosition;
	}
	/// <summary>
	/// Checks if the given particle scene is set to OneShot
	/// </summary>
	/// <param name="particles">The particle scene to check</param>
	/// <exception cref="Exception">Thrown when the given particle scene is not set to OneShot</exception>
	private static void CheckForOneShot(GpuParticles2D particles)
	{
		if (!particles.OneShot)
		{
			throw new Exception("The given particle scene is not set to OneShot: " + particles.Name);
		}
	}
	/// <summary>
	/// Sets the rotation of the particle system to the rotation of the given object
	/// </summary>
	/// <param name="particles">The particle scene</param>
	/// <param name="rotationSource">The object to get the rotation from</param>
	private static void SetRotation(GpuParticles2D particles, Node2D rotationSource)
	{
		var material = particles.ProcessMaterial;
		material.Set("angle_min", rotationSource.GlobalRotationDegrees);
		material.Set("angle_max", rotationSource.GlobalRotationDegrees);
	}

	/// <summary>
	/// Emitts the chosen particle scene at the given nodes locataion. The particle scene needs to have OneShot enabled.
	/// </summary>
	/// <param name="position">The node to get the position from</param>
	/// <param name="scene">The particle scene to emitt</param>
	public async void EmitParticles(Node2D position, PackedScene scene)
	{
		//create a new instance of the given particle scene
		var particles = scene.Instantiate<GpuParticles2D>();
		CheckForOneShot(particles);
		//set the position of the particles to the position of the given node
		SetPostion(particles, position);

		Node targetNode = GetTree().CurrentScene.Name == "MainGame" ? GetTree().GetFirstNodeInGroup("Level") : GetTree().GetFirstNodeInGroup("MainMenu");
		targetNode.AddChild(particles); //add the particle to the scene  
		
		//Emitt particles
		particles.Emitting = true;
		//wait until the particles are finished
		await ToSignal(particles, "finished");
		//queue free the particles
		particles.QueueFree();
	}

	/// <summary>
	/// Emitts the chosen particle scene at the given vector2 locataion. The particle scene needs to have OneShot enabled.
	/// </summary>
	/// <param name="position">The position to emitt the particles at</param>
	/// <param name="scene">The particle scene to emitt</param>
	public async void EmitParticles(Vector2 position, PackedScene scene)
	{
		//create a new instance of the given particle scene
		var particles = scene.Instantiate<GpuParticles2D>();
		CheckForOneShot(particles);
		//set the position of the particles to the position of the given node
		particles.GlobalPosition = position;

		Node targetNode = GetTree().CurrentScene.Name == "MainGame" ? GetTree().GetFirstNodeInGroup("Level") : GetTree().GetFirstNodeInGroup("MainMenu");
		targetNode.AddChild(particles); //add the particle to the scene  
		
		//Emitt particles
		particles.Emitting = true;
		//wait until the particles are finished
		await ToSignal(particles, "finished");
		//queue free the particles
		particles.QueueFree();
	}
}
