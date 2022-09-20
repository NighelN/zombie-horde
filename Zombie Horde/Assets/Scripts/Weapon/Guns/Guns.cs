using UnityEngine;

public class Guns
{
    public GunData gun;
    public int slot;
    public int bulletsInChamber;
    public bool reloading;
    public float gunDurability;

    public Guns(GunData gun, int slot)
    {
        this.gun = gun;
        this.slot = slot;
        this.bulletsInChamber = 0;
        this.reloading = false;
        this.gunDurability = gun.weaponDurability;
    }

    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
}