using Godot;
using Godot.Collections;

namespace Code.Scripts.Enemies;


/// <summary>
/// Invis behaivour of the invis (purple) enemy.
/// </summary>
public partial class InvisBehaviour : Behaviour
{

	private Sprite2D _tankSprite;
	private AnimatedSprite2D _gunSprite;

	private InvisEnemyState _state = InvisEnemyState.Fallback;

	public override void Setup()
	{
		_tankSprite = GetNode<Sprite2D>("../TankBaseSprite");
		_gunSprite = GetNode<AnimatedSprite2D>("../Gun/GunSprite");
		StartFallback();
	}

	/// <summary>
	/// Executes the behaivour of the enemy.
	/// The enemy will move towards the player until the player is in sight.
	///	Once in sight of the player, reveal the tank 
	///	Then, after 1.5 seconds, stop moving and start shooting for 2 seconds
	///	After shooting, fallback to a random location. 
	///	This lasts for 5 seconds or until the enemy reaches the target location
	///	Repeat
	/// </summary>
	public override void ExecuteBehaivour()
	{
		Gun.RotateTowards(Player.GlobalPosition);
		Navigation.NavigateTowards(TargetLocation);

		// State machine would be better here, but this will do it for now

		if (_state.Equals(InvisEnemyState.Shooting))
		{
			Gun.Shoot();
			return;
		}

		if (_state.Equals(InvisEnemyState.Fallback))
		{

			if ((Enemy.GlobalPosition - TargetLocation).Length() <= PathGoalHitRadius)
			{
				TargetLocation = GetRandomTarget();
			}
			return;
		}


		bool inSight = _checkIfPlayerInSight();
		if (inSight)
		{
			ShowSprites();
			GetTree().CreateTimer(1.5).Timeout += StartShooting;
			return;
		}

		TargetLocation = Player.GlobalPosition;
	}


	/// <summary>
	/// Checks if the player is in the line of sight of the enemy by casting a ray from the enemy to the player.
	/// </summary>
	/// <returns name="bool">True if the player is in sight, false otherwise</returns>
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
			return true;
		}

		return false;
	}

	/// <summary>
	/// Get all RIDs of objects that should be excluded from the raycast.
	/// This includes the player, enemies, coins and bullets.
	/// </summary>
	/// <returns name="Array<Rid>">An array of Rids of the objects to exclude from the raycast</returns>
	private Array<Rid> _getExcludedObjects()
	{
		Array<Rid> a = new Array<Rid>(new [] { Enemy.GetRid() });
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

	private void StartShooting()
	{
		_state = InvisEnemyState.Shooting;
		Navigation.IsMooving = false;
		GetTree().CreateTimer(2).Timeout += StartFallback;
	}
	private void StartFallback()
	{
		_state = InvisEnemyState.Fallback;
		HideSprites();
		TargetLocation = GetRandomTarget();

		Navigation.IsMooving = true;
		GetTree().CreateTimer(5).Timeout += StopFallback;
	}

	private void StopFallback()
	{
		TargetLocation = Player.GlobalPosition;
		_state = InvisEnemyState.SearchingForPlayer;
	}


	private void HideSprites()
	{
		_tankSprite.Visible = false;
		_gunSprite.Visible = false;
	}

	private void ShowSprites()
	{
		_tankSprite.Visible = true;
		_gunSprite.Visible = true;
	}
}

enum InvisEnemyState
{
	SearchingForPlayer,
	Shooting,
	Fallback
}
