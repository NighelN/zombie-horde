using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    private Player player;
    
    [SerializeField] public int slot;
    private Image image;
    private CraftingUI crafting;

    public void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
        crafting = GameManager.instance.GetComponent<CraftingUI>();
        image = GetComponent<Image>();
    }

    public void Craft()
    {
        //Grabs the recipe your trying to craft
        var recipe = crafting.craftingRecipes[slot];
        //The max amount of items the player is able to craft
        var playerMaxAmount = crafting.selectedTotal;

        //Loops though all the items required in this recipe
        foreach (var item in recipe.items)
        {
            //Checks how many of the item the player has
            var itemAmount = player.inventory.GetAmountFromItem(item.item.itemId);
            //Calculates the total amount of items it can make
            var total = itemAmount / item.amount;

            //Checks if the total is lower then the selected amount
            //And updates accordingly
            if (total < crafting.selectedTotal)
                playerMaxAmount = total;
        }

        if (playerMaxAmount <= 0) return;

        //Creates a new list of required items with the updated amount
        //And checks if the player has all these items
        var itemsRequired = recipe.items.Select(item => new ItemData(item.item, item.amount * playerMaxAmount)).ToList();
        var containsAll = player.inventory.ContainsAll(itemsRequired);
        if (!containsAll) return;

        //Loops though all the items and removes them
        foreach (var item in recipe.items)
            player.inventory.Remove(item.item.itemId, item.amount * playerMaxAmount);

        //Adds the crafted item
        player.inventory.Add(recipe.craftedItem.item.itemId, recipe.craftedItem.amount * playerMaxAmount);
    }

    public void HoverEnter()
    {
        image.color = new Color(1, 0.9682621f, 0, 0.09803922f);
    }

    public void HoverExit()
    {
        image.color = new Color(1, 1, 1, 0);
    }
}
