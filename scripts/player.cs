using Godot;
using System;
using System.Collections.Generic;

public partial class player : Area2D
{
	private int _speed = 250;
	private int _healthPoints = 1000;
	private Vector2 screen_size;
	private Joystick joystick;
	private int screen_border_adjuster = 50;
	private Vector2 start_position;
	private AnimatedSprite2D animated_sprite_2D;
	private Vector2 velocity = Vector2.Zero;
	private Weapon_Controller weaponController;
	public bool CanMove = true;
	private List<Node2D> _enemiesInRange;
	private List<Node2D> _enemiesOnCharacter;
	private Node2D _cloestEnemy;
	public bool IsEnemyInRange = false;
	private bool IsEnemyOnCharacter = false;
	private int _enemyDamage = 0;
	private Timer _enemyDamageTimer;

	public override void _Ready()
	{
		Hide();
		screen_size = GetViewportRect().Size;
		joystick = GetNode<Joystick>("../Joystick-UI/Joystick");
		start_position = GetNode<Marker2D>("../Start-Position").Position;
		animated_sprite_2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		weaponController = GetNode<Weapon_Controller>("../Weapon-Controller");
		_enemiesInRange = new List<Node2D>();
		_enemiesOnCharacter = new List<Node2D>();
		_enemyDamageTimer = GetNode<Timer>("EnemyDamageTimer");
		_enemyDamageTimer.WaitTime = 0.5;
		_enemyDamageTimer.Start();

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
	}

	private void _on_joystick_use_move_vector(Vector2 move_vector)
	{
		velocity = move_vector * (float)_speed;

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
			_enemiesInRange.Add(body);
		}
	}

	private void _on_body_exited(Node2D body)
	{
		_enemiesInRange.Remove(body);
	}


	public Node2D FindCloestEnemy()
	{
		foreach (Node2D _enemy in _enemiesInRange)
		{
			_cloestEnemy = _enemy;
			if (_enemy.GlobalPosition.DistanceTo(GlobalPosition) < _cloestEnemy.GlobalPosition.DistanceTo(GlobalPosition))
			{
				_cloestEnemy = _enemy;
			}
		}
		
		if (_enemiesInRange.Count <= 0)
		{
			IsEnemyInRange = false;
		}

		return _cloestEnemy;
	}
	
	private void _on_body_area_body_entered(Node2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			IsEnemyOnCharacter = true;
			_enemiesOnCharacter.Add(body);
			_enemyDamage += (int)body.Call("GetDamagePoints");
		}
	}

	private void _on_body_area_body_exited(Node2D body)
	{
		_enemiesOnCharacter.Remove(body);
	}

	private void _on_enemy_damage_timer_timeout()
	{
		if (IsEnemyOnCharacter)
		{
			TakeDamage(_enemyDamage);
		}
	}

	private void TakeDamage(int Damage)
	{
		_healthPoints = Math.Max(_healthPoints - Damage, 0);
		if (_healthPoints <= 0)
		{
			Death();
		}

		if (_enemiesOnCharacter.Count <= 0)
		{
			IsEnemyOnCharacter = false;
		}
	}

	private void Death()
	{
		QueueFree();
		// Insert game ending mechanic.
	}

	// private void attack(){
	// 	foreach(KeyValuePair<String, bool> i in weaponController.GetWeaponList()){
	// 		if(i.Value == true){
	// 			GD.Print(String.Format("Attack {} is available", i.Key));
	// 		}
	// 	}
	// }
}



