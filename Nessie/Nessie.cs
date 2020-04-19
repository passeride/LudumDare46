using Godot;
using System;

public class Nessie : Spatial, WaterCompatible, DirtCompatible
{
    [Export] public float WaterDrainPrSec = 15.0f;
    [Export] public float FoodDrainPrSec = 15.0f;

    [Export] public float WaterNotifiyThreshhold = 40f;
    [Export] public float WaterDangerThreshhold = 20f;

    float mThirst = 100;
    float mHunger = 100;

    AnimationPlayer mAnim;

    SpeachBubble mSpeachBubble;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mAnim = (AnimationPlayer)GetNode("AnimationPlayer");
        mSpeachBubble = ((SpeachBubble)GetNode("UI/SpeachBubble"));
        mSpeachBubble.Visible = false;
    }


    public bool NeedWater(){
        return true;
    }

    public void AddWater(){
        if ( mAnim.IsPlaying() && ( mAnim.CurrentAnimation == "NessieIsThirsty" || mAnim.CurrentAnimation == "NessieIsVeryThirsty" ))
        {
            mAnim.Stop(true);
            mSpeachBubble.Visible = false;
        }
        mThirst = 100;
    }

    public bool NeedDirt(){
        return false;
    }

    public void AddDirt(){
        mHunger -= 20;
        mAnim.Play("NessieGotDirt", -1, 1, false);
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

        if(( (Node)body ).GetGroups().Contains("Food")){
            mHunger += 10;
            ((RigidBody)body).QueueFree();
            mAnim.Play("NessieGotFood", -1, 1, false);
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

        var pos = GetTranslation();
        var cam = GetTree().GetRoot().GetCamera();
        var screenPos = cam.UnprojectPosition(pos);
        ((Control)GetNode("UI")).SetPosition(screenPos);

        mThirst -= WaterDrainPrSec * delta;
        mHunger -= FoodDrainPrSec * delta;


        if(mThirst <= WaterNotifiyThreshhold){
            if (mThirst <= WaterDangerThreshhold)
                mAnim.Play("NessieIsVeryThirsty", -1, 1, false);
            else
                mAnim.Play("NessieIsThirsty", -1, 1, false);
        }
    }
}
