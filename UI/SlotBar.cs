using Godot;
using System;

public class SlotBar : Control
{
    int ActiveSlot = 1;

    EquipmentSlot waterSlot;
    EquipmentSlot dirtSlot;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        waterSlot = (EquipmentSlot)GetNode("WaterSlot");
        dirtSlot = (EquipmentSlot)GetNode("DirtSlot");

        dirtSlot.SetIcon("💩", new Color(0.3f, 0.3f, 0.3f));
        UpdateActiveSlot();
    }

    void UpdateActiveSlot(){
        switch(ActiveSlot){
            case 1:
                waterSlot.Select();
                dirtSlot.UnSelect();
                break;
            case 2:
                waterSlot.UnSelect();
                dirtSlot.Select();
                break;
            default:
                break;
        }
    }

    public int GetSelectedToolIndex(){
        return ActiveSlot;
    }

    public void SetWaterCount(int waterCount){
         waterSlot.SetCount(waterCount);
    }

    public void SetDirtCount(int dirtCount){
        dirtSlot.SetCount(dirtCount);
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("Slot1")){
            GD.Print("Slot1 Selected");
            ActiveSlot = 1;
            UpdateActiveSlot();
        }
        if(Input.IsActionJustPressed("Slot2")){
            GD.Print("Slot2 Selected");
            ActiveSlot = 2;
            UpdateActiveSlot();
        }
    }
}
