using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New crafting recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Item Created")]
    public ItemData craftedItem;
    [Header("Item Required")]
    public List<ItemData> items = new List<ItemData>();
}
