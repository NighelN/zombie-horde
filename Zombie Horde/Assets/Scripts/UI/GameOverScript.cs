using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private GameObject gameoverMenu;
    [SerializeField] private Text gameoverText;
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private GameObject[] ObjectsToDisable;
    private Highscore highscore;
    private Player player;

    private void Start()
    {
        highscore = GameManager.instance.GetComponent<Highscore>();
        player = GameManager.playerObject.GetComponent<Player>();
    }

    public void OpenGameoverMenu()
    {
        gameoverMenu.SetActive(true);
        player.playerStatus = "Died";
        highscore.Add(new HighscoreEntry(player.playerName, dayNightCycle.daysPassed, player.zombiesKilled, player.damageDealt, player.damageTaken, player.playerStatus));
        foreach (var item in ObjectsToDisable)
        {
            item.SetActive(false);
        }

        if (dayNightCycle.daysPassed == 1)
        {
            gameoverText.text = "You died after " + dayNightCycle.daysPassed + " day.";
        }
        else
        {
            gameoverText.text = "You died after " + dayNightCycle.daysPassed + " days.";
        }
    }
}
