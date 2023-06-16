using Godot;
using System;
using System.Collections.Generic;

public partial class player : Area2D
{
	public int speed = 250;
	private Vector2 screen_size;
	private Joystick joystick;
	private int screen_border_adjuster = 50;
	private Vector2 start_position;
	private AnimatedSprite2D animated_sprite_2D;
	private Vector2 velocity = Vector2.Zero;
	private Weapon_Controller weaponController;
	public bool CanMove = true;
	private List<Node2D> _enemies;
	private Node2D _cloestEnemy;
	public bool IsEnemyInRange = false;

	public override void _Ready()
	{
		Hide();
		screen_size = GetViewportRect().Size;
		joystick = GetNode<Joystick>("../Joystick-UI/Joystick");
		start_position = GetNode<Marker2D>("../Start-Position").Position;
		animated_sprite_2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		weaponController = GetNode<Weapon_Controller>("../Weapon-Controller");
		_enemies = new List<Node2D>();

		joystick.Connect("UseMoveVector", new Callable(this, nameof(_on_joystick_use_move_vector)));
	}

	public override void _Process(double delta)
	{
		if (!joystick.touched)
		{
			animated_sprite_2D.Animation = "idle";
			velocity = Vector2.Zero;
		}
		if (CanMove)
		{
			Position += velocity * (float)delta;
		}

		if (IsEnemyInRange)
		{
			FindCloestEnemy();
		}
		// Move limited to screen size.
		// Position = new Vector2(
		// 	x: Mathf.Clamp(Position.X, screen_border_adjuster, screen_size.X - screen_border_adjuster),
		// 	y: Mathf.Clamp(Position.Y, screen_border_adjuster, screen_size.Y - screen_border_adjuster)
		// );
	}

	public void start_new_game(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void _on_joystick_use_move_vector(Vector2 move_vector)
	{
		velocity = move_vector * (float)speed;

		if (move_vector.X > 0.9 || move_vector.X < -0.9)
		{
			animated_sprite_2D.Animation = "idle";
		}
		else if (move_vector.Y < -0.3)
		{
			animated_sprite_2D.Animation = "up";
		}
		else if (move_vector.Y > 0.3)
		{
			animated_sprite_2D.Animation = "down";
		}

		animated_sprite_2D.FlipH = move_vector.X < 0;
	}

	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			IsEnemyInRange = true;
			_enemies.Add(body);
		}
	}

	private void _on_body_exited(Node2D body)
	{
		_enemies.Remove(body);
	}


	public Node2D FindCloestEnemy()
	{
		foreach (Node2D _enemy in _enemies)
		{
			_cloestEnemy = _enemy;
			if (_enemy.GlobalPosition.DistanceTo(GlobalPosition) < _cloestEnemy.GlobalPosition.DistanceTo(GlobalPosition))
			{
				_cloestEnemy = _enemy;
			}
		}
		
		if (_enemies.Count <= 0)
		{
			IsEnemyInRange = false;
		}

		return _cloestEnemy;
	}

	// private void attack(){
	// 	foreach(KeyValuePair<String, bool> i in weaponController.GetWeaponList()){
	// 		if(i.Value == true){
	// 			GD.Print(String.Format("Attack {} is available", i.Key));
	// 		}
	// 	}
	// }
}
