using Godot;

namespace Scripts;

public partial class HelloWorld : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello World");
	}

	public int Add(int a, int b)
	{
		return a + b;
	}

}