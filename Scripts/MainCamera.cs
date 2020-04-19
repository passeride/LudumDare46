using Godot;
using System;

public class MainCamera : Camera
{
    [Export] public string CameraTargetNodePath;
    [Export] public float CameraFollowDamening = 0.2f;
    [Export] public bool ActivateFollow = false;

    Spatial mTargetNode;
    Vector3 mTargetOffset;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mTargetNode = (Spatial)GetNode(CameraTargetNodePath);

        if (mTargetNode != null)
            mTargetOffset = GetTranslation() - mTargetNode.GetTranslation();
    }

     public override void _Process(float delta)
     {
         if (mTargetNode != null && ActivateFollow){
             SetTranslation(GetTranslation().LinearInterpolate(mTargetNode.Translation + mTargetOffset, CameraFollowDamening));
         }
     }
}
