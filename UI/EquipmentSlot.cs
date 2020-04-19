using Godot;
using System;

public class EquipmentSlot : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export] public Color selectedColor;
	Color unSelectedColor = new Color(0.2f, 0.2f, 0.2f);
	bool mIsSelected = false;
	Label mLabel;
	CanvasItem mOutline;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	  mLabel = (Label)GetNode("SlotBackground/Label");
	  mOutline = (CanvasItem)GetNode("SlotBackground/SlotOutline");
	}

	public void SetIcon(string emoji){
		mLabel.Text = emoji;
	}

	public void SetIcon(string emoji, Color color){
		mLabel.Text = emoji;
		mLabel.Set("custom_colors/font_color", color);
	}

	public void Select(){
		mIsSelected = true;
		mOutline.SelfModulate = selectedColor;
	}

	public void UnSelect(){
		mIsSelected = false;
		mOutline.SelfModulate = unSelectedColor;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
