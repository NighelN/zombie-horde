using UnityEngine;

[CreateAssetMenu(fileName = "New gun", menuName = "Gun")]
public class GunData : WeaponData
{
    [Header("Gun Data")]
    public ToolTip tooltip;
    public Item bullets;
    public int maxBullets;
    public float reloadSpeed;
    public float bulletMovementSpeed;
    public float durabilityDamage;
}
