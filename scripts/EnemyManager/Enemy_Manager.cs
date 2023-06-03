using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy_Manager : Node
{
    [Export]
    public PackedScene a_bot_scene { get; set; }

    private List<Marker2D> spawns = new List<Marker2D>();
    private int spawner_count = 4;
    private int spawn_count = 5;
    private Node2D player;

    public override void _Ready()
    {
        player = GetNode<player>("../Player");
        for (int i = 0; i < spawner_count; i++)
        {
            spawns.Add(player.GetNode<Marker2D>(string.Format("Spawn{0}", i)));
        }
    }

    private void _on_start_timer_timeout()
    {
        GetTree().CallGroup("A-BOTs", Node.MethodName.QueueFree);
        a_bot_spawn();
    }

    private void a_bot_spawn()
    {
        for (int i = 0; i < spawner_count; i++)
        {
            for (int j = 0; j < spawn_count; j++)
            {
                a_bot a_bot = a_bot_scene.Instantiate<a_bot>();
                a_bot.Position = spawns[i].Position;
                AddChild(a_bot);
            }
        }
    }
}
