using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnZombies : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemySpawner.SetActive(true);
    }
}
