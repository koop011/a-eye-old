using Godot;
using System;

public partial class player : Area2D
{
	public int speed = 50;
	public Vector2 screen_size;
	public Joystick joystick;

	public override void _Ready()
	{
		//Hide();
		screen_size = GetViewportRect().Size;
		joystick = GetNode<Joystick>("../Joystick-UI/Joystick");

	}

	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;


		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, screen_size.X),
			y: Mathf.Clamp(Position.Y, 0, screen_size.Y)
		);

	}
}
