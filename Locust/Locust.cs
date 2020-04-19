using Godot;
using System;
using System.Collections;

public class Locust : KinematicBody, WaterCompatible
{
    [Export] public float Speed = 20.0f;
    [Export] public float DistanceTouch = 1f;
    public FlowerSlot mDestination;
    bool mIsAlive = true;
    AnimationPlayer mAnim;
    Animation mFlyingLoop;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mAnim = ((AnimationPlayer)GetNode("AnimationPlayer"));
        mFlyingLoop = mAnim.GetAnimation("LocustFlying");
        mFlyingLoop.SetLoop(true);
        mAnim.Play("LocustFlying", -1, 3, false);


        var children = GetNode("/root/World").GetChildren();
        ArrayList FlowerSlots = new ArrayList();
        foreach(var c in children){
            if( c is FlowerSlot ){
                if (((FlowerSlot)c).GetState() != FlowerPot.PotState.EMPTY)
                    FlowerSlots.Add((FlowerSlot) c);
            }
        }

        var rand = new Random();

        if(FlowerSlots.Count > 0)
            mDestination = (FlowerSlot)FlowerSlots[rand.Next(FlowerSlots.Count)];
        else
            QueueFree();
    }

    public bool NeedWater(){
        return false;
    }

    public void AddWater(){
        mIsAlive = false;
        var anim = ( AnimationPlayer )GetNode("AnimationPlayer");
        anim.Play("LocustDying", -1, 5, false);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
        if(mDestination != null && mIsAlive)
        {
            Translate((mDestination.GetTranslation() - GetTranslation()).Normalized() * Speed * delta);
            if(GetTranslation().DistanceTo(mDestination.GetTranslation()) <= DistanceTouch){
                mIsAlive = false;
                mFlyingLoop.SetLoop(false);
                mAnim.Stop();
                mDestination.QueueFree();
                QueueFree();
            }
        }
     }
}
