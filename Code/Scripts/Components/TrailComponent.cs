using Godot;
using Managers.Level;

namespace Components;
public partial class TrailComponent : Node2D
{
    [Export]
    float timeBetweenTrails = 0.1f;
    protected ParticleController particles;
    protected Marker2D leftChain1;
    protected Marker2D leftChain2;
    protected Marker2D rightChain1;
    protected Marker2D rightChain2;
    protected Timer trailTimer;
    protected bool canEmittTrail = true;
    protected LevelManager levelManager;


    protected Godot.Collections.Dictionary<int, PackedScene> WorldIdToTrailScene = new Godot.Collections.Dictionary<int, PackedScene>
    {
        {0, Scene.DrivingMud},
        {1, Scene.DrivingGrass},
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        particles = GetNode<ParticleController>("/root/ParticleController");
        leftChain1 = GetNode<Marker2D>("LeftChain1");
        rightChain1 = GetNode<Marker2D>("RightChain1");
        leftChain2 = GetNode<Marker2D>("LeftChain2");
        rightChain2 = GetNode<Marker2D>("RightChain2");
        trailTimer = GetNode<Timer>("TrailTimer");
        trailTimer.WaitTime = timeBetweenTrails;
        trailTimer.Timeout += () => canEmittTrail = true;

        levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

    }

    private PackedScene GetSceneForWorld()
    {
        int worldId = levelManager.currentWorldID;
        return WorldIdToTrailScene[worldId];
    }

    public void EmitTrail()
    {
        if (canEmittTrail)
        {
            // If the parent is the DummyTank, we use mud as the trail else we use the trail for the current world
            PackedScene scene = GetParent().Name == "DummyTank" ? Scene.DrivingMud : GetSceneForWorld(); 
            EmitParticlesForAllChains(scene);
            canEmittTrail = false;
            trailTimer.WaitTime = timeBetweenTrails;
            trailTimer.Start();
        }
    }

    private void EmitParticlesForAllChains(PackedScene scene)
    {
        particles.EmitParticles(leftChain1, scene);
        particles.EmitParticles(rightChain1, scene);
        particles.EmitParticles(leftChain2, scene);
        particles.EmitParticles(rightChain2, scene);
    }
}
