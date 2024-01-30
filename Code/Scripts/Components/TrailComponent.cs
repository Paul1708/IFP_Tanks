using Godot;
using System;
using System.Runtime.Serialization;
namespace Movement;
public partial class TrailComponent : Node2D
{
    protected ParticleController particles;
    protected Marker2D leftChain1;
    protected Marker2D leftChain2;
    protected Marker2D rightChain1;
    protected Marker2D rightChain2;
    protected Timer trailTimer;
    protected bool canEmittTrail = true;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        particles = GetNode<ParticleController>("/root/ParticleController");
        leftChain1 = GetNode<Marker2D>("LeftChain1");
        rightChain1 = GetNode<Marker2D>("RightChain1");
        leftChain2 = GetNode<Marker2D>("LeftChain2");
        rightChain2 = GetNode<Marker2D>("RightChain2");
        trailTimer = GetNode<Timer>("TrailTimer");
        trailTimer.Timeout += () => canEmittTrail = true;
    }

    public void EmitTrail()
    {
        if (canEmittTrail)
        {
            particles.EmitParticles(leftChain1, Scene.DrivingMud);
            particles.EmitParticles(rightChain1, Scene.DrivingMud);
            particles.EmitParticles(leftChain2, Scene.DrivingMud);
            particles.EmitParticles(rightChain2, Scene.DrivingMud);
            canEmittTrail = false;
            trailTimer.Start();
        }
    }
}
