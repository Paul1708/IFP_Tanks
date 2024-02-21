using Godot;
using Managers.Level;
using System;

public partial class UpgradeStats : TabBar
{
	HScrollBar hScrollBar;
	Node2D control;
	LevelManager levelManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hScrollBar = GetNode<HScrollBar>("HScrollBar");	
		control = GetNode<Node2D>("RichTextLabel/Control");
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;
		levelManager.OnLevelChanged += ResetScrollBar;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Scroll();
	}

	private void ResetScrollBar()
	{
		hScrollBar.Value = 0;
	}

	private void Scroll()
	{
		Vector2 position = control.Position;
		position.X = (float)-hScrollBar.Value;
		control.Position = position;
	}

}
