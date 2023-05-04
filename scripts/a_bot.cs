using Godot;
using System;

public partial class a_bot : RigidBody2D
{
    private NavigationLink2D nav_link;
    private NavigationAgent2D nav_agent;
    public override void _Ready()
    {
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        var player = GetNode<player>("Player");
        nav_link = GetNode<NavigationLink2D>("NavigationLink2D");
        nav_link.EndPosition = player.GlobalPosition;
    }

    public override void _Process(double delta)
    {
        
    }
}
