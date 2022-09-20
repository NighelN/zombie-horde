using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPauseMenu : MonoBehaviour
{
    public static bool pauseMenuOpen = false;
    [SerializeField] private GameObject pauseMenu;
    private Player player;
    public GameObject[] objectsToDeactivate;
    public GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.playerObject.GetComponent<Player>();
        pauseMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerHealth.playerAlive)
        {
            if (player.inventoryOpened) player.HandleInventory(false);
            else if (player.craftingOpened) player.HandleCrafting(false);
            else togglePauseMenu();
        }
    }

    public void togglePauseMenu()
    {
        if (pauseMenuOpen) //uit
        {
            pauseMenu.SetActive(false);
            pauseMenuOpen = false;
            foreach (GameObject gameObject in objectsToDeactivate)
            {
                gameObject.SetActive(true);
                print(gameObject.name);
                controls.SetActive(false);
            }
        }
        else //aan
        {
            pauseMenu.SetActive(true);
            pauseMenuOpen = true;
            foreach (GameObject gameObject in objectsToDeactivate)
            {
                gameObject.SetActive(false);
                print(gameObject.name);
            }
        }
    }
}
