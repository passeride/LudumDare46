using Godot;
using System;

public class Droplet : Area
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Vector3 direction = Vector3.Up;
	[Export] public float Speed = 30.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public void SetPositionAndDirection(Vector3 pos, Vector3 mousePosition){
		GD.Print("Got position:" + pos);
		var mouseDirection = mousePosition - pos;
		// this.LookAt(mousePosition, Vector3.Forward);
	this.LookAtFromPosition(pos, mousePosition, Vector3.Forward);
		this.SetTranslation(pos);
	}



	private void _on_Droplet_body_entered(object body)
	{
		if(( (Node)body ).GetGroups().Contains("WaterCompatible")){
			var waterCompatible = (WaterCompatible)body;
			waterCompatible.AddWater();
			QueueFree();
		}
	}


	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		this.Translate(Vector3.Forward * Speed * delta);
	}
}
