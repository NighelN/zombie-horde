using UnityEngine;

[System.Serializable]
public class Tools
{
    public ToolData tool;
    public int slot;
    public float toolDurability;

    public Tools(ToolData tool, int slot)
    {
        this.tool = tool;
        this.slot = slot;
        toolDurability = tool.weaponDurability;
    }

    public void SetSlot(int newSlot)
    {
        slot = newSlot;
    }
}