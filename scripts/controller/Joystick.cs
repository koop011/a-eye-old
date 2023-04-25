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

	public Vector2 move_vector;
	private bool touched = false;

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
			float radius = joystick_area_radius; //radius of *black circle*
			Vector2 center_position = joystick_pad_center; //center of *black circle*
			float distance = joystick.GetGlobalMousePosition().DistanceTo(center_position); //distance from ~green object~ to *black circle*

			if (distance > radius) //If the distance is less than the radius, it is already within the circle.
			{
				Vector2 from_origin_to_object = joystick.GetGlobalMousePosition() - center_position; //~GreenPosition~ - *BlackCenter*
				from_origin_to_object *= radius / distance; //Multiply by radius //Divide by Distance
				joystick.GlobalPosition = center_position + from_origin_to_object; //*BlackCenter* + all that Math
			}
			else
			{
				joystick.GlobalPosition = GetGlobalMousePosition();
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

		if (@event is InputEventScreenDrag dragged){
			move_vector = calculate_move_vector(dragged.Position);
			EmitSignal("UseMoveVectorEventHandler", move_vector);
		}
	}

	public Vector2 calculate_move_vector(Vector2 event_position) {
		var texture_center = new Vector2(150,150);
		//joystick_velocity.X = joystick.GlobalPosition.X / joystick_area_radius;
		//joystick_velocity.Y = joystick.GlobalPosition.Y / joystick_area_radius;
		
		return (event_position - texture_center).Normalized();
	}
}


