using Godot;
using System;

public class LadyBug : KinematicBody, WaterCompatible
{
    [Export] public float Speed = 20.0f;
    public FlowerSlot mDestination;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public bool NeedWater(){
		return false;
	}

	public void AddWater(){
		var anim = ( AnimationPlayer )GetNode("AnimationPlayer");
		anim.Play("LadybugDyingAnimation", -1, 1, false);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
       if(mDestination != null)
       Translate((GetTranslation() - mDestination.GetTranslation()).Normalized() * Speed * delta);

	 }
}
