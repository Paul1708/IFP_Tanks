using Godot;
using Godot.Collections;

namespace Enemies;

public partial class BasicBehaviour : Behaviour
{
	
	public override void Setup()
	{
		TargetLocation = GetRandomTarget();
	}
	
	public override void ExecuteBehaivour()
	{
		Gun.RotateTowards(Player.GlobalPosition);
		bool inSight = _checkIfPlayerInSight();
		if (inSight)
		{
			if (!_inRandomMove)
			{
				TargetLocation = GetRandomTarget();
				_inRandomMove = true;
			}
		}
		else
		{
			TargetLocation = Player.GlobalPosition;
			_inRandomMove = false;
		}
		
		
		//check if target reached, if so, find a new random target
		Navigation.NavigateTowards(TargetLocation);
		if ((Enemy.GlobalPosition - TargetLocation).Length() <= PathGoalHitRadius)
		{
			//find a new target
			TargetLocation = GetRandomTarget();
		}
	}

	protected bool _checkIfPlayerInSight()
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		Dictionary sightCheck = spaceState.IntersectRay(PhysicsRayQueryParameters2D.Create(Enemy.Position, 
			Player.Position, Enemy.CollisionMask, _getExcludedObjects()));
		var colliderIdObj = sightCheck["collider_id"].Obj;
		long colliderId = colliderIdObj != null ? (long) colliderIdObj : -1;

		var collider = InstanceFromId((ulong) colliderId);
		//if the collided object is a wall, then the player is not in the line of sight
		if (collider is not TileMap)
		{
			Gun.Shoot();
			//abort current navigation and 
			return true;
		}

		return false;
	}

	//Ignore bullets, coins and the casting enemy itself
	private Array<Rid> _getExcludedObjects()
	{
		Array<Rid> a = new Array<Rid>(new Rid[] { Enemy.GetRid() });
		_addFromList(a, GetTree().GetNodesInGroup("Enemy"));
		_addFromList(a, GetTree().GetNodesInGroup("Coins"));
		_addFromList(a, GetTree().GetNodesInGroup("Bullets"));

		return a;
	}

	private void _addFromList(Array<Rid> array, Array<Node> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null) continue;
			array.Add((list[i] as CollisionObject2D).GetRid());
		}
	}
	
}
