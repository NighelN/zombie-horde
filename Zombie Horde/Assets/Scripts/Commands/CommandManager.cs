using System.Collections.Generic;

public static class CommandManager
{
    /// <summary>
    /// All the possible commands in the game
    /// </summary>
    static Dictionary<string, Command> commands = new Dictionary<string, Command>()
    {
        { "item", new SpawnItem() }
    };

    public static void Execute(string[] args)
    {
        //The command the player is trying to execute
        string commandName = args[0];
        //Checks if the commands exits
        if (!CommandExists(commandName))
        {
            //Sends the error to the console
            ConsoleHandler.instance.AddTextToConsole($"The {commandName} command does not exist!");
            return;
        }
        //Handles the command
        commands[commandName].HandleCommand(args);
    }

    static bool CommandExists(string command)
    {
        //Loops though the commands dictionary
        foreach (KeyValuePair<string, Command> entry in commands)
        {
            //Checks if any of the key's matches the command
            if (entry.Key.Equals(command))
            {
                return true;
            }
        }
        return false;
    }
}
