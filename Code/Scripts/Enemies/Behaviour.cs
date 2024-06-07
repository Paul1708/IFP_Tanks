using System;
using Godot;
using Code.Scripts.Managers.Level;
using Code.Scripts.Audio;
using Code.Scripts.Managers;
using Code.Scripts.Weapons;

namespace Code.Scripts.Enemies;

public abstract partial class Behaviour : Node2D
{

    [Export] public int MinPathLength;
    [Export] public int MaxPathLength;
    [Export] public float PathGoalHitRadius;
    [Export] public uint MaxRandomTargetSearchTries;

    protected GunController Gun { get; set; }
    protected Node2D Player { get; set; }
    protected NavigationController Navigation { get; set; }

    protected Enemy Enemy { get; private set; }

    protected LevelManager LevelManager { get; private set; }

    protected Vector2 TargetLocation { get; set; }

    protected bool InRandomMove;
    protected ParticleController Particles;
    protected MusicController MusicController;

    public override void _Ready()
    {
        // Get instances
        Gun = GetParent().GetNode<GunController>("Gun");
        Player = GetTree().GetNodesInGroup("Player")[0] as Node2D;
        Navigation = GetParent().GetNode<NavigationController>("NavigationAgent2D");
        Enemy = GetParent() as Enemy;
        LevelManager = Enemy.GetParent().GetParent().GetParent() as LevelManager;
        Particles = GetNode<ParticleController>("/root/ParticleController");
        MusicController = GetNode<MusicController>("/root/MusicController");
        Setup();
    }

    abstract public void ExecuteBehaivour();
    abstract public void Setup();

    public override void _Process(double delta)
    {
        ExecuteBehaivour();
    }

    public Vector2 GetRandomTarget()
    {
        Vector2 vector = _createRandomVector();
        int count = 0;
        while (!_isValidTarget(vector) && count <= MaxRandomTargetSearchTries)
        {
            vector = _createRandomVector();
            count++;
        }

        return vector;
    }

    private Vector2 _createRandomVector() //weighted towards players direction
    {
        Random rdm = new Random();
        float pathLength = rdm.Next(MinPathLength, MaxPathLength);
        float xPositive = rdm.NextDouble() <= 0.5 ? -1.0F : 1.0F;
        float yPositive = rdm.NextDouble() <= 0.5 ? -1.0F : 1.0F;
        return Enemy.GlobalPosition + (new Vector2(xPositive * rdm.NextSingle(), yPositive * rdm.NextSingle()) * pathLength);
    }

    private bool _isValidTarget(Vector2 candidate)
    {
        Vector2 backup = Navigation.TargetPosition;
        if (candidate.X < 100 || candidate.Y < 100 || candidate.X > 1825 || candidate.Y > 1000)
            return false;
        Navigation.TargetPosition = candidate;
        bool valid = Navigation.IsTargetReachable();
        Navigation.TargetPosition = backup;
        return valid;

    }

}
