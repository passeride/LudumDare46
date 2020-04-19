using Godot;
using System;

public class FlowerSlot : StaticBody, WaterCompatible
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export] public int waterDrainPrSecond = 5;
	FlowerPot mPot;
	float mWater = 100f;
	bool hasDirt = false;

	SpeachBubble mSpeachBubble;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mPot = (FlowerPot)GetNode("FlowerPot");
		var pos = GetTranslation();
		var cam = GetTree().GetRoot().GetCamera();
		var screenPos = cam.UnprojectPosition(pos);
		((Control)GetNode("UI")).SetPosition(screenPos);
		mSpeachBubble = ((SpeachBubble)GetNode("UI/SpeachBubble"));
		mSpeachBubble.Visible = false;
	}

	public bool NeedWater(){
		return true;
	}

	public void AddWater(){
		mWater = 100;
	}


	public void SetStatusMessage(){

		if(!hasDirt){
			mSpeachBubble.SetMessage("💩", new Color(0.5f, 0.7f, 0.2f));
		}else{
			mSpeachBubble.SetMessage("👍", new Color(0.9f, 0.7f, 0f));
		}
	}

	public bool NeedDirt(){
		return !hasDirt;
	}

	private void _on_TriggerArea_body_entered(object body)
	{
		if(( (Node)body ).GetGroups().Contains("Player")){
			SetStatusMessage();
			mSpeachBubble.Visible = true;
		}
	}

	private void _on_TriggerArea_body_exited(object body)
	{
		if(( (Node)body ).GetGroups().Contains("Player")){
			mSpeachBubble.Visible = false;
		}
	}
	public void AddDirt(){
		hasDirt = true;
		mPot.SetPotState(FlowerPot.PotState.FILLED);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
	   var waterMeterRect = ((ColorRect)GetNode("UI/WaterMeter"));
	   waterMeterRect.RectSize = new Vector2(waterMeterRect.RectSize.x, mWater);

	 mWater -= waterDrainPrSecond * delta;
	 }
}





