using Godot;
using System;

public partial class a_bot : RigidBody2D
{
    private NavigationAgent2D nav_agent;
    private Timer target_find_timer;
    private int speed = 300;
    private int _healthPoints = 100;
    private Vector2 velocity;
    private Vector2 next_location;
    private Vector2 direction;
    private player player;
    private float movement_delta;
    private int counter;
    private AnimatedSprite2D animated_sprite_2D;

    public override void _Ready()
    {
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        player = GetNode<player>("../../Player");
        nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        target_find_timer = GetNode<Timer>("../Target-Finder");
        target_find_timer.Connect("timeout", new Callable(this, nameof(_on_target_finder_timeout)));
        animated_sprite_2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        nav_agent.PathDesiredDistance = 1;
        nav_agent.TargetDesiredDistance = 1;
        nav_agent.AvoidanceEnabled = true;
        LockRotation = true;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (nav_agent.IsTargetReachable())
        {
            next_location = nav_agent.GetNextPathPosition();
            direction = Position.DirectionTo(next_location).Normalized();
            velocity = direction * speed;
            nav_agent.SetVelocity(velocity);
        }
        else
        {
            LinearVelocity = Vector2.Zero;
        }

        if (direction.X > 0.9 || direction.X < -0.9)
        {
            animated_sprite_2D.Animation = "idle";
        }
        else if (direction.Y < -0.3)
        {
            animated_sprite_2D.Animation = "up";
        }
        else if (direction.Y > 0.3)
        {
            animated_sprite_2D.Animation = "down";
        }

        animated_sprite_2D.FlipH = direction.X < 0;
    }

    private void _on_navigation_agent_2d_velocity_computed(Vector2 safe_velocity)
    {
        LinearVelocity = safe_velocity;
    }

    private void _on_target_finder_timeout()
    {
        nav_agent.TargetPosition = player.GlobalPosition;
    }

    private void TakeDamage(int Damage)
    {
        _healthPoints = Math.Max(_healthPoints - Damage, 0);

        if (_healthPoints <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        QueueFree();
    }
}
