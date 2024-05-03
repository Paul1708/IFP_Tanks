using Godot;
using Managers.Level;

namespace Components;
public partial class TrailComponent : Node2D
{
    [Export]
    public float timeBetweenTrails = 0.1f;
    private ParticleController _particles;
    private Marker2D _leftChain1;
    private Marker2D _leftChain2;
    private Marker2D _rightChain1;
    private Marker2D _rightChain2;
    private Timer _trailTimer;
    private bool _canEmittTrail = true;
    private LevelManager _levelManager;


    private Godot.Collections.Dictionary<int, PackedScene> WorldIdToTrailScene = new Godot.Collections.Dictionary<int, PackedScene>
    {
        {0, Scene.DrivingMud},
        {1, Scene.DrivingGrass},
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _particles = GetNode<ParticleController>("/root/ParticleController");
        _leftChain1 = GetNode<Marker2D>("LeftChain1");
        _rightChain1 = GetNode<Marker2D>("RightChain1");
        _leftChain2 = GetNode<Marker2D>("LeftChain2");
        _rightChain2 = GetNode<Marker2D>("RightChain2");
        _trailTimer = GetNode<Timer>("TrailTimer");
        _trailTimer.WaitTime = timeBetweenTrails;
        _trailTimer.Timeout += () => _canEmittTrail = true;

        _levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

    }

    private PackedScene GetSceneForWorld()
    {
        int worldId = _levelManager.currentWorldID;
        return WorldIdToTrailScene[worldId];
    }

    public void EmitTrail()
    {
        if (_canEmittTrail)
        {
            // If the parent is the DummyTank, we use mud as the trail else we use the trail for the current world
            PackedScene scene = GetParent().Name == "DummyTank" ? Scene.DrivingMud : GetSceneForWorld(); 
            EmitParticlesForAllChains(scene);
            _canEmittTrail = false;
            _trailTimer.WaitTime = timeBetweenTrails;
            _trailTimer.Start();
        }
    }

    private void EmitParticlesForAllChains(PackedScene scene)
    {
        _particles.EmitParticles(_leftChain1, scene);
        _particles.EmitParticles(_rightChain1, scene);
        _particles.EmitParticles(_leftChain2, scene);
        _particles.EmitParticles(_rightChain2, scene);
    }
}
