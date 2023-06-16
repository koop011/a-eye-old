using Godot;
using System;

public partial class ElectricBasic : Node2D
{
	private int coolDown = 2;
	private int _damage = 30;
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
		Position = player.GlobalPosition;
		player.CanMove = false;
		animationPlayer.Play("attack");
		LookAt(player.FindCloestEnemy().GlobalPosition);
		// rotate node 90 degrees in radians to fix the animation.
		Rotate((float)1.5708);
	}

	public override void _Process(double delta)
	{
		// TODO: figure out how to position/rotate from player to cloest enemy.
	}

	private void _on_damage_area_body_entered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			body.Call("TakeDamage", _damage);
		}
	}


	private void OnAnimationPlayerAnimationFinished(StringName anim_name)
	{
		player.CanMove = true;
		QueueFree();
	}
}
