using Godot;
using System;
using System.Collections;

public class Locust : KinematicBody, WaterCompatible
{
    [Export] public float Speed = 20.0f;
    [Export] public float DistanceTouch = 2f;


    float mTurnCountdown = 1.0f;
    public FlowerSlot mDestination;
    bool mIsAlive = true;
    AnimationPlayer mAnim;
    Animation mFlyingLoop;
    Random rand = new Random();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mAnim = ((AnimationPlayer)GetNode("AnimationPlayer"));
        mFlyingLoop = mAnim.GetAnimation("LocustFlying");
        mFlyingLoop.SetLoop(true);
        mAnim.Play("LocustFlying", -1, 3, false);


        FindTarget();
    }

    void FindTarget(){
        var children = GetNode("/root/World").GetChildren();
        ArrayList FlowerSlots = new ArrayList();
        foreach(var c in children){
            if( c is FlowerSlot ){
                if (((FlowerSlot)c).GetState() != FlowerPot.PotState.EMPTY)
                    FlowerSlots.Add((FlowerSlot) c);
            }
        }


        if(FlowerSlots.Count > 0)
        {
            mDestination = (FlowerSlot)FlowerSlots[rand.Next(FlowerSlots.Count)];
            LookAt(mDestination.GetTranslation(), Vector3.Up);
            SetRotationDegrees(new Vector3( 0.0f, GetRotationDegrees().y, GetRotationDegrees().z ));
        }
        else
            QueueFree();
    }

    public bool NeedWater(){
        return false;
    }

    public void AddWater(){
        if(!mIsAlive)
            return;
        mIsAlive = false;
        var anim = ( AnimationPlayer )GetNode("AnimationPlayer");
        anim.Play("LocustDying", -1, 8, false);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
        if(mDestination != null && mIsAlive)
        {
            // Translate((mDestination.GetTranslation() - GetTranslation()).Normalized() * Speed * delta);
            Translate(Vector3.Forward * Speed * delta);

            mTurnCountdown -= delta;
            if(mTurnCountdown <= 0f){
                mTurnCountdown = (float)rand.NextDouble() * 1f;
                Rotate(Vector3.Up, rand.Next(-10, 10));
            }

            try{
                if(GetTranslation().DistanceTo(mDestination.GetTranslation()) <= DistanceTouch){
                    mIsAlive = false;
                    mFlyingLoop.SetLoop(false);
                    mAnim.Stop();
                    mDestination.QueueFree();
                    QueueFree();
                }
            }catch(Exception e){
                FindTarget();
            }
        }
     }
}
