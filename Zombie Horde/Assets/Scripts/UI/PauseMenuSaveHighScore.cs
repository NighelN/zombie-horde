using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSaveHighScore : MonoBehaviour
{
    [SerializeField] private DayNightCycle dayNightCycle;
    private Highscore highscore;
    private Player player;

    private void Start()
    {
        highscore = GameManager.instance.GetComponent<Highscore>();
        player = GameManager.playerObject.GetComponent<Player>();
    }

    public void SetHighScore()
    {
        highscore.Add(new HighscoreEntry(player.playerName, dayNightCycle.daysPassed, player.zombiesKilled, player.damageDealt, player.damageTaken, player.playerStatus));
    }
}
