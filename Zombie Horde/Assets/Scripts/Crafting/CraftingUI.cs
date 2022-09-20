using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();
    [SerializeField] private GameObject recipePrefab, recipeParent;
    [SerializeField] private Text selectedAmount;
    [HideInInspector] public int selectedTotal = 1;
    public Player player;
    
    private void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
        Init();
    }

    private void Update()
    {
        //The parent object
        var parent = recipeParent.transform;
        
        //Loops though all the crafting recipes
        //And keeps them up to date with if the player has the correct amount of items
        for (var slot = 0; slot < craftingRecipes.Count; slot++)
        {
            var recipe = craftingRecipes[slot];
            var recipeObject = parent.GetChild(slot);
            
            //Updates the UI with all the required items etc
            UpdateUI(recipeObject.gameObject, recipe);
        }
    }

    void Init()
    {
        //Loops though all the crafting recipes
        for(var slot = 0; slot < craftingRecipes.Count; slot++)
        {
            //Grabs the recipe
            var recipe = craftingRecipes[slot];
            //Spawns the recipe
            var recipeObject = Instantiate(recipePrefab, recipeParent.transform, true);
            //Assigns the slot of this recipe
            recipeObject.GetComponent<CraftItem>().slot = slot;
            //Updates the scale of the item
            recipeObject.transform.localScale = new Vector3(1, 1, 1);
            UpdateUI(recipeObject, recipe);
        }
    }
    
    void UpdateUI(GameObject recipeObject, CraftingRecipe recipe)
    {
        //How many items are required in this recipe
        var itemCount = recipe.items.Count;

        var recipeTransform = recipeObject.transform;
        
        //Grabs all the related stuff for the item your trying to create
        //Also updates the sprite
        var sprite = recipeTransform.GetChild(0);
        var makeImage = sprite.GetComponent<Image>();
        makeImage.sprite = recipe.craftedItem.item.uiSprite;
        
        //Grabs all the related stuff for the item name
        var name = recipeTransform.GetChild(1);
        var nameText = name.GetComponent<Text>();
        nameText.text = $"{recipe.craftedItem.item.itemName}";
            
        //Grabs all the related stuff for the item description
        var desc = recipeTransform.GetChild(2);
        var descText = desc.GetComponent<Text>();
        descText.text = $"{recipe.craftedItem.item.itemDescription}";

        var resources = recipeTransform.GetChild(3);

        var items = resources.GetChild(1);

        //Loops though the required items for this recipe
        for (var index = 0; index < 5; index++)
        {
            //Grabs the object for the item
            var item = items.GetChild(index);
            //If the index is higher then the items count turn of the object
            if (index >= itemCount)
                item.gameObject.SetActive(false);
            else
            {
                //Grabs the required item sprite and text transform
                var itemSprite = item.GetChild(0);
                var itemText = item.GetChild(1);

                //Grabs the image and text component
                var image = itemSprite.GetComponent<Image>();
                var text = itemText.GetComponent<Text>();

                //Update the sprite of the item
                image.sprite = recipe.items[index].item.uiSprite;
                    
                //Checks how many of the item the player has in his inventory
                var itemAmount = player.inventory.GetAmountFromItem(recipe.items[index].item.itemId);
                //The amount required for this item
                var itemAmountRequired = recipe.items[index].amount;
                //Checks if the player has the correct amount of items
                var hasCorrectAmount = itemAmount >= itemAmountRequired;
                    
                //Updates the color and text of the item
                text.text = $"{itemAmount}/{itemAmountRequired}";
                text.color = hasCorrectAmount ? Color.green : Color.red;
            }
        }
    }

    public void UpdateSelectedTotal(int total)
    {
        selectedTotal = total;
        selectedAmount.text = selectedTotal == -1 ? "Select\namount: All" : $"Select\namount: {selectedTotal}";
    }
}
