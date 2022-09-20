using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Container
{
    /// <summary>
    /// Reference to the game manager
    /// </summary>
    GameManager gameManager => GameManager.instance;
    
    /// <summary>
    /// An array of all the items in the container
    /// </summary>
    public ItemData[] items;

    /// <summary>
    /// Initialize the container
    /// </summary>
    /// <param name="size">The size of the container</param>
    public Container(int size)
    {
        //Sets the array to the correct size
        items = new ItemData[size];
        //Fills array with empty item data
        for (int index = 0; index < size; index++)
            items[index] = new ItemData();
    }

    /// <summary>
    /// Handles swapping items from one slot to another slot
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void Swap(int from, int to)
    {
        //Creates an copy of the item
        var temp = Get(from);
        
        //Spawns around the 2 items
        items[from] = Get(to);
        items[to] = temp;
    }

    /// <summary>
    /// Grabs the item for a slot
    /// </summary>
    /// <param name="slot">The slot your trying to get the item for</param>
    /// <returns>Grabs an item based on a slot</returns>
    public ItemData Get(int slot)
    {
        return items[slot];
    }
    
    /// <summary>
    /// Checks if the player has the required item
    /// </summary>
    /// <param name="itemId">The item id</param>
    /// <param name="amount">The amount the player has to have</param>
    /// <returns>If the player has the required items</returns>
    public bool Contains(int itemId, int amount = 1)
    {
        return items.Where(i => i != null && i.item != null).Any(i => i.item.itemId == itemId && i.amount >= amount);
    }

    /// <summary>
    /// Checks if the player has all the items in he list
    /// </summary>
    /// <param name="itemsData">A list of all the items required</param>
    /// <returns>Checks if the player has all the items in he list</returns>
    public bool ContainsAll(List<ItemData> itemsData)
    {
        return itemsData.All(item => Contains(item.item.itemId, item.amount));
    }

    /// <summary>
    /// Grabs the slot based on an item id
    /// </summary>
    /// <param name="id">The item id</param>
    /// <returns>An slot based on item id</returns>
    public int GetSlot(int id)
    {
        //Loops though all the items
        for (var index = 0; index < items.Length; index++)
        {
            //Checks if the itemdata or item is null
            if (items[index] == null || items[index].item == null) continue;
            //Checks if the ids match
            if (items[index].item.itemId == id) return index;
        }
        return -1;
    }

    /// <summary>
    /// Grabs the amount of items you have in a certain slot
    /// </summary>
    /// <param name="slot">The slot your trying to grab the amount for</param>
    /// <returns>Grabs the amount of items you have in a certain slot</returns>
    public int GetAmount(int slot)
    {
        return (from i in items where i != null && !i.item where Get(slot).item.itemId == i.item.itemId select i.amount).Sum();
    }

    /// <summary>
    /// Grabs the amount of items you have of a certain item id
    /// </summary>
    /// <param name="id">The item id your looking up</param>
    /// <returns>Grabs the amount of items you have of a certain item id</returns>
    public int GetAmountFromItem(int id)
    {
        return (from i in items where i != null && i.item != null where id == i.item.itemId select i.amount).Sum();
    }

    /// <summary>
    /// Grabs the next free available slot
    /// </summary>
    /// <returns>Grabs the next free available slot</returns>
    public int FreeSlot()
    {
        //Loops though all the items
        //And checks if there is no item on that slot
        for (var index = 0; index < items.Length; index++)
            if (items[index] == null || (items[index].item == null || items[index].item.itemId == -1)) return index;
        return -1;
    }

    /// <summary>
    /// Checks how many free slots the player has in the container
    /// </summary>
    /// <returns>Checks how many free slots the player has in the container</returns>
    public int FreeSlots()
    {
        return items.Count(i => i == null || i.item == null);
    }

    /// <summary>
    /// Handles adding an item to the ontainer
    /// </summary>
    /// <param name="itemId">The item id your trying to add</param>
    /// <param name="amount">The amount of the item you want</param>
    /// <param name="slot">The preferred slot you want to put the item in</param>
    /// <returns></returns>
    public bool Add(int itemId, int amount = 1, int slot = -1)
    {
        //Grabs the item data which your trying to add
        //And checks if the item is null
        var itemToAdd = gameManager.itemDefinition[itemId];
        if (itemToAdd == null) return false;
        
        //Grabs the new slot your trying to add the item to
        //Checks if the item is stackable and has the item to grab the slot of that item
        //Also checks if the slot is -1
        var newSlot = slot == -1 ? FreeSlot() : slot;
        if (itemToAdd.stackable && Contains(itemId)) newSlot = GetSlot(itemId);
        if (newSlot == -1) return false;

        //Checks if the item is stackable
        if (itemToAdd.stackable)
        {
            //Grabs the item for the lot
            var item = items[newSlot];
            //Checks if the item inside the data is null and sets the new item
            if (item.item == null) item.item = itemToAdd;
            
            //Grabs the updated total
            //Checks if the new amount is higher then the max in or under 1
            var totalAmount = item.amount + amount;
            if (totalAmount >= int.MaxValue || totalAmount < 1) return false;
            
            //Update the amount of the selected item
            item.amount = totalAmount;
            return true;
        }
        else
        {
            //Grabs the amount of free slots
            int openSlots = FreeSlots();
            //Checks if the amount is higher then the free slots
            if (amount > openSlots) amount = openSlots;
            
            //Checks if the open slots is the same or higher then amount
            if (openSlots >= amount)
            {
                //Loops though the amount of items its has to add
                for (var index = 0; index < amount; index++)
                {
                    //If it isn't the first slot grab the next free available slot
                    if (index > 0) newSlot = FreeSlot();
                    //Grabs the item for the slot and if its null set the item
                    var item = items[newSlot];
                    if (item.item == null) item.item = itemToAdd;
                    
                    //Update the item amount
                    item.amount = 1;
                }
                return true;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Handles removing items from the container
    /// </summary>
    /// <param name="itemId">The item id your trying to remove</param>
    /// <param name="amount">The amount your trying to remove</param>
    /// <param name="preferredSlot">The preferred slot you want to remove the slot from</param>
    /// <returns>Handles removing items from the container</returns>
    public bool Remove(int itemId, int amount = 1, int preferredSlot = -1)
    {
        //Grabs the item your trying to remove
        //And checks if that item actuall exists
        var itemToRemove = gameManager.itemDefinition[itemId];
        if (itemToRemove == null) return false;
        
        //Grabs the slot for the item id
        //Checks if the slot isn't -1
        var slot = GetSlot(itemId);
        if (slot == -1) return false;
        
        //Grabs the item for the slot
        var item = items[slot];

        //Checks if the item is stackable
        if (itemToRemove.stackable)
        {
            //Checks if the item data or item is null
            if (item == null || item.item == null) return false;
            //Checks if the item amount is higher then the amount your trying to remove
            if (item.amount > amount)
                item.amount -= amount;
            else
            {
                //Sets the item to null and update the item amount
                item.item = null;
                item.amount = 0;
            }
        }
        else
        {
            //Loops though the amount of items your trying to remove
            for (var index = 0; index < amount; index++)
            {
                //Updates the slot
                slot = GetSlot(itemId);
                //If its the first item and it has a preferred slot remove that instead
                if (index == 0 && preferredSlot != -1)
                {
                    //Grabs the item for the preferred slot
                    //And if the ids match set that slot
                    var inSlot = Get(preferredSlot);
                    if (inSlot.item.itemId == itemId) slot = preferredSlot;
                }
                //If the slot isn't -1 set the data to null
                if (slot != -1)
                {
                    item = items[slot];
                    item.item = null;
                    item.amount = 0;
                }
            }
            return true;
        }
        
        return true;
    }

    /// <summary>
    /// Handles updating the inventory hot bar and inventory
    /// </summary>
    /// <param name="ui"></param>
    public void UpdateUI(GameObject[] ui)
    {
        //Loops though all the possible ui options
        for (int i = 0; i < ui.Length; i++)
        {
            //Loops though all the items
            for (var index = 0; index < items.Length; index++)
            {
                //Grabs the player
                var player = GameManager.playerObject.GetComponent<Player>();
                //Grabs how many children there are in the ui component
                var childCount = ui[i].transform.childCount;
                //If the index (aka item slot) is higher then the child count
                if (index >= childCount) continue;
                
                //Grab the slot its trying to update
                var slot = ui[i].transform.GetChild(index);

                //Handles highlighting the slot in the bottom inventory hotbar
                if (index < 9 && i == 0 && ui.Length > 1)
                {
                    var slotImage = slot.GetComponent<Image>();
                    if(index != player.inventorySlot)
                     
                        slotImage.color = new Color(1, 1, 1, 0.588f);
                    else
                        slotImage.color = new Color(1, 1, 1, 1);
                }
                
                //Grab all the required children from the item slot
                var canvas = slot.GetChild(0);
                var image = canvas.GetChild(0);
                var text = image.GetChild(0);
                var bullets = image.GetChild(1);
                var durability = image.GetChild(2);

                //Grabs the required components from the item slot
                var itemSprite = image.GetComponent<Image>();
                var itemAmount = text.GetComponent<Text>();
                var durabilityBar = durability.GetChild(1).GetComponent<Image>();

                var itemSlot = ui.Length == 1 ? index + 9 : index;
                var item = items[itemSlot];

                //Checks if the item data or item is null
                if (item == null || item.item == null)
                {
                    durability.gameObject.SetActive(false);
                    bullets.gameObject.SetActive(false);
                    itemSprite.enabled = false;
                    itemAmount.enabled = false;
                }
                else
                {
                    //Disable the bullet section
                    bullets.gameObject.SetActive(false);
                    
                    //Enables the required comps
                    itemSprite.enabled = true;
                    itemAmount.enabled = true;
                    
                    //Update the item sprite
                    itemSprite.sprite = item.item.uiSprite;
                    
                    //How many of the items the player has
                    var amount = $"{item.amount}";
                    
                    //Checks if the item has a gun attached to it
                    if (item.item.gun != null)
                    {
                        //Grabs the weapon the player is wearing
                        var weapon = player.gun.Get(player.gun.GetWeapon(itemSlot), itemSlot);
                        //Checks if the weapon isn't null
                        if (weapon != null)
                        {
                            //Enables the buttom sprites
                            bullets.gameObject.SetActive(true);
                            //Updates the amount of bullets the player has in his inventory
                            amount = $"{weapon.bulletsInChamber}/{player.inventory.GetAmountFromItem(weapon.gun.bullets.itemId)}";
                        }
                    }
                    //Dont show the amount of non stackable objects
                    else if (!item.item.stackable) amount = "";

                    //Checks if the item is a gun or tool
                    if (item.item.gun != null || item.item.tool != null)
                    {
                        //Grabs the gun or tool data
                        var gun = player.gun.Get(player.gun.GetWeapon(itemSlot), itemSlot);
                        var tool = player.tool.Get(player.tool.GetWeapon(itemSlot), itemSlot);
                        
                        //Enable the durability 
                        durability.gameObject.SetActive(true);

                        //The start and current durability
                        var startDurability = 0f;
                        var currentDurability = 0f;

                        //Checks if the gun isn't null
                        //Then update the variables accordingly
                        if (gun != null)
                        {
                            startDurability = gun.gun.weaponDurability;
                            currentDurability = gun.gunDurability;
                        }
                        //Checks if the tool isn't null
                        //Then update the variables accordingly
                        else if (tool != null)
                        {
                            startDurability = tool.tool.weaponDurability;
                            currentDurability = tool.toolDurability;
                        }
                        
                        durabilityBar.fillAmount = currentDurability / startDurability;
                    }
                    
                    itemAmount.text = amount;
                }
            }
        }
    }
}
