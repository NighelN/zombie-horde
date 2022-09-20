using UnityEngine;

public class SpawnItem : Command
{
    int amount = 1;

    public override void HandleCommand(string[] args)
    {
        int itemId = int.Parse(args[1]);
        if (args.Length > 2)
            amount = int.Parse(args[2]);

        GameManager.playerObject.GetComponent<Player>().inventory.Add(itemId, amount);

        MonoBehaviour.print($"itemId: {itemId}, amount: {amount}");

        base.HandleCommand(args);
    }

    public override string GetResponse()
    {
        return $"The player has spawned {amount}x {GameManager.instance.itemDefinition[int.Parse(args[1])].itemName}.";
    }
}
