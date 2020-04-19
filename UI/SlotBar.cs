using Godot;
using System;

public class SlotBar : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	int ActiveSlot = 1;

	EquipmentSlot waterSlot;
	EquipmentSlot dirtSlot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		waterSlot = (EquipmentSlot)GetNode("WaterSlot");
		dirtSlot = (EquipmentSlot)GetNode("DirtSlot");

		dirtSlot.SetIcon("💩", new Color(0.3f, 0.3f, 0.3f));
		UpdateActiveSlot();
	}

	void UpdateActiveSlot(){
		switch(ActiveSlot){
			case 1:
				waterSlot.Select();
				dirtSlot.UnSelect();
				break;
			case 2:
				waterSlot.UnSelect();
				dirtSlot.Select();
				break;
			default:
				break;
		}
	}


	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
	   if(Input.IsActionJustPressed("Slot1")){
		   GD.Print("Slot1 Selected");
		   ActiveSlot = 1;
		   UpdateActiveSlot();
	   }
	   if(Input.IsActionJustPressed("Slot2")){
		   GD.Print("Slot2 Selected");
		   ActiveSlot = 2;
		   UpdateActiveSlot();
	   }
	 }
}
