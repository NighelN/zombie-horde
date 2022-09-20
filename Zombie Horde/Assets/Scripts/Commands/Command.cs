public abstract class Command
{
    /// <summary>
    /// The args used in the command
    /// </summary>
    public string[] args { get; set; }

    /// <summary>
    /// Handles the commands
    /// </summary>
    /// <param name="player">The player executing the command</param>
    /// <param name="args">All the arguments of the command</param>
    public virtual void HandleCommand(string[] args)
    {
        this.args = args;
        ConsoleResponse();
    }

    /// <summary>
    /// The response the server will send to the players console (in the client)
    /// </summary>
    /// <returns>Sends a string that the server will send to the players client</returns>
    public abstract string GetResponse();

    /// <summary>
    /// Handles the console respone of the command
    /// </summary>
    /// <param name="player">The player executing the command</param>
    public void ConsoleResponse()
    {
        ConsoleHandler.instance.AddTextToConsole(GetResponse());
    }
}
