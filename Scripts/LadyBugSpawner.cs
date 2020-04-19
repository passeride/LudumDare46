using Godot;
using System;

public class LadyBugSpawner : Spatial
{

    [Export] public float SpawnDelay = 1.3f;
    [Export] public float SpawnDelayRandom = 1.3f;

    float mSpawnCountdown = 0.0f;
    PackedScene mLadyBugScene;
    Random rnd = new Random();
    public override void _Ready()
    {
        mLadyBugScene = (PackedScene)ResourceLoader.Load("res://Ladybug/LadyBug.tscn");
    }

    public override void _Process(float delta)
    {
        mSpawnCountdown -= delta;

        if(mSpawnCountdown <= 0f){
            mSpawnCountdown = SpawnDelay + (float)rnd.NextDouble() * SpawnDelayRandom;

            LadyBug ladybug = (LadyBug) mLadyBugScene.Instance();
            ladybug.SetTranslation(GetTranslation() + Vector3.Up);
            GetNode("/root/World").AddChild(ladybug);
        }
    }
}
