using Godot;
using System;

public class FlowerSlot : StaticBody, WaterCompatible, DirtCompatible
{
    [Export] public int WaterDrainPrSecond = 5;
    [Export] public int FlowerWaterSpwanThreshhold = 80;
    [Export] public int WaterEmojiThreshhold = 30;
    [Export] public int FlowerTotalWaterNeed = 120;
    FlowerPot mPot;
    float mWaterConsumed = 0f;
    float mWater = 100f;

    PackedScene mFlowerPickup;
    SpeachBubble mSpeachBubble;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mSpeachBubble = ((SpeachBubble)GetNode("UI/SpeachBubble"));
        mSpeachBubble.Visible = false;
        mFlowerPickup = (PackedScene)ResourceLoader.Load("res://Enviroment/Pickups/SunflowerPickup.tscn");
    }

    public bool NeedWater(){
        return true;
    }

    public void AddWater(){
        mWater = 100;
    }

    public bool NeedDirt(){
        return mPot.GetPotState() == FlowerPot.PotState.EMPTY;
    }

    public void AddDirt(){
        if(mPot.GetPotState() == FlowerPot.PotState.EMPTY)
            mPot.SetPotState(FlowerPot.PotState.FILLED);
    }

    public FlowerPot.PotState GetState(){
        return mPot!= null ? mPot.GetPotState() : FlowerPot.PotState.EMPTY;
    }

    public void SetStatusMessage(){
        if(mPot.GetPotState() == FlowerPot.PotState.EMPTY){
            mSpeachBubble.SetMessage("💩", new Color(0.5f, 0.7f, 0.2f));
        }else{
            if (mPot.GetPotState() == FlowerPot.PotState.SUNFLOWER && mWater > WaterEmojiThreshhold){
                mSpeachBubble.SetMessage("👍", new Color(0.9f, 0.7f, 0f));
            }else{
                mSpeachBubble.SetMessage("❗💧", new Color(1f, 0f, 0f));
            }
        }
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

    void UseWater(float waterUsed){
        mWater -= waterUsed;
        mWaterConsumed += waterUsed;
    }

    void CompleteFlower(){
        RigidBody flowerpickup = (RigidBody) mFlowerPickup.Instance();
        flowerpickup.SetTranslation(GetTranslation() + Vector3.Up);
        GetNode("/root/World").AddChild(flowerpickup);


        mWater = 0;
        mWaterConsumed = 0;
        mPot.SetPotState(FlowerPot.PotState.EMPTY);
    }

    public override void _Process(float delta)
    {

        mPot = (FlowerPot)GetNode("FlowerPot");
        var pos = GetTranslation();
        var cam = GetTree().GetRoot().GetCamera();
        var screenPos = cam.UnprojectPosition(pos);
        ((Control)GetNode("UI")).SetPosition(screenPos);

        var waterMeterRect = ((ColorRect)GetNode("UI/WaterMeter"));
        waterMeterRect.RectSize = new Vector2(waterMeterRect.RectSize.x, mWater);

        if(mPot.GetPotState() == FlowerPot.PotState.SUNFLOWER){
            UseWater(WaterDrainPrSecond * delta);
        }

        if(mPot.GetPotState() == FlowerPot.PotState.FILLED && mWater > FlowerWaterSpwanThreshhold){
            UseWater(20);
            mPot.SetPotState(FlowerPot.PotState.SUNFLOWER);
        }

        if (mPot.GetPotState() == FlowerPot.PotState.SUNFLOWER && mWater <= 0){
            mPot.SetPotState(FlowerPot.PotState.EMPTY);
        }

        if (mPot.GetPotState() == FlowerPot.PotState.SUNFLOWER && mWaterConsumed >= FlowerTotalWaterNeed){
            CompleteFlower();
        }

        SetStatusMessage();
    }
}





