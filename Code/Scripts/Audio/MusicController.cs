using Godot;
using System;

public partial class MusicController : Node
{
	public void ClickButton() {
		GetNode<AudioStreamPlayer>("ButtonClick").Play();
	}
}
