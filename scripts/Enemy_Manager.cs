using Godot;
using System;

public partial class Enemy_Manager : Node
{
	private Marker2D spawn1;
	private Marker2D spawn2;
	private Marker2D spawn3;
	private Marker2D spawn4;
	public override void _Ready()
	{
		spawn1 = GetNode<Marker2D>("Player/Spawn1");
		spawn2 = GetNode<Marker2D>("Player/Spawn2");
		spawn3 = GetNode<Marker2D>("Player/Spawn3");
		spawn4 = GetNode<Marker2D>("Player/Spawn4");
	}
	private void _on_start_timer_timeout()
	{
		
	}
}
