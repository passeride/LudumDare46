using Godot;
using System;

public class MainCamera : Camera
{
    [Export] public string CameraTargetNodePath;
    [Export] public float CameraFollowDamening = 0.2f;
    [Export] public bool ActivateFollow = false;
    [Export] public bool RotateWith = false;
    [Export] public bool MoveParent = true;

    Spatial mTargetNode;
    Vector3 mTargetOffset;
    Vector3 mTargetInitaialRotation;
    Spatial parent;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mTargetNode = (Spatial)GetNode(CameraTargetNodePath);


        if (mTargetNode != null)
        {
            if(RotateWith)
                mTargetInitaialRotation = mTargetNode.GetRotationDegrees();
            if(!MoveParent){
                mTargetOffset = GetTranslation() - mTargetNode.GetTranslation();
            }else{
                parent = (Spatial)GetParent();
                mTargetOffset = parent.GetTranslation() - mTargetNode.GetTranslation();
            }
        }
    }

     public override void _Process(float delta)
     {
         if (mTargetNode != null && ActivateFollow){
             if(!MoveParent){
                 SetTranslation(GetTranslation().LinearInterpolate(mTargetNode.Translation + mTargetOffset, CameraFollowDamening));
             if(RotateWith)
                 SetRotationDegrees(GetRotationDegrees().LinearInterpolate(mTargetNode.GetRotationDegrees() - mTargetInitaialRotation, CameraFollowDamening));
             }else{
                 parent.SetTranslation(parent.GetTranslation().LinearInterpolate(mTargetNode.Translation + mTargetOffset, CameraFollowDamening));
             if(RotateWith)
                 parent.SetRotationDegrees(mTargetNode.GetRotationDegrees() - mTargetInitaialRotation);
             }
         }
     }
}
