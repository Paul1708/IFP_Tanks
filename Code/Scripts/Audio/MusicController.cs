using Godot;

public partial class MusicController : Node
{
	public void Play(string soundName)
	{
		GetNode<AudioStreamPlayer>(soundName).Play();
	}
}
struct Sound
{
	public const string ButtonClick = "ButtonClick";
	public const string RocketExplosion = "RocketExplosion";
	public const string TankShooting = "TankShooting";
	public const string CoinPickup = "CoinPickup";
}
