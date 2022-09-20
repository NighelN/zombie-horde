using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealing : MonoBehaviour
{
    GameManager gameManager => GameManager.instance;
    PlayerHealth playerHealth => PlayerHealth.instance;
    
    private Player player;
    private float lastEat = 0f;
    
    void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
    }

    void Update()
    {
        //Decreases the eating delay
        if (lastEat > 0)
        {
            lastEat -= Time.deltaTime;
            if (lastEat <= 0)
                lastEat = 0f;
        }
        
        //Checks if the player pressed the right mouse button and there is no delay for eating
        if (player.inputManager.placeStructure && lastEat <= 0 && player.AllowedToScroll())
        {
            //Grabs the item the player has selected in his inventory
            var item = player.inventory.Get(player.inventorySlot).item;
            if (item == null) return;
            
            //Grabs the food reference from the tiem
            var food = item.food;
            if (food == null) return;

            //Play the eating sound
            gameManager.soundPlayer.PlaySound(Sounds.PLAYER_EATING);
            
            //Removes the food
            player.inventory.Remove(item.itemId, 1);
            
            //Increases the players health
            playerHealth.currentHealth += food.healthIncrease;
            
            //Make sure the health cant be higher then the starting health
            if (playerHealth.currentHealth > playerHealth.startingHealth)
                playerHealth.currentHealth = playerHealth.startingHealth;

            //Adds the delay for eating
            lastEat = 1.85f;
        }
    }
}
