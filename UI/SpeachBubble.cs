using Godot;
using System;

public class SpeachBubble : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private string Droplet = "💧";

	public Spatial target;

	public Label textField;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textField = ((Label)GetNode("SpeachBubbleBaseWhite/Message"));

		// textField.Text = Droplet;
	// textField.Set("custom_colors/font_color", new Color(0.0f, 0.0f, 1.0f));
	SetMessage(Droplet, new Color(0f, 0f, 1f));
	}

	public void SetMessage(string icon, Color color){
		textField.Text = icon;
		textField.Set("custom_colors/font_color", color);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
