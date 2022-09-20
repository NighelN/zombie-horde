
using System.Linq;
using UnityEngine;

public abstract class Weapon<T>
{
    /// <summary>
    /// the game manager of the game
    /// </summary>
    public GameManager gameManager => GameManager.instance;
    /// <summary>
    /// The input system for the player
    /// </summary>
    public InputManager inputManager => InputManager.instance;
    public ResourceSystem resourceSystem => ResourceSystem.instance;
    /// <summary>
    /// 
    /// </summary>
    public Player player;
    
    public GameObject inventoryHotbar;

    public Weapon(Player player)
    {
        this.player = player;
        inventoryHotbar = GameObject.Find("Inventory Hotbar");
    }

    public abstract bool CanUse();

    public abstract void Use();
    
    public abstract float GetDamage();
    
    public abstract T GetWeapon(int slot);

    public abstract bool WeaponExists(T weapon, int slot);
}