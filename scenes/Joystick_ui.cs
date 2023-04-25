using Godot;
using System;

public partial class Joystick_ui : CanvasLayer
{
	[Signal]
	public delegate Vector2 UseMoveVectorEventHandler();

	private Joystick joystick;

	public override void _Process(double delta)
	{
		joystick = GetNode<Joystick>("Joystick");
		EmitSignal("UseMoveVectorEventHandler", joystick.move_vector);
	}
}
