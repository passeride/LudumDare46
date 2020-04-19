using Godot;
using System;

public class Player : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export] public bool ThirdPerson = true;
    [Export] public float Speed = 200.0f;
    [Export] public float DashLength = 5.0f;
    [Export] public float DashCoolDown = 0.8f;
    [Export] public int MaxWaterCapacity = 100;
    [Export] public int WaterShotCost = 10;
    [Export] public int DirtShotCost = 10;
    [Export] public int MaxDirtCapacity = 100;


    int _currentWaterCapacity = 100;
    int _currentDirtCapacity = 100;
    float _DashCDRemaining = 0.0f;
    Vector3 velocity = new Vector3();
    Vector3 mousePosition = new Vector3();

    private const float rayLength = 1000;

    PackedScene waterProjectile;
    PackedScene dirtProjectile;

    SlotBar mSlotBar;

    public enum ToolSelected{
        WATER, DIRT
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        waterProjectile = (PackedScene)ResourceLoader.Load("res://Enviroment/Droplet.tscn");
        dirtProjectile = (PackedScene)ResourceLoader.Load("res://Enviroment/DirtBall.tscn");

        mSlotBar = ((SlotBar)GetNode("/root/World/SlotBar"));
    }


    ToolSelected GetCurrentTool(){
        var toolIndx = mSlotBar.GetSelectedToolIndex();

        switch(toolIndx){
            case 1:
                return ToolSelected.WATER;
            case 2:
                return ToolSelected.DIRT;
            default:
                return ToolSelected.WATER;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouse eventMouseMove){
            var camera = (Camera)GetNode("/root/World/Camera");
            var from = camera.ProjectRayOrigin(eventMouseMove.Position);
            var to = from + camera.ProjectRayNormal(eventMouseMove.Position) * rayLength;

            var raycast = (RayCast)GetNode("/root/World/RayCast");
            raycast.Translation = from;
            raycast.CastTo = to;
            var collision_point = raycast.GetCollisionPoint();
            mousePosition = collision_point;
        }
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == 1)
        {


                var camera = (Camera)GetNode("/root/World/Camera");
                var from = camera.ProjectRayOrigin(eventMouseButton.Position);
                var to = from + camera.ProjectRayNormal(eventMouseButton.Position) * rayLength;

                var raycast = (RayCast)GetNode("/root/World/RayCast");
                raycast.Translation = from;
                raycast.CastTo = to;
                var collision_point = raycast.GetCollisionPoint();

                var tool = GetCurrentTool();

                if (tool == ToolSelected.WATER){

                    if(_currentWaterCapacity >= WaterShotCost){
                        _currentWaterCapacity -= WaterShotCost;
                        GD.Print("Shoooting water");
                        Droplet droplet = (Droplet) waterProjectile.Instance();
                        droplet.SetPositionAndDirection(this.GetTranslation(), collision_point);
                        GetNode("/root/World").AddChild(droplet);
                    }
                }else if (tool == ToolSelected.DIRT){
                    if(_currentDirtCapacity >= DirtShotCost){
                        _currentDirtCapacity -= DirtShotCost;
                        GD.Print("Shoooting dirt");
                        DirtBall dirtball = (DirtBall) dirtProjectile.Instance();
                        dirtball.SetPositionAndDirection(this.GetTranslation(), collision_point);
                        GetNode("/root/World").AddChild(dirtball);
                    }
                }
        }

        if (@event is InputEventAction eventAction && eventAction.Action == "water" && eventAction.Pressed){
            GD.Print("Pouring out that sweet H2Flow");
            _currentWaterCapacity += 20;
        }
    }

    public override void _PhysicsProcess(float delta){
        var camera = (Camera)GetNode("/root/World/Camera");
        var from = camera.ProjectRayOrigin(GetViewport().GetMousePosition());
        var to = from + camera.ProjectRayNormal(GetViewport().GetMousePosition()) * rayLength;
        var spaceState = GetWorld().DirectSpaceState;
        var result = spaceState.IntersectRay(from, to);

    }

    public void GetInput(){
        velocity = new Vector3();

        if (Input.IsActionPressed("right"))
            velocity.x += 1;

        if (Input.IsActionPressed("left"))
            velocity.x -= 1;

        if (Input.IsActionPressed("down"))
            velocity.z += 1;

        if (Input.IsActionPressed("up"))
            velocity.z -= 1;

        velocity = velocity.Normalized() * Speed;
    }

    public void ProcessCollision(KinematicCollision collision){
        if(((Node)collision.Collider).GetGroups().Contains("WaterWell")){
            _currentWaterCapacity = MaxWaterCapacity;
        }

    }

    private void _on_TriggerArea_body_entered(object body)
    {

        GD.Print((( (Node)body ).GetGroups()));
        if(( (Node)body ).GetGroups().Contains("WaterWell")){
            _currentWaterCapacity = MaxWaterCapacity;
        }

        if(( (Node)body ).GetGroups().Contains("DirtPile")){
            _currentDirtCapacity = MaxDirtCapacity;
        }

        if(( (Node)body ).GetGroups().Contains("PlantSlot")){
            var slot = ((FlowerSlot)body);
        }
    }

    public override void _Process(float delta)
    {

        SetTranslation(new Vector3( GetTranslation().x, 0.0f, GetTranslation().z ));

        if (Input.IsActionJustPressed("water")){
            GD.Print("Pouring out that sweet H2Flow");
            _currentWaterCapacity -= 20;
        }

        var waterMeterRect = ((ColorRect)GetNode("/root/World/ResourceHUD/WaterMeter"));
        var dirtMeterRect = ((ColorRect)GetNode("/root/World/ResourceHUD/DirtMeter"));

        waterMeterRect.RectSize = new Vector2(waterMeterRect.RectSize.x, _currentWaterCapacity);
        dirtMeterRect.RectSize = new Vector2(dirtMeterRect.RectSize.x, _currentDirtCapacity);

        LookAt(mousePosition, Vector3.Up);
        GetInput();

        if (_DashCDRemaining > 0.0f){
            _DashCDRemaining =- delta;
        }

        bool dash = false;
        if (Input.IsActionPressed("dash") && _DashCDRemaining <= 0.0f && velocity != Vector3.Zero){
            dash = true;
            _DashCDRemaining = DashCoolDown;
            ((Particles)GetNode("DashParticles")).Emitting = true;
        }


        velocity = MoveAndSlide(velocity * (dash ? DashLength : 1));
        var collisionCount = GetSlideCount();
        for( int i = 0; i < collisionCount; i++){
            var kinematicCollision = GetSlideCollision(i);
            ProcessCollision(kinematicCollision);
        }

        mSlotBar.SetWaterCount(_currentWaterCapacity / WaterShotCost);
        mSlotBar.SetDirtCount(_currentDirtCapacity / DirtShotCost);

        // var coMoveAndSlide(velocity * (dash ? DashLength : 1));
        // var collision = MoveAndCollide(velocity * (dash ? DashLength : 1));
        // velocity = new Vector3();
        //   if (collision != null && collision.Collider != null)

        //       GD.Print(collision.Collider);
    }
}



