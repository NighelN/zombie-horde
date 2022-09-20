using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //public static EnemyHealth instance;

    public float currentHealth;
    public EnemyObject enemyObject;
    public GameObject enemy;
    public GameObject bloodParticle;
    public SpriteRenderer shadow;
    public DayNightCycle dayNightCycle;
    private Player player;

    PlayerAttack playerAttack => PlayerAttack.instance;

    private void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
    }


    public void TakePlayerDamage(int amount)
    {
        //Updates the damage dealt of the player
        player.damageDealt += amount;
        //Decreases the health of the zombie
        currentHealth -= amount;
        //Checks if the zombie has died
        if(currentHealth <= 0)
        {
            //Increases the zombies killed for the player
            player.zombiesKilled++;
            //Removes the zombies shadow from the system
            dayNightCycle.movingShadows.Remove(shadow);
            
            Destroy(gameObject);
            
            //Grabs the rotation of the zombie
            //And spawns the blood particle
            var startRot = Quaternion.LookRotation(enemy.transform.forward - enemy.transform.forward * 2);
            Instantiate(bloodParticle, enemy.transform.position, startRot);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Checks if the enemy made collision with the player his fists and checks if the player is actually punching
        if(collision.gameObject.CompareTag("PlayerFist") && playerAttack.playerAnimator.GetBool("isPunching"))
        {
            //The default amount of damage the player his fists does
            var damage = 2;
            var weapon = player.tool.Get(player.tool.GetWeapon(player.inventorySlot), player.inventorySlot);
            if (weapon != null)
            {
                damage = (int) weapon.tool.weaponDamage;
                player.tool.LoseDurability(weapon, 8);
            }
            TakePlayerDamage(damage);
        }

    }
}
