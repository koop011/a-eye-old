using Godot;
using System;
using System.Collections.Generic;

public partial class Weapon_Controller : Node2D
{
	[Export]
	public PackedScene ElectricBasicScene { get; set; }
	private Node2D weaponConsole;
	private Dictionary<String, bool> weaponArmory = new Dictionary<String, bool>();
	private player player;
	private Timer timer;
	public override void _Ready()
	{
		weaponConsole = GetNode<Node2D>("Weapon-Console");
		player = GetNode<player>("../Player");
		// weapons list
		weaponArmory.Add("electric-basic", true);
		timer = GetNode<Timer>("Timer");
		timer.WaitTime = 2;
		timer.Start();
	}

	public override void _Process(double delta)
	{

	}

	private void OnTimerTimeout()
	{
		if (player.IsEnemyInRange)
		{
			WeaponActivation();
		}
	}

	public void AddWeapon(String name)
	{
		weaponArmory[name] = true;
	}

	public Dictionary<String, bool> GetWeaponList()
	{
		return weaponArmory;
	}

	private void WeaponActivation()
	{
		// TODO: play scene if weapon is enabled and the scene is not playing.
		ElectricBasic ElectricBasic = ElectricBasicScene.Instantiate<ElectricBasic>();

		AddChild(ElectricBasic);
		//var attack = GetNode<Node2D>("ElectricBasic");
		//attack.GetNode<Timer>("Timer").Start();
	}
}



