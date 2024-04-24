using Godot;

namespace Weapons;

public partial class LaserGun : Bullet
{

	private Laser _laser;

	protected override void Setup()
	{
		LinearVelocity = Vector2.Zero; //prevent from moving laser like a bullet
		
		_laser = GetNode<Laser>("LaserRay");
		_laser.Setup(this);
	}

	public override void Destroy()
	{
		_laser.SetActive(false);
		QueueFree();
	}

	protected override Node Move()
	{
		_laser.SetActive(true);
		//move laser-source along with player
		var muzzle = player.GetNode<GunController>("Gun").GetNode<Node2D>("muzzle");
		GlobalPosition = muzzle.GlobalPosition;
		_laser.MoveLaserRay(muzzle.GlobalRotation);

		return null;
	}
}
