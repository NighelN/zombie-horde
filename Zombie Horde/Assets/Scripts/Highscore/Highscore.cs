using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : JsonHandler<HighscoreEntry>
 {
     [Header("Main menu variables")]
     [SerializeField] private GameObject highscorePrefab;
     [SerializeField] private GameObject highscoreParent;
     private Color defaultColor = new Color(0.6117647f, 0.4117647f, 0.1294118f);
     private Color secondColor = new Color(0.5372549f, 0.3647059f, 0.1254902f);
     
     /// <summary>
     /// Name of the json file
     /// </summary>
     /// <returns>The name of the json file its saving/loading</returns>
     protected override string GetFileName()
     {
         return "highscores.json";
     }
 
     /// <summary>
     /// The path of where the file can be found
     /// </summary>
     /// <returns>The path where the json file can be found</returns>
     protected override string GetPath()
     {
         return $"{Application.persistentDataPath}/SaveData";
     }

     public override void Start()
     {
         base.Start();
         
         //Handles sorting the list by days surviving (top -> bottom)
         entries.Sort((a, b) => a.daysSurvived.CompareTo(b.daysSurvived));
         entries.Reverse();
         
         //Checks if the script is being loaded in the main menu
         if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
             UpdateUI();
     }

     /// <summary>
     /// Handles sorting the list with days survived
     /// Top -> Bottom
     /// </summary>
     public void SortByDays()
     {
         entries.Sort((a, b) => a.daysSurvived.CompareTo(b.daysSurvived));
         entries.Reverse();

         UpdateUI();
     }

     /// <summary>
     /// Handles sorting the list with zombies killed
     /// Top -> Bottom
     /// </summary>
     public void SortByZombiesKilled()
     {
         entries.Sort((a, b) => a.zombiesKilled.CompareTo(b.zombiesKilled));
         entries.Reverse();

         UpdateUI();
     }

     /// <summary>
     /// Handles sorting the list with damage dealt
     /// Top -> Bottom
     /// </summary>
     public void SortByDamageDealt()
     {
         entries.Sort((a, b) => a.damageDealt.CompareTo(b.damageDealt));
         entries.Reverse();

         UpdateUI();
     }

     /// <summary>
     /// Handles sorting the list with damage taken
     /// Top -> Bottom
     /// </summary>
     public void SortByDamageTaken()
     {
         entries.Sort((a, b) => a.damageTaken.CompareTo(b.damageTaken));
         entries.Reverse();

         UpdateUI();
     }

     public void UpdateUI()
     {
         foreach (Transform child in highscoreParent.transform)
             Destroy(child.gameObject);
         
         for (var slot = 0; slot < entries.Count; slot++)
         {
             var entry = entries[slot];
             
             //Handles spawning the prefab for the highscore entry
             var entryObject = Instantiate(highscorePrefab, highscoreParent.transform, true);
             entryObject.transform.localScale = new Vector3(1,1,1);
             entryObject.transform.name = $"Highscore: {entry.playerName}";

             entryObject.GetComponent<Image>().color = slot % 2 == 0 ? defaultColor : secondColor;

             var entryTransform = entryObject.transform;
                 
             //All variables from the first row
             var rowOne = entryTransform.GetChild(0);
             var playerName = rowOne.transform.GetChild(0);
             var daysSurvived = rowOne.transform.GetChild(1);
             var zombiesKilled = rowOne.transform.GetChild(2);

             playerName.GetComponent<Text>().text = $"Player: {entry.playerName}";
             daysSurvived.GetComponent<Text>().text = $"Days survived: {entry.daysSurvived}";
             zombiesKilled.GetComponent<Text>().text = $"Zombies killed: {entry.zombiesKilled}";
                 
             //all variables from the second row
             var rowTwo = entryTransform.GetChild(1);
             var damageDealt = rowTwo.GetChild(0);
             var damageTaken = rowTwo.GetChild(1);
             var playerStatus = rowTwo.GetChild(2);

             damageDealt.GetComponent<Text>().text = $"Damage dealt: {entry.damageDealt}";
             damageTaken.GetComponent<Text>().text = $"Damage taken: {entry.damageTaken}";
             playerStatus.GetComponent<Text>().text = $"Player status: {entry.playerStatus}";
         }
     }

    public void Add(HighscoreEntry entry)
    {
        entries.Add(entry);
        Save();
    }
}