using Godot;
using System;

public partial class a_bot : RigidBody2D
{
	private NavigationAgent2D nav_agent;
	private Timer target_find_timer;
	private int nav_agent_speed = 300;
	private Vector2 a_bot_velocity;
	private player player;
	private float movement_delta;
	private int counter;
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		player = GetNode<player>("../../Player");
		nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		target_find_timer = GetNode<Timer>("../Target-Finder");
		target_find_timer.Connect("timeout", new Callable(this, nameof(_on_target_finder_timeout)));
		nav_agent.PathDesiredDistance = 1;
		nav_agent.TargetDesiredDistance = 1;		
		nav_agent.AvoidanceEnabled = true;
		
	}

	public override void _Process(double delta){
		// GD.Print(nav_agent.IsTargetReachable());
		// GD.Print(a_bot_velocity);
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state){
		//if(nav_agent.IsTargetReachable()){
			
			var next_location = nav_agent.GetNextPathPosition();
			a_bot_velocity = Position.DirectionTo(next_location).Normalized() * nav_agent_speed;
			GD.Print(a_bot_velocity);
			nav_agent.SetVelocity(a_bot_velocity);
			
		//}
		//else{
			LinearVelocity = Vector2.Zero;
		//}
	}

	private void _on_navigation_agent_2d_velocity_computed(Vector2 safe_velocity)
	{
		LinearVelocity = safe_velocity;
	}

	private void _on_target_finder_timeout()
	{
		counter += 1;
		GD.Print(counter);
		nav_agent.TargetPosition = player.GlobalPosition;
	}
}
