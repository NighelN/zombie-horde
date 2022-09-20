using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    float cursorSpeed = 5f;
    InputManager inputManager => InputManager.instance;
    PlayerAiming playerAiming => PlayerAiming.instance;
    Vector3 mousePosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePosition = Input.mousePosition;
        if (!inputManager.controllerConnected)
        {
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
        else transform.position = new Vector3(inputManager.horizontalMovementRightStick * cursorSpeed, inputManager.verticalMovementRightStick * cursorSpeed);
    }

}
