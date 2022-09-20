using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth playerHealth => PlayerHealth.instance;
    BuildingSystem buildingSystem;
    
    public EnemyObject enemyObject;
    public GameObject enemy;
    public float timer;
    public LayerMask structure;
    public float attackCooldown;
    public float damage;
    public GameObject bloodParticle;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        buildingSystem = FindObjectOfType<BuildingSystem>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (OpenPauseMenu.pauseMenuOpen) return;
        if (Time.time > timer)
        {
            //Checks in 3 different directions if the enemy hit a player or structure
            var hit = Physics2D.Raycast(enemy.transform.position, Vector2FromAngle(enemy.transform.eulerAngles.z), 1f,layerMask);
            var hit1 = Physics2D.Raycast(enemy.transform.position, Vector2FromAngle(enemy.transform.eulerAngles.z + 30), 1f, layerMask);
            var hit2 = Physics2D.Raycast(enemy.transform.position, Vector2FromAngle(enemy.transform.eulerAngles.z - 30), 1f, layerMask);
            
            //Checks if any of the raycasts hit a target
            if (hit.collider != null)
                HitObject(hit, Vector2FromAngle(enemy.transform.eulerAngles.z) * new Vector2(0.1f, 0.1f));
            else if (hit1.collider != null)
                HitObject(hit1, Vector2FromAngle(enemy.transform.eulerAngles.z + 30) * new Vector2(0.1f, 0.1f));
            else if (hit2.collider != null)
                HitObject(hit2, Vector2FromAngle(enemy.transform.eulerAngles.z - 30) * new Vector2(0.1f, 0.1f));
        }

    }

    public void Attack()
    {
        //Takes health away from the player
        playerHealth.TakeDamage(damage);
        //Spawns the blood particle
        Instantiate(bloodParticle, playerHealth.transform.position, Quaternion.identity);
        //Sets the cool
        timer = Time.time + attackCooldown;
    }

    public void HitObject(RaycastHit2D hit, Vector2 addedVector)
    {
        //Checks if the zombie has hit the structure layer
        if (hit.collider.gameObject.layer == 9)
        {
            buildingSystem.DestroyStructure(hit.point + addedVector, damage);
            timer = Time.time + attackCooldown;
        }
        //Checks if the zombie has hit the player layer
        else if (hit.collider.gameObject.layer == 10)
            Attack();
    }

    private Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }
}
