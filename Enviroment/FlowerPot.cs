using Godot;
using System;

public class FlowerPot : Spatial
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public enum PotState{
		EMPTY, FILLED, SUNFLOWER
	}

	PotState CurrentPotState = PotState.EMPTY;

	Spatial PotFill;
	Spatial Sunflower;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PotFill = (Spatial)GetNode("FlowerPot/PotFill");
		Sunflower = (Spatial)GetNode("FlowerPot/Sunflower");
	}

	public void SetPotState(PotState newPotState){
		CurrentPotState = newPotState;
	UpdateModel();
	}

	void UpdateModel(){
		switch(CurrentPotState){
		case PotState.EMPTY:
			PotFill.Visible = false;
			Sunflower.Visible = false;
			break;
		case PotState.FILLED:
			PotFill.Visible = true;
			Sunflower.Visible = false;
			break;
		case PotState.SUNFLOWER:
			PotFill.Visible = false;
			Sunflower.Visible = true;
			break;
		default:
			break;
		}
	}
}
