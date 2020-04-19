using Godot;
using System;

public class LocustSpawner : Spatial
{
    [Export] public float SpawnDelay = 1.3f;
    [Export] public float SpawnDelayRandom = 1.3f;

    float mSpawnCountdown = 0.0f;
    PackedScene mLocustScene;
    Random rnd = new Random();
    public override void _Ready()
    {
        mLocustScene = (PackedScene)ResourceLoader.Load("res://Locust/Locust.tscn");
    }

    public override void _Process(float delta)
    {
        mSpawnCountdown -= delta;

        if(mSpawnCountdown <= 0f){
            mSpawnCountdown = SpawnDelay + (float)rnd.NextDouble() * SpawnDelayRandom;

            Locust locust = (Locust) mLocustScene.Instance();
            locust.SetTranslation(GetTranslation() + Vector3.Up);
            GetNode("/root/World").AddChild(locust);
        }
    }
}
