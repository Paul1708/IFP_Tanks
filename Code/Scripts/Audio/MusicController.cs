using System.Linq;
using Godot;

namespace Code.Scripts.Audio
{
	public partial class MusicController : Node
	{
		/// <summary>
		/// Plays a sound from the the child nodes of the MusicController
		/// </summary>
		/// <param name="soundName"></param>
		public void Play(string soundName)
		{
			GetNode<AudioStreamPlayer>(soundName).Play();
		}

		/// <summary>
		/// Plays a sound that is used as music from the the child nodes of the MusicController and adds it to the PlayingMusic group
		/// </summary>
		/// <param name="musicName"></param>
		public void PlayMusic(string musicName)
		{
			AudioStreamPlayer music = GetNode<AudioStreamPlayer>(musicName);
			music.Play();
			music.AddToGroup("PlayingMusic");
		}

		/// <summary>
		/// Plays the music by using "PlayMusic" for the current world by using the world id
		/// </summary>
		/// <param name="currentWorldId"></param>
		public void PlayWorldMusic(int currentWorldId)
		{
			string currentWorld = currentWorldId.ToString();
			PlayMusic("MusicWorld" + currentWorld);
		}
	
		/// <summary>
		/// Stops the music that is currently playing by checking the PlayingMusic group and removing the music from the group
		/// </summary>
		public void StopCurrentMusic()
		{
			// Check if there is any music playing if not return
			if (GetTree().GetNodesInGroup("PlayingMusic").Count == 0)
				return;
		
			foreach (AudioStreamPlayer music in GetTree().GetNodesInGroup("PlayingMusic").Cast<AudioStreamPlayer>())
			{
				music.Stop();
				music.RemoveFromGroup("PlayingMusic");
			}
			StopCurrentMusic(); // Recursively call the function to make sure all music is stopped
		}
	}

	/// <summary>
	/// Struct that contains all the music names
	/// </summary>
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
}

