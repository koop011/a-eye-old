using Godot;
using System;

public partial class main : Node
{
	public override void _Ready()
	{
		var player = GetNode<player>("Player");
		var start_position = GetNode<Marker2D>("Start-Position");
		player.start_new_game(start_position.Position);
		GetNode<Timer>("Start-Timer").Start();
	}
}
