[System.Serializable]
public class HighscoreEntry
{
    /// <summary>
    /// The name of the player
    /// </summary>
    public string playerName;

    /// <summary>
    /// The amount of days the player has survived
    /// </summary>
    public int daysSurvived;

    /// <summary>
    /// The amount of zombies the player has killed
    /// </summary>
    public int zombiesKilled;

    /// <summary>
    /// The amount of damage the player has done
    /// </summary>
    public int damageDealt;

    /// <summary>
    /// The amount of damage the player has taken
    /// </summary>
    public int damageTaken;

    /// <summary>
    /// The status of the player
    /// </summary>
    public string playerStatus;

    public HighscoreEntry(string playerName, int daysSurvived, int zombiesKilled, int damageDealt, int damageTaken,
        string playerStatus)
    {
        this.playerName = playerName;
        this.daysSurvived = daysSurvived;
        this.zombiesKilled = zombiesKilled;
        this.damageDealt = damageDealt;
        this.damageTaken = damageTaken;
        this.playerStatus = playerStatus;
    }
}
