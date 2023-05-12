using Godot;
using System;

public partial class Enemy_Manager : Node
{
	[Export]
	public PackedScene a_bot_scene {get; set;}
	private System.Collections.Generic.List<Marker2D> spawns = new System.Collections.Generic.List<Marker2D>();
	private int spawn_count = 5;
	
	private Node2D player;
	public override void _Ready()
	{
		player = GetNode<player>("../Player");
		for(int i = 1; i <= spawn_count; i++){
			spawns.Add(player.GetNode<Marker2D>(string.Format("Spawn{0}",i)));
		}		
	}
	private void _on_start_timer_timeout()
	{
		GetTree().CallGroup("A-BOTs", Node.MethodName.QueueFree);
		a_bot_spawn();
	}

	private void a_bot_spawn(){
		for(int i = 0; i < spawn_count; i++){
			a_bot a_bot = a_bot_scene.Instantiate<a_bot>();
			a_bot.Position = spawns[i].Position;
			AddChild(a_bot);
		}	
			// a_bot a_bot = a_bot_scene.Instantiate<a_bot>();
			// a_bot.Position = spawns[0].Position;
			// AddChild(a_bot);
			// a_bot a_bots = a_bot_scene.Instantiate<a_bot>();
			// a_bots.Position = spawns[3].Position;
			// AddChild(a_bots);	
	}
}
