using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("Weapon Data")]
    public Item item;
    public WeaponType weaponType;
    public Sprite weaponSprite;
    public Sprite weaponSpriteHand;
    public string weaponName;
    public float weaponSpeed;
    public float movementSpeed;
    public bool twoHanded;
    public float weaponDurability;
    public float weaponDamage;
}

public enum WeaponType
{
    MELEE,
    RANGED
}
