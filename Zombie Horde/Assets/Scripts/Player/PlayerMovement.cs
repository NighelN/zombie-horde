using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private Tile[] slowTiles;

    InputManager inputManager => InputManager.instance;

    [SerializeField] private float speed = 4;
    [SerializeField] private float slowSpeed = 2;
    [SerializeField] private float timer = 0;

    public GameObject waterWalkEffect;
    bool spawnParticle;

    public Rigidbody2D rb;

    private Player player;
    
    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.playerObject.GetComponent<Player>();

    }
    private void FixedUpdate()
    {
        //Checks if the pause menu is opened
        if (OpenPauseMenu.pauseMenuOpen || ConsoleHandler.instance.consoleOpened) return;
        
        //Checks if there is a particle that needs to be spawned
        timer += Time.deltaTime;
        if(timer > 1)
        {
            spawnParticle = true;
        }
        if(timer < 1)
        {
            spawnParticle = false;
        }
        HandleMovement();
    }
    void HandleMovement()
    {
        //Checks if the player has the inventory/crafting or pause menu open
        if (player.inventoryOpened || player.craftingOpened || OpenPauseMenu.pauseMenuOpen) return;
        
        //Grabs the horizontal and vertical input
        var horizontal = inputManager.horizontalMovementLeftStick;
        var vertical = inputManager.verticalMovementLeftStick;
        
        //Normalize the input
        var inputVector = new Vector2(horizontal, vertical);
        inputVector.Normalize();

        //Grabs the position the player is standing on
        Vector3Int gridPosition = backgroundTilemap.WorldToCell(this.transform.position);
        
        //Grabs the tile the player is standing on
        //Also checks if the tile inst null
        TileBase tile = backgroundTilemap.GetTile(gridPosition);
        if (tile != null)
        {
            //Loops though all the slow tiles
            foreach (var slowTile in slowTiles)
            {
                //Checks if the names match
                if (slowTile.name == tile.name)
                {
                    //Sets the speed op the player
                    rb.velocity = inputVector * slowSpeed;
                    //Checks the parctile should spawn
                    if (spawnParticle)
                    {
                        Instantiate(waterWalkEffect, this.transform.position, Quaternion.identity);
                        timer = 0;

                    }
                    return;
                }
            }
        }
        //Makes the player move
        rb.velocity = inputVector*speed;
    }

}
