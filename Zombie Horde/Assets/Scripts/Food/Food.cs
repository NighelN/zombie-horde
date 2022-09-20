using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New food", menuName = "Food")]
[System.Serializable]
public class Food : ScriptableObject
{
    public int healthIncrease;
}
