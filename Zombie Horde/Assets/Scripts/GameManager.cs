using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SoundPlayer soundPlayer => SoundPlayer.instance;
    public static GameObject playerObject;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    
    public List<Item> itemDefinition = new List<Item>();
    public List<GunData> gunDefinition = new List<GunData>(); 

    public void Awake()
    {
        instance = this;
        playerObject = GameObject.Find("Player");
        
        itemDefinition.Sort((a, b) => a.itemId.CompareTo(b.itemId));
    }
}
