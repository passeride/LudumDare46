using Godot;
using System;

public class Droplet : Area
{
    Vector3 direction = Vector3.Up;
    [Export] public float Speed = 30.0f;
    [Export] public float TTD = 10.0f;

    float mCountDown = float.MaxValue;
    bool move = true;

    public void SetPositionAndDirection(Vector3 pos, Vector3 mousePosition){
        mCountDown = TTD;
        var mouseDirection = mousePosition - pos;
        this.LookAtFromPosition(pos, mousePosition, Vector3.Forward);
        this.SetTranslation(pos);
    }

    private void _on_Droplet_body_entered(object body)
    {
        if(( (Node)body ).GetGroups().Contains("WaterCompatible")){
            var waterCompatible = (WaterCompatible)body;
            waterCompatible.AddWater();

            move = false;
            ((AnimationPlayer)GetNode("AnimationPlayer")).Play("DropletHit", -1, 1, false);
        }
    }

    public override void _Process(float delta)
    {

        mCountDown -= delta;
        if (mCountDown <= 0f)
            this.QueueFree();

        if(move)
            this.Translate(Vector3.Forward * Speed * delta);
    }
}
