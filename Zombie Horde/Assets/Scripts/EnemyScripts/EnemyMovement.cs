using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap backgroundTilemap;
    public Tile[] slowTiles;
    PlayerHealth playerHealth => PlayerHealth.instance;
    public ParticleSystem dust;
    public Rigidbody2D rb2d;
    public Transform target;
    public float speed;
    public float slowSpeed;
    public float minimumDistance;
    public float maximumDistance;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.playerAlive = true;
        rb2d.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if player is alive and doesnt have the pause menu open
        if (PlayerHealth.playerAlive && !OpenPauseMenu.pauseMenuOpen)
        {
            //Checks if the targets position is within the min and max distance
            if (Vector2.Distance(this.transform.position, target.position) > minimumDistance && Vector2.Distance(this.transform.position, target.position) < maximumDistance)
            {
                //Grabs the difference between target and the zombies position
                //Also normalizes the difference
                Vector3 diff = target.position - transform.position;
                diff.Normalize();

                //Rémon?
                var rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                
                //Handles moving the zombie
                Move();
                //Handles the particle effect for the zombie
                CreateDust();
            }
            //If the zombie cannot see an enemy it will not move
            else if (target) rb2d.velocity = Vector2.zero;
        }
        else if (target) rb2d.velocity = Vector2.zero;
    }

    private void Move()
    {
        //Checks if the pause menu is open to stop the enemy position
        if (OpenPauseMenu.pauseMenuOpen) return;
        
        //Grabs the current position of the zombie
        //Also grabs what tile the zombie is standing on and checks if its not null
        Vector3Int gridPosition = backgroundTilemap.WorldToCell(this.transform.position);
        TileBase tile = backgroundTilemap.GetTile(gridPosition);
        if (tile != null)
        {
            //Loops though all the slow walking tiles
            foreach (var slowTile in slowTiles)
            {
                //Checks if the name matches
                if (slowTile.name.Equals(tile.name))
                {
                    //Sets the movement speed slow
                    rb2d.velocity = rb2d.transform.rotation * new Vector2(slowSpeed, 0);
                    return;
                }
            }
        }
        //Moves the enemy
        rb2d.velocity = rb2d.transform.rotation * new Vector2(speed, 0);
    }

    public void CreateDust()
    {
        dust.Play();
    }
}
