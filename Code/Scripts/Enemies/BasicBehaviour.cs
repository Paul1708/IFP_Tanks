using Godot;
using Godot.Collections;

namespace Code.Scripts.Enemies;


/// <summary>
/// Basic behaivour of the basic (red) enemy.
/// </summary>
public partial class BasicBehaviour : Behaviour
{

	public override void Setup()
	{
		TargetLocation = GetRandomTarget();
	}

	/// <summary>
	/// Executes the behaivour of the enemy.
	/// The enemy will move towards the player until the player is in sight.
	/// When the player is in sight, the enemy will shoot at the player and move towards a random target.
	/// </summary>
	public override void ExecuteBehaivour()
	{
		Gun.RotateTowards(Player.GlobalPosition);
		bool inSight = _checkIfPlayerInSight();
		if (inSight)
		{
			if (!InRandomMove)
			{
				TargetLocation = GetRandomTarget();
				InRandomMove = true;
			}
		}
		else
		{
			TargetLocation = Player.GlobalPosition;
			InRandomMove = false;
		}


		//check if target reached, if so, find a new random target
		Navigation.NavigateTowards(TargetLocation);
		if ((Enemy.GlobalPosition - TargetLocation).Length() <= PathGoalHitRadius)
		{
			//find a new target
			TargetLocation = GetRandomTarget();
		}
	}

	/// <summary>
	/// Checks if the player is in the line of sight of the enemy by casting a ray from the enemy to the player.
	/// </summary>
	protected bool _checkIfPlayerInSight()
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		Dictionary sightCheck = spaceState.IntersectRay(PhysicsRayQueryParameters2D.Create(Enemy.Position,
			Player.Position, 1, _getExcludedObjects()));
		var colliderIdObj = sightCheck["collider_id"].Obj;
		long colliderId = colliderIdObj != null ? (long)colliderIdObj : -1;

		var collider = InstanceFromId((ulong)colliderId);
		//if the collided object is a wall, then the player is not in the line of sight
		if (collider is not TileMap)
		{
			Gun.Shoot();
			//abort current navigation and 
			return true;
		}

		return false;
	}

	/// <summary>
	/// Get all RIDs of objects that should be excluded from the raycast.
	/// This includes the player, enemies, coins and bullets.
	/// </summary>
	/// <returns>An array of Rids of the objects to exclude from the raycast</returns>
	private Array<Rid> _getExcludedObjects()
	{
		Array<Rid> a = new Array<Rid>(new []{ Enemy.GetRid() });
		_addFromList(a, GetTree().GetNodesInGroup("Enemy"));
		_addFromList(a, GetTree().GetNodesInGroup("Coins"));
		_addFromList(a, GetTree().GetNodesInGroup("Bullets"));

		return a;
	}

	/// <summary>
	/// Adds all the Rids of the nodes in the list to the array.
	/// </summary>
	/// <param name="array">The array to add the Rids to</param>
	/// <param name="list">The list of nodes to get the Rids from</param>
	private void _addFromList(Array<Rid> array, Array<Node> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null) 
				continue;
			
			array.Add((list[i] as CollisionObject2D).GetRid());
		}
	}

}
