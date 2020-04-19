using Godot;
using System;

public class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }


    private void _on_StartGameButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/GameScene_1.tscn");
    }

    private void _on_QuitGameButton_pressed()
    {
        GetTree().Quit();
    }
}
