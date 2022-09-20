using UnityEngine;
using UnityEngine.UI;

public class ConsoleHandler : MonoBehaviour
{
    public static ConsoleHandler instance;

    /// <summary>
    /// The game object for the console
    /// </summary>
    [SerializeField]
    GameObject console;
    /// <summary>
    /// The input field the player is typing in
    /// </summary>
    [SerializeField]
    InputField input;
    /// <summary>
    /// Shows all the commands the user has put in
    /// </summary>
    [SerializeField]
    Text commandConsole;
    /// <summary>
    /// Checks if the player has the console open or not
    /// </summary>
    public bool consoleOpened;

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        OpenConsole();

        EnterCommand();
    }

    /// <summary>
    /// Handles opening the console
    /// </summary>
    void OpenConsole()
    {
        //Checks if the user pressed the console key
        if (InputManager.instance.pressedConsole)
        {
            //Sets the console opened to the oppesite
            consoleOpened = !consoleOpened;
            //Opens or closes the console
            console.SetActive(consoleOpened);
            //Focus on the input field
            input.ActivateInputField();
        }
    }

    /// <summary>
    /// Handles entering the commands
    /// </summary>
    void EnterCommand()
    {
        //Checks if the user pressed enter and has the console opened
        if (InputManager.instance.pressedEnter && consoleOpened)
        {
            //Grabs the input of the user
            string command = input.text;
            //Adds the command to the console
            AddTextToConsole(command);

            //Splits the command into an array of arguments
            string[] args = command.Split(' ');

            CommandManager.Execute(args);

            //Clears the input field
            input.text = "";
            //Refocus on the input field
            input.ActivateInputField();
        }
    }

    /// <summary>
    /// Handles adding the text to the text field
    /// </summary>
    /// <param name="text">The command the player filled in</param>
    public void AddTextToConsole(string text)
    {
        commandConsole.text += "\n" + text;
    }
}
