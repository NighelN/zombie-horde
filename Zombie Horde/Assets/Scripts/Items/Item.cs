using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    [Header("Item Data")]
    public int itemId;
    public Sprite uiSprite;
    public string itemName;
    public string itemDescription;
    public bool stackable;
    [Header("Gun reference")]
    public GunData gun;
    [Header("Tool reference")]
    public ToolData tool;
    [Header("Food reference")]
    public Food food;
}
