using Godot;
using System;
using System.Threading;

public partial class settings_menu : Control
{
		private void _on_back_pressed() {
			var MusicController = GetNode<MusicController>("/root/MusicController");
			MusicController.ClickButton();
			GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
		}
		
}
