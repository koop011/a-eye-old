using Godot;
using System;

public partial class ElectricBasic : Node2D
{
	private int coolDown = 2;
	private int damage = 30;
	private Timer timer;
	private AnimationPlayer animationPlayer;
	private player player;
	public bool IsDisabled = true;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.WaitTime = coolDown;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		player = GetNode<player>("../../Player");
		timer.SetMeta("Disabled", IsDisabled);
	}

	public override void _Process(double delta)
	{
		// TODO: figure out how to position/rotate from player to cloest enemy.
	}

	private void OnTimerTimeout()
	{
		GD.Print("Basic Attack Timeout");
		Position = player.Position;
		animationPlayer.Play();
		QueueFree();


		// attack enemy
		// TODO: get cloest enemy position to attack from player attack range collision.
	}

}
