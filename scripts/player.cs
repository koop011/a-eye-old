using Godot;
using System;

public partial class player : Area2D
{
	public int speed = 250;
	private Vector2 screen_size;
	private Joystick joystick;
	private int screen_border_adjuster = 50;
	private Vector2 start_position;
	private AnimatedSprite2D animatedSprite2D;
	private Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		Hide();
		screen_size = GetViewportRect().Size;
		joystick = GetNode<Joystick>("../Joystick-UI/Joystick");
		start_position = GetNode<Marker2D>("../Start-Position").Position;
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		joystick.Connect("UseMoveVector", new Callable(this, nameof(_on_joystick_use_move_vector)));
	}

	public override void _Process(double delta)
	{
		if (!joystick.touched)
		{
			animatedSprite2D.Animation = "idle";
			velocity = Vector2.Zero;
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, screen_border_adjuster, screen_size.X - screen_border_adjuster),
			y: Mathf.Clamp(Position.Y, screen_border_adjuster, screen_size.Y - screen_border_adjuster)
		);
	}

	public void start_new_game(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void _on_joystick_use_move_vector(Vector2 move_vector)
	{
		velocity.X = move_vector.X * speed;
		velocity.Y = move_vector.Y * speed;

		if (move_vector.X > 0.9 || move_vector.X < -0.9)
		{
			animatedSprite2D.Animation = "idle";
		}
		else if (move_vector.Y < -0.3)
		{
			animatedSprite2D.Animation = "up";
		}
		else if (move_vector.Y > 0.3)
		{
			animatedSprite2D.Animation = "down";
		}

		animatedSprite2D.FlipH = move_vector.X < 0;
	}
}
