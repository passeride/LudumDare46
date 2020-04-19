using Godot;
using System;

public class Eyeball : Spatial
{
    Spatial mTarget;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mTarget = (Spatial)GetNode("/root/World/Player");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        LookAt(mTarget.GetTranslation(), Vector3.Up);
    }
}
