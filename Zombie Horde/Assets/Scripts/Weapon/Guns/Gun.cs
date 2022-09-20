using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Weapon<GunData>
{
    /// <summary>
    /// The current time of the reload
    /// </summary>
    private float reloadTimer;
    /// <summary>
    /// Time between each possible shot
    /// </summary>
    public float cooldownRatePerBulletShot;
    /// <summary>
    /// Time when a new bullet can be fired again
    /// </summary>
    private float _newBulletTimeStamp;

    public Gun(Player player) : base(player)
    {
        _newBulletTimeStamp = Time.time;
    }
    
    /// <summary>
    /// Checks if the player has an usable gun
    /// </summary>
    /// <returns></returns>
    public override bool CanUse()
    {
        //Grabs the weapon in the selected inventory slot
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null)
        {
            //Loops though all the guns the player has and resets the reloading
            foreach (var gun in player.guns.Where(gun => gun != null).Where(gun => gun.reloading))
                ResetReload(gun);

            return false;
        }

        //Sets the cooldown of the gun
        cooldownRatePerBulletShot = 1.0f / weapon.gun.weaponSpeed;

        return true;
    }
    
    /// <summary>
    /// Handles shooting the gun
    /// </summary>
    public override void Use()
    {
        //Checks if the player hasn't pressed attack or has the inventory/crafting/pause menu opened
        if (!inputManager.pressedAttack || player.inventoryOpened || player.craftingOpened || OpenPauseMenu.pauseMenuOpen) return;
        
        //Grabs the weapon the player is holding
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null || weapon.reloading) return;

        //Checks if the gun has any bullets in the chamber
        if (weapon.bulletsInChamber <= 0)
        {
            ToolTipSystem.showReload = true;
            return;
        }

        //Checks if the time for shooting the gun has passed
        if (Time.time > _newBulletTimeStamp)
        {
            //Decrease the weapons durability
            weapon.gunDurability -= weapon.gun.durabilityDamage;
        
            //Checks if the durability is lower then 1
            //Remove the weapon from the inventory and gun list
            if (weapon.gunDurability <= 0)
            {
                player.inventory.Remove(weapon.gun.item.itemId, 1, player.inventorySlot);
                player.guns.Remove(weapon);
            }
        
            //Removes a bullet from the chamber
            weapon.bulletsInChamber--;
            
            //Spawn a bullet
            SpawnBullet(weapon.gun);
            
            //Update the bullet delay
            _newBulletTimeStamp = Time.time + cooldownRatePerBulletShot;
        }
    }

    /// <summary>
    /// How many damage the gun does
    /// </summary>
    /// <returns>How many damage the gun does</returns>
    public override float GetDamage()
    {
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null) return 0;
        return weapon.gun.weaponDamage;
    }

    /// <summary>
    /// Grab the gun data for a specific slot
    /// </summary>
    /// <param name="slot">The slot the weapon is in</param>
    /// <returns></returns>
    public override GunData GetWeapon(int slot)
    {
        //Grabs the selected item for the slot
        var selectedItem = player.inventory.Get(slot);
        if (selectedItem == null || selectedItem.item == null || selectedItem.item.gun == null) return null;

        //Checks if the weapon already exists 
        if(!WeaponExists(selectedItem.item.gun, slot))
            player.guns.Add(new Guns(selectedItem.item.gun, slot));
        
        return selectedItem.item.gun;
    }

    /// <summary>
    /// Checks if the gun exists
    /// </summary>
    /// <param name="gun">The gun data</param>
    /// <param name="slot">The slot</param>
    /// <returns></returns>
    public override bool WeaponExists(GunData gun, int slot)
    {
        return player.guns.Where(weapon => weapon != null && weapon.gun != null).Any(weapon => weapon.gun.Equals(gun) && weapon.slot == slot);
    }

    /// <summary>
    /// Spawns the bullet
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="totalBullets"></param>
    void SpawnBullet(GunData gun, int totalBullets = 1)
    {
        for (int index = 0; index < totalBullets; index++)
        {
            var bulletObject = MonoBehaviour.Instantiate(gameManager.bulletPrefab, gameManager.bulletSpawnLocation.position, gameManager.bulletSpawnLocation.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.Initialize(gameManager.bulletSpawnLocation.up * gun.bulletMovementSpeed);
        }
    }

    /// <summary>
    /// Handles reloading the gun
    /// </summary>
    public void Reload()
    {
        //Grabs the weapon the player is using
        var weapon = Get(GetWeapon(player.inventorySlot), player.inventorySlot);
        if (weapon == null) return;
        var gun = weapon.gun;

        //Checks if the bullets in the chamber is higher then the max amount of bullets or if the player doesnt have any bullets
        if (weapon.bulletsInChamber >= gun.maxBullets || GetBulletAmount(gun) <= 0) return;

        //Checks if the player isn't reloading yet
        //If so start the reloading process
        if (!weapon.reloading && (inputManager.pressedReload && !player.inventoryOpened)) weapon.reloading = true;

        if (!weapon.reloading) return;
        
        //Calculate the opacity of the bullets
        var opacity = -(1 - (1 / gun.reloadSpeed) * reloadTimer) + 1;

        //Sets the transparency of the bullets
        SetBulletOpacity(player.inventorySlot, opacity);
        
        //Updates the reload timer
        reloadTimer += Time.deltaTime;
        if (!(reloadTimer >= gun.reloadSpeed)) return;
        
        //Updates the amount of bullets it needs to reload
        var ammoAmount = gun.maxBullets - weapon.bulletsInChamber;
        if (ammoAmount > GetBulletAmount(gun)) ammoAmount = GetBulletAmount(gun);

        //Removes the bullets from the inventory
        player.inventory.Remove(gun.bullets.itemId, ammoAmount);
        weapon.bulletsInChamber += ammoAmount;
        ResetReload(weapon);
    }
    
    /// <summary>
    /// Handles resetting reloading for the gun
    /// </summary>
    /// <param name="gun">The gun your trying to reset</param>
    private void ResetReload(Guns gun)
    {
        reloadTimer = 0;
        gun.reloading = false;
        SetBulletOpacity(gun.slot);
    }

    private void SetBulletOpacity(int invSlot, float opacity = 1f)
    {
        //Grabs the slot of the bullets you want to update
        var slot = inventoryHotbar.transform.GetChild(invSlot);
        //Grabs all the required children
        var canvas = slot.GetChild(0);
        var itemSprite = canvas.GetChild(0);
        var bullets = itemSprite.GetChild(1);

        //Loops though all bullet sprites and updates all the transparency
        for (var index = 0; index < 3; index++)
        {
            var image = bullets.GetChild(index).GetComponent<Image>();
            var color = image.color;
            color = new Color(color.r, color.g, color.b, opacity);
            image.color = color;
        }
    }

    /// <summary>
    /// Gets the amount of bullets the player has
    /// </summary>
    /// <param name="gun">The gun your looking up</param>
    /// <returns></returns>
    private int GetBulletAmount(GunData gun)
    {
        return player.inventory.GetAmountFromItem(gun.bullets.itemId);
    }
    
    /// <summary>
    /// Get the gun data
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public Guns Get(GunData gun, int slot)
    {
        return player.guns.Where(weapon => weapon != null && weapon.gun != null).FirstOrDefault(weapon => weapon.gun.Equals(gun) && weapon.slot == slot);
    }
}