using Godot;
using System;

public class NessieBall : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Spatial mTarget;
    MeshInstance leftEye;
    MeshInstance rightEye;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mTarget = (Spatial)GetNode("/root/World/Player");
        leftEye = (MeshInstance)GetNode("NessieEyeballLeft");
        rightEye = (MeshInstance)GetNode("NessieEyeballRight");
    }

    public override void _Process(float delta)
    {
        leftEye.LookAt(mTarget.GetTranslation(), Vector3.Up);
        leftEye.Rotate(Vector3.Up, -90f);
        rightEye.LookAt(mTarget.GetTranslation(), Vector3.Up);
        rightEye.Rotate(Vector3.Up, -90f);
        // leftEye.LookAtFromPosition(leftEye.Transform.origin, mTarget.GetTranslation(), Vector3.Up);
        // rightEye.LookAtFromPosition(rightEye.Transform.origin, mTarget.GetTranslation(), Vector3.Up);
    }
}
