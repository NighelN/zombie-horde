using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public static PlayerAiming instance;
    PlayerMovement playerMovement => PlayerMovement.instance;
    private Camera _camera;
    public Vector2 mousePos;
    public float angleOffset = 90f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        instance = this;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenPauseMenu.pauseMenuOpen || ConsoleHandler.instance.consoleOpened) return;
        LookAtMouse();
    }
    
    /// <summary>
    /// Makes the player look at the mouse
    /// TODO: Find a fix for the player not properly looking at the mouse 
    /// </summary>
    void LookAtMouse()
    {
        mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - playerMovement.rb.position;
        float angle = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) - angleOffset;
        playerMovement.rb.rotation = angle;
    }
}
