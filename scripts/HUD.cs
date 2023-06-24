using Godot;
using System;

public partial class HUD : CanvasLayer
{
	public void update_score(int score)
	{
		GetNode<Label>("Score").Text = score.ToString();
	}
}
