using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Structure", menuName = "New Structure")]
public class BuildingObject : ScriptableObject
{
    public ItemData item;
    public float structureHealth;
    public Tile tile;
    public Tile[] tilesToBuildOn;
    public GameObject particleEffect;

}
