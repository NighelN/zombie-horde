using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InputManager inputManager => InputManager.instance;
    public PlayerAttack playerAttack => PlayerAttack.instance;
    ResourceSystem resourceSystem => ResourceSystem.instance;
    
    /// <summary>
    /// The left and right hand of the player
    /// </summary>
    private GameObject leftHand, rightHand;
    /// <summary>
    /// The sprite renderer of the weapon in the players right hand
    /// </summary>
    private SpriteRenderer weaponRender;
    /// <summary>
    /// The game object for the inventory ui
    /// </summary>
    public GameObject inventoryUI;
    /// <summary>
    /// The game object for the crafting ui
    /// </summary>
    public GameObject craftingUI;

    public Text itemName;
    /// <summary>
    /// Check if the player has his inventory opened
    /// </summary>
    [HideInInspector] public bool inventoryOpened = false;
    /// <summary>
    /// Check if the player has his crafting interface opened
    /// </summary>
    [HideInInspector] public bool craftingOpened = false;
    /// <summary>
    /// The players inventory
    /// </summary>
    [HideInInspector] public Container inventory;
    /// <summary>
    /// The slot currently selected in the hotbar
    /// </summary>
    [HideInInspector] public int inventorySlot = 0;
    /// <summary>
    /// Handles the gun
    /// </summary>
    public Gun gun;
    /// <summary>
    /// Handles the tool
    /// </summary>
    public Tool tool;
    /// <summary>
    /// A list of all the weapons the player has
    /// </summary>
    [HideInInspector] public List<Guns> guns = new List<Guns>();

    public Guns GetGun(int slot)
    {
        return guns.Where(gun => gun != null).FirstOrDefault(gun => gun.slot == slot);
    }
    
    /// <summary>
    /// A listt of all the tools the player has
    /// </summary>
    [HideInInspector] public List<Tools> tools = new List<Tools>();

    public Tools GetTool(int slot)
    {
        return tools.Where(tool => tool != null).FirstOrDefault(tool => tool.slot == slot);
    }
    
    /// <summary>
    /// The name of the player
    /// </summary>
    public string playerName;
    /// <summary>
    /// How many zombies the player has killed
    /// </summary>
    [HideInInspector] public int zombiesKilled;
    /// <summary>
    /// How many damage the player has dealt
    /// </summary>
    [HideInInspector] public int damageDealt;
    /// <summary>
    /// How much damage the player has taken
    /// </summary>
    [HideInInspector] public int damageTaken;
    /// <summary>
    /// The status of the player
    /// </summary>
    [HideInInspector] public string playerStatus = "Alive";

    private void Start()
    {
        leftHand = GameManager.playerObject.transform.GetChild(0).gameObject;
        rightHand = GameManager.playerObject.transform.GetChild(1).gameObject;
        weaponRender = rightHand.transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        inventory = new Container(36);
        gun = new Gun(this);
        tool = new Tool(this);
        
        playerName = PlayerPrefs.GetString("CurrentPlayer");
    }

    private void Update()
    {
        SwitchSlot();
        OpenInventory();
        OpenCrafting();

        if (AllowedToScroll())
        {
            int damage = 1;

            //Checks if the player has an useable gun or weapon
            if (gun.CanUse() || tool.CanUse())
            {
                //Disables the left hand
                leftHand.SetActive(false);
                Sprite sprite = null;
                //Handles setting the gun sprite
                if (gun.CanUse())
                {
                    var weapon = gun.Get(gun.GetWeapon(inventorySlot), inventorySlot);
                    if (weapon == null) return;
                    sprite = weapon.gun.weaponSpriteHand;
                    
                } 
                //Handles setting the tool sprite
                else if (tool.CanUse())
                {
                    var weapon = tool.Get(tool.GetWeapon(inventorySlot), inventorySlot);
                    if (weapon == null) return;
                    sprite = weapon.tool.weaponSpriteHand;
                }
                //Assign the sprite
                weaponRender.sprite = sprite;
            }
            //Handles setting back the left hand
            else
            {
                leftHand.SetActive(true);
                weaponRender.sprite = null;
            }
            //Handles the use of the guns
            if (gun.CanUse())
            {
                gun.Use();
                gun.Reload();
            }
            //Handles the use of the tools
            else if (tool.CanUse())
            {
                tool.Use();
            }
            //Handles the player punching
            else if (inputManager.pressedAttack && !inventoryOpened && !craftingOpened)
            {
                playerAttack.StartSwinging();
                resourceSystem.DestroyResource(damage);
            }
        }
    }

    /// <summary>
    /// Checks if the player is holding a tool
    /// </summary>
    /// <returns></returns>
    public bool IsHoldingTool()
    {
        return tool.CanUse();
    }

    /// <summary>
    /// Checks if the player is holding a gun
    /// </summary>
    /// <returns></returns>
    public bool IsHoldingGun()
    {
        return gun.CanUse();
    }

    /// <summary>
    /// Grabs the gun the player is holding
    /// </summary>
    /// <returns>The gun the player is holding (if he is holding any)</returns>
    public GunData GetGun()
    {
        return gun.GetWeapon(inventorySlot);
    }

    /// <summary>
    /// Grabs what tool the player is holding
    /// </summary>
    /// <returns>The tool the player is holding (if he is holding any)</returns>
    public ToolData GetTool()
    {
        return tool.GetWeapon(inventorySlot);
    }
    
    public void OpenInventory()
    {
        if (!inputManager.pressedInventory || craftingOpened || OpenPauseMenu.pauseMenuOpen || ConsoleHandler.instance.consoleOpened) return;
        HandleInventory(!inventoryOpened);
    }

    void OpenCrafting()
    {
        if (!inputManager.pressedCrafting || inventoryOpened || OpenPauseMenu.pauseMenuOpen || ConsoleHandler.instance.consoleOpened) return;
        HandleCrafting(!craftingOpened);
    }

    public void HandleInventory(bool open)
    {
        inventoryOpened = open;
        inventoryUI.SetActive(open);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void HandleCrafting(bool open)
    {
        craftingOpened = open;
        craftingUI.SetActive(open);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public bool AllowedToScroll()
    {
        return !inventoryOpened && !craftingOpened && !OpenPauseMenu.pauseMenuOpen && !ConsoleHandler.instance.consoleOpened;
    }
    
    void SwitchSlot()
    {
        if (!AllowedToScroll()) return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) inventorySlot--;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) inventorySlot++;
        
        if (inventorySlot > 8) inventorySlot = 0;
        else if (inventorySlot < 0) inventorySlot = 8;
        
        if(inputManager.pressedOne) inventorySlot = 0;
        if(inputManager.pressedTwo) inventorySlot = 1;
        if(inputManager.pressedThree) inventorySlot = 2;
        if(inputManager.pressedFour) inventorySlot = 3;
        if(inputManager.pressedFive) inventorySlot = 4;
        if(inputManager.pressedSix) inventorySlot = 5;
        if(inputManager.pressedSeven) inventorySlot = 6;
        if(inputManager.pressedEight) inventorySlot = 7;
        if(inputManager.pressedNine) inventorySlot = 8;

        itemName.text = inventory.items[inventorySlot].item == null ? $"" : $"{inventory.items[inventorySlot].item.itemName}";
    }
}
