using System.Linq;
using UnityEngine;

public class Tool : Weapon<ToolData>
{
    public Tool(Player player) : base(player) { }

    /// <summary>
    /// Checks if the player has a usable tool
    /// </summary>
    /// <returns></returns>
    public override bool CanUse()
    {
        return Get(GetWeapon(player.inventorySlot), player.inventorySlot) != null;
    }

    /// <summary>
    /// Makes the player use a tool
    /// </summary>
    public override void Use()
    {
        //Checks if the player hasn't pressed attack or has the inventory/crafting open
        if (!inputManager.pressedAttack || player.inventoryOpened || player.craftingOpened) return;
        
        //Grabs the tool the player is holding
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null) return;

        //Makes the player start swinging
        player.playerAttack.StartSwinging();

        //Checks if the player has a correct tool
        var correctTool = resourceSystem.CorrectTool(weapon.tool);
        
        if (!correctTool) return;

        //Makes the tool lose durability
        LoseDurability(weapon, resourceSystem.GetResource().resourceObject.toolDamage);
        
        resourceSystem.DestroyResource((int) GetDamage());
    }

    /// <summary>
    /// The amount of damage the tool does to the resources
    /// </summary>
    /// <returns></returns>
    public override float GetDamage()
    {
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null) return 0;
        return weapon.tool.weaponDamage;
    }

    /// <summary>
    /// Grabs the tool data for a specific slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public override ToolData GetWeapon(int slot)
    {
        //Grabs the item the player has selected in his inventory
        var selectedItem = player.inventory.Get(slot);
        if (selectedItem == null || selectedItem.item == null || selectedItem.item.tool == null) return null;

        //Checks if the tool exists in the list if not add it
        if (!WeaponExists(selectedItem.item.tool, slot))
            player.tools.Add(new Tools(selectedItem.item.tool, slot));
        
        return selectedItem.item.tool;
    }

    /// <summary>
    /// Checks if the tool exists in the list
    /// </summary>
    /// <param name="tool"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public override bool WeaponExists(ToolData tool, int slot)
    {
        return player.tools.Where(tools => tools != null && tools.tool != null).Any(tools => tools.tool.Equals(tool) && tools.slot == slot);
    }
    
    /// <summary>
    /// Grabs the tools 
    /// </summary>
    /// <param name="tool"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public Tools Get(ToolData tool, int slot)
    {
        return player.tools.Where(tools => tools != null && tools.tool != null).FirstOrDefault(tools => tools.tool.Equals(tool) && tools.slot == slot);
    }

    /// <summary>
    /// Makes a tool lose durability
    /// </summary>
    /// <param name="weapon">The tool losing durability</param>
    /// <param name="durabilityLost">The amount of durability the tool is losing</param>
    public void LoseDurability(Tools weapon, int durabilityLost)
    {
        weapon.toolDurability -= durabilityLost;
        
        //Removes the tool if it has no durability
        if (weapon.toolDurability <= 0)
        {
            player.inventory.Remove(weapon.tool.item.itemId, 1, weapon.slot);
            player.tools.Remove(weapon);
        }
    }
}