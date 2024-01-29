using Godot;
using System;

//Particle scenes
struct Scene
{
	public static readonly PackedScene BulletCrack = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/BulletCrack.tscn");
	public static readonly PackedScene Explosion = ResourceLoader.Load<PackedScene>("res://Scenes/Enviroment/Particles/Explosion.tscn");
}

public partial class ParticleController : Node2D
{
	//Sets the position of the particle system to the position of the given opject
	private static void SetPostion(GpuParticles2D particles, Node2D position)
	{
		particles.GlobalPosition = position.GlobalPosition;
	}
	//Checks if the given particle scene is set to OneShot
	private static void CheckForOneShot(GpuParticles2D particles)
	{
		if (particles.OneShot == false)
		{
			throw new Exception("The given particle scene is not set to OneShot: " + particles.Name);
		}
	}

	//Emitts the chosen particle scene at the given nodes locataion
	//The particle scene needs to have OneShot enabled
	public async void EmitParticles(Node2D position, PackedScene scene)
	{
		//create a new instance of the given particle scene
		var particles = scene.Instantiate<GpuParticles2D>();
		CheckForOneShot(particles);
        GetTree().GetFirstNodeInGroup("Level").AddChild(particles); //add the particle to the scene  
        //set the position of the particles to the position of the given node
        SetPostion(particles, position);
		//Emitt particles
		particles.Emitting = true;
		//wait until the particles are finished
		await ToSignal(particles, "finished");
		//queue free the particles
		particles.QueueFree();

	}
}
