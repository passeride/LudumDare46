using Godot;
using System;

public class Nessie : Spatial
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	float mThirst = 100;

	SpeachBubble mSpeachBubble;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var pos = GetTranslation();
		var cam = GetTree().GetRoot().GetCamera();
		var screenPos = cam.UnprojectPosition(pos);
		((Control)GetNode("UI")).SetPosition(screenPos);
		mSpeachBubble = ((SpeachBubble)GetNode("UI/SpeachBubble"));
		mSpeachBubble.Visible = false;
	}

	public void SetStatusMessage(){
		if(mThirst <= 50){
			mSpeachBubble.SetMessage("💧", new Color(0.5f, 0.7f, 0.2f));
		}else{
			mSpeachBubble.SetMessage("👍", new Color(0.9f, 0.7f, 0f));
		}
	}

	private void _on_NessieTriggerArea_body_entered(object body)
	{
		if(( (Node)body ).GetGroups().Contains("Player")){
			SetStatusMessage();
			mSpeachBubble.Visible = true;
		}
	}
	private void _on_NessieTriggerArea_body_exited(object body)
	{
		if(( (Node)body ).GetGroups().Contains("Player")){
			mSpeachBubble.Visible = false;
		}
	}


	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
		 mThirst -= 50f * delta;
	 }
}
