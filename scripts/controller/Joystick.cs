using Godot;
using System;

public partial class Joystick : Area2D
{
	[Signal]
	public delegate Vector2 UseMoveVectorEventHandler();
	
	private Node2D joystick_pad;
	private Node2D joystick;
	private float joystick_area_radius;
	private Vector2 joystick_pad_center;
	private Vector2 move_vector;
	private Vector2 joystick_radius_limit;
	public bool touched = false;

	public override void _Ready()
	{
		joystick_pad = GetNode<Sprite2D>("joystick_pad");
		joystick = joystick_pad.GetNode<Sprite2D>("joystick");
		joystick_area_radius = ((CircleShape2D)GetNode<CollisionShape2D>("joystick_area").Shape).Radius;
		Hide();
	}

	public override void _Process(double delta)
	{
		if (touched)
		{
			float joystick_radius = joystick_area_radius;
			Vector2 center_position = joystick_pad_center;
			float distance = joystick.GetGlobalMousePosition().DistanceTo(center_position);
			Vector2 from_origin_to_object = joystick.GetGlobalMousePosition() - center_position;
			from_origin_to_object *= joystick_radius / distance;
			try
			{
				joystick_radius_limit = joystick_radius_limit.Clamp(new Vector2(0, 0), center_position + from_origin_to_object);
			}
			// Ignore joystick math clamp exception, when the user drags the joystick outside of the gameplay scene.
			catch
			{ }

			if (distance > joystick_radius)
			{
				joystick.GlobalPosition = center_position + from_origin_to_object;
			}
			else
			{
				joystick.GlobalPosition = joystick.GetGlobalMousePosition();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch touch)
		{
			if (touch.Pressed)
			{
				touched = true;
				joystick_pad.GlobalPosition = GetGlobalMousePosition();
				joystick_pad_center = joystick_pad.GlobalPosition;

				Show();
			}
			else
			{
				touched = false;
				Hide();
			}
		}

		if (@event is InputEventScreenTouch touched_event)
		{
			move_vector = calculate_move_vector(touched_event.Position);

		}
		else if (@event is InputEventScreenDrag drag_event)
		{

			move_vector = calculate_move_vector(drag_event.Position);
			EmitSignal(SignalName.UseMoveVector, calculate_move_vector(drag_event.Position));
		}
	}

	public Vector2 calculate_move_vector(Vector2 event_position)
	{
		return (event_position - joystick_pad_center).Normalized();
	}
}
