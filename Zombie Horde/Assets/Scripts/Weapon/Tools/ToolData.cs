using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New tool", menuName = "Tool")]
public class ToolData : WeaponData
{
    [Header("Tool Data")]
    public List<ResourceObject> resourceType;
}
