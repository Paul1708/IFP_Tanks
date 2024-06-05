using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;
using Godot.Collections;

namespace Code.Scripts.Components;
public partial class TrailComponent : Node2D
{
    [Export]
    public float TimeBetweenTrails = 0.1f;
    private ParticleController _particles;
    private Marker2D _leftChain1;
    private Marker2D _leftChain2;
    private Marker2D _rightChain1;
    private Marker2D _rightChain2;
    private Timer _trailTimer;
    private bool _canEmittTrail = true;
    private LevelManager _levelManager;


    private Dictionary<int, PackedScene> _worldIdToTrailScene = new Dictionary<int, PackedScene>()
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
        _trailTimer.WaitTime = TimeBetweenTrails;
        _trailTimer.Timeout += () => _canEmittTrail = true;

        _levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

    }

    private PackedScene GetSceneForWorld()
    {
        int worldId = _levelManager.CurrentWorldId;
        return _worldIdToTrailScene[worldId];
    }

    public void EmitTrail()
    {
        if (_canEmittTrail)
        {
            // If the parent is the DummyTank, we use mud as the trail else we use the trail for the current world
            PackedScene scene = GetParent().Name == "DummyTank" ? Scene.DrivingMud : GetSceneForWorld(); 
            EmitParticlesForAllChains(scene);
            _canEmittTrail = false;
            _trailTimer.WaitTime = TimeBetweenTrails;
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
