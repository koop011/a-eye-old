using Godot;
using System;

public partial class start_screen : Node
{
	private void _on_start_game_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
}
