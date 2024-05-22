using System.Linq;
using Godot;

public partial class MusicController : Node
{
	public void Play(string soundName)
	{
		GetNode<AudioStreamPlayer>(soundName).Play();
	}

	public void PlayMusic(string musicName)
	{
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>(musicName);
		music.Play();
		music.AddToGroup("PlayingMusic");
	}

	public void PlayWorldMusic(int currentWorldID)
	{
		string currentWorld = currentWorldID.ToString();
		PlayMusic("MusicWorld" + currentWorld);
	}
	
	public void StopCurrentMusic()
	{
		// Check if there is any music playing if not return
		if (GetTree().GetNodesInGroup("PlayingMusic").Count == 0)
		{
			return;
		}
		else
		{
			foreach (AudioStreamPlayer music in GetTree().GetNodesInGroup("PlayingMusic").Cast<AudioStreamPlayer>())
			{
				music.Stop();
				music.RemoveFromGroup("PlayingMusic");
			}
			StopCurrentMusic(); // Recursively call the function to make sure all music is stopped
		}
	}
}
struct Sound
{
	public const string ButtonClick = "ButtonClick";
	public const string RocketExplosion = "RocketExplosion";
	public const string TankShooting = "TankShooting";
	public const string CoinPickup = "CoinPickup";
	public const string CountDown = "CountDown";
	public const string OpenShop = "OpenShop";
	public const string Death = "Death";
	public const string Victory = "Victory";
}
