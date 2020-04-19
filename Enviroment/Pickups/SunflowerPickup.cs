using Godot;
using System;

public class SunflowerPickup : RigidBody
{

    [Export] public float PushForce;
    Spatial mTarget;
    public override void _Ready()
    {
        mTarget = (Spatial)GetNode("../Nessie");
    }

    public override void _Process(float delta)
    {
        Vector3 direction = mTarget.GetTranslation() - GetTranslation();
        AddForce(direction.Normalized() * PushForce * delta, Vector3.Zero);
    }
}
