using Godot;
using Godot.Collections;

namespace Enemies;

public partial class InvisBehaviour : Behaviour
{

	private Sprite2D tankSprite;
	private AnimatedSprite2D gunSprite;

	private InvisEnemyState state = InvisEnemyState.FALLBACK;

	public override void Setup()
	{
		tankSprite = GetNode<Sprite2D>("../TankBaseSprite");
		gunSprite = GetNode<AnimatedSprite2D>("../Gun/GunSprite");
		StartFallback();
	}

	public override void ExecuteBehaivour()
	{
		Gun.RotateTowards(Player.GlobalPosition);
		Navigation.NavigateTowards(TargetLocation);
		/*
		Behviour:
		- Move towards the player
		- Once in sight of the player, reveal the tank 
		- Then, after 1.5 seconds, stop moving and start shooting for 2 seconds
		- After shooting, fallback to a random location. 
			- This lasts for 5 seconds or until the enemy reaches the target location
		- Repeat
		*/

		// State machine would be better here, but this will do it for now

		if (state.Equals(InvisEnemyState.SHOOTING))
		{
			Gun.Shoot();
			return;
		}

		if (state.Equals(InvisEnemyState.FALLBACK))
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

	private void StartShooting()
	{
		state = InvisEnemyState.SHOOTING;
		Navigation.IsMooving = false;
		GetTree().CreateTimer(2).Timeout += StartFallback;
	}
	private void StartFallback()
	{
		state = InvisEnemyState.FALLBACK;
		HideSprites();
		TargetLocation = GetRandomTarget();

		Navigation.IsMooving = true;
		GetTree().CreateTimer(5).Timeout += StopFallback;
	}

	private void StopFallback()
	{
		TargetLocation = Player.GlobalPosition;
		state = InvisEnemyState.SEARCHINGFORPLAYER;
	}


	private void HideSprites()
	{
		tankSprite.Visible = false;
		gunSprite.Visible = false;
		//Enemy.AnimationHandler.Active = false;
	}

	private void ShowSprites()
	{
		tankSprite.Visible = true;
		gunSprite.Visible = true;
		//Enemy.AnimationHandler.Active = true;
	}
}

enum InvisEnemyState
{
	SEARCHINGFORPLAYER,
	SHOOTING,
	FALLBACK
}
