using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    GameManager gameManager => GameManager.instance;
    
    public static PlayerHealth instance;

    public float startingHealth = 100f;
    public float currentHealth;



    public Image healthBar;
    public Text healthText;
    public GameObject deadParticle;

    public static bool playerAlive;

    [SerializeField] private UnityEvent OnPlayerDie = new UnityEvent();
    private Player player;

    void Awake()
    {
        instance = this;
        currentHealth = startingHealth;
        playerAlive = true;
        //Find the HP bar object and retrieve the image component
    }

    private void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
    }

    public void TakeDamage(float amount)
    {
        player.damageTaken += (int) amount;
        gameManager.soundPlayer.PlaySound(Sounds.PLAYER_HIT);
        currentHealth -= amount;
        //Checks if health drops below a threshold and switches to game over scene
        if (currentHealth <= 0)
        {
            Instantiate(deadParticle, this.transform.position, Quaternion.identity);
            playerAlive = false;
            OnPlayerDie.Invoke();
            Destroy(this.gameObject, 0.2f);
        }
        else playerAlive = true;
    }
    
    private void Update()
    {
        healthBar.fillAmount = currentHealth / startingHealth;
        healthText.text = $"{currentHealth}/{startingHealth}";
    }
}
