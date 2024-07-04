using Godot;

namespace Code.Scripts.Weapons;

/// <summary>
/// Controls the laser gun which emits a laser beam
/// </summary>
public partial class LaserGun : Bullet
{

	private Laser _laser;

	protected override void Setup()
	{
		LinearVelocity = Vector2.Zero; //prevent from moving laser like a bullet
		
		_laser = GetNode<Laser>("LaserRay");
		_laser.Setup(this);
	}

	/// <summary>
	/// Destroys the laser beam
	/// </summary>
	public override void Destroy()
	{
		_laser.SetActive(false);
		QueueFree();
	}

	/// <summary>
	/// Moves the laser beam along with the player
	/// </summary>
	/// <returns></returns>
	protected override Node Move()
	{
		_laser.SetActive(true);
		//move laser-source along with player
		var muzzle = Player.GetNode<GunController>("Gun").GetNode<Node2D>("muzzle");
		GlobalPosition = muzzle.GlobalPosition;
		_laser.MoveLaserRay(muzzle.GlobalRotation);

		return null;
	}
}
