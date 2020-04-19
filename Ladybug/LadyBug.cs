using Godot;
using System;
using System.Collections;

public class LadyBug : KinematicBody, WaterCompatible
{
    [Export] public float Speed = 20.0f;
    [Export] public float DistanceTouch = 1f;
    public FlowerSlot mDestination;
    bool mIsAlive = true;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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
        anim.Play("LadybugDyingAnimation", -1, 5, false);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(mDestination != null && mIsAlive)
        {

            try{
                LookAt(mDestination.GetTranslation(), Vector3.Up);
                Translate((mDestination.GetTranslation() - GetTranslation()).Normalized() * Speed * delta);
                if(GetTranslation().DistanceTo(mDestination.GetTranslation()) <= DistanceTouch){
                    mDestination.QueueFree();
                    QueueFree();
                }
            }catch(Exception e){
                FindTarget();
            }
        }
        // Translate((GetTranslation() - mDestination.GetTranslation()).Normalized() * Speed * delta);

    }
}
