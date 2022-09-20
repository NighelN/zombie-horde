using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool controllerConnected
    {
        get;
        private set;
    }
    public float horizontalMovementLeftStick
    {
        get;
        private set;
    }

    public float verticalMovementLeftStick
    {
        get;
        private set;
    }
    public float horizontalMovementRightStick
    {
        get;
        private set;
    }

    public float verticalMovementRightStick
    {
        get;
        private set;
    }

    public bool pressedAttack
    {
        get;
        private set;
    }

    public bool pressedReload
    {
        get;
        private set;
    }

    public bool pressedInventory
    {
        get;
        private set;
    }

    public bool pressedCrafting
    {
        get;
        private set;
    }

    public bool placeStructure
    {
        get;
        private set;
    }

    public bool pressedOne
    {
        get;
        private set;
    }

    public bool pressedTwo
    {
        get;
        private set;
    }

    public bool pressedThree
    {
        get;
        private set;
    }

    public bool pressedFour
    {
        get;
        private set;
    }

    public bool pressedFive
    {
        get;
        private set;
    }

    public bool pressedSix
    {
        get;
        private set;
    }

    public bool pressedSeven
    {
        get;
        private set;
    }

    public bool pressedEight
    {
        get;
        private set;
    }

    public bool pressedNine
    {
        get;
        private set;
    }

    public bool pressedConsole
    {
        get;
        private set;
    }

    public bool pressedEnter
    {
        get;
        private set;
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        //Loops though all connected joysticks
        foreach (string name in Input.GetJoystickNames())
        {
            //Debug.Log($"ControllerName: {name}");
            //Wireless gamepad = nintendo pro controller/joycons
            if (name.Equals("Wireless Gamepad")||name.Equals("Wireless Controller"))
            {
                controllerConnected = true;
            }
        }

        //Nintendo joycon connected
        if (controllerConnected)
        {
            horizontalMovementLeftStick = Input.GetAxisRaw("LeftStickX-AxisPS4");
            verticalMovementLeftStick = Input.GetAxisRaw("LeftStickY-AxisPS4");
            horizontalMovementRightStick = Input.GetAxisRaw("RightStickX-AxisPS4");
            verticalMovementRightStick = Input.GetAxisRaw("RightStickY-AxisPS4");
        }
        //No controller connected so use keyboard
        else
        {
            horizontalMovementLeftStick = Input.GetAxisRaw("Horizontal");
            verticalMovementLeftStick = Input.GetAxisRaw("Vertical");
            pressedAttack = Input.GetMouseButtonDown(0);
            pressedReload = Input.GetKeyDown(KeyCode.R);

            pressedOne = Input.GetKeyDown(KeyCode.Alpha1);
            pressedTwo = Input.GetKeyDown(KeyCode.Alpha2);
            pressedThree = Input.GetKeyDown(KeyCode.Alpha3);
            pressedFour = Input.GetKeyDown(KeyCode.Alpha4);
            pressedFive = Input.GetKeyDown(KeyCode.Alpha5);
            pressedSix = Input.GetKeyDown(KeyCode.Alpha6);
            pressedSeven = Input.GetKeyDown(KeyCode.Alpha7);
            pressedEight = Input.GetKeyDown(KeyCode.Alpha8);
            pressedNine = Input.GetKeyDown(KeyCode.Alpha9);
            
            pressedInventory = Input.GetKeyDown(KeyCode.E);
            pressedCrafting = Input.GetKeyDown(KeyCode.C);

            placeStructure = Input.GetKeyDown(KeyCode.Mouse1);

            pressedConsole = Input.GetKeyDown(KeyCode.Backslash);
            pressedEnter = Input.GetKeyDown(KeyCode.Return);
        }
    }

}
